using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [Header("Configuración del nivel")]
    public int mecanismosRequeridos = 4; // recomendado múltiplo de 4 para que 25/50/75% caigan en números limpios
    public int reintentosPermitidos = 2; // cuántas veces pueden PERDER antes del reinicio total (jugar la primera vez no cuenta)

    [Header("Checkpoints del nivel (un punto central por etapa)")]
    [Tooltip("Índice 0 = inicio (0%), 1 = 25%, 2 = 50%, 3 = 75%. Arrastra 4 GameObjects vacíos.")]
    public Transform[] checkpoints = new Transform[4];

    [Header("Jugadores activos (se llena solo desde PlayerSpawner)")]
    public List<Transform> jugadoresActivos = new List<Transform>();

    [Header("Eventos")]
    public UnityEvent alResolverMecanismo;
    public UnityEvent alCompletarNivel;
    public UnityEvent alPerderIntento;
    public UnityEvent alAgotarIntentos;

    // Offsets relativos para formar el mismo patrón de "cuadrado" que ya usan en el spawn inicial
    private static readonly Vector2[] offsetsFormacion = new Vector2[]
    {
        new Vector2(-1, 1),
        new Vector2(1, 1),
        new Vector2(-1, -1),
        new Vector2(1, -1)
    };

    private int mecanismosResueltos;
    private int mecanismosEnCheckpointActual;
    private int checkpointActualIndex; // 0 = inicio, 1 = 25%, 2 = 50%, 3 = 75%
    private int reintentosRestantes;

    void Awake()
    {
        reintentosRestantes = reintentosPermitidos;
    }

    public void RegistrarMecanismoResuelto()
    {
        mecanismosResueltos++;
        alResolverMecanismo.Invoke();
        Debug.Log($"[LevelManager] Mecanismo resuelto: {mecanismosResueltos}/{mecanismosRequeridos}");

        ActualizarCheckpoint();

        if (mecanismosResueltos >= mecanismosRequeridos)
        {
            alCompletarNivel.Invoke();
            Debug.Log("[LevelManager] ¡Nivel completado!");
        }
    }

    private void ActualizarCheckpoint()
    {
        float progreso = (float)mecanismosResueltos / mecanismosRequeridos;

        // Revisamos del umbral más alto al más bajo para no "saltarnos" checkpoints intermedios
        if (progreso >= 0.75f && checkpointActualIndex < 3)
        {
            checkpointActualIndex = 3;
            mecanismosEnCheckpointActual = mecanismosResueltos;
            Debug.Log("[LevelManager] Checkpoint 75% alcanzado.");
        }
        else if (progreso >= 0.5f && checkpointActualIndex < 2)
        {
            checkpointActualIndex = 2;
            mecanismosEnCheckpointActual = mecanismosResueltos;
            Debug.Log("[LevelManager] Checkpoint 50% alcanzado.");
        }
        else if (progreso >= 0.25f && checkpointActualIndex < 1)
        {
            checkpointActualIndex = 1;
            mecanismosEnCheckpointActual = mecanismosResueltos;
            Debug.Log("[LevelManager] Checkpoint 25% alcanzado.");
        }
    }

    // Llamado cuando se acaba el tiempo del nivel (lo conectaremos en la fase del timer)
    public void PerderIntento()
    {
        reintentosRestantes--;
        Debug.Log($"[LevelManager] Perdieron. Reintentos restantes: {reintentosRestantes}");

        if (reintentosRestantes < 0)
        {
            alAgotarIntentos.Invoke();
            ReiniciarPartidaCompleta();
            return;
        }

        alPerderIntento.Invoke();

        // Revertimos el progreso al valor que tenía cuando se activó el checkpoint actual
        mecanismosResueltos = mecanismosEnCheckpointActual;
        ReposicionarEnCheckpointActual();
    }

    private void ReposicionarEnCheckpointActual()
    {
        Transform centro = (checkpoints != null && checkpointActualIndex < checkpoints.Length)
            ? checkpoints[checkpointActualIndex]
            : null;
        Vector3 posicionCentro = centro != null ? centro.position : Vector3.zero;

        for (int i = 0; i < jugadoresActivos.Count; i++)
        {
            if (jugadoresActivos[i] == null) continue;

            Vector2 offset = i < offsetsFormacion.Length ? offsetsFormacion[i] : Vector2.zero;
            Vector3 destino = posicionCentro + (Vector3)offset;

            jugadoresActivos[i].position = destino;
            Rigidbody2D rb = jugadoresActivos[i].GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;
        }
    }

    private void ReiniciarPartidaCompleta()
    {
        Debug.Log("[LevelManager] Reintentos agotados. Reinicio total desde el 0% del Nivel 1.");
        mecanismosResueltos = 0;
        mecanismosEnCheckpointActual = 0;
        checkpointActualIndex = 0;
        reintentosRestantes = reintentosPermitidos;
        ReposicionarEnCheckpointActual();
        // Cuando tengan más de un nivel real, aquí también dispararían la carga del Nivel 1.
    }
}
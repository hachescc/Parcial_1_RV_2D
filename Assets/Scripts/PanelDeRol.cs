using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

// Mecanismo interactivo que SOLO responde al jugador que tenga el rol correcto asignado
// en esta partida (ver AsignadorDeRoles). El mismo jugador que hoy es Ingeniero, la
// próxima vez que se reinicie el nivel puede tocarle Médico — por eso el panel no
// pregunta "quién sos" sino "qué rol tenés ahora".
//
// CÓMO ARMARLO EN EL EDITOR:
// 1. Crear un GameObject (ej. "Panel_Ingenieria"), con SpriteRenderer y BoxCollider2D
//    (Is Trigger ✔), en la layer Interactable (la misma que usan las PressurePlate).
// 2. Agregar este componente PanelDeRol y elegir en "Rol Requerido" cuál de los 4 roles
//    puede operarlo.
// 3. Conectar Al Activar / Al Desactivar / Al Resolver Definitivamente igual que en
//    DualPressurePlate (típicamente: cambiar color con DebugColorFlip, abrir una puerta
//    con GameObject.SetActive, y Al Resolver Definitivamente -> LevelManager.RegistrarMecanismoResuelto()).
public class PanelDeRol : MonoBehaviour, IInteractable
{
    [Header("Rol requerido para operar este panel")]
    public RolJugador rolRequerido;

    [Header("Qué pasa cuando se activa / desactiva")]
    public UnityEvent alActivar;
    public UnityEvent alDesactivar;

    [Header("Progreso del nivel (se dispara UNA sola vez, la primera vez que se activa)")]
    public UnityEvent alResolverDefinitivamente;

    [Header("Revelado por proximidad (para guiarse con el audio 360, no a la vista)")]
    [Tooltip("Si está marcado, el panel arranca invisible y solo se muestra cuando el jugador con el rol correcto está lo bastante cerca. Pensado para usarse junto a un AudioSource posicional en el mismo objeto (igual configuración que FuenteSonidoOculta) — el jugador correcto se guía por el sonido hasta que el panel se revela.")]
    public bool ocultoHastaAcercarse = false;
    public float radioRevelado = 4f;

    private bool yaFueResuelto;
    private bool estaActiva;
    private bool revelado = true;
    private SpriteRenderer sr;
    private readonly HashSet<GameObject> jugadoresCorrectosSosteniendo = new HashSet<GameObject>();

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (ocultoHastaAcercarse && sr != null)
        {
            revelado = false;
            sr.enabled = false;
        }
    }

    void Update()
    {
        if (!ocultoHastaAcercarse || sr == null) return;

        bool jugadorCorrectoCerca = false;
        Collider2D[] cercanos = Physics2D.OverlapCircleAll(transform.position, radioRevelado);
        foreach (Collider2D col in cercanos)
        {
            AsignadorDeRoles asignadorCercano = col.GetComponent<AsignadorDeRoles>();
            if (asignadorCercano != null && asignadorCercano.Rol == rolRequerido)
            {
                jugadorCorrectoCerca = true;
                break;
            }
        }

        if (jugadorCorrectoCerca != revelado)
        {
            revelado = jugadorCorrectoCerca;
            sr.enabled = revelado;
            Debug.Log($"[PanelDeRol] Panel de {InfoRoles.NombreRol(rolRequerido)} {(revelado ? "revelado — jugador correcto cerca" : "oculto de nuevo")}.");
        }
    }

    public void OnPlayerEnter(GameObject jugador)
    {
        AsignadorDeRoles asignador = jugador.GetComponent<AsignadorDeRoles>();
        if (asignador == null || asignador.Rol != rolRequerido)
        {
            Debug.Log($"[PanelDeRol] {jugador.name} intentó operar el panel de {InfoRoles.NombreRol(rolRequerido)}, pero no tiene ese rol esta partida.");
            MensajesEnPantalla.Mostrar($"Este panel necesita al {InfoRoles.NombreRol(rolRequerido)}");
            return;
        }

        jugadoresCorrectosSosteniendo.Add(jugador);
        Debug.Log($"[PanelDeRol] {jugador.name} ({InfoRoles.NombreRol(rolRequerido)}) está operando el panel.");
        RevisarEstado();
    }

    public void OnPlayerExit(GameObject jugador)
    {
        if (jugadoresCorrectosSosteniendo.Remove(jugador))
        {
            RevisarEstado();
        }
    }

    private void RevisarEstado()
    {
        bool debeEstarActiva = jugadoresCorrectosSosteniendo.Count > 0;

        if (debeEstarActiva && !estaActiva)
        {
            estaActiva = true;
            alActivar.Invoke();

            if (!yaFueResuelto)
            {
                yaFueResuelto = true;
                alResolverDefinitivamente.Invoke();
            }
        }
        else if (!debeEstarActiva && estaActiva)
        {
            estaActiva = false;
            alDesactivar.Invoke();
        }
    }
}

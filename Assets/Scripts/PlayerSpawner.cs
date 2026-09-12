using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Prefab del jugador")]
    public GameObject playerPrefab;

    [Header("Puntos de spawn")]
    public Transform[] spawnPoints;

    [Header("Cámara compartida")]
    public CameraFollow cameraFollow;

    [Header("Gestor del nivel (checkpoints, progreso, intentos)")]
    public LevelManager levelManager;

    [Header("Anuncio de roles al iniciar el nivel")]
    public AnuncioDeRoles anuncioDeRoles;

    private readonly string[] esquemas =
    {
        "Jugador1_WASD",
        "Jugador2_Flechas",
        "Jugador3_IJKL",
        "Jugador4_Numpad"
    };

    private readonly Color[] coloresJugador =
    {
        new Color(0.90f, 0.15f, 0.15f), // Jugador 1 - Rojo
        new Color(0.20f, 0.45f, 0.95f), // Jugador 2 - Azul
        new Color(0.20f, 0.80f, 0.30f), // Jugador 3 - Verde
        new Color(0.95f, 0.80f, 0.10f)  // Jugador 4 - Amarillo
    };

    private readonly string[] nombresColor = { "Rojo", "Azul", "Verde", "Amarillo" };

    private readonly string[] descripcionesControles =
    {
        "WASD | Interactuar: Shift Izquierdo",
        "Flechas | Interactuar: Shift Derecho",
        "IJKL | Interactuar: U",
        "Numpad 8/4/5/6 | Interactuar: Numpad 0"
    };

    void Start()
    {
        Debug.Log("[PlayerSpawner] Start() ejecutándose. playerPrefab=" + (playerPrefab != null ? playerPrefab.name : "NULL")
            + " | anuncioDeRoles=" + (anuncioDeRoles != null ? "asignado" : "NULL"));

        List<RolJugador> rolesDisponibles = new List<RolJugador>
        {
            RolJugador.Ingeniero,
            RolJugador.Medico,
            RolJugador.Piloto,
            RolJugador.Comunicaciones
        };
        BarajarRoles(rolesDisponibles);
        Debug.Log("[PlayerSpawner] Orden de roles esta partida: " + string.Join(", ", rolesDisponibles));

        List<AsignadorDeRoles> jugadoresParaAnuncio = new List<AsignadorDeRoles>();
        List<Collider2D> collidersJugadores = new List<Collider2D>();

        for (int i = 0; i < esquemas.Length; i++)
        {
            Vector3 posicion = (spawnPoints != null && i < spawnPoints.Length)
                ? spawnPoints[i].position
                : Vector3.zero;

            PlayerInput jugador = PlayerInput.Instantiate(
                playerPrefab,
                playerIndex: i,
                controlScheme: esquemas[i],
                pairWithDevice: Keyboard.current
            );

            jugador.transform.position = posicion;
            jugador.gameObject.name = $"Player_{i + 1}_{esquemas[i]}";

            SpriteRenderer sprite = jugador.GetComponentInChildren<SpriteRenderer>();
            if (sprite != null)
            {
                sprite.color = coloresJugador[i % coloresJugador.Length];
            }

            AsignadorDeRoles asignador = jugador.GetComponent<AsignadorDeRoles>();
            if (asignador == null)
            {
                asignador = jugador.gameObject.AddComponent<AsignadorDeRoles>();
                Debug.Log($"[PlayerSpawner] Jugador {i + 1}: AsignadorDeRoles no estaba en el prefab, se agregó en tiempo de ejecución.");
            }

            RolJugador rolAsignado = rolesDisponibles[i % rolesDisponibles.Count];
            asignador.Configurar(
                $"Jugador {i + 1}",
                nombresColor[i % nombresColor.Length],
                descripcionesControles[i % descripcionesControles.Length],
                rolAsignado);
            jugadoresParaAnuncio.Add(asignador);

            Collider2D colliderJugador = jugador.GetComponent<Collider2D>();
            if (colliderJugador != null)
            {
                collidersJugadores.Add(colliderJugador);
            }

            if (cameraFollow != null)
            {
                cameraFollow.targets.Add(jugador.transform);
            }

            if (levelManager != null)
            {
                levelManager.jugadoresActivos.Add(jugador.transform);
            }
        }

        for (int i = 0; i < collidersJugadores.Count; i++)
        {
            for (int j = i + 1; j < collidersJugadores.Count; j++)
            {
                Physics2D.IgnoreCollision(collidersJugadores[i], collidersJugadores[j], true);
            }
        }

        Debug.Log($"[PlayerSpawner] jugadoresParaAnuncio.Count = {jugadoresParaAnuncio.Count}. Llamando a anuncioDeRoles.MostrarRoles()...");

        if (anuncioDeRoles != null)
        {
            anuncioDeRoles.MostrarRoles(jugadoresParaAnuncio);
        }
        else
        {
            Debug.LogError("[PlayerSpawner] anuncioDeRoles es NULL — el campo 'Anuncio De Roles' no está asignado en el Inspector de este PlayerSpawner.");
        }
    }

    private void BarajarRoles(List<RolJugador> roles)
    {
        for (int i = roles.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            RolJugador temp = roles[i];
            roles[i] = roles[j];
            roles[j] = temp;
        }
    }
}

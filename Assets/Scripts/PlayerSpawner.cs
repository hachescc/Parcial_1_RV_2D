using UnityEngine;
using UnityEngine.InputSystem;

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

    private readonly string[] esquemas =
    {
        "Jugador1_WASD",
        "Jugador2_Flechas",
        "Jugador3_IJKL",
        "Jugador4_Numpad"
    };

    void Start()
    {
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

            if (cameraFollow != null)
            {
                cameraFollow.targets.Add(jugador.transform);
            }

            if (levelManager != null)
            {
                levelManager.jugadoresActivos.Add(jugador.transform);
            }
        }
    }
}
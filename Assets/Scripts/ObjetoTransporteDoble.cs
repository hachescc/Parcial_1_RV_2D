using UnityEngine;
using System.Collections.Generic;

public class ObjetoTransporteDoble : MonoBehaviour, IInteractable
{
    [Header("Cuántos jugadores hacen falta para mover el objeto")]
    public int jugadoresRequeridos = 2;

    [Header("Velocidad del objeto cuando lo cargan entre los requeridos")]
    public float velocidadTransporte = 3f;

    private Rigidbody2D rb;
    private readonly List<GameObject> jugadoresSujetando = new List<GameObject>();
    private readonly Dictionary<GameObject, PlayerMovement> movimientoDeCadaJugador = new Dictionary<GameObject, PlayerMovement>();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnPlayerEnter(GameObject jugador)
    {
        if (!jugadoresSujetando.Contains(jugador))
        {
            jugadoresSujetando.Add(jugador);
            if (!movimientoDeCadaJugador.ContainsKey(jugador))
            {
                movimientoDeCadaJugador[jugador] = jugador.GetComponent<PlayerMovement>();
            }
        }
        Debug.Log($"[TransporteDoble] {jugador.name} agarró el objeto. Total sujetando: {jugadoresSujetando.Count}");
    }

    public void OnPlayerExit(GameObject jugador)
    {
        jugadoresSujetando.Remove(jugador);
        Debug.Log($"[TransporteDoble] {jugador.name} soltó el objeto. Total sujetando: {jugadoresSujetando.Count}");
    }

    void FixedUpdate()
    {
        if (jugadoresSujetando.Count < jugadoresRequeridos)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direccionPromedio = Vector2.zero;
        int conDatos = 0;

        foreach (GameObject jugador in jugadoresSujetando)
        {
            if (movimientoDeCadaJugador.TryGetValue(jugador, out PlayerMovement mov) && mov != null)
            {
                direccionPromedio += mov.UltimoInput;
                conDatos++;
            }
        }

        if (conDatos > 0 && direccionPromedio.sqrMagnitude > 0.0001f)
        {
            direccionPromedio = (direccionPromedio / conDatos).normalized;
        }
        else
        {
            direccionPromedio = Vector2.zero;
        }

        rb.linearVelocity = direccionPromedio * velocidadTransporte;
        Debug.Log($"[TransporteDoble] Sosteniendo: {jugadoresSujetando.Count}/{jugadoresRequeridos}. Direccion promedio: {direccionPromedio}. Velocidad aplicada: {rb.linearVelocity}");
    }
}

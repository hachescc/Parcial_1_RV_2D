using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class DualPressurePlate : MonoBehaviour, IInteractable
{
    [Header("Cuántos jugadores se necesitan para activar")]
    public int jugadoresRequeridos = 2;

    [Header("Qué pasa cuando se activa / desactiva")]
    public UnityEvent alActivar;
    public UnityEvent alDesactivar;

    [Header("Progreso del nivel (se dispara UNA sola vez, la primera vez que se activa)")]
    public UnityEvent alResolverDefinitivamente;

    private bool yaFueResuelto;

    private HashSet<GameObject> jugadoresPresionando = new HashSet<GameObject>();
    private bool estaActiva;

    public void OnPlayerEnter(GameObject jugador)
    {
        jugadoresPresionando.Add(jugador);
        Debug.Log($"[PressurePlate] Entró: {jugador.name}. Total ahora: {jugadoresPresionando.Count}");
        RevisarEstado();
    }

    public void OnPlayerExit(GameObject jugador)
    {
        jugadoresPresionando.Remove(jugador);
        Debug.Log($"[PressurePlate] Salió: {jugador.name}. Total ahora: {jugadoresPresionando.Count}");
        RevisarEstado();
    }

    private void RevisarEstado()
    {
        bool debeEstarActiva = jugadoresPresionando.Count >= jugadoresRequeridos;
        Debug.Log($"[PressurePlate] RevisarEstado — actuales: {jugadoresPresionando.Count}, requeridos: {jugadoresRequeridos}, debeEstarActiva: {debeEstarActiva}, estaActiva: {estaActiva}");

        if (debeEstarActiva && !estaActiva)
        {
            estaActiva = true;
            Debug.Log("[PressurePlate] ¡ACTIVANDO!");
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
            Debug.Log("[PressurePlate] Desactivando");
            alDesactivar.Invoke();
        }
        else if (!debeEstarActiva && jugadoresPresionando.Count > 0)
        {
            // Hay gente parada ahí pero todavía no alcanza — avisar cuánto falta.
            int faltan = jugadoresRequeridos - jugadoresPresionando.Count;
            MensajesEnPantalla.Mostrar($"Falta{(faltan == 1 ? "" : "n")} {faltan} jugador{(faltan == 1 ? "" : "es")} más para activar el mecanismo");
        }
    }
}
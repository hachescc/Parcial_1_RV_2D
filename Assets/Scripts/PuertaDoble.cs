using UnityEngine;
using UnityEngine.Events;

public class PuertaDoble : MonoBehaviour
{
    [Header("Comportamiento")]
    [Tooltip("true = una vez que ambas placas coincidieron activas la primera vez, la puerta queda abierta para siempre (Puerta_G2, permanente). false = la puerta se abre/cierra en vivo según el estado actual de las dos placas (Puerta_G4, se cierra al soltar).")]
    public bool permanente = true;

    [Header("Qué pasa cuando se abre / se cierra la puerta")]
    public UnityEvent alAbrir;
    public UnityEvent alCerrar;

    [Header("Progreso del nivel (se dispara UNA sola vez, la primera vez que ambas coinciden)")]
    public UnityEvent alResolverDefinitivamente;

    private bool placaAActiva;
    private bool placaBActiva;
    private bool yaFueResuelto;
    private bool estaAbierta;

    public void PlacaA_Activada()
    {
        placaAActiva = true;
        RevisarEstado();
    }

    public void PlacaA_Desactivada()
    {
        placaAActiva = false;
        RevisarEstado();
    }

    public void PlacaB_Activada()
    {
        placaBActiva = true;
        RevisarEstado();
    }

    public void PlacaB_Desactivada()
    {
        placaBActiva = false;
        RevisarEstado();
    }

    private void RevisarEstado()
    {
        bool ambasActivas = placaAActiva && placaBActiva;
        Debug.Log($"[PuertaDoble] PlacaA: {placaAActiva}, PlacaB: {placaBActiva}, ambasActivas: {ambasActivas}, permanente: {permanente}");

        if (ambasActivas && !estaAbierta)
        {
            AbrirPuerta();
        }
        else if (!ambasActivas && estaAbierta && !permanente)
        {
            estaAbierta = false;
            Debug.Log("[PuertaDoble] Se soltó una de las dos placas: cerrando puerta.");
            alCerrar.Invoke();
        }

    }

    private void AbrirPuerta()
    {
        estaAbierta = true;
        Debug.Log("[PuertaDoble] ¡Ambas placas activas a la vez! Abriendo puerta.");
        alAbrir.Invoke();

        if (!yaFueResuelto)
        {
            yaFueResuelto = true;
            alResolverDefinitivamente.Invoke();
        }
    }
}

using UnityEngine;
using UnityEngine.Events;

public class LevelTimer : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float duracionNivel = 180f;
    [SerializeField] private LevelManager levelManager;

    [Header("Eventos de tensión (para conectar música/UI en el Inspector)")]
    public UnityEvent alLlegarMitad;
    public UnityEvent alLlegarCuarto;
    public UnityEvent alAgotarTiempo;

    private float tiempoRestante;
    private bool mitadDisparada;
    private bool cuartoDisparado;
    private bool timerActivo = true;

    private void Start()
    {
        ReiniciarTimer();
    }

    private void Update()
    {
        if (!timerActivo) return;

        tiempoRestante -= Time.deltaTime;
        float progresoRestante = tiempoRestante / duracionNivel;

        if (!mitadDisparada && progresoRestante <= 0.5f)
        {
            mitadDisparada = true;
            alLlegarMitad.Invoke();
            Debug.Log("[LevelTimer] Mitad del tiempo alcanzada.");
        }

        if (!cuartoDisparado && progresoRestante <= 0.25f)
        {
            cuartoDisparado = true;
            alLlegarCuarto.Invoke();
            Debug.Log("[LevelTimer] Cuarto del tiempo alcanzado.");
        }

        if (tiempoRestante <= 0f)
        {
            tiempoRestante = 0f;
            timerActivo = false;
            alAgotarTiempo.Invoke();
            Debug.Log("[LevelTimer] Tiempo agotado.");

            if (levelManager != null)
                levelManager.PerderIntento();
        }
    }

    public void ReiniciarTimer()
    {
        tiempoRestante = duracionNivel;
        mitadDisparada = false;
        cuartoDisparado = false;
        timerActivo = true;
    }

    public void DetenerTimer()
    {
        timerActivo = false;
    }

    public float TiempoRestante => tiempoRestante;
    public float DuracionNivel => duracionNivel;
}
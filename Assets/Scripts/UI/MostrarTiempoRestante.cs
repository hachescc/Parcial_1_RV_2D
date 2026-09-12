using UnityEngine;
using UnityEngine.UI;

// Muestra en pantalla el tiempo restante del LevelTimer (antes solo se veía en consola).
public class MostrarTiempoRestante : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private LevelTimer levelTimer;
    [SerializeField] private Text textoTiempo;

    void Update()
    {
        if (levelTimer == null || textoTiempo == null) return;

        float tiempo = Mathf.Max(0f, levelTimer.TiempoRestante);
        int minutos = Mathf.FloorToInt(tiempo / 60f);
        int segundos = Mathf.FloorToInt(tiempo % 60f);
        textoTiempo.text = $"{minutos:00}:{segundos:00}";
    }
}

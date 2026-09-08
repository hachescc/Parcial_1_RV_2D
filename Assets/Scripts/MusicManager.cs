using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    [Header("Pistas (ambas en loop, Spatial Blend = 0)")]
    [SerializeField] private AudioSource pistaCalma;
    [SerializeField] private AudioSource pistaTensa;

    [Header("Configuración")]
    [SerializeField] private float duracionCrossfade = 2f;

    private Coroutine crossfadeEnCurso;

    private void Start()
    {
        pistaCalma.volume = 1f;
        pistaTensa.volume = 0f;

        if (!pistaCalma.isPlaying) pistaCalma.Play();
        if (!pistaTensa.isPlaying) pistaTensa.Play();
    }

    public void SubirTensionMedia()
    {
        IniciarCrossfade(0.5f, 0.5f);
    }

    public void SubirTensionAlta()
    {
        IniciarCrossfade(0f, 1f);
    }

    public void Calmar()
    {
        IniciarCrossfade(1f, 0f);
    }

    private void IniciarCrossfade(float volCalmaDestino, float volTensaDestino)
    {
        if (crossfadeEnCurso != null) StopCoroutine(crossfadeEnCurso);
        crossfadeEnCurso = StartCoroutine(Crossfade(volCalmaDestino, volTensaDestino));
    }

    private IEnumerator Crossfade(float volCalmaDestino, float volTensaDestino)
    {
        float volCalmaInicio = pistaCalma.volume;
        float volTensaInicio = pistaTensa.volume;
        float t = 0f;

        while (t < duracionCrossfade)
        {
            t += Time.deltaTime;
            float progreso = t / duracionCrossfade;
            pistaCalma.volume = Mathf.Lerp(volCalmaInicio, volCalmaDestino, progreso);
            pistaTensa.volume = Mathf.Lerp(volTensaInicio, volTensaDestino, progreso);
            yield return null;
        }

        pistaCalma.volume = volCalmaDestino;
        pistaTensa.volume = volTensaDestino;
    }
}
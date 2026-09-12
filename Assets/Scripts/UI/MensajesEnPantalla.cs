using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Utilidad simple para mostrar mensajes cortos en pantalla en vez de solo en la
// Console (ej. "Mecanismo resuelto 3/6", "No tenés el rol correcto"). Cualquier
// script puede llamar MensajesEnPantalla.Mostrar("...") sin necesitar una
// referencia directa ni cablear un UnityEvent por cada instancia.
public class MensajesEnPantalla : MonoBehaviour
{
    private static MensajesEnPantalla instancia;

    [Header("Texto donde aparece el mensaje")]
    [SerializeField] private Text textoMensaje;

    [Header("Segundos que se queda un mensaje si no se especifica otro valor")]
    [SerializeField] private float duracionPorDefecto = 2.5f;

    private Coroutine ocultando;

    void Awake()
    {
        instancia = this;
        if (textoMensaje != null) textoMensaje.text = "";
    }

    void OnDestroy()
    {
        if (instancia == this) instancia = null;
    }

    public static void Mostrar(string mensaje, float duracion = -1f)
    {
        if (instancia == null)
        {
            Debug.LogWarning("[MensajesEnPantalla] No hay instancia en esta escena — mensaje perdido: " + mensaje);
            return;
        }
        instancia.MostrarInterno(mensaje, duracion);
    }

    private void MostrarInterno(string mensaje, float duracion)
    {
        if (textoMensaje == null) return;
        if (duracion < 0f) duracion = duracionPorDefecto;

        textoMensaje.text = mensaje;
        if (ocultando != null) StopCoroutine(ocultando);
        ocultando = StartCoroutine(OcultarDespuesDe(duracion));
    }

    private IEnumerator OcultarDespuesDe(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        textoMensaje.text = "";
    }
}

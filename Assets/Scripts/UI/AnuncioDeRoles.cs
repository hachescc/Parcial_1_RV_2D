using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

// Muestra en pantalla, apenas arranca el nivel, qué rol le tocó a cada jugador esta
// partida (los roles se reparten al azar cada vez, así que sin este anuncio nadie
// sabría qué panel le toca operar). Se oculta solo después de unos segundos.
public class AnuncioDeRoles : MonoBehaviour
{
    [Header("Texto donde se lista Jugador (Color): Rol")]
    [SerializeField] private Text textoRoles;

    [Header("Segundos que se queda visible el anuncio")]
    [SerializeField] private float duracionVisible = 15f;

    [Header("Se dispara cuando el anuncio se termina de ocultar (ej. arrancar el timer)")]
    public UnityEvent alTerminarAnuncio;

    public void MostrarRoles(List<AsignadorDeRoles> jugadores)
    {
        if (textoRoles == null)
        {
            // Red de seguridad: si por lo que sea el campo no quedó asignado en el
            // Inspector, lo busca solo entre sus hijos antes de rendirse.
            textoRoles = GetComponentInChildren<Text>(true);
            if (textoRoles != null)
            {
                Debug.LogWarning("[AnuncioDeRoles] 'Texto Roles' no estaba asignado en el Inspector — se encontró automáticamente en un hijo.");
            }
        }

        Debug.Log($"[AnuncioDeRoles] MostrarRoles() llamado con {jugadores.Count} jugadores. textoRoles={(textoRoles != null ? "asignado" : "NULL")}");

        if (textoRoles != null)
        {
            string contenido = "REPARTO DE ROLES\n\n";
            foreach (AsignadorDeRoles jugador in jugadores)
            {
                contenido += $"{jugador.NombreJugador} ({jugador.NombreColor}) — {jugador.DescripcionControles}\n{jugador.NombreRol}: {jugador.DescripcionRol}\n\n";
            }
            textoRoles.text = contenido;
            Debug.Log("[AnuncioDeRoles] Texto asignado:\n" + contenido);
        }
        else
        {
            Debug.LogError("[AnuncioDeRoles] El campo 'Texto Roles' no está asignado en el Inspector de este AnuncioDeRoles.");
        }

        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(OcultarDespuesDe(duracionVisible));
    }

    private IEnumerator OcultarDespuesDe(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        gameObject.SetActive(false);
        Debug.Log("[AnuncioDeRoles] Anuncio terminado, disparando alTerminarAnuncio (arranca el timer).");
        alTerminarAnuncio.Invoke();
    }
}

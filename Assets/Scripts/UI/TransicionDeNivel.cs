using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransicionDeNivel : MonoBehaviour
{
    [Header("Nombre exacto de la escena a cargar")]
    [SerializeField] private string escenaSiguiente;

    [Header("Segundos de espera antes de cambiar de escena")]
    [SerializeField] private float esperaSegundos = 3f;

    public void IrAEscenaConEspera()
    {
        StartCoroutine(Esperar());
    }

    private IEnumerator Esperar()
    {
        yield return new WaitForSeconds(esperaSegundos);
        SceneManager.LoadScene(escenaSiguiente);
    }
}
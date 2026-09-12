using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoriaController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panelVictoria;

    [Header("Escena del menú principal")]
    [SerializeField] private string escenaMenuPrincipal = "Menu";

    public void MostrarVictoria()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
        if (panelVictoria != null) panelVictoria.SetActive(true);
    }

    public void IrAMenuPrincipal()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene(escenaMenuPrincipal);
    }
}
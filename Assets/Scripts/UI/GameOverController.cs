using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panelGameOver;

    [Header("Escena del menú principal (debe existir en Build Settings)")]
    [SerializeField] private string escenaMenuPrincipal = "MainMenu";

    public void MostrarGameOver()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true; 
        if (panelGameOver != null) panelGameOver.SetActive(true);
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        Scene escenaActual = SceneManager.GetActiveScene();
        SceneManager.LoadScene(escenaActual.name);
    }

    public void IrAMenuPrincipal()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false; 
        SceneManager.LoadScene(escenaMenuPrincipal);
    }
}
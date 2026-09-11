using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject panelMenuPrincipal;
    [SerializeField] private GameObject panelSelectorNiveles;

    private void Start()
    {
        MostrarMenuPrincipal();
    }

    public void MostrarSelectorNiveles()
    {
        if (panelMenuPrincipal != null) panelMenuPrincipal.SetActive(false);
        if (panelSelectorNiveles != null) panelSelectorNiveles.SetActive(true);
    }

    public void MostrarMenuPrincipal()
    {
        if (panelSelectorNiveles != null) panelSelectorNiveles.SetActive(false);
        if (panelMenuPrincipal != null) panelMenuPrincipal.SetActive(true);
    }

    public void CargarNivel1()
    {
        CargarEscena("Nivel 1");
    }

    public void CargarNivel2()
    {
        CargarEscena("Nivel 2");
    }

    private void CargarEscena(string nombreEscena)
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(nombreEscena);
    }

    public void Salir()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
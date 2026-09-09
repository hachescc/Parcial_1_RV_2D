using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panelPausa;

    [Header("Escena del menú principal (debe existir en Build Settings)")]
    [SerializeField] private string escenaMenuPrincipal = "MainMenu";

    private InputAction accionPausa;
    private bool juegoPausado;

    private void Awake()
    {
        accionPausa = new InputAction("Pausa", binding: "<Keyboard>/escape");
        accionPausa.performed += _ => AlternarPausa();
    }

    private void OnEnable()
    {
        accionPausa.Enable();
    }

    private void OnDisable()
    {
        accionPausa.Disable();
    }

    private void OnDestroy()
    {
        accionPausa.Dispose();
    }

    private void AlternarPausa()
    {
        if (juegoPausado) Reanudar();
        else Pausar();
    }

    private void Pausar()
    {
        juegoPausado = true;
        Time.timeScale = 0f;
        AudioListener.pause = true; 
        if (panelPausa != null) panelPausa.SetActive(true);
    }

    public void Reanudar()
    {
        juegoPausado = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;
        if (panelPausa != null) panelPausa.SetActive(false);
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


    public void Salir()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
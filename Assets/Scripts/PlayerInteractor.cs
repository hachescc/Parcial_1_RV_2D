using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Detección de objetos interactivos")]
    public float interactRadius = 0.6f;
    public LayerMask interactableLayer;

    private IInteractable objetoActual;
    private GameObject objetoActualGO;
    private bool interactuando;

    void OnInteract(InputValue value)
    {
        interactuando = value.isPressed;
        Debug.Log($"[{gameObject.name}] OnInteract llamado. isPressed: {value.isPressed}. interactuando ahora: {interactuando}");
    }

    void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactRadius, interactableLayer);
        IInteractable detectado = hit != null ? hit.GetComponent<IInteractable>() : null;
        GameObject detectadoGO = hit != null ? hit.gameObject : null;

        if (objetoActual != null && objetoActual != detectado)
        {
            objetoActual.OnPlayerExit(gameObject);
            objetoActual = null;
            objetoActualGO = null;
        }

        if (detectado != null && interactuando)
        {
            if (objetoActual != detectado)
            {
                objetoActual = detectado;
                objetoActualGO = detectadoGO;
                objetoActual.OnPlayerEnter(gameObject);
            }
        }
        else if (objetoActual != null && !interactuando)
        {
            objetoActual.OnPlayerExit(gameObject);
            objetoActual = null;
            objetoActualGO = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
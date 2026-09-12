using UnityEngine;

public class DebugColorFlip : MonoBehaviour
{
    private SpriteRenderer sr;
    public Color colorActivo = Color.green;
    private Color colorOriginal;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        colorOriginal = sr.color;
    }

    public void Activar() => sr.color = colorActivo;
    public void Desactivar() => sr.color = colorOriginal;
}
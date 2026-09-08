using UnityEngine;

// Script temporal SOLO para probar visualmente que la placa funciona.
// Lo vas a reemplazar más adelante por la puerta/plataforma real.
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
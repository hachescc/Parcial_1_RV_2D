using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [Header("Jugadores a seguir")]
    public List<Transform> targets = new List<Transform>();

    [Header("Movimiento")]
    public float followSmoothSpeed = 5f;
    public Vector2 offset = new Vector2(0f, 1f);

    [Header("Zoom dinámico")]
    public float minZoom = 5f;
    public float maxZoom = 10f;
    public float zoomPadding = 3f;
    public float zoomSmoothSpeed = 4f;

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        targets.RemoveAll(t => t == null);

        if (targets.Count == 0) return;

        Bounds bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 1; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        Vector3 puntoCentral = bounds.center;
        Vector3 posicionDeseada = new Vector3(
            puntoCentral.x + offset.x,
            puntoCentral.y + offset.y,
            transform.position.z
        );
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, followSmoothSpeed * Time.deltaTime);

        float distanciaMax = Mathf.Max(bounds.size.x, bounds.size.y / cam.aspect);
        float zoomDeseado = Mathf.Clamp(distanciaMax / 2f + zoomPadding, minZoom, maxZoom);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomDeseado, zoomSmoothSpeed * Time.deltaTime);
    }
}
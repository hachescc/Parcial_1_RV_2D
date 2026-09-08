using UnityEngine;

public interface IInteractable
{
    void OnPlayerEnter(GameObject jugador);

    void OnPlayerExit(GameObject jugador);
}
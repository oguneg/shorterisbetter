using UnityEngine;

public interface IInteractable
{
    public void Interact();
    public Transform transform { get; }
}

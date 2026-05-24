using UnityEngine;

public interface IInteractable
{
    public void Interact();
    public Transform transform { get; }

    public void SetColliderStatus(bool isActive)
    {
        transform.GetComponent<Collider>().enabled = isActive;
    }
}

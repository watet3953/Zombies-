using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    private List<IInteractable> inRange;

    public IInteractable Closest { get; private set; } = null;

    public void Awake() => inRange = new List<IInteractable>();

    [HideInInspector] public bool currentlyInteracting = false;

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable;
        if (other.TryGetComponent<IInteractable>(out interactable)) 
        {
            inRange.Add(interactable);
            RecalculateClosest();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable;
        if (other.TryGetComponent<IInteractable>(out interactable))
        {
            inRange.Remove(interactable);
            if (currentlyInteracting) ForceEndInteraction();
            RecalculateClosest();
        }
    }

    public void Interact()
    {
        if (Closest == null || !Closest.IsInteractable()) return;
        Closest.StartInteraction();

    }

    public void ForceEndInteraction()
    {
        Closest.ForceEndInteraction();
    }


    public void RecalculateClosest()
    {
        float minDist = Mathf.Infinity;
        IInteractable closest = null;
        foreach (IInteractable i in inRange)
        {
            float distance = (i.GetGameObject().transform.position
                    - gameObject.transform.position).magnitude;
            if (distance < minDist)
            {
                distance = minDist;
                closest = i;
            }
        }
        Closest = closest;
    }
}

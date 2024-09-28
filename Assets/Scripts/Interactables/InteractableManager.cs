using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    private List<IInteractable> inRange;

    public IInteractable Closest { get; private set; } = null;

    public void Awake() => inRange = new List<IInteractable>();

    private Coroutine interactionCoroutine;
    private IInteractable interactionObject;


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
            if (CurrentlyInteracting()) ForceEndInteraction();
            RecalculateClosest();
        }
    }

    public void Interact()
    {
        if (Closest == null || !Closest.IsInteractable() || CurrentlyInteracting()) return;
        interactionCoroutine = Closest.GetSelf().StartCoroutine(
                Closest.StartInteraction());
        interactionObject = Closest;

    }

    public void ForceEndInteraction()
    {
        Debug.Log("Kill Interaction");
        if (CurrentlyInteracting()) Closest.ForceEndInteraction(interactionCoroutine);
        interactionCoroutine = null;
        interactionObject = null;
    }

    public bool CurrentlyInteracting()
    {
        if (interactionCoroutine == null) return false;
        return interactionObject.IsBeingInteractedWith();
    }

    public void RecalculateClosest()
    {
        float minDist = Mathf.Infinity;
        IInteractable closest = null;
        foreach (IInteractable i in inRange)
        {
            float distance = (i.GetSelf().transform.position
                    - gameObject.transform.position).magnitude;

            if (distance < minDist)
            {
                distance = minDist;
                closest = i;
            }
        }
        Closest?.Selected(false); // swap which one is selected
        Closest = closest;
        Closest?.Selected(true);
    }
}

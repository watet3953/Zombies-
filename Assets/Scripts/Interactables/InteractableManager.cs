using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    /// <summary> A list of all interactables currently in range. </summary>
    protected List<IInteractable> inRange;

    /// <summary> The closest interactable to the manager. </summary>
    public IInteractable Closest { get; protected set; }

    /// <summary> The coroutine of the active interaction. </summary>
    protected Coroutine interactionCoroutine;

    /// <summary> The object currently being interacted with. </summary>
    protected IInteractable interactionObject;

    public void Awake()
    {
        inRange = new List<IInteractable>();
        Closest = null;
    }

    protected void Update()
    {
        // NOTE: this may just fire every frame and be pointless.
        // If the transform of the manager has changed since last calculated
        if (transform.hasChanged)
        {
            transform.hasChanged = false;
            RecalculateClosest(); // recalculate closest
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        // If the entering body is interactable, add it to the list.
        IInteractable interactable;
        if (other.TryGetComponent<IInteractable>(out interactable))
        {
            inRange.Add(interactable);
            RecalculateClosest();
        }

    }

    protected void OnTriggerExit(Collider other)
    {
        // If the exiting body is interactable, remove it from the list.
        IInteractable interactable;
        if (other.TryGetComponent<IInteractable>(out interactable))
        {
            inRange.Remove(interactable);
            if (CurrentlyInteracting()) // If interacting with this, stop.
            {
                ForceEndInteraction();
            }
            RecalculateClosest();
        }
    }

    /// <summary> Starts an interaction with the current 
    /// closest interactable object within range. </summary>
    public void Interact()
    {
        // No closest object, give up.
        if (Closest == null) 
        {
            return;
        }

        // Not available to interact with, or currently busy, give up.
        if (!Closest.IsInteractable() || CurrentlyInteracting()) 
        {
            return;
        }

        interactionCoroutine = Closest.GetSelf().StartCoroutine(
                Closest.StartInteraction()
            );
        
        interactionObject = Closest;

    }

    /// <summary> Forcibly ends the current interaction. </summary>
    public void ForceEndInteraction()
    {
        Debug.Log("Kill Interaction");
        if (CurrentlyInteracting()) {
            Closest.ForceEndInteraction(interactionCoroutine);
        } 
        interactionCoroutine = null;
        interactionObject = null;
    }

    /// <summary> Checks if the manager is in an interaction. </summary>
    /// <returns> True if the manager is currently interacting. </returns>
    public bool CurrentlyInteracting()
    {
        if (interactionCoroutine == null) 
        {
            return false;
        }
        return interactionObject.IsBeingInteractedWith();
    }

    /// <summary> Recalculates the closest interactable. </summary>
    public void RecalculateClosest()
    {
        //Debug.Log("Recalculating Closest Interaction");
        float minDist = Mathf.Infinity;
        IInteractable closest = null;
        foreach (IInteractable i in inRange)
        {
            float distance = (
                    i.GetSelf().transform.position
                    - gameObject.transform.position
                ).magnitude;

            if (distance < minDist && i.IsInteractable())
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

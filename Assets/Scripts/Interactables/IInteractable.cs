using System.Collections;
using UnityEngine;

public interface IInteractable
{

    /// <summary> Gets the GameObject the interactable is on. </summary>
    /// <returns> The attached GameObject.</returns>
    public MonoBehaviour GetSelf();

    /// <summary> Checks if the interactable is currently 
    /// available to be interacted with. </summary>
    /// <returns> True if the interactable is able to 
    /// be interacted with. </returns>
    public bool IsInteractable();

    /// <summary> Toggles if the object should behave like
    /// it's being selected.  </summary>
    /// <param name="isSelected"> If true, is selected. </param>
    public void Selected(bool isSelected);

    /// <summary> Starts an interaction with this object,
    /// returning a coroutine that outputs signals related to 
    /// the interaction's state. </summary>
    /// <returns> A stream of the interaction state. </returns>
    public IEnumerator StartInteraction();

    /// <summary> Forces the interaction to end, the interaction
    /// coroutine will output InteractionSignals.ClosedForced
    /// when this occurs. </summary>
    public void ForceEndInteraction(Coroutine interactor);

    /// <summary> Checks if the interactable is currently 
    /// being interacted with. </summary>
    /// <returns> True if the interactable is being interacted with. </returns>
    public bool IsBeingInteractedWith();

    /// <summary> Outputs the text to display if able to 
    /// interact. if null then no display is needed. </summary>
    /// <returns> The displayed text, null if blank. </returns>
    public string GetInteractionText();
}

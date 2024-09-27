using System.Collections;
using UnityEngine;

public interface IInteractable 
{
    /// <summary> Signals that can be sent by the 
    /// Interactable Coroutine during an interaction. </summary>
    enum InteractionSignals {
        
        // continuing for another frame
        Continue,

        // Interaction completed and ended
        ClosedOK, 

        // Interaction failed on the interactable side
        ClosedFail,             
        
        // Interaction was force-closed by the player side
        ClosedForced,   
    }

    /// <summary> Gets the GameObject the interactable is on. </summary>
    /// <returns> The attached GameObject.</returns>
    public GameObject GetGameObject();

    /// <summary> Checks if the interactable is currently 
    /// available to be interacted with. </summary>
    /// <returns>True if able to be interacted with,
    /// false otherwise</returns>
    public bool IsInteractable();

    /// <summary> Starts an interaction with this object,
    /// returning a coroutine that outputs signals related to 
    /// the interaction's state. </summary>
    /// <returns> A stream of the interaction state. </returns>
    public IEnumerable StartInteraction();

    /// <summary> Forces the interaction to end, the interaction
    /// coroutine will output InteractionSignals.ClosedForced
    /// when this occurs. </summary>
    public void ForceEndInteraction();

    /// <summary> Outputs the text to display if able to 
    /// interact. if null then no display is needed. </summary>
    /// <returns> The displayed text, null if blank. </returns>
    public string GetInteractionText();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSwapper : MonoBehaviour, IInteractable
{
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private Material unselectedMaterial;

    [SerializeField] private ParticleSystem smoke;

    [SerializeField] private string targetMap;

    public void ForceEndInteraction(Coroutine interaction)
    {
        StopCoroutine(interaction);
    }

    public string GetInteractionText()
    {
        return "Swap Maps";
    }

    public MonoBehaviour GetSelf()
    {
        return this;
    }

    public bool IsBeingInteractedWith()
    {
        return false; // destroys itself immediately, basically always this will be false
    }

    public bool IsInteractable()
    {
        return true;
    }

    public void Selected(bool isSelected)
    {
        // modify the local version to confirm it is interactable
        isSelected &= IsInteractable();

        Debug.Assert(
                GetComponent<Renderer>() != null,
                "Could not find renderer for Map Swapper"
            );

        GetComponent<Renderer>().material =
                isSelected
                ? selectedMaterial
                : unselectedMaterial;
        if (isSelected) smoke.Play();
        else smoke.Stop();
    }

    public IEnumerator StartInteraction()
    {
        GameManager.Instance.StartCoroutine(GameManager.Instance.LoadMap(targetMap, true));
        yield break; // fake coroutine to appease the interface
    }
}

using System.Collections.Generic;
using UnityEngine;

public class FogHider : MonoBehaviour
{
    /// <summary> The camera the player views. </summary>
    [SerializeField] protected Camera playerCamera;

    /// <summary> The camera used for calculating the fog. </summary>
    [SerializeField] protected Camera fogCamera;

    /// <summary> The fog texture to use for lookup on a given frame. </summary>
    protected static Texture2D workingFogTexture;

    /// <summary> Does the working texture need updating? </summary>
    protected static bool needsUpdating = true;

    /// <summary> A list of the renderers modified by this instance of 
    /// the FogHider. </summary>
    protected Renderer[] affectedRenderers;

    protected void Start()
    {
        Debug.Assert(
                playerCamera != null,
                "No reference to the Player Camera found."
            );
        Debug.Assert(
                fogCamera != null,
                "No reference to the Fog Camera found."
            );
        RefreshRenderCache();
    }

    protected void Update() 
    {
        // updating static to ensure only reset once a frame
        needsUpdating = true; 
    }
    protected void LateUpdate()
    {
        UpdateVisibility();
    }

    /// <summary>
    /// Refreshes the cached list of renderers to check every frame. BE CAREFUL
    /// USING THIS, IT IS VERY PERFORMANCE HEAVY.
    /// </summary>
    private void RefreshRenderCache() 
    {
        List<Renderer> output = new();
        Queue<Transform> toCheck = new();

        toCheck.Enqueue(transform);

        while (toCheck.Count > 0) // iterative deep search
        { 

            Transform parent = toCheck.Dequeue();

            foreach(Transform child in parent) 
            {
                toCheck.Enqueue(child);
            }

            if (parent.TryGetComponent<Renderer>(out Renderer render)) 
            {
                output.Add(render);
            }
        }
        affectedRenderers = output.ToArray();
    }

    /// <summary> Set the object's visibility based on the fog. </summary>
    private void UpdateVisibility() {
        
        RenderTexture fogTexture = fogCamera.targetTexture;

        if (workingFogTexture == null) { // if no working fog texture, make one.
            workingFogTexture = new Texture2D(
                    fogTexture.width,
                    fogTexture.height, 
                    TextureFormat.R8, 
                    false);
        }

        if (needsUpdating) { // if not updated this frame, read in from camera.
            RenderTexture.active = fogTexture;
            workingFogTexture.ReadPixels(
                    new Rect(0,0, fogTexture.width, fogTexture.height),
                    0,
                    0);
            RenderTexture.active = null;
            needsUpdating = false;
        }

        foreach (Renderer render in affectedRenderers) { // check each render
            Vector3 pixel = fogCamera.WorldToScreenPoint(transform.position);
            float fogLevel = workingFogTexture.GetPixel(
                    (int)pixel.x,
                    (int)pixel.y
                ).r;
            render.enabled = fogLevel > 0.1f;
        }
    }
}

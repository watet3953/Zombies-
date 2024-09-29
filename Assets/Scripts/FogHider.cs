using System.Collections.Generic;
using UnityEngine;

public class FogHider : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Camera fogCamera;

    private static Texture2D workingFogTexture;

    /// <summary> Does the working texture need updating? </summary>
    private static bool needsUpdating = true;

    private Renderer[] affectedRenderers;

    void Start() => RefreshRenderCache();

    void Update() {
        // updating static to ensure only reset once a frame
        needsUpdating = true; 
    }
    void LateUpdate() => UpdateVisibility();

    /// <summary>
    /// Refreshes the cached list of renderers to check every frame. BE CAREFUL
    /// USING THIS, IT IS VERY PERFORMANCE HEAVY.
    /// </summary>
    private void RefreshRenderCache() {
        List<Renderer> output = new();
        Queue<Transform> toCheck = new();
        toCheck.Enqueue(transform);
        while (toCheck.Count > 0) {
            Transform parent = toCheck.Dequeue();
            foreach(Transform child in parent) toCheck.Enqueue(child);
            if (parent.TryGetComponent<Renderer>(out Renderer render)) {
                output.Add(render);
            }
        }
        affectedRenderers = output.ToArray();
    }

    private void UpdateVisibility() {
        
        RenderTexture fogTexture = fogCamera.targetTexture;

        if (workingFogTexture == null) {
            workingFogTexture = new Texture2D(
                    fogTexture.width,
                    fogTexture.height, 
                    TextureFormat.R8, 
                    false);
        }

        if (needsUpdating) {
            RenderTexture.active = fogTexture;
            workingFogTexture.ReadPixels(
                    new Rect(0,0, fogTexture.width, fogTexture.height),
                    0,
                    0);
            RenderTexture.active = null;
            needsUpdating = false;
        }
        foreach (Renderer render in affectedRenderers) {
            Vector3 pixel = fogCamera.WorldToScreenPoint(transform.position);
            float fogLevel = workingFogTexture.GetPixel(
                    (int)pixel.x,
                    (int)pixel.y).r;
            render.enabled = fogLevel > 0.1f;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreloadMask : MonoBehaviour
{
    // component references
    public GameObject mask;
    public SpriteRenderer spriteRenderer;

    // state variables
    private float alpha;
    private bool isAwake = false;

    void Update()
    {
        if (isAwake)
        {
            alpha -= (Time.deltaTime / 2);
            if (alpha <= 0)
            {
                Destroy(mask);
            }
            else
            {
                UpdateMaskColor();
            }
        }
    }

    public void WakeUp()
    {
        // set alpha slightly above 1 so the screen stays completely blank for a moment
        alpha = 1.1F;
        UpdateMaskColor();
        isAwake = true;
    }

    void UpdateMaskColor()
    {
        spriteRenderer.color = new Color(255, 255, 255, alpha);
    }
}

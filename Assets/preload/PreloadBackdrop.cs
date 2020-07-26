using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreloadBackdrop : MonoBehaviour
{
    // component references
    public GameObject backdrop;
    public GameObject cardCard;
    public GameObject cardGee;
    public GameObject cardPee;
    public PreloadMask preloadMask;
    public PreloadTitle preloadTitle;

    // state variables
    private bool triggered = false;

    void Update()
    {
        if  (!triggered && (Time.timeSinceLevelLoad > 3))
        {
            Explode();
        }
    }

    void Explode()
    {
        triggered = true;
    	preloadMask.WakeUp();
    	preloadTitle.WakeUp();
        Destroy(cardCard);
        Destroy(cardPee);
        Destroy(cardGee);
        Destroy(backdrop);
    }
}

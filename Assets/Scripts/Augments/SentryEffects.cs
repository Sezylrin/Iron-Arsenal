using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryEffects : MonoBehaviour
{
    // Start is called before the first frame update
    public Sentry hostSentry;

    public bool isDoubleProjectile = false;
    public bool isFireRate = false;
    public bool isRangeUp = false;
    private bool isRangeChanged = true;
    private bool isRateUp = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void UpdateAugments()
    {
        if (!isDoubleProjectile)
            isDoubleProjectile = AugmentManager.Instance.activeAugments.Contains(Augments.DoubleProjectiles); 
        if (!isFireRate)
            isFireRate = AugmentManager.Instance.activeAugments.Contains(Augments.FireRate); 
        if (!isRangeUp)
            isRangeUp = AugmentManager.Instance.activeAugments.Contains(Augments.RangeUp);
        UpdateSentryEffects();
    }

    public void UpdateSentryEffects()
    {
        if (isDoubleProjectile && !hostSentry.isDouble)
        {
            hostSentry.isDouble = true;
        }
        if (isFireRate && isRateUp)
        {
            hostSentry.fireRate *= 1.2f;
            isRateUp = false;
        }
        if (isRangeUp && isRangeChanged)
        {
            hostSentry.range *= 1.2f;
            isRangeChanged = false;
        }
    }
}

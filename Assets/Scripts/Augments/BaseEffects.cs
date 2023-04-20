using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEffects : MonoBehaviour
{
    // Start is called before the first frame update
    public BaseFunctions baseFunction;

    public bool isThorn = false;
    public bool isShieldRegenUp = false;
    public bool isMoreShield = false;
    private bool isShieldIncreased = true;
    private bool isRegenChanged = true;
    private bool isThornApplied = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateAugments()
    {
        if (!isThorn)
            isThorn = AugmentManager.Instance.activeAugments.Contains(Augments.Thorns);
        if (!isShieldRegenUp)
            isShieldRegenUp = AugmentManager.Instance.activeAugments.Contains(Augments.FasterShieldRegen);
        if (!isMoreShield)
            isMoreShield = AugmentManager.Instance.activeAugments.Contains(Augments.FasterShieldRegen);

        UpdateValues();
    }

    private void UpdateValues()
    {
        if (isThorn && isThornApplied)
        {
            baseFunction.collisionFactor *= 2;
            isThornApplied = false;
        }

        if(isShieldRegenUp && isRegenChanged)
        {
            baseFunction.shieldRecoverFactor *= 1.5f;
            baseFunction.shieldRecoverDelay *= 0.75f;
            baseFunction.maxShieldFactor *= 0.75f;
            isRegenChanged = false;
        }
        if(isMoreShield && isShieldIncreased)
        {
            baseFunction.shieldRecoverFactor *= 0.9f;
            baseFunction.shieldRecoverDelay *= 1.2f;
            baseFunction.maxShieldFactor *= 1.2f;
            isShieldIncreased = false;
        }
    }
}

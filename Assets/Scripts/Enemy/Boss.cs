using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossType
{
    Reaper,
    Breeder,
    Destroyer
};

public abstract class Boss : Enemy
{
    public BossType bossType;
    public int NumberOfPatterns { get; set; }
    [field: SerializeField]
    public int ActivePattern { get; set; }
    public float PatternEndTime { get; set; }
    public bool bPatternActive { get; set; }

    protected override void Init()
    {
        ActivePattern = 0;
        bPatternActive = false;
        base.Init();
    }

    protected virtual IEnumerator DelayChoosingPattern(int delay)
    {
        yield return new WaitForSeconds(delay);
        ChoosePattern();
    }
    
    protected virtual void ChoosePattern()
    {
        int random = Random.Range(1, NumberOfPatterns + 1);

        if (random == 1 && ActivePattern != 1)
        {
            ActivePattern = 1;
            return;
        }
        else if (random == 2 && ActivePattern != 2)
        {
            ActivePattern = 2;
            return;
        }
        else if (random == 3 && ActivePattern != 3)
        {
            ActivePattern = 3;
            return;
        }
        else if (random == 4 && ActivePattern != 4)
        {
            ActivePattern = 4;
            return;
        }
        else if (random == 5 && ActivePattern != 5)
        {
            ActivePattern = 5;
            return;
        }
        else if (random == 6 && ActivePattern != 6)
        {
            ActivePattern = 6;
            return;
        }
        else if (random == 7 && ActivePattern != 7)
        {
            ActivePattern = 7;
            return;
        }
        else if (random == 8 && ActivePattern != 8)
        {
            ActivePattern = 8;
            return;
        }
        else ChoosePattern();
    }

    public virtual IEnumerator PatternLength(int patternLength)
    {
        if (!bPatternActive)
        {
            bPatternActive = true;
            yield return new WaitForSeconds(patternLength);
            ActivePattern = 0;
            StartCoroutine(DelayNextPattern());
        }    
    }

    public virtual IEnumerator DelayNextPattern()
    {
        yield return new WaitForSeconds(3);
        ChoosePattern();
        bPatternActive = false;
    }


    public virtual void Heal(float healthToHeal)
    {
        if (CurrentHealth + healthToHeal > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        else CurrentHealth += healthToHeal;
    }
}
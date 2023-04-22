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
    [field: Header("Patterns")]
    [field: SerializeField] public int ActivePattern { get; set; }
    [field: SerializeField] public float LastPattern { get; set; }
    [field: SerializeField] public int NumberOfPatterns { get; set; }
    [field: SerializeField] public bool bPatternActive { get; set; }

    [field: Header("Boss Other")]
    public BossType bossType;

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

        if (random == 1 && LastPattern != 1)
        {
            ActivePattern = 1;
            LastPattern = 1;
            return;
        }
        else if (random == 2 && LastPattern != 2)
        {
            ActivePattern = 2;
            LastPattern = 2;
            return;
        }
        else if (random == 3 && LastPattern != 3)
        {
            ActivePattern = 3;
            LastPattern = 3;
            return;
        }
        else if (random == 4 && LastPattern != 4)
        {
            ActivePattern = 4;
            LastPattern = 4;
            return;
        }
        else if (random == 5 && LastPattern != 5)
        {
            ActivePattern = 5;
            LastPattern = 5;    
            return;
        }
        else if (random == 6 && LastPattern != 6)
        {
            ActivePattern = 6;
            LastPattern = 6;
            return;
        }
        else if (random == 7 && LastPattern != 7)
        {
            ActivePattern = 7;
            LastPattern = 7;
            return;
        }
        else if (random == 8 && LastPattern != 8)
        {
            ActivePattern = 8;
            LastPattern = 8;
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

    protected override void OnDeath()
    {
        Manager.BossDeath();
        base.OnDeath();
    }

    public virtual float GetPercentageHealthRemaining()
    {
        return CurrentHealth / MaxHealth;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossType
{
    Reaper,
    Destroyer,
    Breeder
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
    public ParticleSystem heal;

    protected bool canHeal;

    protected override void Init()
    {
        ActivePattern = 0;
        bPatternActive = false;
        canHeal = true;
        LevelCanvasManager.Instance.EnableBossHealthBar();
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
        LevelCanvasManager.Instance.SetBossHealthBar(GetPercentageHealthRemaining());
        heal.Play();
    }

    protected override void OnDeath()
    {
        Manager.BossDeath(transform);
        LevelCanvasManager.Instance.DisableBossHealthBar();
        base.OnDeath();
    }

    public virtual int GetPercentageHealthRemaining()
    {
        return Mathf.CeilToInt((CurrentHealth / MaxHealth) * 100);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        LevelCanvasManager.Instance.SetBossHealthBar(GetPercentageHealthRemaining());
    }

    protected virtual void PatternActivate(int ActivePattern)
    {
        switch (ActivePattern)
        {
            case 1:
                Pattern1();
                break; 
            case 2: 
                Pattern2(); 
                break;
            case 3: 
                Pattern3();
                break;
            case 4:
                Pattern4();
                break;
            case 5:
                Pattern5();
                break;
            case 6:
                Pattern6();
                break;
            case 7:
                Pattern7();
                break;
            case 8:
                Pattern8();
                break;
        }
    }

    protected virtual void WaitingForNextPattern()
    {
        canHeal = true;
    }

    protected virtual void Pattern1() //Common Move and Melee 
    {
        StartCoroutine(PatternLength(10));

        Move();
    }

    protected virtual void Pattern2() //Common Heal
    {
        StartCoroutine(PatternLength(3));

        if (canHeal)
        {
            Heal(MaxHealth / 20);
            canHeal = false;
        }
    }

    protected virtual void Pattern3() {} //Override
    protected virtual void Pattern4() {} //Override
    protected virtual void Pattern5() {} //Override
    protected virtual void Pattern6() {} //Override
    protected virtual void Pattern7() {} //Override
    protected virtual void Pattern8() {} //Override

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffects : MonoBehaviour
{
    // Start is called before the first frame update
    public Enemy hostenemy;

    public GameObject[] augmentPFList;

    public int fireTick = 0;

    public int poisonTick = 0;

    public int frozenAmount = 0;

    private int maxFreeze = 50;
    private float poisonFactor = 1;

    public bool callPoisonDamage = true;
    public bool callFireDamage = true;
    public bool isExplode = false;
    public bool isFireTrail = false;
    public bool isFireSpread = false;
    public bool isResistance = false;
    public bool isPoisonHealth = false;
    public bool isInstantDeath = false;
    public bool isFreezeDeath = false;
    public bool isFreezeShard = false;
    public bool isBossDamage = false;
    public bool isShieldSteal = false;
    public bool isLifeSteal = false;
    private bool isHealthDecrease = false;
    private bool isFrozen = false;
    private Vector3 lastTrailPoint;
    private float lastTrailDist = float.MaxValue;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckOnFire();
        CheckPoisoned();
        SpawnFireTrail();
        FreezeTarget();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isFireSpread && collision.gameObject.CompareTag("Enemy"))
        {
            SpreadFireTick(collision.gameObject.GetComponent<EnemyEffects>());
        }
    }
    public void SetAugmentState()
    {
        if (!isExplode)
            isExplode = AugmentManager.Instance.activeAugments.Contains(Augments.ExplosiveEnemy);
        if (!isFireTrail)
            isFireTrail = AugmentManager.Instance.activeAugments.Contains(Augments.FireTrail);
        if (!isFireSpread)
            isFireSpread = AugmentManager.Instance.activeAugments.Contains(Augments.FireSpread);
        if (!isResistance)
            isResistance = AugmentManager.Instance.activeAugments.Contains(Augments.DecreaseResistence);
        if (!isPoisonHealth)    
            isPoisonHealth = AugmentManager.Instance.activeAugments.Contains(Augments.PoisonMaxHealth);
        if (!isInstantDeath)
            isInstantDeath = AugmentManager.Instance.activeAugments.Contains(Augments.InstantDeath);
        if (!isFreezeDeath)
            isFreezeDeath = AugmentManager.Instance.activeAugments.Contains(Augments.FreezeDeath);
        if (!isFreezeShard)
            isFreezeShard = AugmentManager.Instance.activeAugments.Contains(Augments.FreezeShards);
        if (!isBossDamage)
            isBossDamage = AugmentManager.Instance.activeAugments.Contains(Augments.BossDamageIncrease);
         if (!isShieldSteal)
            isShieldSteal = AugmentManager.Instance.activeAugments.Contains(Augments.ShieldSteal);
         if (!isLifeSteal)
            isLifeSteal = AugmentManager.Instance.activeAugments.Contains(Augments.LifeSteal);
        callPoisonDamage = true;
        callFireDamage = true;
        isHealthDecrease = false;
        lastTrailDist = float.MaxValue;
        poisonFactor = 1;
        ResetFreeze();
    }

    public void FreezeTarget()
    {
        float freezeRate = (float)frozenAmount / (float)maxFreeze;
        if (!hostenemy.type.Equals(EnemyType.Boss))
            hostenemy.speedFactor = 1 - freezeRate;
        if (freezeRate > 0.5f && isFreezeDeath)
        {
            if (hostenemy.CurrentHealth <= hostenemy.MaxHealth * 0.2)
            {
                hostenemy.TakeDamage(hostenemy.CurrentHealth);
            }
        }
    }

    public void IncreaseFreeze(int amount)
    {
        if (isFrozen)
            return;
        if (frozenAmount + amount < maxFreeze)
        {
            frozenAmount += amount;
        }
        else
        {
            frozenAmount = maxFreeze;
            isFrozen = true;
            Invoke("ResetFreeze", 3);
        }
    }
    
    public void ResetFreeze()
    {
        frozenAmount = 0;
        isFrozen = false;
    }
    public void CheckPoisoned()
    {
        if (poisonTick > 0 && callPoisonDamage)
        {
            InvokeRepeating("TakePoisonDamage", 1, 1);
            callPoisonDamage = false;
        }
    }

    public void ReleaseShard()
    {
        Vector3 pos = transform.position;
        pos.y = 0.5f;
        Instantiate(augmentPFList[2], pos, Quaternion.identity).GetComponent<ASShardSpawner>().Init(frozenAmount/maxFreeze);
    }
    public void TakePoisonDamage()
    {
        poisonTick--;
        if (isPoisonHealth && !isHealthDecrease)
        {
            isHealthDecrease = true;
            hostenemy.TakeDamage(hostenemy.type.Equals(EnemyType.Boss) ? hostenemy.MaxHealth * 0.1f : hostenemy.MaxHealth * 0.2f);
        }
        if (isInstantDeath && Random.Range(1,101) <= 5 && !hostenemy.type.Equals(EnemyType.Boss))
        {
            hostenemy.TakeDamage(hostenemy.CurrentHealth);
        }
        hostenemy.TakeDamage(StatsManager.Instance.elementalDamage * 0.1f * poisonFactor);
        if(poisonFactor < 32)
            poisonFactor *= 2;
        if (poisonTick <= 0)
        {
            CancelInvoke("TakePoisonDamage");
            callPoisonDamage = true;
            poisonTick = 0;
            poisonFactor = 1;
        }
    }

    public void CheckOnFire()
    {
        if (fireTick > 0 && callFireDamage)
        {
            InvokeRepeating("TakeFireDamage", 0.25f, 0.25f);
            callFireDamage = false;
        }
    }
    private void TakeFireDamage()
    {
        fireTick -= 2;
        if (isResistance && !hostenemy.damageFactor.Equals(1.2f))
        {
            hostenemy.damageFactor = 1.2f;
        }
        hostenemy.TakeDamage(StatsManager.Instance.elementalDamage * 0.5f);
        if (fireTick <= 0)
        {
            CancelInvoke("TakeFireDamage");
            callFireDamage = true;
            fireTick = 0;
        }
    }

    public void SpreadFireTick(EnemyEffects other)
    {
        int amount = Mathf.FloorToInt(fireTick * 0.5f);
        if (other.fireTick < amount)
        {
            other.fireTick = amount;
        }
    }

    public void SpawnFireTrail()
    {
        if (!isFireSpread || fireTick <= 5)
            return;
        Vector3 pos = transform.position;
        pos.y = 0;
        if (lastTrailDist > 1)
        {
            Instantiate(augmentPFList[1], pos, Quaternion.identity).GetComponent<ASFireTrail>().Init(gameObject);
            lastTrailPoint = pos;
        }
        lastTrailDist = Vector3.Distance(lastTrailPoint, pos);
    }
}

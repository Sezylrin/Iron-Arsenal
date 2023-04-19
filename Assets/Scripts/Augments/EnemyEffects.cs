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
    private float poisonFactor = 1;

    public bool callPoisonDamage = true;
    public bool callFireDamage = true;
    public bool isExplode = false;
    public bool isFireTrail = false;
    public bool isFireSpread = false;
    public bool isResistance = false;
    public bool isPoisonHealth = false;
    public bool isInstantDeath = false;
    private bool isHealthDecrease = false;
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
        callPoisonDamage = true;
        callFireDamage = true;
        isHealthDecrease = false;
        lastTrailDist = float.MaxValue;
        poisonFactor = 1;
    }

    public void CheckPoisoned()
    {
        if (poisonTick > 0 && callPoisonDamage)
        {
            InvokeRepeating("TakePoisonDamage", 1, 1);
            callPoisonDamage = false;
        }
    }

    public void TakePoisonDamage()
    {
        poisonTick--;
        if (isPoisonHealth && !isHealthDecrease)
        {
            isHealthDecrease = true;
            hostenemy.TakeDamage(hostenemy.MaxHealth * 0.2f);
        }
        if (isInstantDeath && Random.Range(1,101) <= 5)
        {
            hostenemy.TakeDamage(hostenemy.CurrentHealth);
        }
        hostenemy.TakeDamage(StatsManager.Instance.elementalDamage * 0.1f * poisonFactor);
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
        if (other.fireTick < (int)(fireTick * 0.5f))
        {
            other.fireTick = (int)(fireTick * 0.5f);
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

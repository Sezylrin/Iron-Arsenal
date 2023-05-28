using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Enemy
{
    private bool ableToCharge;
    private bool charging = false;
    private Vector3 chargeDirection;
    public Animator anim;
    public ParticleSystem blueFlame;

    // Start is called before the first frame update
    void Start()
    {
        ableToCharge = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        CheckEffectState();
        SetRotation();

        if (!charging)
        {
            Move();
        }

        if (Vector3.Distance(Player.transform.position, transform.position) < 10 && ableToCharge)
        {
            ableToCharge = false;
            StartCoroutine(StartChargingUp());
        }

        /*if (Vector3.Distance(Player.transform.position, transform.position) > 20)
        {
            StopCoroutine(StartChargingUp());
            ableToCharge = true;
        }*/
    }

    IEnumerator StartChargingUp()
    {
        anim.SetTrigger("PrepCharge");
        charging = true;
        yield return new WaitForSeconds(2);
        StartCoroutine(Charge());
        charging = false;
    }

    IEnumerator Charge()
    {
        anim.SetTrigger("Charge");
        blueFlame.Play();
        chargeDirection = (Player.transform.position - transform.position).normalized;
        EnemyRB.AddForce(chargeDirection * 1500);
        Invoke("Walk", 1);
        yield return new WaitForSeconds(3);
        ableToCharge = true;
    }

    public void Walk()
    {
        anim.SetTrigger("Walk");
        blueFlame.Stop();
    }
}

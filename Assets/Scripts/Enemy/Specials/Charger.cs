using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Enemy
{
    private bool ableToCharge;
    private Vector3 chargeDirection;
    public Animator anim;

    void Awake()
    {
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        ableToCharge = true;
    }

    // Update is called once per frame
    void Update()
    {
        SetRotation();

        if (Vector3.Distance(Player.transform.position, transform.position) > 15)
        {
            Move();
        }

        if (Vector3.Distance(Player.transform.position, transform.position) < 15 && ableToCharge)
        {
            ableToCharge = false;
            StartCoroutine(StartChargingUp());
        }

        if (Vector3.Distance(Player.transform.position, transform.position) > 20)
        {
            StopCoroutine(StartChargingUp());
            ableToCharge = true;
        }
    }

    IEnumerator StartChargingUp()
    {
        anim.SetTrigger("PrepCharge");
        yield return new WaitForSeconds(2);
        StartCoroutine(Charge());
    }

    IEnumerator Charge()
    {
        anim.SetTrigger("Charge");
        chargeDirection = (Player.transform.position - transform.position).normalized;
        EnemyRB.AddForce(chargeDirection * 1000);
        yield return new WaitForSeconds(3);
        ableToCharge = true;
        anim.SetTrigger("Walk");
    }
}

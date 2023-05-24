using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digger : Enemy
{
    public BoxCollider enemyBC;
    public bool digging;

    // Start is called before the first frame update
    void Start()
    {
        digging = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        CheckEffectState();
        SetRotation();
        Move();

        if (Vector3.Distance(Player.transform.position, transform.position) > 15 && !digging)
        {
            digging = true;
            StartCoroutine(DigDown());
        }

        if (Vector3.Distance(Player.transform.position, transform.position) < 15 && digging)
        {
            digging = false;
            StartCoroutine(DigUp());
        }
    }

    IEnumerator DigDown()
    {
        StopCoroutine(DigUp());
        enemyBC.enabled = false;
        while (transform.position.y >= -2.5) 
        {
            yield return new WaitForSeconds(0.01f);
            transform.Translate(0, -0.03f, 0);
        }
    }

    IEnumerator DigUp()
    {
        StopCoroutine(DigDown());
        while (transform.position.y <= 0)
        {
            yield return new WaitForSeconds(0.01f);
            transform.Translate(0, 0.03f, 0);
        }
        enemyBC.enabled = true;
    }
}

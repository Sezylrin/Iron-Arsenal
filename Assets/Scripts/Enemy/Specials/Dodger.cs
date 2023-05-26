using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodger : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Dodge());
    }

    IEnumerator Dodge()
    {
        while (true)
        {
            int randomDelay = Random.Range(1, 6);
            yield return new WaitForSeconds(randomDelay);
            int random = Random.Range(1, 3);
            if (random == 1)
            {
                EnemyRB.AddForce(transform.right * 750);
            }
            else EnemyRB.AddForce(transform.right * -750);
        }
    }
}

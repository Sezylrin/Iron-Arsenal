using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : Enemy
{
    public GameObject basicEnemy;

    void Awake()
    {
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetRotation();
        Move();
    }

    protected override void OnDeath()
    {
        int numberToSpawn = 2;
        GameObject newEnemy = null;
        int positionModifier = -1;

        while (numberToSpawn > 0)
        {
            if (Manager.pools[0].ListCount() > 0)
            {
                newEnemy = Manager.pools[0].FirstObj();
                newEnemy.SetActive(true);
                Manager.pools[0].RemoveObj(newEnemy);
            }
            else
            {
                newEnemy = Instantiate(basicEnemy, Manager.transform);
            }
            Enemy enemyScript = newEnemy.GetComponent<Enemy>();
            enemyScript.CurrentHealth = enemyScript.MaxHealth;

            newEnemy.transform.position = new Vector3(transform.position.x + positionModifier, transform.position.y, transform.position.z + positionModifier);

            positionModifier += 2;
            numberToSpawn--;
        } 
        base.OnDeath();
    }
}

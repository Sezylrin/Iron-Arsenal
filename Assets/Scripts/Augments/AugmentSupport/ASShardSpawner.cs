using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASShardSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject iceShard;
    private float maxShard = 8;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(float frozenRate)
    {
        int ShardsToSpawn = Mathf.CeilToInt(maxShard * frozenRate);
        for (int i = 0; i < ShardsToSpawn; i++)
        {
            Instantiate(iceShard, transform.position, Quaternion.identity).GetComponent<ASIceShards>().Init(RandomPointOnCircleEdge(1));
        }
        Destroy(gameObject);
    }

    private Vector3 RandomPointOnCircleEdge(float radius)
    {
        var vector2 = Random.insideUnitCircle.normalized * radius;
        return new Vector3(vector2.x, 0, vector2.y);
    }

}

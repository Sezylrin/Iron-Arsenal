using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberManager : MonoBehaviour
{
    public static NumberManager Instance { get; private set; }
    public Pooling textPool = new Pooling();
    public GameObject textObj;
    private Transform cam;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }

    }

    private void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnText(Vector3 pos, string number, float scale, Color color)
    {
        GameObject tmpText;
        if (textPool.ListCount() > 0)
        {
            tmpText = textPool.FirstObj();
            tmpText.SetActive(true);
            textPool.RemoveObj(tmpText);
        }
        else
        {
            tmpText = Instantiate(textObj);
        }
        DamageNumbers tempNum = tmpText.GetComponent<DamageNumbers>();
        tempNum.Init(pos, number, scale, cam, color);
        tmpText.transform.parent = transform;
    }

    public void PoolObj(GameObject obj)
    {
        textPool.AddObj(obj);
        obj.SetActive(false);
    }
}

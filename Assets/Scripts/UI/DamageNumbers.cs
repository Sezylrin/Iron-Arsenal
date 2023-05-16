using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumbers : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text tmpText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(Vector3 pos, string text, float scale, Transform cam, Color color)
    {
        transform.position = pos;
        tmpText.text = text;
        tmpText.color = color;
        transform.localScale *= scale;
        transform.LookAt((transform.position - cam.position) + transform.position, Vector3.up);
        Invoke("PoolSelf", 0.5f);
    }

    public void PoolSelf()
    {
        NumberManager.Instance.PoolObj(gameObject);
    }
}

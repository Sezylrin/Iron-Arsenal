using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDeletion : MonoBehaviour
{
    // Start is called before the first frame update
    private SentrySocket socket;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeleteTurret()
    {
        socket.DeleteTurret();
    }

    public void SetSocket(SentrySocket socket)
    {
        this.socket = socket;
    }

    private void DestroySelf()
    {
        Destroy(this.gameObject);

    }

    public void InvokeDestroy()
    {
        Invoke("DestroySelf", 1f);
    }
}

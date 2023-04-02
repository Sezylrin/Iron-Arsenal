using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDeletion : MonoBehaviour
{
    // Start is called before the first frame update
    private TurretSocket socket;
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

    public void SetSocket(TurretSocket socket)
    {
        this.socket = socket;
    }
}

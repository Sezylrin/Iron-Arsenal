using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretBuildMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject turretButton;

    private TurretSocket currentSocket;

    public GameObject TestTurret;

    private List<GameObject> availableTurrets = new List<GameObject>();

    private Button[] allButtons;

    private int unlockedTurrets = 0;
    void Start()
    {
        
        if (TestTurret)
            availableTurrets.Add(TestTurret);
        allButtons = GetComponentsInChildren<Button>();
        AddListeners();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdateMenu();
        }
    }
    public void UpdateMenu()
    {
        GameObject tempButton = Instantiate(turretButton, transform.position + (Vector3.forward * 3), Quaternion.identity,transform);
        tempButton.transform.RotateAround(transform.position, Vector3.up, 90 * unlockedTurrets);
        tempButton.transform.Rotate(new Vector3(90, 0, 0));
        //Vector3 TempRot = tempButton.transform.eulerAngles;
        //TempRot.x = 0;
        //tempButton.transform.eulerAngles = TempRot;

        unlockedTurrets++;
    }
    private void AddListeners()
    {
        Debug.Log("Adding Listeners");
        for (int i = 0; i < allButtons.Length; i++)
        {
            int cpy = i;
            allButtons[i].onClick.AddListener(() => SpawnButton(cpy));
        }
    }
    public void SpawnButton(int assignedTurret)
    {
        //add check condition
        if(availableTurrets.Count > assignedTurret)
            SetTurret(availableTurrets[assignedTurret]);
    }

    public void SetSocket(TurretSocket Socket)
    {
        currentSocket = Socket;
    }

    public void SetTurret(GameObject TurretPrefab)
    {
        currentSocket.SetTurret(TurretPrefab);
    }

  


}

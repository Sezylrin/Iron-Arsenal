using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretBuildMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject turretButton;

    private TurretSocket currentSocket;

    public GameObject SentryPF;

    public Button[] switchMenu;

    public List<SentryData> availableTurrets = new List<SentryData>();

    private Button[] allButtons;

    public List<GameObject> buttonGroup = new List<GameObject>();

    public int currentPage = 0;

    public int unlockedTurrets = 0;

    private bool initArrow = true;
    void Start()
    {
        //Debug.Log("AvailableTurrets:" + availableTurrets.Count + " buttonMenus:" + buttonMenus.Count + " navigationButton:" + navigationButton.Count + " buttonGroup:" + buttonGroup.Count);
        /*if (SentryPF)
            availableTurrets.Add(SentryPF);*/
        Debug.Log(unlockedTurrets);
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
        GameObject tempButton = Instantiate(turretButton, transform.position + (Vector3.forward * 3), Quaternion.identity);
        //tempButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.01f;
        tempButton.transform.RotateAround(transform.position, Vector3.up, 90 * unlockedTurrets);
        tempButton.transform.Rotate(new Vector3(90, 0, 0));
        GroupButton(tempButton);

        //Vector3 TempRot = tempButton.transform.eulerAngles;
        //TempRot.x = 0;
        //tempButton.transform.eulerAngles = TempRot;

        unlockedTurrets++;
    }
    private void AddListeners()
    {
        for (int i = 0; i < allButtons.Length; i++)
        {
            int cpy = i;
            if(allButtons[i].tag == "TurretButton")
                allButtons[i].onClick.AddListener(() => SpawnButton(cpy));
        }
    }
    public void SpawnButton(int assignedTurret)
    {
        //add check condition
        if(availableTurrets.Count > assignedTurret)
            SetTurret(availableTurrets[assignedTurret]);
    }

    private void GroupButton(GameObject newButton)
    {
        
        if (unlockedTurrets % 4 == 0)
        {
            GameObject group = new GameObject("Button Group");
            group.transform.SetParent(transform);
            buttonGroup.Add(group);
            if (buttonGroup.Count > 1)
            {
                group.SetActive(false);
            }
        }
        newButton.tag = "TurretButton";
        newButton.transform.SetParent(buttonGroup[unlockedTurrets / 4].transform);
        if (unlockedTurrets >= 4 && initArrow)
        {
            initArrow = !initArrow;
            switchMenu[1].gameObject.SetActive(true);
        }
    }

    public void MoveMenuRight()
    {
        Debug.Log(buttonGroup.Count + " " + currentPage);
        buttonGroup[currentPage].SetActive(false);
        currentPage++;
        buttonGroup[currentPage].SetActive(true);
        Debug.Log(((unlockedTurrets - 1) / 4));
        if (currentPage == ((unlockedTurrets - 1) / 4))
        {
            switchMenu[1].gameObject.SetActive(false);
        }
        switchMenu[0].gameObject.SetActive(true);
    }

    public void MoveMenuLeft()
    {
        buttonGroup[currentPage].SetActive(false);
        currentPage--;
        buttonGroup[currentPage].SetActive(true);
        if (currentPage == 0)
        {
            switchMenu[0].gameObject.SetActive(false);
        }
        switchMenu[1].gameObject.SetActive(true);
    }
    public void SetSocket(TurretSocket Socket)
    {
        currentSocket = Socket;
    }

    public void SetTurret(SentryData data)
    {
        currentSocket.SetTurret(data);
    }

    public void AddToMenu(SentryData data)
    {
        availableTurrets.Add(data);
        UpdateMenu();
    }

    public void SetValue(List<SentryData> data, int unlockedTurrets)
    {
        //switchMenu = data.switchMenu;
        availableTurrets = data;
        //buttonGroup = data.buttonGroup;
        this.unlockedTurrets = unlockedTurrets;
    }


}

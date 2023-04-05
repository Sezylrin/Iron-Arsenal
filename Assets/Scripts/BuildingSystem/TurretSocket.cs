using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurretSocket : MonoBehaviour
{
    private GameObject turret;

    public GameObject sentryPF;

    private GameObject buildMenu;

    private GameObject deletionMenu;

    private TurretBuildMenu buildMenuScript;

    private TurretDeletion deletionScript;

    private GameObject menuTemp;

    private LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        if (!buildMenu)
            buildMenu = GameObject.FindWithTag("UISelection");
        if (!buildMenu)
        {
            Debug.Break();
            Debug.Log("Place a UISelection Prefab");
        }
        buildMenuScript = buildMenu.GetComponent<TurretBuildMenu>();
        if (!deletionMenu)
            deletionMenu = GameObject.FindWithTag("UIDeletion");
        if (!deletionMenu)
        {
            Debug.Break();
            Debug.Log("Place a UIDeletion Prefab");
        }
        mask = 512;
    }

    // Update is called once per frame
    void Update()
    {
        //add build state check condition
        if (Input.GetMouseButtonDown(0) && LevelManager.Instance.currentState == State.Building)
        {
            Vector3 MousePos = MousePosition.MouseToWorld3D(Camera.main, -1);
            MousePos.y = transform.position.y;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, mask) && menuTemp == null && hit.collider.gameObject == gameObject)
            {
                //play animation for ui
                if (menuTemp == null && turret == null)
                {
                    buildMenu.SetActive(true);
                    menuTemp = Instantiate(buildMenu, transform.position + Vector3.up, Quaternion.identity);
                    TurretBuildMenu temp = menuTemp.GetComponent<TurretBuildMenu>();
                    temp.SetSocket(this);
                    temp.SetValue(buildMenuScript.availableTurrets,buildMenuScript.unlockedTurrets);
                }
                else
                {
                    menuTemp = Instantiate(deletionMenu, transform.position + Vector3.up, Quaternion.identity);
                    menuTemp.GetComponent<TurretDeletion>().SetSocket(this);
                }
                menuTemp.transform.Rotate(new Vector3(90, 0, 0));
            }
            else if (Vector3.Distance(MousePos, transform.position) >= (menuTemp != null && turret == null? 4.5f * buildMenuScript.scale : 1.5f))
            {
                RemoveMenu();
               //deletionMenu.transform.Translate(Vector3.up * 100);
            }
        }
        if (LevelManager.Instance.currentState == State.Normal && menuTemp)
        {
            RemoveMenu();
        }
    }

    public void SetTurret(SentryData data)
    {
        if(turret == null)
        {
            turret = Instantiate(sentryPF, transform.position, Quaternion.identity, transform);
            turret.GetComponent<Sentry>().SetData(data);
        }
        RemoveMenu();
    }

    private void RemoveMenu()
    {
        //play leaving animation for ui
        if (!menuTemp)
            return;
        Destroy(menuTemp);
    }

    public void DeleteTurret()
    {
        Destroy(turret);
        RemoveMenu();
    }


}

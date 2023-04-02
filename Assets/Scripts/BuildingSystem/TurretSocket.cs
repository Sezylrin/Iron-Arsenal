using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurretSocket : MonoBehaviour
{
    private GameObject turret;

    public GameObject turretPrefab;

    private GameObject buildMenu;

    private GameObject deletionMenu;

    private TurretBuildMenu buildMenuScript;

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
        if (!deletionMenu)
            deletionMenu = GameObject.FindWithTag("UIDeletion");
        if (!buildMenu)
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
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 MousePos = MousePosition.MouseToWorld3D(Camera.main, -1);
            MousePos.y = transform.position.y;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, mask) && menuTemp == null && hit.collider.gameObject == gameObject)
            {
                //play animation for ui
                if (menuTemp == null && turret == null)
                {
                    menuTemp = Instantiate(buildMenu, transform.position + Vector3.up, Quaternion.identity);
                    menuTemp.transform.Rotate(new Vector3(90, 0, 0));
                    menuTemp.GetComponent<TurretBuildMenu>().SetSocket(this);
                }
                else
                {
                    menuTemp = Instantiate(deletionMenu, transform.position + Vector3.up, Quaternion.identity);
                    menuTemp.transform.Rotate(new Vector3(90, 0, 0));
                    menuTemp.GetComponent<TurretDeletion>().SetSocket(this);
                }
            }
            else if (Vector3.Distance(MousePos, transform.position) >= (menuTemp != null && turret == null? 3.5f : 1.5f))
            {
                //Debug.Log("Running");
                RemoveMenu();
            }
        }
    }

    public void SetTurret(GameObject turret)
    {
        if(this.turret == null)
            this.turret = Instantiate(turret, transform.position, Quaternion.identity, transform);
        RemoveMenu();
    }

    private void RemoveMenu()
    {
        //play leaving animation for ui
        Destroy(menuTemp);
    }

    public void DeleteTurret()
    {
        Destroy(turret);
        RemoveMenu();
    }


}

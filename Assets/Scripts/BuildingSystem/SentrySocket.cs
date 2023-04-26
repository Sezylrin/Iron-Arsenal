using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SentrySocket : MonoBehaviour
{
    private GameObject sentry;

    public GameObject sentryPF;

    public MeshRenderer meshRender;

    public Material mat;
    // Start is called before the first frame update
    void Start()
    {
        /*if (!buildMenu)
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
        }*/
        LevelCanvasManager.Instance.allSockets.Add(this);
        meshRender = GetComponentInChildren<MeshRenderer>();
        mat = meshRender.material;
        
    }

    // Update is called once per frame
    void Update()
    {
        //add build state check condition

    }

    public void SetSentry(SentryData data)
    {
        if (sentry == null)
        {
            sentry = Instantiate(sentryPF, transform.position, Quaternion.identity, transform);
            sentry.GetComponent<Sentry>().SetData(data);
        }
        LevelCanvasManager.Instance.CloseBuildMenu();
    }

    public void DeleteTurret()
    {
        Sentry sentryComp = sentry.GetComponent<Sentry>();
        sentryComp.DestroyPool();
        AugmentManager.Instance.activeSentries.Remove(sentryComp);
        Destroy(sentry);
        LevelCanvasManager.Instance.CloseBuildMenu();
    }

    public bool HasSentry()
    {
        return sentry;
    }
}
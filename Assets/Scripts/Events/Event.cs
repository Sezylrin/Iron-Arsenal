using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MiningOutpostType
{
    Novacite,
    Voidstone
};

public abstract class Event : MonoBehaviour
{
    [field: SerializeField] public int LengthInSeconds { get; set; }
    [field: SerializeField] public bool EndCondition { get; set; }
    [field: SerializeField] public bool CanStart { get; set; }
    [field: SerializeField] public bool Active { get; set; }
    public GameObject trigger;
    public Canvas canvas;

    private Transform cameraTransform;

    protected virtual void Init()
    {
        CanStart = false;
        EndCondition = false;
        cameraTransform = GameObject.Find("Main Camera").transform;
    }

    protected virtual void Begin()
    {
        if (!EventManager.Instance.EventActive)
        {
            trigger.SetActive(false);
            EventManager.Instance.EventActive = true;
            Active = true;
            StartCoroutine(BeginTimer());
        }
    }

    protected virtual IEnumerator BeginTimer()
    {
        if (LengthInSeconds > 0)
        {
            yield return new WaitForSeconds(LengthInSeconds);
        }
        else
        {
            while (!EndCondition)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
        End();
    }

    protected virtual void End()
    {
        EventManager.Instance.EventActive = false;
        Active = false;
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CanStart = true;
            canvas.enabled = true;
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            OrientCanvasToCamera();
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CanStart = false;
            canvas.enabled = false;
        }
    }

    protected virtual void CheckBeginInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanStart && !EventManager.Instance.EventActive)
        {
            Begin();
            canvas.enabled = false;
        }
    }

    private void OrientCanvasToCamera()
    {
        if (!canvas.enabled) return;
        canvas.transform.rotation = Quaternion.LookRotation(transform.position - new Vector3(transform.position.x,
                                                                                            cameraTransform.position.y,
                                                                                            cameraTransform.position.z));
    }
}

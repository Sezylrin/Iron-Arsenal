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
    [field: SerializeField] public bool Condition { get; set; }
    [field: SerializeField] public bool CanStart { get; set; }
    [field: SerializeField] public bool Active { get; set; }
    public GameObject trigger;

    protected virtual void Init()
    {
        CanStart = false;
        Condition = false;
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
            while (!Condition)
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
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CanStart = false;
        }
    }

    protected virtual void CheckBeginInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanStart && !EventManager.Instance.EventActive)
        {
            Begin();
        }
    }
}

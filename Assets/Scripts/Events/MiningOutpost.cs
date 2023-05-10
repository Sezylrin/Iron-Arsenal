using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningOutpost : Event
{
    public MiningOutpostType type;

    public GameObject miningArea;

    // Start is called before the first frame update

    void Start()
    {
        Init();
        miningArea.SetActive(false);
    }

    void Update()
    {
        CheckBeginInput();
    }

    protected override void Begin()
    {
        miningArea.SetActive(true);
        EnemyManager.Instance.StartRush();
        StartCoroutine(Mine());
        base.Begin();
    }

    protected override void End()
    {
        miningArea.SetActive(false);
        base.End();
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CanStart = false;
            canvas.enabled = false;

            if (Active)
            {
                EndCondition = true;
            }
        }
    }

    private IEnumerator Mine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            int random = Random.Range(5, 11);
            if (type == MiningOutpostType.Novacite)
            {
                LevelManager.Instance.GainNovacite(random);
            }
            else if (type == MiningOutpostType.Voidstone)
            {
                LevelManager.Instance.GainVoidStone(random);
            }
        }
    }
}

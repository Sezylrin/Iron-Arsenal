using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MiningOutpost : Event
{
    public MiningOutpostType type;

    public GameObject miningArea;

    private Mining miningScript;

    public Animator anim;

    [field: SerializeField] private TextMeshProUGUI text { get; set; }

    // Start is called before the first frame update

    void Start()
    {
        Init();
        miningArea.SetActive(false);
        miningScript = LevelManager.Instance.player.GetComponent<Mining>();
    }

    void Update()
    {
        CheckBeginInput();
    }

    protected override void Begin()
    {
        anim.SetTrigger("Start");
        miningArea.SetActive(true);
        EnemyManager.Instance.StartRush();
        StartCoroutine(Mine());
        base.Begin();
    }

    protected override void End()
    {
        miningArea.SetActive(false);
        EnemyManager.Instance.StopRush();
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
        text.text = "Stay inside the zone to keep mining";
        while (true)
        {
            yield return new WaitForSeconds(1);

            int random = Random.Range(miningScript.miningOutput, miningScript.miningOutput * 2);
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

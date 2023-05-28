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
    private bool mining = false;

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
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        if (GameManager.Instance.currentSelection == CurrentSelection.Paused && mining && audioSource.isPlaying)
        {
            audioSource.Pause();
        }
        else if (GameManager.Instance.currentSelection == CurrentSelection.Playing && mining && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    protected override void Begin()
    {
        mining = true;
        anim.SetTrigger("Start");
        gameObject.GetComponent<AudioSource>().Play();
        miningArea.SetActive(true);
        EnemyManager.Instance.StartRush();
        StartCoroutine(Mine());
        base.Begin();
    }

    protected override void End()
    {
        mining = false;
        miningArea.SetActive(false);
        gameObject.GetComponent<AudioSource>().Pause();
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

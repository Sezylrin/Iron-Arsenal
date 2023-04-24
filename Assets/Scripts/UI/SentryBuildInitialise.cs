using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SentryBuildInitialise : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Sentry Info")]
    [SerializeField] private Image SentryIcon;
    [SerializeField] private TMP_Text xenoriumCost;
    [SerializeField] private TMP_Text novaciteCost;
    [SerializeField] private TMP_Text voidStoneCost;

    [Header("Tooltip Info")]
    [SerializeField] private GameObject tooltipPrefab;


    private GameObject instantiatedToolTip;
    private SentryData sentry;
    private SentrySocket currentSocket;

    public void InitialiseSentryContainer(SentryData sentry)
    {
        this.sentry = sentry;
        SentryIcon.sprite = sentry.SentryIcon;
        xenoriumCost.text = sentry.xenoriumCost.ToString();
        novaciteCost.text = sentry.novaciteCost.ToString();
        voidStoneCost.text = sentry.voidStoneCost.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        instantiatedToolTip = Instantiate(tooltipPrefab);
        instantiatedToolTip.transform.SetParent(LevelCanvasManager.Instance.transform, false);

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        Vector3 center = new Vector3(screenWidth / 2f, screenHeight / 1.5f, 0f);
        instantiatedToolTip.transform.position = center;

        instantiatedToolTip.transform.GetChild(0).GetComponent<TMP_Text>().text = sentry.sentryName;
        instantiatedToolTip.transform.GetChild(1).GetComponent<TMP_Text>().text = sentry.description;

        Transform resourceContainer = instantiatedToolTip.transform.GetChild(2);
        resourceContainer.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>().text = sentry.xenoriumCost.ToString();
        resourceContainer.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>().text = sentry.novaciteCost.ToString();
        resourceContainer.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>().text = sentry.voidStoneCost.ToString();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(instantiatedToolTip);
    }

    public void SpawnSentry()
    {
        if (LevelManager.Instance.CanBuildSentry(sentry.Sentry))
        {
            LevelManager.Instance.BuildSentry(sentry.Sentry);
            Destroy(instantiatedToolTip);
            currentSocket.SetSentry(sentry);
        }
        
    }

    public void SetSocket(SentrySocket socket)
    {
        currentSocket = socket;
    }
}

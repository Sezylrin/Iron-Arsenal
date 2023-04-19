using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SentryBuildInitialise : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image SentryIcon;
    [SerializeField] private TMP_Text xenoriumCost;
    [SerializeField] private TMP_Text novaciteCost;
    [SerializeField] private TMP_Text voidStoneCost;
    [SerializeField] private GameObject tooltipPrefab;

    private GameObject instantiatedToolTip;

    public void InitialiseSentryContainer(SentryData sentry)
    {
        SentryIcon.sprite = sentry.SentryIcon;
        xenoriumCost.text = sentry.xenoriumCost.ToString();
        novaciteCost.text = sentry.novaciteCost.ToString();
        voidStoneCost.text = sentry.voidStoneCost.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        instantiatedToolTip = Instantiate(tooltipPrefab);
        instantiatedToolTip.transform.SetParent(LevelCanvasManager.Instance.transform, false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(instantiatedToolTip);
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AugmentMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button firstAugBtn;
    [SerializeField] private Button secondAugBtn;
    [SerializeField] private Button thirdAugBtn;

    [Header("Augment Info")]
    [SerializeField] private Image firstAugImage;
    [SerializeField] private TMP_Text firstAugName;
    [SerializeField] private TMP_Text firstAugDesc;
    [SerializeField] private Image secondAugImage;
    [SerializeField] private TMP_Text secondAugName;
    [SerializeField] private TMP_Text secondAugDesc;
    [SerializeField] private Image thirdAugImage;
    [SerializeField] private TMP_Text thirdAugName;
    [SerializeField] private TMP_Text thirdAugDesc;

    private List<AugmentData> currentAugments;

    private void Start()
    {
        firstAugBtn.onClick.AddListener(SelectFirstAug);
        secondAugBtn.onClick.AddListener(SelectSecondAug);
        thirdAugBtn.onClick.AddListener(SelectThirdAug);
    }

    public void CreateAugmentChoices(List<AugmentData> augments)
    {
        currentAugments = augments;
        firstAugImage.sprite = augments[0].icon;
        firstAugName.text = augments[0].augName;
        firstAugDesc.text = augments[0].description;
        if (augments.Count >= 2)
        {
            secondAugImage.sprite = augments[1].icon;
            secondAugName.text = augments[1].augName;
            secondAugDesc.text = augments[1].description;
        }
        else
        {
            secondAugName.text = "Empty Augment";
            secondAugDesc.text = "To be fixed. We ran out of augments to show :(";
        }
        if (augments.Count >= 3)
        {
            thirdAugImage.sprite = augments[2].icon;
            thirdAugName.text = augments[2].augName;
            thirdAugDesc.text = augments[2].description;
        }
        else
        {
            thirdAugName.text = "Empty Augment";
            thirdAugDesc.text = "To be fixed. We ran out of augments to show :(";
        }
    }

    public void SelectFirstAug()
    {

    }

    public void SelectSecondAug()
    {

    }

    public void SelectThirdAug()
    {

    }
}

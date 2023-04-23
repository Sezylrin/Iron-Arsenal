using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AugmentMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button[] augmentButtons;

    [Header("Augment Info")]
    [SerializeField] private AugmentSlot[] augmentSlots;

    private List<AugmentData> currentAugments;

    private void Start()
    {
        for (int i = 0; i < augmentButtons.Length; i++)
        {
            int augmentIndex = i; // To capture the current value of 'i' for the lambda expression
            augmentButtons[i].onClick.AddListener(() => SelectAugment(augmentIndex));
        }
    }

    public void CreateAugmentChoices(List<AugmentData> augments)
    {
        currentAugments = augments;

        for (int i = 0; i < augmentSlots.Length; i++)
        {
            if (i < augments.Count)
            {
                augmentSlots[i].SetAugmentData(augments[i]);
            }
            else
            {
                augmentSlots[i].SetEmptyAugment();
            }
        }
    }

    public void SelectAugment(int augmentIndex)
    {
        AugmentManager.Instance.AddAugment(currentAugments[augmentIndex].augmentType);
        LevelManager.Instance.RemoveAugmentMenu();
    }
}

[System.Serializable]
public class AugmentSlot
{
    [SerializeField] private Image augmentImage;
    [SerializeField] private TMP_Text augmentName;
    [SerializeField] private TMP_Text augmentDesc;

    public void SetAugmentData(AugmentData augmentData)
    {
        augmentImage.sprite = augmentData.icon;
        augmentName.text = augmentData.augName;
        augmentDesc.text = augmentData.description;
    }

    public void SetEmptyAugment()
    {
        augmentName.text = "Empty Augment";
        augmentDesc.text = "To be fixed. We ran out of augments to show :(";
    }
}
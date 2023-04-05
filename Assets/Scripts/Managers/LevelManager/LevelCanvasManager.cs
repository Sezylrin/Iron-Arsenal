using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LevelCanvasManager : MonoBehaviour
{
    [Header("Ore Labels")]
    [SerializeField] private TMP_Text ironLabel;
    [SerializeField] private TMP_Text copperLabel;
    [SerializeField] private TMP_Text goldLabel;

    [Header("Augments")]
    [SerializeField] private TMP_Text firstAugName;
    [SerializeField] private TMP_Text firstAugDesc;
    [SerializeField] private TMP_Text secondAugName;
    [SerializeField] private TMP_Text secondAugDesc;
    [SerializeField] private TMP_Text thirdAugName;
    [SerializeField] private TMP_Text thirdAugDesc;

    [Header("AugmentContainer")]
    [SerializeField] private GameObject augmentContainer;

    public static LevelCanvasManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void SetIronAmount(int ironAmount)
    {
        ironLabel.text = ironAmount.ToString();
    }

    public void SetCopperAmount(int copperAmount)
    {
        copperLabel.text = copperAmount.ToString();
    }

    public void SetGoldAmount(int goldAmount)
    {
        goldLabel.text = goldAmount.ToString();
    }

    public void ShowAugmentChoices(List<AugmentData> augments)
    {
        firstAugName.text = augments[0].augName + ": Press 1";
        firstAugDesc.text = augments[0].description;
        if (augments.Count >= 2)
        {
            secondAugName.text = augments[1].augName + ": Press 2";
            secondAugDesc.text = augments[1].description;
        }
        else
        {
            secondAugName.text = "Empty Augment";
            secondAugDesc.text = "To be fixed. We ran out of augments to show :(";
        }
        if (augments.Count >= 3)
        {
            thirdAugName.text = augments[2].augName + ": Press 3";
            thirdAugDesc.text = augments[2].description;
        }
        else
        {
            thirdAugName.text = "Empty Augment";
            thirdAugDesc.text = "To be fixed. We ran out of augments to show :(";
        }
        augmentContainer.SetActive(true);
    }

    public void RemoveAugmentChoices()
    {
        augmentContainer.SetActive(false);
    }
}

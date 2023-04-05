using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class LevelUIManager : MonoBehaviour
{
    [SerializeField] private UIDocument uiDoc;

    private Label ironLabel;
    private Label copperLabel;
    private Label goldLabel;
    private VisualElement augmentContainer;

    private Label firstAugName;
    private Label firstAugDesc;
    private Label secondAugName;
    private Label secondAugDesc;
    private Label thirdAugName;
    private Label thirdAugDesc;

    private void Start()
    {
        ironLabel = uiDoc.rootVisualElement.Q<Label>("iron-value");
        copperLabel = uiDoc.rootVisualElement.Q<Label>("copper-value");
        goldLabel = uiDoc.rootVisualElement.Q<Label>("gold-value");
        augmentContainer = uiDoc.rootVisualElement.Q<VisualElement>("Augments Outer");
        firstAugName = uiDoc.rootVisualElement.Q<Label>("first-augment-name");
        firstAugDesc = uiDoc.rootVisualElement.Q<Label>("first-augment-desc");
        secondAugName = uiDoc.rootVisualElement.Q<Label>("second-augment-name");
        secondAugDesc = uiDoc.rootVisualElement.Q<Label>("second-augment-desc");
        thirdAugName = uiDoc.rootVisualElement.Q<Label>("third-augment-name");
        thirdAugDesc = uiDoc.rootVisualElement.Q<Label>("third-augment-desc");
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

    //TODO: Add handling for empty augment slots
    public void ShowAugmentChoices(List<AugmentData> augments)
    {
        firstAugName.text = augments[0].augName + ": 1";
        firstAugDesc.text = augments[0].description;
        if (augments.Count >= 2)
        {
            secondAugName.text = augments[1].augName + ": 2";
            secondAugDesc.text = augments[1].description;
        }
        else
        {
            secondAugName.text = "Empty Augment";
            secondAugDesc.text = "To be fixed. We ran out of augments to show :(";
        }
        if (augments.Count >= 3)
        {
            thirdAugName.text = augments[2].augName + ": 3";
            thirdAugDesc.text = augments[2].description;
        }
        else
        {
            thirdAugName.text = "Empty Augment";
            thirdAugDesc.text = "To be fixed. We ran out of augments to show :(";
        }
        augmentContainer.RemoveFromClassList("d-none");
    }

    public void RemoveAugmentChoices()
    {
        augmentContainer.AddToClassList("d-none");
    }
}

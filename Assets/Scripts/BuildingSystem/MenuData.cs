using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuData
{
    // Start is called before the first frame updatepublic Button[] switchMenu;
    public Button[] switchMenu;

    public List<GameObject> availableTurrets;

    public List<GameObject> buttonGroup;

    public int unlockedTurrets = 0;
    public MenuData(Button[] switchMenu, List<GameObject> availableTurrets, List<GameObject> buttonGroup, int unlockedTurrets)
    {
        this.switchMenu = switchMenu;
        this.availableTurrets = availableTurrets;
        this.buttonGroup = buttonGroup;
        this.unlockedTurrets = unlockedTurrets;
    }
}

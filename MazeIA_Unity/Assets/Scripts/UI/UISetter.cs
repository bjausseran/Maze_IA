using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class UISetter : MonoBehaviour
{
    [SerializeField] static ColorBlock buttonColorBlock = ColorBlock.defaultColorBlock;
    [SerializeField] static ColorBlock buttonSelected = ColorBlock.defaultColorBlock;

    static public ColorBlock GetDefaultColorBlock()
    {
        return buttonColorBlock;
    }
    static public ColorBlock GetSelectedColorBlock()
    {
        if (buttonColorBlock == ColorBlock.defaultColorBlock) SetSelectedColor();
        return buttonSelected;
    }
    static void SetSelectedColor()
    {
        buttonSelected.normalColor = new Color(0.2f, 0.45f, 0.45f);
        buttonSelected.highlightedColor = new Color(0.2f, 0.55f, 0.55f);
        buttonSelected.pressedColor = new Color(0.2f, 0.65f, 0.65f);
        buttonSelected.selectedColor = new Color(0.2f, 0.45f, 0.45f);
    }
}

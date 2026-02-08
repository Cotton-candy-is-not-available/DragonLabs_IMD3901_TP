using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public Image crosshairIMG;
    public Color normalColor = Color.white;
    public Color interactColor = Color.blue;
    public void setInteract(bool canInteract)
    {
        crosshairIMG.color = canInteract ? interactColor : normalColor;
    }
}

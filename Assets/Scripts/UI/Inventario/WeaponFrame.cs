using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponFrame : UIFrame
{
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI municaoTxt;
    public Image weaponImage;
    public Color equipedColor;
    public Color normalColor;

    public void Init(ArmaDeFogo usableRef)
    {
        base.Init(usableRef);
        nameTxt.text = usableRef.Nome;
        municaoTxt.text = $"{usableRef.MunicaoCartucho}/{usableRef.Municao}";
        ResetColor();
    }

    public void EquipedColor()
    {
        weaponImage.color = equipedColor;
    }

    public void ResetColor()
    {
        weaponImage.color = normalColor;
    }
}

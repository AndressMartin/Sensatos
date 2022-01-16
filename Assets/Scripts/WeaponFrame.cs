using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponFrame : MonoBehaviour
{
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI municaoTxt;
    public Image weaponImage;
    public Color equipedColor;
    public Color normalColor;
    private ArmaDeFogo savedWeaponRef;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(ArmaDeFogo armaRef)
    {
        savedWeaponRef = armaRef;
        nameTxt.text = armaRef.Nome;
        municaoTxt.text = $"{armaRef.MunicaoCartucho}/{armaRef.Municao}";
        weaponImage.sprite = armaRef.ImagemInventario;
        ResetColor();
    }

    public ArmaDeFogo GetSavedWeapon()
    {
        return savedWeaponRef;
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

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
        nameTxt.text = armaRef.nome;
        municaoTxt.text = $"{armaRef.municaoAtual}/{armaRef.municaoMax}";
        weaponImage.sprite = armaRef.myImage;
    }
}

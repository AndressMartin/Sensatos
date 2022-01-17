using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExplainPanel : MonoBehaviour
{
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI description;

    /*
     *  List<melhoriaUI> ->> TODO: Primeiro implementar a lista de melhorias na própria arma. 
     *
     */

    public void ChangeUI(ArmaDeFogo arma)
    {
        weaponName.text = arma.Nome;
        description.text = arma.Descricao;
    }
}

public class melhoriaUI
{
    public TextMeshProUGUI melhoriaName;
    public TextMeshProUGUI melhoriaDescription;
}

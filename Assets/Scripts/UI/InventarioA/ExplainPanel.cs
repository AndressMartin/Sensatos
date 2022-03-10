using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExplainPanel : MonoBehaviour
{
    public TextMeshProUGUI nametxt;
    public TextMeshProUGUI description;

    /*
     *  TODO: List<melhoriaUI>
     */

    public void ChangeUI(ItemDoInventario element)
    {
        nametxt.text = element.Nome;
        description.text = element.Descricao;
    }

    public void CheckIfFlip(RectTransform optionsPanel)
    {
        var centerOfScreen = Screen.width * .5f;
        var pos = GetComponent<RectTransform>().position;
        //Está na parte esquerda da tela
        if (optionsPanel.position.x <= centerOfScreen && pos.x <= centerOfScreen)
        {
            GetComponent<RectTransform>().position = new Vector3(pos.x + centerOfScreen, pos.y, pos.z);
        }
        //Está na parte direita da tela
        else if (optionsPanel.position.x > centerOfScreen && pos.x > centerOfScreen)
        {
            GetComponent<RectTransform>().position = new Vector3(pos.x + centerOfScreen*-1, pos.y, pos.z);
        }
    }
}

public class melhoriaUI
{
    public TextMeshProUGUI melhoriaName;
    public TextMeshProUGUI melhoriaDescription;
}

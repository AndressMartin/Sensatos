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

    public void ChangeUI(Usable element)
    {
        nametxt.text = element.Nome;
        description.text = element.Descricao;
    }
}

public class melhoriaUI
{
    public TextMeshProUGUI melhoriaName;
    public TextMeshProUGUI melhoriaDescription;
}

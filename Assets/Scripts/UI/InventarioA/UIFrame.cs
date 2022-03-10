using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFrame : MonoBehaviour
{
    protected ItemDoInventario savedUsable;
    public Image image;
    public UIScreen screenThatIOpen;

    public virtual void Init(ItemDoInventario usableRef)
    {
        if (!image) image = GetComponent<Image>();
        savedUsable = usableRef;
        image.sprite = usableRef.ImagemInventario;
    }

    public virtual ItemDoInventario GetSavedElement()
    {
        return savedUsable;
    }

    public virtual void RemoveElement()
    {
        savedUsable = null;
        image.sprite = null;
    }
}

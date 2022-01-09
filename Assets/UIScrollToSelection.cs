/// Credit zero3growlithe
/// sourced from: http://forum.unity3d.com/threads/scripts-useful-4-6-scripts-collection.264161/page-2#post-2011648

/*USAGE:
Simply place the script on the ScrollRect that contains the selectable children we'll be scroling to
and drag'n'drop the RectTransform of the options "container" that we'll be scrolling.*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIScrollToSelection : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform contentPanel;

    private void Update()
    {
        
    }
    public void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        var halfVerticalSize = contentPanel.sizeDelta.y / 2;
        var halfVerticalSizeTarget = target.sizeDelta.y / 2;
        var offset = target.sizeDelta.y * 0.05f;
        var yChange = scrollRect.transform.InverseTransformPoint(contentPanel.position).y - 
            (scrollRect.transform.InverseTransformPoint(target.position).y - halfVerticalSizeTarget - offset);

        contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, yChange - halfVerticalSize);
    }
}
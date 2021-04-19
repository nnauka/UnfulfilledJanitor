using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighlightedObjectView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private ObjectHighlighter highlighter;

    private void OnEnable()
    {
        highlighter.OnObjectHighlight += DisplayHighlightInfo;
    }

    private void OnDisable()
    {
        highlighter.OnObjectHighlight -= DisplayHighlightInfo;
    }

    private void DisplayHighlightInfo(Transform obj)
    {
        text.SetText(obj?.name ?? "");
    }
}

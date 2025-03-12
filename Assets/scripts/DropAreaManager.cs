using UnityEngine;
using UnityEngine.UI;

public class DropAreaManager : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Transform dropAreaContent;

    void Start()
    {
       
        scrollRect.content = dropAreaContent.GetComponent<RectTransform>();
    }
}
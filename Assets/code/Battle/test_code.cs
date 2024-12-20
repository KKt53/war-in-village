using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonCustomHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("ボタンが押された！");
        // 押したときの処理をここに記述
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("ボタンが離された！");
        // 離したときの処理をここに記述
    }
}

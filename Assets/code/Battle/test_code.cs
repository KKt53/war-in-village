using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonCustomHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("�{�^���������ꂽ�I");
        // �������Ƃ��̏����������ɋL�q
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("�{�^���������ꂽ�I");
        // �������Ƃ��̏����������ɋL�q
    }
}

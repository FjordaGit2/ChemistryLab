using UnityEngine;
using UnityEngine.EventSystems;
public class AtomDisplay : MonoBehaviour, IDragHandler, IScrollHandler
{
    [SerializeField] Transform view = null;
    [SerializeField] Transform cam = null;

    public void OnDrag(PointerEventData eventData)
    {
        view.transform.Rotate(Vector3.up * eventData.delta.x , Space.World);
        view.transform.Rotate(Vector3.left * eventData.delta.y, Space.Self);
    }

    public void OnScroll(PointerEventData eventData)
    {
        float d = Mathf.Clamp(cam.localPosition.y - eventData.scrollDelta.y * 0.05f, 0.2f, 1.5f);
        cam.localPosition = new Vector3(0, d, 0);
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

// AddしたRectTransformをドラッグ移動できるようにする
public class DragObject : MonoBehaviour ,IDragHandler,IBeginDragHandler,IEndDragHandler
{
	[SerializeField] private RectTransform canvas;

	private RectTransform rectTransform;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
	}

	public void OnEndDrag(PointerEventData eventData)
	{
	}

	public void OnDrag(PointerEventData eventData)
	{
		Vector2 localPosition = GetLocalPosition(eventData.position);
		rectTransform.localPosition = localPosition;
	}

	private Vector2 GetLocalPosition(Vector2 screenPosition)
	{
		Vector2 result = Vector2.zero;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPosition, Camera.main, out result);
		return result;
	}
}
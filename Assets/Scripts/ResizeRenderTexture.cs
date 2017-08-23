using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// ウィンドウサイズにあわせてRenderTextureの表示サイズを調整
public class ResizeRenderTexture : MonoBehaviour {
	[SerializeField] private bool keepCenter = true;
	[SerializeField] private ResizeDirection resizeDirection;

	public enum ResizeDirection
	{
		x,
		y,
		both
	}

	private Rect GUIRect;
	private Rect RenderTexRect;
	private float ImageAspect;

	// Update is called once per frame
	void Update () {
		GUIRect = this.GetComponent<RectTransform> ().rect;
		if ((GUIRect.width>GUIRect.height && resizeDirection == ResizeDirection.both) || resizeDirection == ResizeDirection.y) {
			ImageAspect = GUIRect.height / GUIRect.width;
			RenderTexRect = this.gameObject.GetComponent<RawImage> ().uvRect;
			RenderTexRect.width = 1;
			RenderTexRect.height = ImageAspect;
			if (keepCenter) {
				RenderTexRect.y = (1-ImageAspect) * 0.5f;
			}
			this.gameObject.GetComponent<RawImage> ().uvRect = RenderTexRect;
		}
		if ((GUIRect.height>GUIRect.width && resizeDirection == ResizeDirection.both) || resizeDirection == ResizeDirection.x) {
			ImageAspect = GUIRect.width / GUIRect.height;
			RenderTexRect = this.gameObject.GetComponent<RawImage> ().uvRect;
			RenderTexRect.width = ImageAspect;
			RenderTexRect.height = 1;
			if (keepCenter) {
				RenderTexRect.x = (1-ImageAspect) * 0.5f;
			}
			this.gameObject.GetComponent<RawImage> ().uvRect = RenderTexRect;
		}
	}
}

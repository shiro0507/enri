using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// 3Dオブジェクトの位置にcanvas上のテクスチャを追従させる
// 3DオブジェクトにAddして使用
public class ShowPointTag : MonoBehaviour {
	public Canvas canvas;
	public string tagString = "Test";
	public Text tagText;

	private bool _isRendered = false;
	private Camera targetCamera;

	private void Update () {
        tagText.gameObject.SetActive(false);

        if (_isRendered){
			var pos = Vector2.zero;
			var uiCamera = targetCamera;
			var worldCamera = targetCamera;
			var canvasRect = canvas.GetComponent<RectTransform> ();

			var screenPos = RectTransformUtility.WorldToScreenPoint (worldCamera, transform.position);
			RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, uiCamera, out pos);
			tagText.rectTransform.localPosition = pos + new Vector2(30,100);

            tagText.gameObject.SetActive(true);
		}

        _isRendered = false;
	}

    void OnDisable()
    {
        if (tagText != null) {
            tagText.gameObject.SetActive(false);
        }
    }

    //カメラに映ってる間に呼ばれる
    private void OnWillRenderObject(){
		if(Camera.current.tag == "MapCamera"){
			targetCamera = Camera.current;
			canvas.worldCamera = Camera.current;
			_isRendered = true;
		}
	}

}

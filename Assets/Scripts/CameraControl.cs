using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

// カメラの切り替え・移動などの制御
// カメラの映像を表示するパネルにaddして使用
public class CameraControl : MonoBehaviour, IDragHandler {
	[SerializeField] private GameObject camera2D;
	[SerializeField] private GameObject camera3D;
	[SerializeField] private GameObject cameraRoot;

	private bool is2D;

	//latitude and longitude of camera root
	private Vector3 cameraCoordinates = new Vector3(35, 140);

	// Use this for initialization
	void Start () {
		ShowCamera2D ();
		is2D = true;

		cameraRoot.transform.localPosition = Constants.Sphere.ToCartesian(cameraCoordinates);
		cameraRoot.transform.localRotation = Quaternion.LookRotation(cameraRoot.transform.parent.position - cameraRoot.transform.position, Vector3.up);
	}

	public float scaleFactor = 1f;

	// マウスドラッグでカメラ移動回転
	public void OnDrag(PointerEventData eventData)
	{
		float moveX = eventData.delta.x;
		float moveY = eventData.delta.y;
		//translation
		if (eventData.button == PointerEventData.InputButton.Left) {
			cameraRoot.transform.RotateAround(cameraRoot.transform.parent.position, cameraRoot.transform.up, moveX * 0.001f);
			cameraRoot.transform.RotateAround(cameraRoot.transform.parent.position, cameraRoot.transform.right, moveY * -0.001f);
		}
		//rotation
		if (eventData.button == PointerEventData.InputButton.Right) {
			cameraRoot.transform.RotateAround(cameraRoot.transform.parent.position, cameraRoot.transform.forward, moveX * 0.1f);
		}
		//zoom
		if (eventData.button == PointerEventData.InputButton.Middle) {
            Zoom(moveY);
		}
	}

    void Zoom(float moveY) {
        if (is2D) {
            camera2D.GetComponent<Camera>().orthographicSize += moveY * 0.1f;
        } else {
            camera3D.transform.Translate (new Vector3 (0, 0, moveY * -0.1f));
        }
    }

    void Update() {
        var translation = Input.mouseScrollDelta;
        Zoom(translation.y);
    }

	void ShowCamera2D(){
		camera2D.gameObject.SetActive(true);
		camera3D.gameObject.SetActive(false);
	}

	void ShowCamera3D(){
		camera2D.gameObject.SetActive(false);
		camera3D.gameObject.SetActive(true);
	}

	public void ToggleCamera(bool val){
		if (val) {
			ShowCamera2D ();
			is2D = true;
		}else{
			ShowCamera3D ();
			is2D = false;
		}
	}
}

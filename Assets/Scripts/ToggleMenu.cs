using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// toggleの表示画像を状態によって切り替え
public class ToggleMenu : MonoBehaviour {
	[SerializeField] private Sprite texOn;
	[SerializeField] private Sprite texOff;

	private GameObject title;

	// Use this for initialization
	void Start () {
		title = this.gameObject.transform.Find("titleIM").gameObject;
	}

	public void SetTitle(bool val){
		Debug.Log (title.name);
		if (val) {
			title.GetComponent<Image>().sprite = texOn;
		}else{
			title.GetComponent<Image>().sprite = texOff;
		}
	}
}

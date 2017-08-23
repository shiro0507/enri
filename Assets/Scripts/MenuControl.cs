using UnityEngine;
using System.Collections;

// タブメニュー関連の操作
public class MenuControl : MonoBehaviour {
	[SerializeField] private RectTransform mainView;
	[SerializeField] private RectTransform menuView;
	[SerializeField] private RectTransform settingPanel;
	[SerializeField] private RectTransform filePanel;
	[SerializeField] private GameObject[] selectOpenBtn;
    
	private bool isOpen;
	private bool[] selectOpen;

	void Awake (){
		isOpen = true;
		selectOpen = new bool[]{false, false, false, false};
	}

	// Use this for initialization
	void Start () {
		ShowSelectOpenList ();
	}

	// 表示するプルダウンメニューの作成
	// 現在はダミーで4項目を表示
	void ShowSelectOpenList(){
		GameObject[] selectOpens = GameObject.FindGameObjectsWithTag ("SelectOpen");
		for (int i = 0; i < selectOpens.Length; i++) {
			Destroy (selectOpens [i]);
		}

		GameObject prefab = (GameObject)Resources.Load ("Prefabs/MenuParts/selectOpenList");

		for (int i = 0; i < 4; i++) {
			if (selectOpen[i]) {
				GameObject selectopenList = Instantiate (prefab, selectOpenBtn [i].transform.position, Quaternion.identity);
				selectopenList.transform.SetParent (transform);
				selectopenList.transform.localScale = new Vector3 (1,1,1);
                selectopenList.GetComponent<SelectOpenListItems>().UpdateItems(i);
			}
		}
	}

	// ボタンから呼び出し
	// プルダウンメニューの切り替え
	public void ShowSelectOpenListBtn(int val){
		for (int i = 0; i < 4; i++) {
			if (val == i) {
				if (selectOpen [i]) {
					selectOpen [i] = false;
				}
				else {
					selectOpen [i] = true;
				}
			} else {
				selectOpen [i] = false;
			}
		}
		ShowSelectOpenList ();
	}

	// タブメニュー全体の開閉
	public void ToggleMenu(){
		if (isOpen) {
			isOpen = false;

			selectOpen = new bool[]{false, false, false, false};
			ShowSelectOpenList ();

			mainView.sizeDelta = new Vector2 (-100, 0);
			menuView.anchoredPosition = new Vector2 (-100, 0);
		} else {
			isOpen = true;
			mainView.sizeDelta = new Vector2 (-555, 0);
			menuView.anchoredPosition = new Vector2 (-555, 0);
		}

	}

	// タブの切り替え
	public void ShowSettingMenu(){
		if (!isOpen) {
			ToggleMenu ();
		}
		settingPanel.gameObject.SetActive (true);
		filePanel.gameObject.SetActive (false);
	}

	// タブの切り替え
	public void ShowFileMenu(){
		if (!isOpen) {
			ToggleMenu ();
		}

		selectOpen = new bool[]{false, false, false, false};
		ShowSelectOpenList ();

		settingPanel.gameObject.SetActive (false);
		filePanel.gameObject.SetActive (true);
	}
}

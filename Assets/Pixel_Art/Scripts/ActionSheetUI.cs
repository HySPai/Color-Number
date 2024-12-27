/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System;
using UnityEngine;
using UnityEngine.UI;

public class ActionSheetUI : MonoBehaviour
{
	public Button[] buttons;

	public GameObject questionPanel;
	public Text questionMessage;
	public Text questionTitle;
	public Button buttonNo;
	public Button buttonYes;
	public Button buttonYes2;

	private Action<int> _callback;
	private Action<int> _questionCallback;

	private void Awake()
	{
		ActionSheetWrapper.actionSheet = this;
		gameObject.SetActive(false);
		DontDestroyOnLoad(gameObject);
	}

	public void ShowButtons(string[] buttonTitles, Action<int> callback)
	{
		questionPanel.SetActive(false);
		gameObject.SetActive(true);
		_callback = callback;
		foreach (var b in buttons)
		{
			b.gameObject.SetActive(false);
		}

		for (int i = 0; i < buttonTitles.Length; i++)
		{
			if (i < buttons.Length)
			{
				buttons[i].gameObject.SetActive(true);
				int index = buttonTitles.Length - i - 1;
				buttons[i].GetComponent<ActionSheetButton>().index = index;
				buttons[i].GetComponentInChildren<Text>().text = buttonTitles[index];
			}
		}
	}
	public void Show2ButtonsDialog(string title, string message, string firstText, string secondText, Action<int> callback)
	{
		_questionCallback = callback;
		foreach (var b in buttons)
		{
			b.gameObject.SetActive(false);
		}

		questionTitle.text = title;
		questionMessage.text = message;
		buttonNo.GetComponentInChildren<Text>().text = firstText;
		buttonYes.GetComponentInChildren<Text>().text = secondText;

		buttonYes.gameObject.SetActive(true);
		buttonNo.gameObject.SetActive(true);
		buttonYes2.gameObject.SetActive(false);

		questionPanel.SetActive(true);
		gameObject.SetActive(true);
	}
	public void ShowOneButtonDialog(string title, string message, string buttonText)
	{
		foreach (var b in buttons)
		{
			b.gameObject.SetActive(false);
		}

		questionTitle.text = title;
		questionMessage.text = message;
		buttonYes2.GetComponentInChildren<Text>().text = buttonText;

		buttonYes.gameObject.SetActive(false);
		buttonNo.gameObject.SetActive(false);
		buttonYes2.gameObject.SetActive(true);

		questionPanel.SetActive(true);
		gameObject.SetActive(true);
	}

	public void OnButtonClick(int index)
	{
		gameObject.SetActive(false);
		if (_callback != null)
		{
			var b = buttons[index];
			_callback(b.GetComponent<ActionSheetButton>().index);
		}
	}

	public void OnButtonOK()
	{
		gameObject.SetActive(false);
		if (_questionCallback != null)
		{
			_questionCallback.SafeInvoke(1);
		}
		questionPanel.gameObject.SetActive(false);
	}

	public void OnButtonOK2()
	{
		gameObject.SetActive(false);
		if (_questionCallback != null)
		{
			_questionCallback.SafeInvoke(1);
		}
		questionPanel.gameObject.SetActive(false);
	}

	public void OnButtonCancel()
	{
		gameObject.SetActive(false);
		if (_questionCallback != null)
		{
			_questionCallback.SafeInvoke(0);
		}
		questionPanel.gameObject.SetActive(false);
	}
}

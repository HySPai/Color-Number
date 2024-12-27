/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/



using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
	private Action m_firstButtonClick;

	private Action m_secondButtonClick;

	[SerializeField]
	private RectTransform m_messagePanel;

	[SerializeField]
	private Text m_message;

	[SerializeField]
	private Text m_button1Text;

	[SerializeField]
	private Text m_button2Text;

	[SerializeField]
	private GameObject m_blockPlane;

	[SerializeField]
	private Vector2 m_messageOpenedPos = Vector2.zero;

	[SerializeField]
	private Vector2 m_messageClosedPos = Vector2.zero;

	[SerializeField]
	private float m_showTime = 4f;

	public void Init()
	{
		this.m_messagePanel.anchoredPosition = this.m_messageClosedPos;
		this.m_messagePanel.gameObject.SetActive(false);
	}

	public void Show(string text, int time = 4)
	{
		if (base.gameObject != null && this.m_message != null && this.m_messagePanel != null && this.m_messagePanel.gameObject != null)
		{
			this.m_showTime = time;
			this.m_message.text = text;
			this.m_messagePanel.gameObject.SetActive(true);
			base.StartCoroutine(this.ShowMessageCoroutine());
		}
	}

	public void Show(string text, Action firstButtonClick, Action secondButtonClick)
	{
		if (base.gameObject != null)
		{
			this.m_message.text = text;
			this.m_firstButtonClick = firstButtonClick;
			this.m_secondButtonClick = secondButtonClick;
			this.m_messagePanel.gameObject.SetActive(true);
			base.StartCoroutine(this.ShowMessageCoroutine());
		}
	}

	public void FirstButtonClick()
	{
		this.m_firstButtonClick.SafeInvoke();
		this.Close();
	}

	public void SecondButtonClick()
	{
		this.m_secondButtonClick.SafeInvoke();
		this.Close();
	}

	public void Close()
	{
		base.StartCoroutine(this.CloseCoroutine());
	}
	private IEnumerator ShowMessageCoroutine()
	{
		yield return null;
		if (this.m_blockPlane != null)
		{
			this.m_blockPlane.SetActive(true);
		}
		this.m_messagePanel.anchoredPosition = this.m_messageClosedPos;
		var time = 0.3f;
		var speed = (this.m_messageOpenedPos - this.m_messageClosedPos) / time;

		while (true)
		{
			var deltaTime = Time.deltaTime;
			if (time <= deltaTime)
			{
				this.m_messagePanel.anchoredPosition = this.m_messageOpenedPos;
				if (this.m_showTime > 0f)
				{
					yield return new WaitForSeconds(this.m_showTime);
					this.Close(); 
				}
				yield break;
			}
			time -= deltaTime; 
			this.m_messagePanel.anchoredPosition += speed * deltaTime;
			yield return null;
		}
	}
	private IEnumerator CloseCoroutine()
	{
		if (this.m_blockPlane != null)
		{
			this.m_blockPlane.SetActive(false);
		}
		var time = 0.3f;
		var speed = (this.m_messageClosedPos - this.m_messageOpenedPos) / time;
		
		while(true)
		{
			var deltaTime = Time.deltaTime;
			if (time <= deltaTime)
			{
				this.m_messagePanel.anchoredPosition = this.m_messageClosedPos;
				this.m_messagePanel.gameObject.SetActive(false);
				yield break;
			}
			time -= deltaTime; 
			this.m_messagePanel.anchoredPosition += speed * deltaTime;
			yield return null;
		}
	}
}

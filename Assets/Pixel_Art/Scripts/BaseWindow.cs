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

public abstract class BaseWindow : MonoBehaviour
{
	public enum WindowState
	{
		Opened,
		Closed
	}

	public enum WindowOpenStyle
	{
		None,
		FromRight,
		FromBottom,
		FromLeft,
		FromTop
	}

	public Action OnStartOpen;

	public Action OnOpen;

	public Action OnStartClose;

	public Action OnClose;

	[SerializeField]
	private WindowOpenStyle m_openStyle = WindowOpenStyle.FromBottom;

	private Canvas m_canvas;

	private RectTransform m_rectTransform;

	private Vector2 m_openedPosition = Vector2.zero;

	private Vector2 m_closedPosition = Vector2.zero;

	private float m_moveTime = 0.1f;

	private WindowState m_currentWindowState = WindowState.Closed;

	public virtual bool EnableBanner
	{
		get
		{
			return false;
		}
	}

	private RectTransform RectTransform
	{
		get
		{
			if (this.m_rectTransform == null)
			{
				this.m_rectTransform = (RectTransform)base.transform;
			}
			return this.m_rectTransform;
		}
	}

	protected abstract string WindowName
	{
		get;
	}

	public virtual void InitCanvas(Canvas canvas, bool setClosePosition = true)
	{
		this.m_canvas = canvas;
		switch (this.m_openStyle)
		{
			case WindowOpenStyle.None:
				this.m_closedPosition = Vector2.zero;
				break;
			case WindowOpenStyle.FromRight:
				this.m_closedPosition = new Vector2(((RectTransform)canvas.transform).rect.width + 50f, 0f);
				break;
			case WindowOpenStyle.FromLeft:
				this.m_closedPosition = new Vector2(0f - ((RectTransform)canvas.transform).rect.width - 50f, 0f);
				break;
			case WindowOpenStyle.FromBottom:
				this.m_closedPosition = new Vector2(0f, 0f - ((RectTransform)canvas.transform).rect.height - 50f);
				break;
			case WindowOpenStyle.FromTop:
				this.m_closedPosition = new Vector2(0f, ((RectTransform)canvas.transform).rect.height - 50f);// 0f - ((RectTransform)canvas.transform).rect.height - 50f);
				break;
		}
		if (setClosePosition)
		{
			this.RectTransform.anchoredPosition = this.m_closedPosition;
		}
	}

	private Vector2 GetClosedPosition(WindowOpenStyle style)
	{
		switch (style)
		{
			case WindowOpenStyle.None:
				return Vector2.zero;
			case WindowOpenStyle.FromRight:
				return new Vector2(((RectTransform)this.m_canvas.transform).rect.width + 50f, 0f);
			case WindowOpenStyle.FromLeft:
				return new Vector2(0f - ((RectTransform)this.m_canvas.transform).rect.width - 50f, 0f);
			case WindowOpenStyle.FromBottom:
				return new Vector2(0f, 0f - ((RectTransform)this.m_canvas.transform).rect.height - 50f);
			case WindowOpenStyle.FromTop:
				return new Vector2(0f, ((RectTransform)this.m_canvas.transform).rect.height - 50f);// 0f - ((RectTransform)this.m_canvas.transform).rect.height - 50f);
			default:
				return Vector2.zero;
		}
	}

	public virtual void Open()
	{
		this.Open(this.m_openStyle);
	}

	public void Open(WindowOpenStyle style)
	{
		this.SendActiveScreenEvent();
		base.StopAllCoroutines();
		this.OnStartOpen.SafeInvoke();
		base.gameObject.SetActive(true);
		switch (style)
		{
			case WindowOpenStyle.FromRight:
				base.StartCoroutine(this.OpenFromSideCoroutine(this.GetClosedPosition(style)));
				break;
			case WindowOpenStyle.FromLeft:
				base.StartCoroutine(this.OpenFromSideCoroutine(this.GetClosedPosition(style)));
				break;
			case WindowOpenStyle.FromBottom:
				base.StartCoroutine(this.OpenFromBottomCoroutine());
				break;
			case WindowOpenStyle.FromTop:
				base.StartCoroutine(this.OpenFromTopCoroutine());
				break;
			case WindowOpenStyle.None:
				this.RectTransform.anchoredPosition = this.m_openedPosition;
				this.OnOpen.SafeInvoke();
				break;
		}
	}

	public void DefferedOpen(float time = 0.1f)
	{
		base.gameObject.SetActive(true);
		this.RectTransform.anchoredPosition = this.m_closedPosition;
		base.StartCoroutine(this.DefferedOpenCoroutine(this.m_openStyle, time));
	}

	public void DefferedOpen(WindowOpenStyle style, float time = 0.1f)
	{
		base.gameObject.SetActive(true);
		this.RectTransform.anchoredPosition = this.GetClosedPosition(style);
		base.StartCoroutine(this.DefferedOpenCoroutine(style, time));
	}

	private IEnumerator DefferedOpenCoroutine(WindowOpenStyle style, float time)
	{
		yield return new WaitForSeconds(time);
		this.Open(style);
	}

	public void FastOpen()
	{
		this.SendActiveScreenEvent();
		base.gameObject.SetActive(true);
		this.RectTransform.anchoredPosition = this.m_openedPosition;
	}

	public virtual bool Close()
	{
		base.StopAllCoroutines();
		this.OnStartClose.SafeInvoke();
		switch (this.m_openStyle)
		{
			case WindowOpenStyle.FromRight:
				base.StartCoroutine(this.CloseToSideCoroutine(this.GetClosedPosition(this.m_openStyle)));
				break;
			case WindowOpenStyle.FromLeft:
				base.StartCoroutine(this.CloseToSideCoroutine(this.GetClosedPosition(this.m_openStyle)));
				break;
			case WindowOpenStyle.FromBottom:
				base.StartCoroutine(this.CloseToBottomCoroutine());
				break;
			case WindowOpenStyle.FromTop:
				base.StartCoroutine(this.CloseToTopCoroutine());
				break;
			case WindowOpenStyle.None:
				base.gameObject.SetActive(false);
				this.OnClose.SafeInvoke();
				break;
		}
		return true;
	}

	public virtual bool Close(WindowOpenStyle style)
	{
		base.StopAllCoroutines();
		this.OnStartClose.SafeInvoke();
		switch (style)
		{
			case WindowOpenStyle.FromRight:
				base.StartCoroutine(this.CloseToSideCoroutine(this.GetClosedPosition(style)));
				break;
			case WindowOpenStyle.FromLeft:
				base.StartCoroutine(this.CloseToSideCoroutine(this.GetClosedPosition(style)));
				break;
			case WindowOpenStyle.FromBottom:
				base.StartCoroutine(this.CloseToBottomCoroutine());
				break;
			case WindowOpenStyle.FromTop:
				base.StartCoroutine(this.CloseToTopCoroutine());
				break;
			case WindowOpenStyle.None:
				base.gameObject.SetActive(false);
				this.OnClose.SafeInvoke();
				break;
		}
		return true;
	}

	public void FastClose()
	{
		base.gameObject.SetActive(false);
		this.RectTransform.anchoredPosition = this.m_closedPosition;
	} 
	 
	private IEnumerator OpenFromSideCoroutine(Vector2 closedPosition)
	{ 
		bool sign = this.RectTransform.anchoredPosition.x > this.m_openedPosition.x;
		float speed = (this.m_openedPosition.x - closedPosition.x) / this.m_moveTime;
		yield return null;

		while (true)
		{
			var delta = speed * Mathf.Min(0.05f, Time.deltaTime);
			var newSign = (this.RectTransform.anchoredPosition.x + delta > this.m_openedPosition.x);
			if (newSign != sign)
			{
				this.RectTransform.anchoredPosition = this.m_openedPosition;
				yield return null;
				this.OnOpen.SafeInvoke();
				yield break;
			} 
			this.RectTransform.anchoredPosition += new Vector2(delta, 0f);
			yield return null;
		}
	}
	private IEnumerator CloseToBottomCoroutine()
	{ 
		bool sign = this.RectTransform.anchoredPosition.y > this.m_closedPosition.y;
		float speed = (this.m_closedPosition.y - this.m_openedPosition.y) / this.m_moveTime;
		yield return null;

		while (true)
		{
			var delta = speed * Mathf.Min(0.05f, Time.deltaTime);
			var newSign = (this.RectTransform.anchoredPosition.y + delta > this.m_openedPosition.y);
			if (newSign != sign)
			{
				this.RectTransform.anchoredPosition = this.m_openedPosition;
				yield return null;
				this.gameObject.SetActive(false);
				this.OnOpen.SafeInvoke();
				yield break;
			}
			this.RectTransform.anchoredPosition += new Vector2(0, delta);
			yield return null;
		}
	}
	private IEnumerator OpenFromBottomCoroutine()
	{ 
		var sign = (this.RectTransform.anchoredPosition.y > this.m_openedPosition.y);
		var speed = (this.m_openedPosition.y - this.m_closedPosition.y) / this.m_moveTime;

		yield return null;

		while (true)
		{
			var delta = speed * Mathf.Min(0.05f, Time.deltaTime); 
			var newSign = (this.RectTransform.anchoredPosition.y + delta > this.m_openedPosition.y);
			if (newSign != sign)
			{
				this.RectTransform.anchoredPosition = this.m_openedPosition;
				yield return null;
				this.OnOpen.SafeInvoke();

				yield break;
			} 
			this.RectTransform.anchoredPosition += new Vector2(0f, delta);

			yield return null;
		}
	}
	private IEnumerator OpenFromTopCoroutine()
	{ 
		var sign = (this.RectTransform.anchoredPosition.y < this.m_openedPosition.y);
		var speed = (this.m_openedPosition.y - this.m_closedPosition.y) / this.m_moveTime;
		yield return null;

		while (true)
		{
			var delta = speed * Mathf.Min(0.05f, Time.deltaTime); 
			var newSign = (this.RectTransform.anchoredPosition.y + delta < this.m_openedPosition.y);
			if (newSign != sign)
			{
				this.RectTransform.anchoredPosition = this.m_openedPosition;
				yield return null;

				this.OnOpen.SafeInvoke();
				yield break;
			}

			this.RectTransform.anchoredPosition += new Vector2(0f, delta);
			yield return null;
		}
	}
	private IEnumerator CloseToSideCoroutine(Vector2 closedPosition)
	{ 
		var sign = (this.RectTransform.anchoredPosition.x > closedPosition.x);
		var speed = (closedPosition.x - this.m_openedPosition.x) / this.m_moveTime;
		yield return null;

		while (true)
		{
			var delta = speed * Mathf.Min(0.05f, Time.deltaTime); 
			var newSign = (this.RectTransform.anchoredPosition.x + delta > closedPosition.x);
			if (newSign != sign)
			{
				this.RectTransform.anchoredPosition = closedPosition;
				this.gameObject.SetActive(false);
				this.OnClose.SafeInvoke();
				yield break;
			} 
			this.RectTransform.anchoredPosition += new Vector2(delta, 0f);
			yield return null;
		}
	} 
	private IEnumerator CloseToTopCoroutine()
	{ 
		var sign = (this.RectTransform.anchoredPosition.y > this.m_closedPosition.y);
		var speed = (this.m_closedPosition.y - this.m_openedPosition.y) / this.m_moveTime;
		yield return null;

		while (true)
		{
			var delta = speed * Mathf.Min(0.05f, Time.deltaTime); 
			var newSign = (this.RectTransform.anchoredPosition.y + delta > this.m_closedPosition.y);
			if (newSign != sign)
			{
				this.RectTransform.anchoredPosition = this.m_closedPosition;
				this.gameObject.SetActive(false);
				this.OnClose.SafeInvoke();
				yield break; 
			} 
			this.RectTransform.anchoredPosition += new Vector2(0f, delta);
		}
	}

	private void OnEnable()
	{
		switch (this.m_currentWindowState)
		{
			case WindowState.Opened:
				this.RectTransform.anchoredPosition = this.m_openedPosition;
				break;
			case WindowState.Closed:
				this.RectTransform.anchoredPosition = this.m_closedPosition;
				break;
		}
	}

	public virtual void SendActiveScreenEvent()
	{
		if (!string.IsNullOrEmpty(this.WindowName) && INPluginWrapper.Instance != null)
		{
			INPluginWrapper.Instance.SetActiveScreen(this.WindowName);
		}
	}
}

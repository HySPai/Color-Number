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
using UnityEngine.EventSystems;

public class FilterWIndowInputReceiver : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IScrollHandler, IEventSystemHandler
{
	public Action<float> OnScrolled;

	public Action<float> OnZoomed;

	public Action<Vector2> OnDragged;

	private Vector2 m_pos = Vector2.zero;

	private InputReceiver.TapState m_currentTapState;

	private float m_touchesDelta = -3.40282347E+38f;

	private void Update()
	{
		if (this.m_currentTapState != InputReceiver.TapState.Multitouch)
		{
			this.CheckMultiTouch();
		}
	}

	private bool CheckMultiTouch()
	{
		if (Input.touches.Length > 1)
		{
			Touch[] touches = Input.touches;
			Vector2 vector = touches[0].position - touches[1].position;
			this.m_touchesDelta = Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y);
			this.m_pos = Input.mousePosition;
			this.m_currentTapState = InputReceiver.TapState.Multitouch;
			return true;
		}
		return false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		switch (this.m_currentTapState)
		{
			case InputReceiver.TapState.Drag:
				this.OnDragged.SafeInvoke(eventData.position - this.m_pos);
				this.m_pos = eventData.position;
				break;
			case InputReceiver.TapState.Multitouch:
				{
					Touch[] touches = Input.touches;
					if (touches.Length >= 2)
					{
						Vector2 vector = touches[0].position - touches[1].position;
						float num = Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y);
						float par = num / this.m_touchesDelta;
						this.OnZoomed.SafeInvoke(par);
						this.m_touchesDelta = num;
					}
					break;
				}
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		this.m_pos = eventData.position;
		this.m_currentTapState = InputReceiver.TapState.Drag;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		this.m_currentTapState = InputReceiver.TapState.None;
	}

	public void OnScroll(PointerEventData data)
	{
		Action<float> onScrolled = this.OnScrolled;
		Vector2 scrollDelta = data.scrollDelta;
		onScrolled.SafeInvoke(scrollDelta.y);
	}
}



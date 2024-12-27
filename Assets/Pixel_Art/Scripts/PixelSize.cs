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

public class PixelSize : MonoBehaviour
{
	[SerializeField]
	private RectTransform m_canvas;

	[SerializeField]
	private float m_pixels;

	private void Awake()
	{
		base.StartCoroutine(this.InitCoroutine());
	}
	private IEnumerator InitCoroutine()
	{
		yield return null;
		 
		var size = this.m_pixels * ScreenToolWrapper.Density * 1.777778f / (this.m_canvas.rect.height / this.m_canvas.rect.width);
		var sizeDelta = new Vector2(size, size);
		(this.transform as RectTransform).sizeDelta = sizeDelta;
	}
}

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

public class BigPreview : MonoBehaviour
{
	private bool m_materialInited;

	private RectTransform m_sampleTransform;

	[SerializeField]
	private RectTransform m_content;

	[SerializeField]
	private RawImage m_image;

	[SerializeField]
	private RawImage m_filterImage;

	[SerializeField]
	private GameObject m_videoLock;

	public void Init(RectTransform sampleTransform, Texture2D tex, bool videoLocked, ISavedWorkData swd = null)
	{
		if (!this.m_materialInited)
		{
			this.m_image.material = new Material(Shader.Find("Custom/GreyTextureShader"));
			this.m_filterImage.material = new Material(Shader.Find("Custom/TilingShader"));
			this.m_materialInited = true;
		}
        this.m_sampleTransform = sampleTransform;
        this.m_image.texture = tex;
		if (swd != null)
		{
			Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
			texture2D.filterMode = (FilterMode)(swd.ImageInfo.Is3D ? 1 : 0);
			texture2D.LoadImage(swd.Preview);
			this.m_image.material.SetTexture("_ResTex", texture2D);
			this.m_image.enabled = true;
			Texture2D filter = MainManager.Instance.FilterManager.GetFilter(swd.FilterId);
			if (filter != null)
			{
				this.m_filterImage.material.mainTextureScale = new Vector2((float)texture2D.width, (float)texture2D.height);
				this.m_filterImage.texture = filter;
				this.m_filterImage.enabled = true;
				this.m_filterImage.material.SetFloat("RepeatX", texture2D.width);
				this.m_filterImage.material.SetFloat("RepeatY", texture2D.height);
			}
			else
			{
				this.m_filterImage.enabled = false;
			}
		}
		else
		{
			this.m_filterImage.enabled = false;
			this.m_image.uvRect = new Rect(0f, 0f, 1f, 1f);
			this.m_image.material.SetTexture("_ResTex", null);
			this.m_filterImage.uvRect = this.m_image.uvRect;
		}
		this.m_videoLock.SetActive(false);
		base.gameObject.SetActive(true);
		base.StartCoroutine(this.FlyCoroutine());
	}

	public void Close()
	{
		base.gameObject.SetActive(false);
	}


	private IEnumerator FlyCoroutine()
	{
		this.m_content.sizeDelta = this.m_sampleTransform.rect.max - this.m_sampleTransform.rect.min;
		this.m_content.pivot = this.m_sampleTransform.pivot;
		this.m_content.position = this.m_sampleTransform.position;
		var resPivot = new Vector2(0.5f, 1f);
		this.m_content.pivot = resPivot;
		var deltaPivot = resPivot - this.m_sampleTransform.pivot; 
		this.m_content.anchoredPosition += new Vector2(deltaPivot.x * this.m_sampleTransform.rect.width, deltaPivot.y * this.m_sampleTransform.rect.height);
		var resPos = Vector2.zero;
		var resSizeDelta = this.m_content.sizeDelta * 2f;
		var time = 0.2f;
		var speed = (resPos - this.m_content.anchoredPosition) / time;
		var sizeDeltaSpeed = (resSizeDelta - this.m_content.sizeDelta) / time;
		yield return null;

		while (true)
		{
			var deltaTime = Mathf.Min(Time.deltaTime, 0.05f);
			time -= deltaTime;
			if (time < 0f)
			{
				this.m_content.anchoredPosition = resPos;
				this.m_content.sizeDelta = resSizeDelta;
				yield break;
			} 
			this.m_content.anchoredPosition += speed * deltaTime; 
			this.m_content.sizeDelta += sizeDeltaSpeed * deltaTime;
			yield return null;
		}
	}

}

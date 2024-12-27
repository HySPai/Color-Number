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
using UnityEngine;

public class BrushTypeSwitcher : MonoBehaviour
{
	[SerializeField]
	private RectTransform m_content;

	[SerializeField]
	private GameObject m_singualarImage;

	[SerializeField]
	private GameObject m_pluralImage;

	private void Start()
	{
		ColorizationModeModel colorizationModeModel = WorkbookModel.Instance.ColorizationModeModel;
		colorizationModeModel.OnSpaceTypeChanged = (Action<ColorizationModeModel>)Delegate.Combine(colorizationModeModel.OnSpaceTypeChanged, new Action<ColorizationModeModel>(this.OnStateChangedHandler));
	}

	private void OnStateChangedHandler(ColorizationModeModel model)
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.ChangeSpriteCoroutine(model));
	}

	private IEnumerator ChangeSpriteCoroutine(ColorizationModeModel model)
	{
		Vector2 openedPos = Vector2.zero;
		Vector2 sizeDelta = ((RectTransform)base.transform).sizeDelta;
		Vector2 closedPos = new Vector2(0f, sizeDelta.y);
		float time = 0.1f;
		Vector2 speed = closedPos / time;
		yield return null;

		var deltaTime = Mathf.Min(0.05f, Time.deltaTime);
		var delta = speed * deltaTime;
		Vector2 vector2 = this.m_content.anchoredPosition + delta;
		if (vector2.y > closedPos.y)
		{
			this.m_content.anchoredPosition = closedPos;
			switch (model.CurrentSpaceType)
			{
				case ColorizationModeModel.BrushType.Singular:
					this.m_singualarImage.SetActive(true);
					this.m_pluralImage.SetActive(false);
					break;
				case ColorizationModeModel.BrushType.Plural:
					this.m_singualarImage.SetActive(false);
					this.m_pluralImage.SetActive(true);
					break;
			}
			yield return null;

			deltaTime = Mathf.Min(0.05f, Time.deltaTime);
			delta = -speed * deltaTime;
			Vector2 vector = this.m_content.anchoredPosition + delta;
			if (vector.y < openedPos.y)
			{
				this.m_content.anchoredPosition = openedPos;
				yield break;
			}
			RectTransform content = this.m_content;
			content.anchoredPosition += delta;
			yield return null;
		}
		RectTransform content2 = this.m_content;
		content2.anchoredPosition += delta;
		yield return null;
	}

	public void Click()
	{
		WorkbookModel.Instance.ColorizationModeModel.ChangeBrushType();
	}
}

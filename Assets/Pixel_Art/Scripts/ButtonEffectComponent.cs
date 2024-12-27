/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/



using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public class ButtonEffectComponent : MonoBehaviour
{
	private Color m_tempColor;

	[SerializeField]
	private List<ButtonEffect> m_elements;

	public void ButtonDownHandler()
	{
		if (base.enabled)
		{
			foreach (ButtonEffect element in this.m_elements)
			{
				if (element.element != null)
				{
					switch (element.buttonEffectType)
					{
						case ButtonEffectType.Color:
							element.element.color = element.activeColor;
							break;
						case ButtonEffectType.HighlightColor:
							this.m_tempColor = element.element.color;
							element.element.color = element.activeColor;
							break;
						case ButtonEffectType.Sprite:
							((Image)element.element).sprite = element.activeSprite;
							break;
						case ButtonEffectType.OnOff:
							element.element.gameObject.SetActive(false);
							break;
						case ButtonEffectType.OffOn:
							element.element.gameObject.SetActive(true);
							break;
					}
				}
			}
		}
	}

	public void ButtonUpHandler()
	{
		if (base.enabled)
		{
			foreach (ButtonEffect element in this.m_elements)
			{
				if (element.element != null)
				{
					switch (element.buttonEffectType)
					{
						case ButtonEffectType.Color:
							element.element.color = element.defaultColor;
							break;
						case ButtonEffectType.HighlightColor:
							element.element.color = this.m_tempColor;
							break;
						case ButtonEffectType.Sprite:
							((Image)element.element).sprite = element.defaultSprite;
							break;
						case ButtonEffectType.OnOff:
							element.element.gameObject.SetActive(true);
							break;
						case ButtonEffectType.OffOn:
							element.element.gameObject.SetActive(false);
							break;
					}
				}
			}
		}
	}
}



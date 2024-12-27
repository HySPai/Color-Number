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

public class RateUsWindow : MonoBehaviour
{
	private Action m_closeHandler;

	public static bool NeedShow(RateUsReason reason)
	{
		return false;
	}

	public void Init(Action closeHandler)
	{
		this.m_closeHandler = closeHandler;
		base.gameObject.SetActive(true);
		AppData.AddNewRateUsView();
	}

	public void YesButtonClick()
	{
		RateUsTool.OpenRateUs();
		AppData.AppRated = true;
	}

	public void Close()
	{
		base.gameObject.SetActive(false);
		this.m_closeHandler.SafeInvoke();
		this.m_closeHandler = null;
	}
}



/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/


using System;

namespace Assets.Scripts.Navigation
{
	internal class BackButtonManager : UnitySingleton<BackButtonManager>
	{
		private bool _pause;

		public Action BackButtonAction;

		public event Action PopupBackButtonAction;

		private BackButtonManager()
		{
		}

		public void SetPause(bool value)
		{
			this._pause = value;
		}

		private void OnPopupBackButtonAction()
		{
			Action popupBackButtonAction = this.PopupBackButtonAction;
			if (popupBackButtonAction != null)
			{
				popupBackButtonAction();
			}
		}
	}
}



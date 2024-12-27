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

namespace Assets.Scripts.Navigation.Scenes.Map
{
	internal class MenuScene : BaseScene
	{
		public override SceneType SceneType
		{
			get
			{
				return SceneType.Menu;
			}
		}

		public override void OnNavigatedTo(NavigationArgs args)
		{
			MenuNavigationArgs menuNavigationArgs = args as MenuNavigationArgs;
			this.SubscribeToPopupClose();
		}

		public override void OnNavigatedFrom()
		{
			BackButtonManager instance = UnitySingleton<BackButtonManager>.Instance;
			instance.BackButtonAction = (Action)Delegate.Remove(instance.BackButtonAction, new Action(((BaseScene)this).BackButtonActions));
			this.UnsubscribeFromPopupClose();
		}

		public override void BackButtonActions()
		{
			Application.Quit();
		}

		private void SubscribeToPopupClose()
		{
		}

		private void UnsubscribeFromPopupClose()
		{
		}
	}
}



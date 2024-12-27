/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

namespace Assets.Scripts.Navigation.Scenes.Game
{
	internal class WorkbookNavigationArgs : NavigationArgs
	{
		private const string GameSceneName = "2DScene";

		public ImageInfo ImageInfo;

		public MainMenuPage Page;

		public SavedWorkData SavedWorkData;

		public int Part
		{
			get;
			set;
		}

		public WorkbookNavigationArgs(ImageInfo imageInfo, MainMenuPage page)
			: base(SceneType.Workbook, "2DScene")
		{
			this.ImageInfo = imageInfo;
			this.Page = page;
		}

		public WorkbookNavigationArgs(SavedWorkData savedWorkData = null)
			: base(SceneType.Workbook, "2DScene")
		{
			this.SavedWorkData = savedWorkData;
		}

		public WorkbookNavigationArgs(ImageInfo imageInfo, int part, SavedWorkData savedWorkData = null)
			: base(SceneType.Workbook, "2DScene")
		{
			this.ImageInfo = imageInfo;
			this.SavedWorkData = savedWorkData;
			this.Part = part;
		}
	}
}



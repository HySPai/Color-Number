/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using Assets.Scripts.Navigation.Scenes;

public class GameNavigationArgs : NavigationArgs
{
	private const string MapSceneName = "3DScene";

	public CashImage3D Data { get; private set; }

	public ImageInfo ImageInfo { get; private set; }

	public SavedWorkData3D SavedWorkData { get; private set; }

	public ImageOpenType ImageOpenType { get; private set; }

	public GameNavigationArgs()
		: base(SceneType.Game, "3DScene")
	{
	}

	public GameNavigationArgs(ImageInfo imageinfo, CashImage3D data, SavedWorkData3D savedWorkData, ImageOpenType imageOpenType)
		: base(SceneType.Game, "3DScene")
	{
		this.Data = data;
		this.ImageInfo = imageinfo;
		this.SavedWorkData = savedWorkData;
		this.ImageOpenType = imageOpenType;
	}
}



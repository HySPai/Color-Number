/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

public class WorkbookModel
{
	public static WorkbookModel Instance { get; private set; }

	public ColorizationModeModel ColorizationModeModel { get; private set; }

	public CurrentColorModel CurrentColorModel { get; private set; }

	public TutorialModel TutorialModel { get; private set; }

	public SpecBoostersModel SpecBoostersModel { get; private set; }

	public WorkbookModel()
	{
		this.ColorizationModeModel = new ColorizationModeModel();
		this.CurrentColorModel = new CurrentColorModel();
		this.TutorialModel = new TutorialModel();
		this.SpecBoostersModel = new SpecBoostersModel();
	}

	public static void Init()
	{
		WorkbookModel.Instance = new WorkbookModel();
	}
}



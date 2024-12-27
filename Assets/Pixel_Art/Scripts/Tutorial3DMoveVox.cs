/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using UnityEngine;

public class Tutorial3DMoveVox : MonoBehaviour
{
	private void Start()
	{
		VoxCubeItem[] componentsInChildren = ((Component)base.transform).GetComponentsInChildren<VoxCubeItem>();
		VoxCubeItem[] array = componentsInChildren;
		foreach (VoxCubeItem voxCubeItem in array)
		{
			voxCubeItem.gameObject.AddComponent<Tutorial3DMoveVoxCube>();
		}
		base.transform.localScale = Vector3.one;
		Object.FindObjectOfType<TutorialWindow>().transform.localScale = Vector3.one;
	}
}



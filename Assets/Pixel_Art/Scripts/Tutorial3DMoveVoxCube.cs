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

public class Tutorial3DMoveVoxCube : MonoBehaviour
{
	private static List<Color32> colorsList = new List<Color32> {
		new Color32(240, 116, 28, 1),
		new Color32(236, 144, 24, 1),
		new Color32(240, 240, 240, 1),
		new Color32(187, 187, 187, 1),
		new Color32(34, 34, 34, 1),
		new Color32(220, 77, 110, 1)
	};

	private static Dictionary<int, Material> s_materials = new Dictionary<int, Material>();

	private void Start()
	{
		MeshRenderer componentInChildren = base.GetComponentInChildren<MeshRenderer>();
		int colorIndex = base.GetComponent<VoxCubeItem>().ColorIndex;
		if (!Tutorial3DMoveVoxCube.s_materials.ContainsKey(colorIndex))
		{
			Color32 c = Tutorial3DMoveVoxCube.colorsList[colorIndex - 1];
			Color color = c;
			Color value = new Color(color.grayscale + 0.2f, color.grayscale + 0.2f, color.grayscale + 0.2f);
			Shader shader = Shader.Find("Custom/StandardVertex");
			Material material = new Material(shader);
			material.SetColor("_Color", value);
			Tutorial3DMoveVoxCube.s_materials.Add(colorIndex, material);
		}
		componentInChildren.sharedMaterial = Tutorial3DMoveVoxCube.s_materials[colorIndex];
	}
}



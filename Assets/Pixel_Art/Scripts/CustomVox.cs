/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[Serializable]
public class CashImage3D
{
	private byte[] vox;
	private long MaxVisibleCount;
	private IDictionary<int, int> colorCount;

	public SerializableVector3 Position
	{
		get
		{
			return new SerializableVector3(0, 0, 0);
		}
	}

	public SerializableQuaternion Rotation
	{
		get
		{
			var q = Quaternion.Euler(22, -35, 0);
			return new SerializableQuaternion(q.x, q.y, q.z, q.w);
		}
	}

	public byte[] Vox
	{
		get
		{
			return this.vox;
		}
	}

	public IDictionary<int, int> ColorsCount
	{
		get
		{
			return this.colorCount;
		}
	}

	public long CubesCount
	{
		get
		{
			return this.MaxVisibleCount;
		}
	}

	public float CamSize { get; private set; }

	public CashImage3D(CashImage ci)
	{
		vox = ci.Bytes;
	}

	public void Init(MVMainChunk mv)
	{
		int sizeX = mv.sizeX;
		int sizeY = mv.sizeY;
		int sizeZ = mv.sizeZ;
		int size = Mathf.Max(sizeX, Mathf.Max(sizeY, sizeZ));
		float initialDistance = Mathf.Sqrt((float)(sizeX * sizeX + sizeY * sizeY + sizeZ * sizeZ));
		this.CamSize = initialDistance;

		MaxVisibleCount = 0;
		colorCount = new Dictionary<int, int>();

		int count = 0;
		for (int i = 0; i < MVImporter.usedPallate.Count; i++)
		{
			int colorIndex = MVImporter.usedPallate[i];
			count = MVImporter.colorCountArray[colorIndex];
			colorCount[i+1] = count;
			MaxVisibleCount += count;
		} 
	}
}
//[Serializable]
//public class CustomVox
//{
//	private byte[] vox;

//	private IDictionary<int, int> colorCount;

//	private long MaxVisibleCount;

//	private string JSONCustomVoxPositionKey = "VoxPosition";

//	private string JSONCustomVoxRotationKey = "VoxRotation";

//	private string JSONCustomVoxCountVisibleKey = "VoxCountVisible";

//	private string JSONCustomVoxColorCountKey = "VoxColorCount";

//	private string JSONCustomVoxOrigrnalKey = "VoxOrigrnal";

//	private string JSONCustomVoxCamSizeKey = "VoxCamSize";

//	public SerializableVector3 Position { get; set; }

//	public SerializableQuaternion Rotation { get; set; }

//	public byte[] Vox
//	{
//		get
//		{
//			return this.vox;
//		}
//	}

//	public IDictionary<int, int> ColorsCount
//	{
//		get
//		{
//			return this.colorCount;
//		}
//	}

//	public long CubesCount
//	{
//		get
//		{
//			return this.MaxVisibleCount;
//		}
//	}

//	public float CamSize { get; private set; }

//	private CustomVox()
//	{
//	}

//	public CustomVox(Dictionary<string, object> dict)
//	{
//		if (dict != null)
//		{
//			if (dict[this.JSONCustomVoxPositionKey] != null)
//			{
//				Dictionary<string, object> dictionary = (Dictionary<string, object>)dict[this.JSONCustomVoxPositionKey];
//				this.Position = new SerializableVector3((float)(long)dictionary["x"], (float)(long)dictionary["y"], (float)(long)dictionary["z"]);
//			}
//			if (dict[this.JSONCustomVoxRotationKey] != null)
//			{
//				Dictionary<string, object> dictionary2 = (Dictionary<string, object>)dict[this.JSONCustomVoxRotationKey];
//				this.Rotation = new SerializableQuaternion((float)(double)dictionary2["x"], (float)(double)dictionary2["y"], (float)(double)dictionary2["z"], (float)(double)dictionary2["w"]);
//			}
//			if (dict[this.JSONCustomVoxCountVisibleKey] != null)
//			{
//				this.MaxVisibleCount = (long)dict[this.JSONCustomVoxCountVisibleKey];
//			}
//			if (dict[this.JSONCustomVoxOrigrnalKey] != null)
//			{
//				this.vox = (from a in (List<object>)dict[this.JSONCustomVoxOrigrnalKey]
//							select (byte)(long)a).ToArray();
//			}
//			if (dict[this.JSONCustomVoxColorCountKey] != null)
//			{
//				this.colorCount = new Dictionary<int, int>();
//				Dictionary<string, object> dictionary3 = (Dictionary<string, object>)dict[this.JSONCustomVoxColorCountKey];
//				foreach (KeyValuePair<string, object> item in dictionary3)
//				{
//					this.colorCount.Add(new KeyValuePair<int, int>(int.Parse(item.Key), int.Parse((string)item.Value)));
//				}
//			}
//			if (dict[this.JSONCustomVoxCamSizeKey] != null)
//			{
//				this.CamSize = (float)(double)dict[this.JSONCustomVoxCamSizeKey];
//			}
//		}
//	}
//}



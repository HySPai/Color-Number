/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System;

[Serializable]
public class MVVoxelChunk
{
	public byte[,,] voxels;

	public MVFaceCollection[] faces;

	public int x;

	public int y;

	public int z;

	public int sizeX
	{
		get
		{
			return this.voxels.GetLength(0);
		}
	}

	public int sizeY
	{
		get
		{
			return this.voxels.GetLength(1);
		}
	}

	public int sizeZ
	{
		get
		{
			return this.voxels.GetLength(2);
		}
	}

	public MVVoxelChunk()
	{
	}
}

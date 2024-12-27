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
using VoxDLL;

public class GameController : UnitySingleton<GameController>
{
	public Action<Color, int> OnCountLeft;
	public Action<Color> OnColorComplete;
	public Action<Color> OnPaintedPixel;

	public Action OnComplete;

	private List<VoxCubeItem> _voxCubeItems;

	private List<bool> _voxCubeProgress;

	private CashImage3D _vox;

	private int colorsCounter;

	private List<Color> _colors;

	public ColorSettings ColorSettings { get; set; }

	public bool IsLongClick { get; set; }

	public Camera VideoCamera { get; set; }

	public List<VoxCubeItem> VoxCubeItems
	{
		get
		{
			return this._voxCubeItems;
		}
		set
		{
			if (this._voxCubeItems == null)
			{
				this._voxCubeItems = new List<VoxCubeItem>();
				this._voxCubeItems = value;
			}
		}
	}

	public List<bool> VoxCubeProgress
	{
		get
		{
			return this._voxCubeProgress;
		}
		private set
		{
			this._voxCubeProgress = value;
		}
	}

	public event Action<List<Color>> VoxLoadComplete;

	public event Action<float> DepthCameraChange;

	public void Initilized(Color[] colorPallete)
	{
		this.ColorSettings = new ColorSettings(colorPallete);
		this._colors = this.ColorSettings.GetColorPalleteList();
		this.OnVoxLoadComplete(this._colors);
		 
		if (DailyGame.IsDailyArt())
		{ 
			DailyGame.UpdateGUI((int)this._vox.CubesCount);
		}
	}

	public void SetCustomData(CashImage3D vox)
	{
		this._vox = vox;
	}

	public void SetCurrentIndexCollorPallete(int index)
	{
		Loader3D.CurrentIndex = index;
		this.SetHighLightColorInCubeItems();
	}

	public void ChangeCamDepthPosition(float z)
	{
		this.SetTransparentColorByOffsetZ(z);
		this.OnDepthCameraChange(z);
	}

	public void SetCubeItemProgress(VoxCubeItem item)
	{
		int index = this.VoxCubeItems.IndexOf(item);
		this._voxCubeProgress[index] = true;
	}
	public bool CheckColorLeft(int colorIndex, bool soundEnabled)
	{
		if (OnCountLeft != null)
		{
			this.OnCountLeft(this._colors[colorIndex - 1], this._vox.ColorsCount[colorIndex]);
			return this._vox.ColorsCount[colorIndex] > 0;
		}
		return false;
	}
	public void OnCubeColored(int colorIndex, bool soundEnabled = true)
	{
		if (colorIndex > 0)
		{
			this._vox.ColorsCount[colorIndex] = this._vox.ColorsCount[colorIndex] - 1;

			CheckColorLeft(colorIndex, soundEnabled);
			if (soundEnabled)
			{
				if (colorIndex < this._colors.Count)
					OnPaintedPixel(this._colors[colorIndex]);
				else
					OnPaintedPixel(this._colors[colorIndex - 1]);
			}
			if (this._vox.ColorsCount[colorIndex] <= 0)
			{
				this.OnColorComplete.SafeInvoke(this._colors[colorIndex - 1]);
				if (soundEnabled)
					AudioManager.Instance.PlayCompleteColor();
			}
			this.colorsCounter++;
			if (this.colorsCounter == this._vox.CubesCount)
			{
				UnitySingleton<ProgressManager>.instance.SetComplete();
				this.OnComplete.SafeInvoke();
				AudioManager.Instance.PlayVictory();
				UnitySingleton<ProgressManager>.instance.SaveWork(delegate (SavedWorkData3D swd)
				{
					AnalyticsManager.Instance.ImageDone(swd.ImageInfo, this.colorsCounter, this._colors.Count);
				});
			}
		}
	}

	public void UpdateColors()
	{
		this.SetProgressOnCubes();
	} 

	private void SetHighLightColorInCubeItems()
	{
		if (AppData.BulbMode)
		{
			foreach (VoxCubeItem voxCubeItem in this.VoxCubeItems)
			{
				voxCubeItem.SetHighLightColorByFaceCube();
			}
		}
	}

	private void SetTransparentColorByOffsetZ(float z)
	{
		if (this.VoxCubeItems != null)
		{
			foreach (VoxCubeItem voxCubeItem in this.VoxCubeItems)
			{
				voxCubeItem.SetTransparentColorByOffsetZ(z);
			}
		}
	}

	private void SetProgressOnCubes()
	{
		SavedWorkData3D savedWorkData3D = UnitySingleton<ProgressManager>.instance.LoadProgressTest();
		List<bool> list = null;
		if (savedWorkData3D != null)
		{
			list = savedWorkData3D.Progress;
		}
		if (list != null && list.Count() > 0)
		{
			this._voxCubeProgress = list;
			for (int i = 0; i < this._voxCubeProgress.Count(); i++)
			{
				if (this._voxCubeProgress[i])
				{
					UnitySingleton<GameController>.Instance.OnCubeColored(this._voxCubeItems[i].ColorIndex, false);
					this._voxCubeItems[i].SetProgressForCube();
				}
			}
		}
		else if (this._voxCubeProgress == null)
		{
			this._voxCubeProgress = new List<bool>();
			for (int j = 0; j < this._voxCubeItems.Count(); j++)
			{
				this._voxCubeProgress.Add(false);
			}
		}
		foreach (VoxCubeItem voxCubeItem in this._voxCubeItems)
		{
			if (voxCubeItem.ColorIndex == 0)
			{
				this.SetCubeItemProgress(voxCubeItem);
			}
		}
		UnitySingleton<ProgressManager>.instance.StartSaveProcess();
	}

	public void OnVoxLoadComplete(List<Color> list)
	{
		Action<List<Color>> voxLoadComplete = this.VoxLoadComplete;
		if (voxLoadComplete != null)
		{
			voxLoadComplete(list);
		}
	}

	private void OnDepthCameraChange(float z)
	{
		Action<float> depthCameraChange = this.DepthCameraChange;
		if (depthCameraChange != null)
		{
			depthCameraChange(z);
		}
	}
}



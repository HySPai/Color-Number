/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VoxDLL;

public class VoxCubeItem : MonoBehaviour
{
	public bool isColored;

	public bool isError;

	public bool isStartCorut;

	public int ColorIndex = -1;

	private List<MeshRenderer> _renderers;

	private MeshRenderer _saveCubeRenderer;

	private UnityEngine.Mesh _mesh;

	private ColorSettings _colorSettings;

	public bool isRender = true;

	public UnityEngine.Color Color { get; set; }

	public UnityEngine.Mesh Mesh
	{
		get
		{
			return this._mesh;
		}
	}

	public VoxCubeItem()
	{
	}

	public void CheatTochCubeItem()
	{
		if (this.isColored)
		{
			return;
		}
		this.SetSuccessColorCube();
	} 

	public void Init(ColorSettings colorSettings, UnityEngine.Color color, UnityEngine.Mesh mesh, bool[] isVisible)
	{
		if (colorSettings == null)
		{
			Debug.LogError("colorSettings is null");
		}
		Transform transform = base.transform.Find("Cube");
		this._renderers = transform.GetComponents<MeshRenderer>().ToList<MeshRenderer>();
		Transform transform1 = base.transform.Find("save_cube");
		this._saveCubeRenderer = transform1.GetComponent<MeshRenderer>(); 
		this.InitResources();
		this._colorSettings = colorSettings;
		this._renderers[0].gameObject.SetActive(((IEnumerable<bool>)isVisible).Any<bool>((bool a) => a));
		this._saveCubeRenderer.gameObject.SetActive(((IEnumerable<bool>)isVisible).Any<bool>((bool a) => a)); 
		this.Color = color;
		//this.ColorIndex = this._colorSettings.GetNumberByColor(this.Color) + 1;
		this._mesh = mesh;
		if (this.ColorIndex == 0)
		{
			this.SetSuccessColorCube();
			return;
		}
		this.SetNumberByFaceCube(this.ColorIndex != Loader3D.CurrentIndex);
		this._saveCubeRenderer.material.color = (this.Color);
		UnityEngine.Color color1 = this.Color;
		float _grayscale = color1.grayscale + 0.4f;
		UnityEngine.Color color2 = new UnityEngine.Color(_grayscale, _grayscale, _grayscale);
		this._saveCubeRenderer.material.SetColor("_BaseColor", color2); 
	}

	private void InitResources()
	{
		Shader shader = Shader.Find("Custom/3DColour");
		Material material = new Material(shader);
		material.SetFloat("_Blend", 1f);
		material.SetColor("_Color", UnityEngine.Color.white);
		foreach (MeshRenderer _renderer in this._renderers)
		{
			_renderer.material = material;
		}
		Shader.Find("Custom/3DColour");
		Material material1 = new Material(shader);
		material1.SetFloat("_Blend", 0f);
		material1.SetColor("_Color", UnityEngine.Color.white);
		this._saveCubeRenderer.material = material1;
		Shader.Find("Custom/3DColour");
		Material material2 = new Material(shader);
		material2.SetFloat("_Blend", 0f);
		material2.SetColor("_Color", UnityEngine.Color.white); 
	}

	private IEnumerator SetErrorColorByFaceCube()
	{
		VoxCubeItem voxCubeItem = this;
		List<UnityEngine.Color> colors = new List<UnityEngine.Color>();
		List<int> nums = new List<int>();
		foreach (MeshRenderer _renderer in voxCubeItem._renderers)
		{
			colors.Add(_renderer.material.color);
			UnityEngine.Color colorByIndex = voxCubeItem._colorSettings.GetColorByIndex(Loader3D.CurrentIndex - 1);
			nums.Add(_renderer.material.GetInt("_Highlighted"));
			_renderer.material.SetInt("_Highlighted", 0);
			_renderer.material.color = (colorByIndex);
		}
		yield return new WaitForSeconds(0.1f);
		int index = 0;
		foreach (MeshRenderer meshRenderer in voxCubeItem._renderers)
		{
			voxCubeItem._colorSettings.GetColorByIndex(Loader3D.CurrentIndex - 1);
			meshRenderer.material.SetInt("_Highlighted", nums[index]);
			meshRenderer.material.color = (colors[index]);
			index++;
		}
		voxCubeItem.isStartCorut = false;
	}

	public void SetHighLightColorByFaceCube()
	{
		if (this.isColored)
		{
			return;
		}
		this.SetNumberByFaceCube(this.ColorIndex == Loader3D.CurrentIndex);
	}

	private Texture2D SetHighLightTextureOnFaceCube()
	{
		Texture2D texture2D;
		try
		{
			texture2D = (Texture2D)Resources.Load(string.Concat("Textures/Grey/", this.ColorIndex.ToString()));
		}
		catch (Exception exception)
		{
			Debug.LogError(string.Concat("Error = ", exception.Message));
			texture2D = null;
		}
		return texture2D;
	}

	private Texture2D SetNormalTextureOnFaceCube()
	{
		Texture2D texture2D;
		try
		{
			texture2D = (Texture2D)Resources.Load(string.Concat("Textures/Black/", this.ColorIndex.ToString()));
		}
		catch (Exception exception)
		{
			Debug.LogError(string.Concat("Error = ", exception.Message));
			texture2D = null;
		}
		return texture2D;
	}

	private void SetNumberByFaceCube(bool highlight)
	{
		UnityEngine.Color color;
		foreach (MeshRenderer _renderer in this._renderers)
		{
			if (highlight)
			{
				_renderer.material.mainTexture = (this.SetHighLightTextureOnFaceCube());
				color = this.Color;
				float single = Mathf.Min(color.grayscale + 0.3f, 0.7f);
				UnityEngine.Color color1 = new UnityEngine.Color(single, single, single);
				_renderer.material.SetColor("_BaseColor", color1);
				_renderer.material.SetInt("_Highlighted", 1);
			}
			else
			{
				_renderer.material.mainTexture = (this.SetNormalTextureOnFaceCube());
				color = this.Color;
				float _grayscale = color.grayscale + 0.4f;
				UnityEngine.Color color2 = new UnityEngine.Color(_grayscale, _grayscale, _grayscale);
				_renderer.material.SetColor("_BaseColor", color2);
				_renderer.material.SetInt("_Highlighted", 0);
			}
		}
	}

	public bool SetProgressForCube()
	{
		if (this.isColored)
		{
			return false;
		}
		this.SetSuccessColorCube();
		return true;
	}

	private void SetSuccessColorCube()
	{
		this.isColored = true;
		(new Material(Shader.Find("Custom/3DColour"))).SetFloat("_Blend", 1f);
		foreach (MeshRenderer meshRenderer in new List<MeshRenderer>(base.GetComponentsInChildren<MeshRenderer>()))
		{
			meshRenderer.sharedMaterial.color = (this.Color);
			meshRenderer.sharedMaterial.SetColor("_BaseColor", this.Color);
			meshRenderer.sharedMaterial.mainTexture = (null);
		}
	}

	public void SetTransparentColorByOffsetZ(float transZ)
	{
		if (this.isColored)
		{
			return;
		}
		foreach (MeshRenderer _renderer in this._renderers)
		{
			UnityEngine.Color color = _renderer.material.GetColor("_Color");
			color.a = transZ;
			_renderer.material.SetColor("_Color", color);
		}
	}

	public void SetVisible(bool isVisible)
	{
		foreach (MeshRenderer meshRenderer in new List<MeshRenderer>(base.gameObject.GetComponentsInChildren<MeshRenderer>()))
		{
			if (meshRenderer == null)
			{
				continue;
			}
			meshRenderer.enabled = (isVisible);
		}
	}

	public int TochCubeItem()
	{
		if (this.isColored)
		{
			return -1;
		}
		if (this.ColorIndex == Loader3D.CurrentIndex)
		{
			this.SetSuccessColorCube();
			return 1;
		}
		if (this.isStartCorut)
		{
			return 0;
		}
		this.isStartCorut = true;
		base.StartCoroutine(this.SetErrorColorByFaceCube());
		return 0;
	}
}

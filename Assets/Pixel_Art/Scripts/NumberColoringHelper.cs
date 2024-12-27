/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/



using UnityEngine;

public class NumberColoringHelper
{
	public static void InitObject(GameObject gameObject, out MeshRenderer gridMr, out MeshRenderer grayMr, out Transform numbersContentParent, out Material resMaterial, out Camera mainCamera, out CameraManager cameraManager, out GameObject numberPrefab)
	{
		GameObject gameObject2 = new GameObject("loupeBack");
		gameObject2.transform.SetParent(gameObject.transform);
		gameObject2.transform.localScale = new Vector3(20f, 20f, 1f);
		SpriteRenderer component = gameObject2.GetComponent<SpriteRenderer>();
		if (component == null)
		{
			component = gameObject2.AddComponent<SpriteRenderer>();
		}
		gameObject2.layer = LayerMask.NameToLayer("Loupe");
		GameObject gameObject3 = default(GameObject);
		MeshFilter meshFilter = default(MeshFilter);
		NumberColoringHelper.CreateOrResetObject("grid", out gameObject3, out gridMr, out meshFilter);
		gameObject3.transform.SetParent(gameObject.transform);
		Texture2D mainTexture = (Texture2D)Resources.Load("grid_cell");
		gridMr.sharedMaterial = new Material(Shader.Find("Custom/TilingShader"));
		gridMr.sharedMaterial.mainTexture = mainTexture;
		GameObject gameObject4 = default(GameObject);
		MeshFilter meshFilter2 = default(MeshFilter);
		NumberColoringHelper.CreateOrResetObject("gray_texture", out gameObject4, out grayMr, out meshFilter2);
		gameObject4.transform.SetParent(gameObject.transform);
		BoxCollider component2 = gameObject4.GetComponent<BoxCollider>();
		if (component2 == null)
		{
			gameObject4.AddComponent<BoxCollider>();
		}
		grayMr.sharedMaterial = new Material(Shader.Find("Custom/GrayShader"));
		numbersContentParent = gameObject.transform.Find("numbers_content");
		if (numbersContentParent == null)
		{
			numbersContentParent = new GameObject("numbers_content").transform;
			numbersContentParent.SetParent(gameObject.transform);
		}
		numbersContentParent.transform.localPosition = new Vector3(0f, 0f, -0.5f);
		NumberColoringHelper.CreateTextPrefab(gameObject.transform, out numberPrefab);
		GameObject gameObject5 = default(GameObject);
		MeshRenderer meshRenderer = default(MeshRenderer);
		MeshFilter meshFilter3 = default(MeshFilter);
		NumberColoringHelper.CreateOrResetObject("res_texture", out gameObject5, out meshRenderer, out meshFilter3);
		gameObject5.transform.SetParent(gameObject.transform);
		gameObject5.transform.localPosition = new Vector3(0f, 0f, -1f);
		meshRenderer.sharedMaterial = new Material(Shader.Find("tk2d/BlendVertexColor"));
		resMaterial = meshRenderer.sharedMaterial;
		gameObject5.layer = LayerMask.NameToLayer("ResTexture");
		mainCamera = Camera.main;
		cameraManager = Object.FindObjectOfType<CameraManager>();
	}

	public static void CreateHighLightedGrid(GameObject gameObject, out MeshRenderer highlightedGridRenderer)
	{
		GameObject gameObject2 = default(GameObject);
		MeshFilter meshFilter = default(MeshFilter);
		NumberColoringHelper.CreateOrResetObject("highlighted_grid", out gameObject2, out highlightedGridRenderer, out meshFilter);
		gameObject2.transform.SetParent(gameObject.transform);
		Texture2D value = (Texture2D)Resources.Load("grid_cell");
		highlightedGridRenderer.sharedMaterial = new Material(Shader.Find("My/HighlightedGridShader2"));
		highlightedGridRenderer.sharedMaterial.SetTexture("_ResTex", value);
	}

	private static void CreateOrResetObject(string goName, out GameObject go, out MeshRenderer mr, out MeshFilter mf)
	{
		go = GameObject.Find(goName);
		if (go == null)
		{
			go = new GameObject(goName);
			go.transform.position = Vector2.zero;
		}
		mr = go.GetComponent<MeshRenderer>();
		if (mr == null)
		{
			mr = go.AddComponent<MeshRenderer>();
		}
		mf = go.GetComponent<MeshFilter>();
		if (mf == null)
		{
			mf = go.AddComponent<MeshFilter>();
		}
		mf.sharedMesh = new Mesh();
		mf.sharedMesh.vertices = new Vector3[4] {
			new Vector2(-1f, -1f),
			new Vector2(1f, -1f),
			new Vector2(-1f, 1f),
			new Vector2(1f, 1f)
		};
		mf.sharedMesh.uv = new Vector2[4] {
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		mf.sharedMesh.triangles = new int[6] {
			0,
			3,
			1,
			2,
			3,
			0
		};
	}

	private static void CreateTextPrefab(Transform parent, out GameObject numberPrefab)
	{
		string name = "number_prefab";
		Transform transform = parent.Find(name);
		if (transform == null)
		{
			transform = new GameObject(name).transform;
			transform.SetParent(parent);
			transform.position = Vector2.zero;
		}
		TextMesh textMesh = ((Component)transform).GetComponent<TextMesh>();
		if (textMesh == null)
		{
			textMesh = transform.gameObject.AddComponent<TextMesh>();
		}
		textMesh.color = Color.black;
		textMesh.characterSize = 0.005f;
		textMesh.fontSize = 50;
		textMesh.alignment = TextAlignment.Center;
		textMesh.anchor = TextAnchor.MiddleCenter;
		numberPrefab = transform.gameObject;
		numberPrefab.SetActive(false);
	}
}



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
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using VoxDLL;

public static class MVImporter
{
	private static Action<Color[]> ColorsPalleteLoadComplete;

	private static Action<List<VoxCubeItem>> CubesDataLoadComplete;

	private static ColorSettings colorSettings;

	public static List<int> usedPallate = new List<int>();

	public static int[] colorCountArray = new int[256];

	public static Material DefaultMaterial
	{
		get
		{
			return new Material(Shader.Find("Diffuse"));
		}
	}

	public static void AddColorSettings(ColorSettings settings)
	{
		MVImporter.colorSettings = settings;
	}

	public static GameObject CreateGameObject(Transform parent, Vector3 pos, string name, Mesh mesh, Material mat)
	{
		GameObject gameObject = Resources.Load("Prefabs/3D Cube") as GameObject;
		if (gameObject == null)
		{
			Debug.LogError("Prefabs/Cube not found");
		}
		GameObject gameObject1 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		gameObject1.name = (name);
		gameObject1.transform.SetParent(parent);
		gameObject1.transform.localPosition = (pos);
		return gameObject1;
	}

	public static GameObject[] CreateIndividualVoxelGameObjects(MVMainChunk chunk, Transform parent, Material mat, float sizePerVox)
	{
		return MVImporter.CreateIndividualVoxelGameObjectsForChunk(chunk.voxelChunk, chunk.palatte, parent, mat, sizePerVox);
	}

	public static GameObject[] CreateIndividualVoxelGameObjects(MVMainChunk chunk, Transform parent, Material mat, float sizePerVox, Vector3 origin)
	{
		return MVImporter.CreateIndividualVoxelGameObjectsForChunk(chunk.voxelChunk, chunk.palatte, parent, mat, sizePerVox, origin);
	}

	public static GameObject[] CreateIndividualVoxelGameObjectsForChunk(MVVoxelChunk chunk, Color[] palatte, Transform parent, Material mat, float sizePerVox)
	{
		Vector3 vector3 = new Vector3((float)chunk.sizeX / 2f, (float)chunk.sizeY / 2f, (float)chunk.sizeZ / 2f);
		return MVImporter.CreateIndividualVoxelGameObjectsForChunk(chunk, palatte, parent, mat, sizePerVox, vector3);
	}

	public static GameObject[] CreateIndividualVoxelGameObjectsForChunk(MVVoxelChunk chunk, Color[] palatte, Transform parent, Material mat, float sizePerVox, Vector3 origin)
	{
		List<GameObject> gameObjects = new List<GameObject>();
		List<VoxCubeItem> voxCubeItems = new List<VoxCubeItem>();

		for (int i = 0; i < chunk.sizeX; i++)
		{
			for (int j = 0; j < chunk.sizeY; j++)
			{
				for (int k = 0; k < chunk.sizeZ; k++)
				{
					if (chunk.voxels[i, j, k] != 0 && MVImporter.IsVoxelVisible(chunk, i, j, k))
					{
						int colorIndex = chunk.voxels[i, j, k] - 1;
						MVImporter.colorCountArray[colorIndex]++;
						if (MVImporter.usedPallate.IndexOf(colorIndex) == -1)
						{
							MVImporter.usedPallate.Add(colorIndex);
						}
					}
				}
			}
		}

		MVImporter.usedPallate.Sort();
		for (int i = 0; i < chunk.sizeX; i++)
		{
			for (int j = 0; j < chunk.sizeY; j++)
			{
				for (int k = 0; k < chunk.sizeZ; k++)
				{
					int index = chunk.voxels[i, j, k];
					if (index != 0 && MVImporter.IsVoxelVisible(chunk, i, j, k))
					{
						int colorNum = MVImporter.usedPallate.IndexOf(index - 1);

						float single = ((float)i - origin.x + 0.5f) * sizePerVox;
						float single1 = ((float)j - origin.y + 0.5f) * sizePerVox;
						float single2 = ((float)k - origin.z + 0.5f) * sizePerVox;
						Mesh mesh = MVImporter.CubeMeshWithColor(sizePerVox, palatte[index], colorNum);
						GameObject gameObject = MVImporter.CreateGameObject(parent, new Vector3(single, single1, single2), string.Format("Voxel ({0}, {1}, {2})", i, j, k), mesh, mat);
						VoxCubeItem voxCubeItem = gameObject.AddComponent<VoxCubeItem>();
						voxCubeItem.ColorIndex = colorNum + 1;
						bool[] flagArray = new bool[] {
							(j + 1 >= chunk.sizeY ? true : chunk.voxels[i, j + 1, k] == 0),
							(j - 1 < 0 ? true : chunk.voxels[i, j - 1, k] == 0),
							(k - 1 < 0 ? true : chunk.voxels[i, j, k - 1] == 0),
							(k + 1 >= chunk.sizeZ ? true : chunk.voxels[i, j, k + 1] == 0),
							(i - 1 < 0 ? true : chunk.voxels[i - 1, j, k] == 0),
							(i + 1 >= chunk.sizeX ? true : chunk.voxels[i + 1, j, k] == 0) };
						voxCubeItem.Init(MVImporter.colorSettings, palatte[index-1], mesh, flagArray);
						gameObject.tag = ("Player");
						voxCubeItems.Add(voxCubeItem);
						gameObjects.Add(gameObject); 
					} 
				}
			}
		} 
		if (MVImporter.ColorsPalleteLoadComplete != null)
		{
			var colors = new Color[MVImporter.usedPallate.Count];
			for (int i = 0; i < MVImporter.usedPallate.Count; i++)
			{
				colors[i] = palatte[MVImporter.usedPallate[i]];
			}
			MVImporter.ColorsPalleteLoadComplete(colors);
		}
		Debug.Log(string.Concat("Cube Items count = ", voxCubeItems.Count<VoxCubeItem>()));
		if (MVImporter.CubesDataLoadComplete != null)
		{
			MVImporter.CubesDataLoadComplete(voxCubeItems);
		}

		return gameObjects.ToArray();
	}

	private static bool IsVoxelVisible(MVVoxelChunk chunk, int px, int py, int pz)
	{
		if (px != 0 && px != chunk.sizeX - 1)
		{
			if (py != 0 && py != chunk.sizeY - 1)
			{
				if (pz != 0 && pz != chunk.sizeZ - 1)
				{
					if (chunk.voxels[px, py + 1, pz] == 0)
					{
						return true;
					}
					if (chunk.voxels[px, py - 1, pz] == 0)
					{
						return true;
					}
					if (chunk.voxels[px, py, pz + 1] == 0)
					{
						return true;
					}
					if (chunk.voxels[px, py, pz - 1] == 0)
					{
						return true;
					}
					if (chunk.voxels[px + 1, py, pz] == 0)
					{
						return true;
					}
					if (chunk.voxels[px - 1, py, pz] == 0)
					{
						return true;
					}
					return false;
				}
				return true;
			}
			return true;
		}
		return true;
	}

	public static Mesh[] CreateMeshes(MVMainChunk chunk, float sizePerVox)
	{
		return MVImporter.CreateMeshesFromChunk(chunk.voxelChunk, chunk.palatte, sizePerVox);
	}

	public static Mesh[] CreateMeshesFromChunk(MVVoxelChunk chunk, Color[] palatte, float sizePerVox)
	{
		Vector3 vector3 = new Vector3((float)chunk.sizeX / 2f, (float)chunk.sizeY / 2f, (float)chunk.sizeZ / 2f);
		return MVImporter.CreateMeshesFromChunk(chunk, palatte, sizePerVox, vector3);
	}

	public static Mesh[] CreateMeshesFromChunk(MVVoxelChunk chunk, Color[] palatte, float sizePerVox, Vector3 origin)
	{
		List<Vector3> vector3s = new List<Vector3>();
		List<Vector3> vector3s1 = new List<Vector3>();
		List<Color> colors = new List<Color>();
		List<int> nums = new List<int>();
		List<Vector2> vector2s = new List<Vector2>();
		Vector3[] _right = new Vector3[] { Vector3.right, Vector3.left, Vector3.up, Vector3.down, Vector3.forward, Vector3.back };
		List<Mesh> meshes = new List<Mesh>();
		if (sizePerVox <= 0f)
		{
			sizePerVox = 0.1f;
		}
		float single = sizePerVox / 2f;
		int num = 0;
		int num1 = 0;
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < chunk.sizeX; j++)
			{
				for (int k = 0; k < chunk.sizeY; k++)
				{
					for (int l = 0; l < chunk.sizeZ; l++)
					{
						int num2 = chunk.faces[i].colorIndices[j, k, l];
						if (num2 != 0)
						{
							float single1 = ((float)j - origin.x + 0.5f) * sizePerVox;
							float single2 = ((float)k - origin.y + 0.5f) * sizePerVox;
							float single3 = ((float)l - origin.z + 0.5f) * sizePerVox;
							int num3 = j;
							int p = k;
							int m = l;
							switch (i)
							{
								case 0:
								case 1:
									{
										p = k + 1;
										while (p < chunk.sizeY && chunk.faces[i].colorIndices[j, p, l] == num2)
										{
											p++;
										}
										p--;
										for (m = l + 1; m < chunk.sizeZ; m++)
										{
											bool flag = true;
											for (int n = k; n <= p; n++)
											{
												flag = flag & chunk.faces[i].colorIndices[j, n, m] == num2;
											}
											if (!flag)
											{
												break;
											}
										}
										m--;
										break;
									}
								case 2:
								case 3:
									{
										num3 = j + 1;
										while (num3 < chunk.sizeX && chunk.faces[i].colorIndices[num3, k, l] == num2)
										{
											num3++;
										}
										num3--;
										for (m = l + 1; m < chunk.sizeZ; m++)
										{
											bool flag1 = true;
											for (int o = j; o <= num3; o++)
											{
												flag1 = flag1 & chunk.faces[i].colorIndices[o, k, m] == num2;
											}
											if (!flag1)
											{
												break;
											}
										}
										m--;
										break;
									}
								case 4:
								case 5:
									{
										num3 = j + 1;
										while (num3 < chunk.sizeX && chunk.faces[i].colorIndices[num3, k, l] == num2)
										{
											num3++;
										}
										num3--;
										for (p = k + 1; p < chunk.sizeY; p++)
										{
											bool flag2 = true;
											for (int q = j; q <= num3; q++)
											{
												flag2 = flag2 & chunk.faces[i].colorIndices[q, p, l] == num2;
											}
											if (!flag2)
											{
												break;
											}
										}
										p--;
										break;
									}
							}
							for (int r = j; r <= num3; r++)
							{
								for (int s = k; s <= p; s++)
								{
									for (int t = l; t <= m; t++)
									{
										if (r != j || s != k || t != l)
										{
											chunk.faces[i].colorIndices[r, s, t] = 0;
										}
									}
								}
							}
							int num4 = num3 - j;
							int num5 = p - k;
							int num6 = m - l;
							switch (i)
							{
								case 0:
									{
										vector3s.Add(new Vector3(single1 + single, single2 - single, single3 - single));
										vector3s.Add(new Vector3(single1 + single, single2 + single + sizePerVox * (float)num5, single3 - single));
										vector3s.Add(new Vector3(single1 + single, single2 + single + sizePerVox * (float)num5, single3 + single + sizePerVox * (float)num6));
										vector3s.Add(new Vector3(single1 + single, single2 - single, single3 + single + sizePerVox * (float)num6));
										break;
									}
								case 1:
									{
										vector3s.Add(new Vector3(single1 - single, single2 - single, single3 - single));
										vector3s.Add(new Vector3(single1 - single, single2 - single, single3 + single + sizePerVox * (float)num6));
										vector3s.Add(new Vector3(single1 - single, single2 + single + sizePerVox * (float)num5, single3 + single + sizePerVox * (float)num6));
										vector3s.Add(new Vector3(single1 - single, single2 + single + sizePerVox * (float)num5, single3 - single));
										break;
									}
								case 2:
									{
										vector3s.Add(new Vector3(single1 + single + sizePerVox * (float)num4, single2 + single, single3 - single));
										vector3s.Add(new Vector3(single1 - single, single2 + single, single3 - single));
										vector3s.Add(new Vector3(single1 - single, single2 + single, single3 + single + sizePerVox * (float)num6));
										vector3s.Add(new Vector3(single1 + single + sizePerVox * (float)num4, single2 + single, single3 + single + sizePerVox * (float)num6));
										break;
									}
								case 3:
									{
										vector3s.Add(new Vector3(single1 + single + sizePerVox * (float)num4, single2 - single, single3 - single));
										vector3s.Add(new Vector3(single1 + single + sizePerVox * (float)num4, single2 - single, single3 + single + sizePerVox * (float)num6));
										vector3s.Add(new Vector3(single1 - single, single2 - single, single3 + single + sizePerVox * (float)num6));
										vector3s.Add(new Vector3(single1 - single, single2 - single, single3 - single));
										break;
									}
								case 4:
									{
										vector3s.Add(new Vector3(single1 - single, single2 + single + sizePerVox * (float)num5, single3 + single));
										vector3s.Add(new Vector3(single1 - single, single2 - single, single3 + single));
										vector3s.Add(new Vector3(single1 + single + sizePerVox * (float)num4, single2 - single, single3 + single));
										vector3s.Add(new Vector3(single1 + single + sizePerVox * (float)num4, single2 + single + sizePerVox * (float)num5, single3 + single));
										break;
									}
								case 5:
									{
										vector3s.Add(new Vector3(single1 - single, single2 + single + sizePerVox * (float)num5, single3 - single));
										vector3s.Add(new Vector3(single1 + single + sizePerVox * (float)num4, single2 + single + sizePerVox * (float)num5, single3 - single));
										vector3s.Add(new Vector3(single1 + single + sizePerVox * (float)num4, single2 - single, single3 - single));
										vector3s.Add(new Vector3(single1 - single, single2 - single, single3 - single));
										break;
									}
							}
							vector3s1.Add(_right[i]);
							vector3s1.Add(_right[i]);
							vector3s1.Add(_right[i]);
							vector3s1.Add(_right[i]);
							Color color = palatte[num2 - 1];
							colors.Add(color);
							colors.Add(color);
							colors.Add(color);
							colors.Add(color);
							nums.Add(num * 4);
							nums.Add(num * 4 + 1);
							nums.Add(num * 4 + 2);
							nums.Add(num * 4 + 2);
							nums.Add(num * 4 + 3);
							nums.Add(num * 4);
							vector2s.Add(new Vector2(((float)num2 - 1f) / 256f, 0f));
							vector2s.Add(new Vector2((float)num2 / 256f, 0f));
							vector2s.Add(new Vector2((float)num2 / 256f, 1f));
							vector2s.Add(new Vector2(((float)num2 - 1f) / 256f, 1f));
							num++;
							if (vector3s.Count + 4 >= 65000)
							{
								Mesh mesh = new Mesh();
								mesh.vertices = vector3s.ToArray();
								mesh.colors = colors.ToArray();
								mesh.normals = (vector3s1.ToArray());
								mesh.triangles = (nums.ToArray());
								mesh.uv = (vector2s.ToArray());
								meshes.Add(mesh);
								vector2s.Clear();
								vector3s.Clear();
								colors.Clear();
								vector3s1.Clear();
								nums.Clear();
								num1 += num;
								num = 0;
							}
						}
					}
				}
			}
		}
		if (vector3s.Count > 0)
		{
			Mesh mesh1 = new Mesh();
			mesh1.vertices = (vector3s.ToArray());
			mesh1.colors = colors.ToArray();
			mesh1.normals = (vector3s1.ToArray());
			mesh1.triangles = (nums.ToArray());
			mesh1.uv = (vector2s.ToArray());
			meshes.Add(mesh1);
			num1 += num;
		}
		Debug.Log(string.Format("[MVImport] Mesh generated, total quads {0}", num1));
		return meshes.ToArray();
	}

	public static GameObject[] CreateVoxelGameObjects(MVMainChunk chunk, Transform parent, Material mat, float sizePerVox)
	{
		return MVImporter.CreateVoxelGameObjectsForChunk(chunk.voxelChunk, chunk.palatte, parent, mat, sizePerVox);
	}

	public static GameObject[] CreateVoxelGameObjects(MVMainChunk chunk, Transform parent, Material mat, float sizePerVox, Vector3 origin)
	{
		return MVImporter.CreateVoxelGameObjectsForChunk(chunk.voxelChunk, chunk.palatte, parent, mat, sizePerVox, origin);
	}

	public static GameObject[] CreateVoxelGameObjectsForChunk(MVVoxelChunk chunk, Color[] palatte, Transform parent, Material mat, float sizePerVox)
	{
		Vector3 vector3 = new Vector3((float)chunk.sizeX / 2f, (float)chunk.sizeY / 2f, (float)chunk.sizeZ / 2f);
		return MVImporter.CreateVoxelGameObjectsForChunk(chunk, palatte, parent, mat, sizePerVox, vector3);
	}

	public static GameObject[] CreateVoxelGameObjectsForChunk(MVVoxelChunk chunk, Color[] palatte, Transform parent, Material mat, float sizePerVox, Vector3 origin)
	{
		List<GameObject> gameObjects = new List<GameObject>();
		int num = 0;
		Mesh[] meshArray = MVImporter.CreateMeshesFromChunk(chunk, palatte, sizePerVox, origin);
		for (int i = 0; i < (int)meshArray.Length; i++)
		{
			Mesh mesh = meshArray[i];
			GameObject gameObject = new GameObject();
			gameObject.name = (string.Format("VoxelMesh ({0})", num));
			gameObject.transform.SetParent(parent);
			gameObject.transform.localPosition = (Vector3.zero);
			gameObject.transform.localRotation = (Quaternion.Euler(Vector3.zero));
			gameObject.transform.localScale = (Vector3.one);
			gameObject.AddComponent<MeshFilter>().mesh = (mesh);
			gameObject.AddComponent<MeshRenderer>().material = (mat);
			gameObject.AddComponent<MVVoxModelMesh>();
			gameObjects.Add(gameObject);
			num++;
		}
		return gameObjects.ToArray();
	}

	public static Mesh CubeMeshWithColor(float size, Color c, int cidx)
	{
		float half = size / 2f;
		Vector3[] vector3 = new Vector3[] { new Vector3(-half, -half, -half), new Vector3(-half, half, -half), new Vector3(half, half, -half), new Vector3(half, -half, -half), new Vector3(half, -half, half), new Vector3(half, half, half), new Vector3(-half, half, half), new Vector3(-half, -half, half) };
		int[] vertices = new int[] { 0, 1, 2, 0, 2, 3, 3, 2, 5, 3, 5, 4, 5, 2, 1, 5, 1, 6, 3, 4, 7, 3, 7, 0, 0, 7, 6, 0, 6, 1, 4, 5, 6, 4, 6, 7 };
		Color[] colors = new Color[] { c, c, c, c, c, c, c, c };
		Vector2[] uvs = new Vector2[] { new Vector2(((float)cidx - 0.5f) / 256f, 0.5f), new Vector2(((float)cidx - 0.5f) / 256f, 0.5f), new Vector2(((float)cidx - 0.5f) / 256f, 0.5f), new Vector2(((float)cidx - 0.5f) / 256f, 0.5f), new Vector2(((float)cidx - 0.5f) / 256f, 0.5f), new Vector2(((float)cidx - 0.5f) / 256f, 0.5f), new Vector2(((float)cidx - 0.5f) / 256f, 0.5f), new Vector2(((float)cidx - 0.5f) / 256f, 0.5f) };
		Mesh mesh = new Mesh();
		mesh.vertices = (vector3);
		mesh.uv = (uvs);
		mesh.colors = colors;
		mesh.triangles = (vertices);
		mesh.RecalculateNormals();
		return mesh;
	}

	public static void GenerateFaces(MVVoxelChunk voxelChunk)
	{
		voxelChunk.faces = new MVFaceCollection[6];
		for (int i = 0; i < 6; i++)
		{
			voxelChunk.faces[i].colorIndices = new byte[voxelChunk.sizeX, voxelChunk.sizeY, voxelChunk.sizeZ];
		}
		for (int j = 0; j < voxelChunk.sizeX; j++)
		{
			for (int k = 0; k < voxelChunk.sizeY; k++)
			{
				for (int l = 0; l < voxelChunk.sizeZ; l++)
				{
					if (j == 0 || voxelChunk.voxels[j - 1, k, l] == 0)
					{
						voxelChunk.faces[1].colorIndices[j, k, l] = voxelChunk.voxels[j, k, l];
					}
					if (j == voxelChunk.sizeX - 1 || voxelChunk.voxels[j + 1, k, l] == 0)
					{
						voxelChunk.faces[0].colorIndices[j, k, l] = voxelChunk.voxels[j, k, l];
					}
					if (k == 0 || voxelChunk.voxels[j, k - 1, l] == 0)
					{
						voxelChunk.faces[3].colorIndices[j, k, l] = voxelChunk.voxels[j, k, l];
					}
					if (k == voxelChunk.sizeY - 1 || voxelChunk.voxels[j, k + 1, l] == 0)
					{
						voxelChunk.faces[2].colorIndices[j, k, l] = voxelChunk.voxels[j, k, l];
					}
					if (l == 0 || voxelChunk.voxels[j, k, l - 1] == 0)
					{
						voxelChunk.faces[5].colorIndices[j, k, l] = voxelChunk.voxels[j, k, l];
					}
					if (l == voxelChunk.sizeZ - 1 || voxelChunk.voxels[j, k, l + 1] == 0)
					{
						voxelChunk.faces[4].colorIndices[j, k, l] = voxelChunk.voxels[j, k, l];
					}
				}
			}
		}
	}

	public static void Init(Action<Color[]> actionColorsComplete, Action<List<VoxCubeItem>> actionCubesComplete)
	{
		MVImporter.ColorsPalleteLoadComplete = actionColorsComplete;
		MVImporter.CubesDataLoadComplete = actionCubesComplete;
	}

	public static MVMainChunk LoadVOX(string path, bool generateFaces = true)
	{
		byte[] numArray = null;
		numArray = File.ReadAllBytes(path);
		if (numArray[0] == 86 && numArray[1] == 79 && numArray[2] == 88 && numArray[3] == 32)
		{
			return MVImporter.LoadVOXFromData(numArray, generateFaces);
		}
		Debug.LogError("Invalid VOX file, magic number mismatch");
		return null;
	}

	public static MVMainChunk LoadVOXFromData(byte[] data, bool generateFaces = true)
	{
		MVImporter.usedPallate.Clear();
		for (int i = 0; i < 256; i++)
		{
			MVImporter.colorCountArray[i] = 0;
		}

		MVMainChunk mVMainChunk;
		using (MemoryStream memoryStream = new MemoryStream(data))
		{
			using (BinaryReader binaryReader = new BinaryReader(memoryStream))
			{
				MVMainChunk _white = new MVMainChunk();
				var dummy = binaryReader.ReadInt32();
				_white.version = binaryReader.ReadBytes(4);
				byte[] numArray = binaryReader.ReadBytes(4);
				if (numArray[0] != 77 || numArray[1] != 65 || numArray[2] != 73 || numArray[3] != 78)
				{
					Debug.LogError("[MVImport] Invalid MainChunk ID, main chunk expected");
					mVMainChunk = null;
				}
				else
				{
					int num = binaryReader.ReadInt32();
					int num1 = binaryReader.ReadInt32();
					binaryReader.ReadBytes(num);
					int num2 = 0;
					while (num2 < num1)
					{
						numArray = binaryReader.ReadBytes(4);
						if (numArray[0] == 80 && numArray[1] == 65 && numArray[2] == 67 && numArray[3] == 75)
						{
							int num3 = binaryReader.ReadInt32();
							int num4 = binaryReader.ReadInt32();
							binaryReader.ReadInt32();
							num2 = num2 + num3 + num4 + 12;
						}
						else if (numArray[0] == 83 && numArray[1] == 73 && numArray[2] == 90 && numArray[3] == 69)
						{
							num2 += MVImporter.ReadSizeChunk(binaryReader, _white);
						}
						else if (numArray[0] == 88 && numArray[1] == 89 && numArray[2] == 90 && numArray[3] == 73)
						{
							num2 += MVImporter.ReadVoxelChunk(binaryReader, _white.voxelChunk);
						}
						else if (numArray[0] != 82 || numArray[1] != 71 || numArray[2] != 66 || numArray[3] != 65)
						{
							Debug.LogWarning(string.Concat("[MVImport] Chunk ID not recognized, got ", Encoding.ASCII.GetString(numArray)));
							int num5 = binaryReader.ReadInt32();
							int num6 = binaryReader.ReadInt32();
							binaryReader.ReadBytes(num5 + num6);
							num2 = num2 + num5 + num6 + 12;
						}
						else
						{
							_white.palatte = new Color[256];
							for (int i = 0; i < (int)_white.palatte.Length; i++)
							{
								_white.palatte[i] = Color.white;
							}
							num2 += MVImporter.ReadPalattee(binaryReader, _white.palatte);
							//if (MVImporter.ColorsPalleteLoadComplete == null)
							//{
							//	continue;
							//}
							//MVImporter.ColorsPalleteLoadComplete(_white.palatte);
						}
					}
					if (generateFaces)
					{
						MVImporter.GenerateFaces(_white.voxelChunk);
					}
					if (_white.palatte == null)
					{
						_white.palatte = MVMainChunk.defaultPalatte;
						//if (MVImporter.ColorsPalleteLoadComplete != null)
						//{
						//	MVImporter.ColorsPalleteLoadComplete(_white.palatte);
						//}
					}
					mVMainChunk = _white;
				}
			}
		}
		return mVMainChunk;
	}

	private static int ReadPalattee(BinaryReader br, Color[] colors)
	{
		int num = br.ReadInt32();
		int num1 = br.ReadInt32();
		for (int i = 0; i < 256; i++)
		{
			colors[i] = new Color((float)br.ReadByte() / 255f, (float)br.ReadByte() / 255f, (float)br.ReadByte() / 255f, (float)br.ReadByte() / 255f);
		}
		if (num1 > 0)
		{
			br.ReadBytes(num1);
			Debug.LogWarning("[MVImporter] Nested chunk not supported");
		}
		return num + num1 + 12;
	}

	private static int ReadSizeChunk(BinaryReader br, MVMainChunk mainChunk)
	{
		int num = br.ReadInt32();
		int num1 = br.ReadInt32();
		mainChunk.sizeX = br.ReadInt32();
		mainChunk.sizeZ = br.ReadInt32();
		mainChunk.sizeY = br.ReadInt32();
		mainChunk.voxelChunk = new MVVoxelChunk()
		{
			voxels = new byte[mainChunk.sizeX, mainChunk.sizeY, mainChunk.sizeZ]
		};
		Debug.Log(string.Format("[MVImporter] Voxel Size {0}x{1}x{2}", mainChunk.sizeX, mainChunk.sizeY, mainChunk.sizeZ));
		if (num1 > 0)
		{
			br.ReadBytes(num1);
			Debug.LogWarning("[MVImporter] Nested chunk not supported");
		}
		return num + num1 + 12;
	}

	private static int ReadVoxelChunk(BinaryReader br, MVVoxelChunk chunk)
	{
		int num = br.ReadInt32();
		int num1 = br.ReadInt32();
		int num2 = br.ReadInt32();
		for (int i = 0; i < num2; i++)
		{
			int num3 = br.ReadByte();
			int num4 = br.ReadByte();
			int num5 = br.ReadByte();
			chunk.voxels[num3, num5, num4] = br.ReadByte();
		}
		if (num1 > 0)
		{
			br.ReadBytes(num1);
			Debug.LogWarning("[MVImporter] Nested chunk not supported");
		}
		return num + num1 + 12;
	}
}

/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using Serialization;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class GenerateImagesEditor : EditorWindow
{
	private static string DESTINATION_LOCAL_FOLDER = @"Assets/StreamingAssets";
	private static string DESTINATION_FOR_ONLINE_FOLDER = @"D:\Cocos\Tools\Data\PixelArt";// @"Assets/StreamingAssets";

	private string sourceFolder2DLocal;
	private string sourceFolder3DLocal;
	private string sourceFolder2DOnline;
	private string sourceFolder3DOnline;

	private string onlineDataset = DESTINATION_FOR_ONLINE_FOLDER;

	private bool appendToggle = false;
	private bool backupToggle = true;
	private bool localToggle = true;

	[MenuItem("Color Art/Generator")]
	static void Init()
	{
		EditorWindow window = GetWindow(typeof(GenerateImagesEditor));
		window.titleContent.text = "Color Art Generator";
		window.Show();

	}

	void OnGUI()
	{ 
		EditorGUILayout.LabelField("---------------- ONLINE ----------------");
		onlineDataset = EditorGUILayout.TextField("Online dataset folder: ", onlineDataset);
		EditorGUILayout.LabelField("Please specify the input folder for ONLINE 2D arts");
		EditorGUILayout.BeginHorizontal();
		sourceFolder2DOnline = EditorGUILayout.TextField("Source folder 2D: ", sourceFolder2DOnline);

		if (GUILayout.Button("Select", GUILayout.Width(80)))
		{
			string path = EditorUtility.OpenFolderPanel("Select input folder for ONLINE 2D arts", sourceFolder2DOnline, "");
			if (!string.IsNullOrEmpty(path))
			{
				sourceFolder2DOnline = path;
				Repaint();
			}
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.LabelField("Please specify the input folder for ONLINE 3D arts");
		EditorGUILayout.BeginHorizontal();
		sourceFolder3DOnline = EditorGUILayout.TextField("Source folder 3D: ", sourceFolder3DOnline);
		if (GUILayout.Button("Select", GUILayout.Width(80)))
		{
			string path = EditorUtility.OpenFolderPanel("Select input folder for ONLINE 3D arts", sourceFolder3DOnline, "");
			if (!string.IsNullOrEmpty(path))
			{
				sourceFolder3DOnline = path;
				Repaint();
			}
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.LabelField("---------------- LOCAL ----------------");
		EditorGUILayout.LabelField("Please specify the input folder for LOCAL 2D arts");
		EditorGUILayout.BeginHorizontal();
		sourceFolder2DLocal = EditorGUILayout.TextField("Source folder 2D: ", sourceFolder2DLocal);

		if (GUILayout.Button("Select", GUILayout.Width(80)))
		{
			string path = EditorUtility.OpenFolderPanel("Select input folder for LOCAL 2D arts", sourceFolder2DLocal, "");
			if (!string.IsNullOrEmpty(path))
			{
				sourceFolder2DLocal = path;
				Repaint();
			}
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.LabelField("Please specify the input folder for LOCAL 3D arts");
		EditorGUILayout.BeginHorizontal();
		sourceFolder3DLocal = EditorGUILayout.TextField("Source folder 3D: ", sourceFolder3DLocal);

		if (GUILayout.Button("Select", GUILayout.Width(80)))
		{
			string path = EditorUtility.OpenFolderPanel("Select input folder for LOCAL 3D arts", sourceFolder3DLocal, "");
			if (!string.IsNullOrEmpty(path))
			{
				sourceFolder3DLocal = path;
				Repaint();
			}
		}
		EditorGUILayout.EndHorizontal();

		appendToggle = GUILayout.Toggle(appendToggle, "Combine the new generated arts with the existing arts");
		backupToggle = GUILayout.Toggle(backupToggle, "Backup the existing arts before generating");
		localToggle = GUILayout.Toggle(localToggle, "For in app dataset. Uncheck to proceed with online dataset");

		EditorGUILayout.LabelField("----------------");
		if (GUILayout.Button("GENERATE NEW ARTS", GUILayout.Width(200)))
		{
			var dest = DESTINATION_LOCAL_FOLDER;
			var sourceFolder2D = sourceFolder2DLocal;
			var sourceFolder3D = sourceFolder3DLocal;
			if (!localToggle)
			{
				dest = DESTINATION_FOR_ONLINE_FOLDER;
				sourceFolder2D = sourceFolder2DOnline;
				sourceFolder3D = sourceFolder3DOnline;
			}
			var imageList = Path.Combine(dest, "images.xml");
			var images2D = Path.Combine(dest, "2D");
			var images3D = Path.Combine(dest, "3D");
			if (backupToggle && !appendToggle)
			{
				if (File.Exists(imageList))
				{
					if (File.Exists(imageList + ".bak"))
					{
						File.Delete(imageList + ".bak");
					}
					File.Move(imageList, imageList + ".bak");
				}
			}

			var imagesInfo = new ImagesInfo();
			imagesInfo.Version = 1;
			if (appendToggle)
			{
				if (File.Exists(imageList))
				{
					imagesInfo = Serializer.LoadFromTextFile<ImagesInfo>(imageList);
				}
			}
			if (File.Exists(imageList))
			{
				File.Delete(imageList);
			}

			if (backupToggle && !appendToggle)
			{
				if (Directory.Exists(images2D))
				{
					if (Directory.Exists(images2D + ".bak"))
					{
						Directory.Delete(images2D + ".bak");
					}
					Directory.Move(images2D, images2D + ".bak");
				}

				if (Directory.Exists(images3D))
				{
					if (Directory.Exists(images3D + ".bak"))
					{
						Directory.Delete(images3D + ".bak");
					}
					Directory.Move(images3D, images3D + ".bak");
				}
			}
			else
			{
				if (Directory.Exists(images2D))
				{
					Directory.Delete(images2D);
				}
				if (Directory.Exists(images3D))
				{
					Directory.Delete(images3D);
				}
			}
			if (!Directory.Exists(images2D))
			{
				Directory.CreateDirectory(images2D);
			}
			if (!Directory.Exists(images3D))
			{
				Directory.CreateDirectory(images3D);
			}

			int count = 0;

			List<ImageInfo> images = new List<ImageInfo>();
			if (Directory.Exists(sourceFolder2D))
			{
				foreach (var accessType in new AccessStatus[] { AccessStatus.Free, AccessStatus.Premium })
				{
					foreach (var file in Directory.GetFiles(Path.Combine(sourceFolder2D, accessType.ToString()), "*.png", SearchOption.AllDirectories))
					{
						string name = file.Replace(sourceFolder2D, "");
						if (name.StartsWith("/") || name.StartsWith("\\"))
							name = name.Substring(1);
						var id = Md5Sum(name);
						var imageInfo = new ImageInfo()
						{
							Name = (++count).ToString(),
							Id = id,
							Is3D = false,
							Source = "gallery_2D",
							AccessStatus = accessType,
							Url = "2D/" + id + ".png"
						};

						ResizeAndCopy(file, Path.Combine(dest, imageInfo.Url));
						images.Add(imageInfo);
					}
				} 
			}

			if (Directory.Exists(sourceFolder3D))
			{
				foreach (var accessType in new AccessStatus[] { AccessStatus.Free, AccessStatus.Premium })
				{
					foreach (var file in Directory.GetFiles(Path.Combine(sourceFolder3D, accessType.ToString()), "*.*", SearchOption.AllDirectories))
					{
						if (file.EndsWith(".png") || file.EndsWith(".jpg"))
						{
							string name = file.Replace(sourceFolder3D, "");
							if (name.StartsWith("/") || name.StartsWith("\\"))
								name = name.Substring(1);
							var id = Md5Sum(name);
							var imageInfo = new ImageInfo()
							{
								Name = (++count).ToString(),
								Id = id,
								Is3D = true,
								Source = "gallery_3D",
								AccessStatus = accessType,
								Url = "3D/" + id + ".png"
							};

							var destFileName = Path.Combine(dest, imageInfo.Url);
							var voxFileName = Path.ChangeExtension(file, ".vox");
                            if (!File.Exists(voxFileName)){
                                voxFileName = file + ".vox";
                            }
							if (File.Exists(voxFileName))
							{
								ResizeAndCopy(file, destFileName);

								var destVoxFileName = Path.Combine(dest, imageInfo.Url + ".vox");
								File.Copy(voxFileName, destVoxFileName);
								images.Add(imageInfo);
							}
						}
					}
				} 
			}
			//Random the list
			while (images.Count > 0)
			{
				int index = Random.Range(0, images.Count);
				imagesInfo.Add(images[index]);
				images.RemoveAt(index);
			}

			if (!localToggle)
			{ 
				var localImageList = Path.Combine(DESTINATION_LOCAL_FOLDER, "images.xml");
				if (File.Exists(localImageList))
				{
					var localInfo = Serializer.LoadFromTextFile<ImagesInfo>(localImageList);
					imagesInfo.Arrange(onlineDataset, localInfo);
				}
			}

			Serializer.SaveToTextFile<ImagesInfo>(imageList, imagesInfo);

			ShowNotification(new GUIContent("Generate done!"));
		}
	}
	private string Md5Sum(string strToEncrypt)
	{
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);

		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);

		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";

		for (int i = 0; i < hashBytes.Length; i++)
		{
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}

		return hashString.PadLeft(32, '0');
	}
	private static void ResizeAndCopy(string src, string dest)
	{
		var fileData = File.ReadAllBytes(src);
		var tex = new Texture2D(2, 2);
		tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.

		int padding = 5;

		var destTex = new Texture2D(tex.width + padding * 2, tex.height + padding * 2, TextureFormat.ARGB32, false);
		Color fillColor = Color.white;
		Color[] fillPixels = new Color[destTex.width * destTex.height];
		for (int i = 0; i < fillPixels.Length; i++)
		{
			fillPixels[i] = fillColor;
		}
		destTex.SetPixels(fillPixels);

		var pixels = tex.GetPixels();

		//remove white background
		//var destPixels = new Color[pixels.Length];
		//for(int i = 0; i < destPixels.Length; i++)
		//{
		//	destPixels[i] = (pixels[i].r == Color.white.r && pixels[i].g == Color.white.g && pixels[i].b == Color.white.b) ? fillColor : pixels[i];
		//}
		destTex.SetPixels(padding, padding, tex.width, tex.height, pixels);
		destTex.Apply();
		var bytes = destTex.EncodeToPNG();

		File.WriteAllBytes(dest, bytes);
	}
}

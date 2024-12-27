/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Serialization
{
	public static class Serializer
	{
		public static void SaveToFile<T>(string fileName, T serializableObject, bool compressed = false)
		{
			FileStream fileStream = null;
			try
			{
				Serializer.SetNormalAttributesForFile(fileName);
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				fileStream = new FileStream(fileName, FileMode.OpenOrCreate);
				binaryFormatter.Serialize(fileStream, serializableObject);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError(ex.Message);
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
		}

		public class Utf8StringWriter : StreamWriter
		{
			public Utf8StringWriter(Stream stream)
				: base(stream)
			{

			}
			public override Encoding Encoding
			{
				get { return Encoding.UTF8; }
			}
		}
		public static void SaveToTextFile<T>(string fileName, T serializableObject, bool compressed = false)
		{
			FileStream fileStream = null;
			try
			{
				XmlSerializer ser = new XmlSerializer(typeof(T)); 

				fileStream = new FileStream(fileName, FileMode.OpenOrCreate);
				var streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
				ser.Serialize(streamWriter, serializableObject);
				streamWriter.Close();
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError(ex.Message);
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
			if (File.Exists(fileName))
			{
				var lines = File.ReadAllLines(fileName);
				if (lines != null && lines.Length > 0)
				{
					lines[0] = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
					File.Delete(fileName);
					File.WriteAllLines(fileName, lines);
				}
			}
		}
		public static T LoadFromTextFile<T>(string fileName)
		{
            DebugLogger.Log("text file : " + fileName);
			FileStream fileStream = null;
			try
			{
				fileStream = new FileStream(fileName, FileMode.Open);
				fileStream.Position = 0L;
				XmlSerializer ser = new XmlSerializer(typeof(T));
				return (T)ser.Deserialize(fileStream);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError(ex.Message);
				return default(T);
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
		}

		public static T LoadFromFile<T>(string fileName)
        {
            DebugLogger.Log("LoadFromFile " + fileName);
			if (File.Exists(fileName))
            { 
            DebugLogger.Log("LoadFromFile file exists");
				FileStream fileStream = null;
				try
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					fileStream = new FileStream(fileName, FileMode.Open);
					fileStream.Position = 0L;
					return (T)binaryFormatter.Deserialize(fileStream);
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError(ex.Message);
					return default(T);
				}
				finally
				{
					if (fileStream != null)
					{
						fileStream.Close();
					}
				}
			}
			else
            {
                DebugLogger.Log("LoadFromFile NOT exist");
				return default(T);
			}
		}

		public static T LoadFromStream<T>(Stream stream)
        {
            DebugLogger.Log("LoadFromStream ");
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			return (T)binaryFormatter.Deserialize(stream);
		}
		public static T LoadFromTextStream<T>(Stream stream)
        {
            DebugLogger.Log("LoadFromTextStream ");
			XmlSerializer ser = new XmlSerializer(typeof(T));
			return (T)ser.Deserialize(stream);
		}

		private static void SetNormalAttributesForFile(string fileName)
		{
			if (File.Exists(fileName))
			{
				FileAttributes attributes = File.GetAttributes(fileName);
				attributes &= ~FileAttributes.ReadOnly;
				File.SetAttributes(fileName, attributes);
			}
		}
	}
}



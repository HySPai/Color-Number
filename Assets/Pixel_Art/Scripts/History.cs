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

public class History
{
	private string m_fileName;

	private StreamWriter m_streamWriter;

	public List<HistoryStep> Steps { get; private set; }

	public void Init(string fileName)
	{
		this.m_fileName = fileName;
		this.Steps = new List<HistoryStep>();
		if (File.Exists(fileName))
		{
			string[] array = File.ReadAllLines(fileName);
			this.Steps = new List<HistoryStep>(array.Length);
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (!string.IsNullOrEmpty(text))
				{
					HistoryStep historyStep = new HistoryStep();
					string[] array3 = text.Split(new char[1] {
						';'
					}, StringSplitOptions.RemoveEmptyEntries);
					string[] array4 = array3;
					foreach (string text2 in array4)
					{
						string[] array5 = text2.Split(new char[1] {
							':'
						}, StringSplitOptions.RemoveEmptyEntries);
						if (array5.Length == 2)
						{
							historyStep.Add(new ShortVector2(short.Parse(array5[0]), short.Parse(array5[1])));
						}
					}
					this.Steps.Add(historyStep);
				}
			}
		}
		this.m_streamWriter = File.AppendText(fileName);
	}

	public void AddStep(HistoryStep historyStep)
	{
		this.Steps.Add(historyStep);
		this.m_streamWriter.WriteLine(historyStep);
	}

	public void Shuffle()
	{
		this.Steps.Shuffle();
	}

	public void Save()
	{
		if (this.m_streamWriter != null)
		{
			this.m_streamWriter.Close();
			this.m_streamWriter = File.AppendText(this.m_fileName);
		}
	}

	public void Close()
	{
		if (this.m_streamWriter != null)
		{
			this.m_streamWriter.Close();
		}
	}
}



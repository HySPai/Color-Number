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
using UnityEngine;

public abstract class AbstractPickerController : MonoBehaviour
{
	public bool IsInit;

	public event Action<string> GetImagePathComplete;

	public event Action<IList<string>> GetImageListPathComplete;

	public event Action<string> GetImagePathError;

	public event Action GetImageListPathError;

	public abstract void GetAllGalleryImagePaths();

	public abstract void Init(Action<bool> InitComplete);

	public abstract void OpenGallery();

	public virtual void OnGetImagePathComplete(string path)
	{ 
		if (this.GetImagePathComplete != null)
		{
			this.GetImagePathComplete(path);
		}
	}

	public virtual void OnGetImageListPathComplete(IList<string> paths)
	{ 
		if (this.GetImageListPathComplete != null)
		{
			this.GetImageListPathComplete(paths);
		}
	}

	public virtual void OnGetImagePathError(string path)
	{ 
		if (this.GetImagePathError != null)
		{
			this.GetImagePathError(path);
		}
	}

	public virtual void OnGetImageListPathError()
	{ 
		if (this.GetImageListPathError != null)
		{
			this.GetImageListPathError();
		}
	}
}



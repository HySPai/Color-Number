/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[SerializeField]
	private AudioClip m_clickClip;

	[SerializeField]
	private AudioClip m_bombClip;

	[SerializeField]
	private AudioClip m_wandClip;

	[SerializeField]
	private AudioClip m_completeClip;

	[SerializeField]
	private AudioClip m_victoryClip;

	[SerializeField]
	private AudioClip m_colorClip;

	public AudioSource m_audioTrackSrc;
	public bool isMuted;

	public static AudioManager Instance { get; private set; }

	private void Awake()
	{
		if (AudioManager.Instance == null)
		{
			AudioManager.Instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			this.m_audioTrackSrc = base.GetComponent<AudioSource>();

			isMuted = !AppData.SoundsEnabled;
			this.m_audioTrackSrc.mute = isMuted;
		}
		else if (this != AudioManager.Instance)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void RefreshSettings()
	{
		isMuted = !AppData.SoundsEnabled;
		this.m_audioTrackSrc.mute = isMuted;
	}

	public void PlayClick()
	{
		if (!isMuted)
		{
			PlayClip(this.m_clickClip);
		}
	}

	public void PlayBomb()
	{
		if (!isMuted)
		{
			PlayClip(m_bombClip);
		}
	}

	public void PlayWand()
	{
		if (!isMuted)
		{
			PlayClip(m_wandClip);
		}
	}

	public void PlayCompleteColor()
	{
		if (!isMuted)
		{
			PlayClip(m_completeClip);
		}
	}

	public void PlayVictory()
	{
		if (!isMuted)
		{
			PlayClip(m_victoryClip);
		}
	}
	public void PlayColor()
	{
		if (!isMuted)
		{
			PlayClip(m_colorClip);
		}
	}
	public void PlayClip(AudioClip clip)
	{
		this.m_audioTrackSrc.PlayOneShot(clip);
	}
}



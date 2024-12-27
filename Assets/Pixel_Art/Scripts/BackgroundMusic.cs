/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System.Collections;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
	public enum MusicTrackId
	{
		NONE = -1,
		TRACK1 = 0,
		TRACK2 = 1,
		TRACK3 = 2
	}

	public static BackgroundMusic musicMgr;

	public AudioSource m_audioTrackSrc;

	public AudioClip[] m_audioTracks;

	[HideInInspector]
	public MusicTrackId currentTrack = MusicTrackId.NONE;

	private MusicTrackId pendingTrack = MusicTrackId.NONE;

	private bool pendingTrackLoop;

	public bool isMuted;
	private void Awake()
	{
		if (BackgroundMusic.musicMgr == null)
		{
			if (!PlayerPrefsWrapper.Exists(AppData.s_soundsKey))
			{
				AppData.SoundsEnabled = true;
			}

			BackgroundMusic.musicMgr = this;
			Object.DontDestroyOnLoad(base.gameObject);
			this.m_audioTrackSrc = base.GetComponent<AudioSource>();
			isMuted = !AppData.SoundsEnabled;
			this.m_audioTrackSrc.mute = isMuted;
		}
		else if (this != BackgroundMusic.musicMgr)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void RefreshSettings()
	{
		isMuted = !AppData.SoundsEnabled;
		this.m_audioTrackSrc.mute = isMuted;
		if (!isMuted)
		{
			currentTrack = MusicTrackId.NONE;
			musicMgr.PlayTrack(MusicTrackId.TRACK1, true);
		}
	}

	private void Start()
	{
		if (AppData.SoundsEnabled)
		{
			StartDelayTrack(MusicTrackId.TRACK1, true);
		}
	}

	private static bool musicPlayFirstTime = true;
	public static void PlayMainBackground()
	{
		if (musicMgr != null)
		{
			if (musicPlayFirstTime)
			{
				musicMgr.PlayTrack(MusicTrackId.TRACK1, true);
				musicPlayFirstTime = false;
			}
			else
			{
				musicMgr.StartDelayTrack(MusicTrackId.TRACK1, true);
			}
		}
	}

	private static BackgroundMusic.MusicTrackId randomMusicTrack = BackgroundMusic.MusicTrackId.TRACK3;
	public static void PlayInGameBackground()
	{
		if (musicMgr != null)
		{
			if (randomMusicTrack == BackgroundMusic.MusicTrackId.TRACK2)
			{
				randomMusicTrack = BackgroundMusic.MusicTrackId.TRACK3;
			}
			else
			{
				randomMusicTrack = BackgroundMusic.MusicTrackId.TRACK2;
			}
			musicMgr.StartDelayTrack(randomMusicTrack, true);
		}
	}

	public void PlayTrack(MusicTrackId trackID, bool loop)
	{
		if (!this.m_audioTrackSrc.mute)
		{
			if (currentTrack != trackID && this.pendingTrack == MusicTrackId.NONE)
			{
				this.m_audioTrackSrc.volume = 1f;
				this.m_audioTrackSrc.loop = loop;
				this.m_audioTrackSrc.clip = this.m_audioTracks[(int)trackID];
				this.m_audioTrackSrc.Play();
				this.currentTrack = trackID;
			}
		}
	}

	public void StopTrack()
	{
		if (this.m_audioTrackSrc.isPlaying)
		{
			this.m_audioTrackSrc.Stop();
		}
		this.currentTrack = MusicTrackId.NONE;
	}

	public void PreparePendingTrack(MusicTrackId id, bool bLoop)
	{
		this.pendingTrack = id;
		this.pendingTrackLoop = bLoop;
	}

	public void FadeOutTrack()
	{
		base.StartCoroutine("ProcessFadeOutTrack");
	}

	public void StartDelayTrack(MusicTrackId id, bool bLoop)
	{
		PreparePendingTrack(id, bLoop);
		FadeOutTrack();
	}

	private IEnumerator ProcessFadeOutTrack()
	{
		if (this.m_audioTrackSrc.mute)
		{
			yield return null;
		}
		float t2 = 1f;
		while (t2 > 0f)
		{
			this.m_audioTrackSrc.volume = t2;
			t2 -= 0.1f;
			yield return new WaitForSeconds(0.1f);
		}
		t2 = 0f;
		this.m_audioTrackSrc.volume = t2;
		if (this.pendingTrack != MusicTrackId.NONE)
		{
			MusicTrackId id = this.pendingTrack;
			this.pendingTrack = MusicTrackId.NONE;
			this.PlayTrack(id, this.pendingTrackLoop);
			this.pendingTrackLoop = false;
		}
	}
}

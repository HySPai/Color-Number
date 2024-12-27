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
using UnityEngine;
using UnityEngine.UI;

public class DailyGame : MonoBehaviour
{
    public enum PuzzleState
    {
        PUZZLE_PLAYING = 1,
        PUZZLE_PAUSED,
        PUZZLE_COMPLETE_SEQUENCE,
        PUZZLE_COMPLETE
    }

    public GameObject[] emptyStarsArray;

    public GameObject[] fullStarsArray;

    public ParticleSystem particlesCascade;

    [HideInInspector]
    public LeaderboardScript leaderBoardScript;

    public Text guiTimerText;

    public Text movesText;

    public int starAwardIncrement = 5;

    private PuzzleState puzzleState = PuzzleState.PUZZLE_PAUSED;

    private float startTime;
    private int numberOfCells;
    private float timer;

    private int errorsCount = 0;

    private long previousSeconds = 0;

    public static DailyGame Instance;
    private static bool isdaily = false;
    void Awake()
    {
        Instance = this;

        if (isdaily)
        {
            ResetState(true);

            if (GetNameScript.Instance != null)
            {
                this.leaderBoardScript = GetNameScript.Instance.leaderBoardScript;
            }
        }
    }
    public static void ResetState(bool isDailyArt)
    {
        isdaily = isDailyArt;
        if (Instance != null)
        {
            if (isDailyArt)
            {
                Instance.movesText.gameObject.SetActive(true);
                Instance.guiTimerText.gameObject.SetActive(true);
                Instance.puzzleState = PuzzleState.PUZZLE_PAUSED;
            }
            else
            {
                Instance.movesText.gameObject.SetActive(false);
                Instance.guiTimerText.gameObject.SetActive(false);
                Instance.puzzleState = PuzzleState.PUZZLE_PAUSED;
            }
        }
        if (!isdaily)
        {
            Instance = null;
        }
    }

    public static void UpdateGUI(int numOfcells)
    {
        if (Instance != null)
        {
            Instance.gameObject.SetActive(isdaily);
            if (isdaily)
            {
                Instance.StartGame(numOfcells);
            }
        }
    }

    public static bool IsDailyArt()
    {
        return isdaily;
    }

    private void Update()
    {
        if (this.puzzleState == PuzzleState.PUZZLE_PLAYING)
        {
            this.timer += Time.smoothDeltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds((double)this.timer);
            if (previousSeconds < (long)timeSpan.TotalSeconds)
            {
                this.guiTimerText.text = string.Format("Time {0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
                this.movesText.text = "Errors " + this.errorsCount.ToString();
                previousSeconds = (long)timeSpan.TotalSeconds;
            }
        }
    }

    public void StartGame(int numOfcells)
    {
        this.previousSeconds = 0;
        this.movesText.gameObject.SetActive(true);
        this.guiTimerText.gameObject.SetActive(true);
        this.puzzleState = PuzzleState.PUZZLE_PLAYING;
        this.errorsCount = 0;
        this.timer = 0;
        this.startTime = Time.deltaTime;
        this.numberOfCells = numOfcells;
    }

    public static void IncresetError()
    {
        if (isdaily && Instance != null)
        {
            Instance.errorsCount++;
            Instance.movesText.text = "Errors " + Instance.errorsCount.ToString();
        }
    }

    public void Complete()
    {
        if (this.puzzleState == PuzzleState.PUZZLE_PLAYING)
        {
            base.StartCoroutine(this.CompletionSchedule());
            this.puzzleState = PuzzleState.PUZZLE_COMPLETE_SEQUENCE;

            int starsCount = this.CalculateStarsWon();
            int oldStarsCount = 0;
            switch (LevelProgressControl.control.dailyDayOffset)
            {
                case -2:
                    oldStarsCount = this.GetDailyStars("dailyereyesterdaydata");
                    PlayerPrefs.SetString("dailyereyesterdaydata", LevelProgressControl.control.puzzleBasename + "-" + starsCount);
                    if (Debug.isDebugBuild)
                    {
                        UnityEngine.Debug.Log("Ereyesterday COMPLETED");
                    }
                    break;
                case -1:
                    oldStarsCount = this.GetDailyStars("dailyyesterdaydata");
                    PlayerPrefs.SetString("dailyyesterdaydata", LevelProgressControl.control.puzzleBasename + "-" + starsCount);
                    if (Debug.isDebugBuild)
                    {
                        UnityEngine.Debug.Log("Yesterday COMPLETED");
                    }
                    break;
                case 0:
                    {
                        oldStarsCount = this.GetDailyStars("dailytodaydata");
                        PlayerPrefs.SetString("dailytodaydata", LevelProgressControl.control.puzzleBasename + "-" + starsCount);
                        if (Debug.isDebugBuild)
                        {
                            UnityEngine.Debug.Log("Today COMPLETED");
                        }
                        if (this.timer < LevelProgressControl.control.dailyTodayPersonalBest)
                        {
                            LevelProgressControl.control.dailyTodayPersonalBest = this.timer;
                            //LevelProgressControl.control.dailyTodayPersonalBestBeaten = true;
                            LevelProgressControl.control.SaveProgressData();
                        }
                        long score = Convert.ToInt64(this.timer);
                        if (leaderBoardScript != null)
                        {
#if UNITY_ANDROID
                            this.leaderBoardScript.ReportLeaderboardScore(score, GPGSIds.leaderboard_best_time);
#else
                            //pixelart_dailybest
                            this.leaderBoardScript.ReportLeaderboardScore(score, "pixelart_dailybest");
#endif
                        }
						break;
					}
			}
			LevelProgressControl.control.SaveProgressData();

			//MainMenu.Instance.RefreshTodayArt();
		}
	}

	private IEnumerator CompletionSchedule()
	{ 
		yield return (object)new WaitForSeconds(1f);
		this.particlesCascade.Play();
		for (int i = 0; i < this.emptyStarsArray.Length; i++)
		{
			this.emptyStarsArray[i].GetComponent<Animation>().Play();
		}
		this.movesText.CrossFadeAlpha(1f, 0.5f, false); 
		this.guiTimerText.CrossFadeAlpha(1f, 0.5f, false); 
		yield return (object)new WaitForSeconds(0.6f);
		int starsWon = this.CalculateStarsWon();
		if (starsWon >= 1)
		{
			this.fullStarsArray[0].GetComponent<SpriteRenderer>().enabled = true;
			this.fullStarsArray[0].GetComponent<RadialParticlesScript>().Reset(false);
		}
		yield return (object)new WaitForSeconds(0.2f);
		if (starsWon >= 2)
		{
			this.fullStarsArray[1].GetComponent<SpriteRenderer>().enabled = true;
			this.fullStarsArray[1].GetComponent<RadialParticlesScript>().Reset(false);
		}
		yield return (object)new WaitForSeconds(0.2f);
		if (starsWon == 3)
		{
			this.fullStarsArray[2].GetComponent<SpriteRenderer>().enabled = true;
			this.fullStarsArray[2].GetComponent<RadialParticlesScript>().Reset(false);
		}
		this.puzzleState = PuzzleState.PUZZLE_COMPLETE; 
	}
	private int CalculateStarsWon()
	{
		float duration = Time.deltaTime - this.startTime;
		if (duration <= numberOfCells + this.starAwardIncrement && errorsCount < 3)
		{
			return 3;
		}
		if (duration <= numberOfCells + this.starAwardIncrement * 2 && errorsCount < 10)
		{
			return 2;
		}
		if (duration <= numberOfCells + this.starAwardIncrement * 3)
		{
			return 1;
		}
		return 0;
	}

	private int GetDailyStars(string prefname)
	{
		string str = PlayerPrefs.GetString(prefname, "nodata");
		string[] array = str.Split('-');
		return Convert.ToInt32(array[2]);
	}

}

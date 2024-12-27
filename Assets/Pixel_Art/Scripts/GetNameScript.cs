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
using System.Globalization;
using System.IO;
using UnityEngine; 
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GetNameScript : MonoBehaviour
{
    public Text resultText;

    public Button goButton;

    public Button todayButton;

    public Button yesterdayButton;

    public Button ereyesterdayButton;

    public Image puzzleReadyTodayImg;

    public Image puzzleReadyYesterdayImg;

    public Image puzzleReadyEreyesterdayImg;

    public GameObject starsParentToday;

    public GameObject starsParentYesterday;

    public GameObject starsParentEreyesterday;

    public Text dateNumTodayText;

    public Text dateNumYesterdayText;

    public Text dateNumEreyesterdayText;

    public Button leaderboardButton;

    public Text leaderboardPromptText;

	public Material grayMaterial;
    //public Button lbDialogSignoutButton;

    //public GameObject dialogLeaderboard;

    //public GameObject dialogGel;

    //public Image dialogTitleImage;

    //public Sprite gcLeaderboardSprite;

    public GameObject dlSpinner;

    public Text rolloverTimeText;

    public Text bestTimeText;

    public Image bestTimeNewImage;

    public LeaderboardScript leaderBoardScript;

    private bool bTodayDataExists;

    private bool bYesterdayDataExists;

    private bool bEreyesterdayDataExists;

    private bool bIsDataFetched;

    private bool bIsImageFetched;

    private string todayFilename;

    private string yesterdayFilename;

    private string ereyesterdayFilename;

    private int todayStars;

    private int yesterdayStars;

    private int ereyesterdayStars;

	public static GetNameScript Instance;
	private void Awake()
	{
		Instance = this;
		//Init();
	}

	public void Init()
    {
        this.todayFilename = GetDailyIdentity(0);
        this.yesterdayFilename = GetDailyIdentity(-1);
        this.ereyesterdayFilename = GetDailyIdentity(-2);
        this.CheckDailyPrefDataExists();
        this.CalcDailyPrefDateDiff();
        this.todayStars = GetDailyStars("dailytodaydata");
        this.yesterdayStars = GetDailyStars("dailyyesterdaydata");
        this.ereyesterdayStars = GetDailyStars("dailyereyesterdaydata");
        if (this.todayStars >= 1 && this.todayStars != 99)
        {
            ((Component)this.starsParentToday.transform.Find("StarL")).GetComponent<Image>().enabled = true;
            ((Component)this.starsParentToday.transform.Find("StarMarkerL")).GetComponent<Image>().enabled = false;
        }
        if (this.todayStars >= 2 && this.todayStars != 99)
        {
            ((Component)this.starsParentToday.transform.Find("StarM")).GetComponent<Image>().enabled = true;
            ((Component)this.starsParentToday.transform.Find("StarMarkerM")).GetComponent<Image>().enabled = false;
        }
        if (this.todayStars >= 3 && this.todayStars != 99)
        {
            ((Component)this.starsParentToday.transform.Find("StarR")).GetComponent<Image>().enabled = true;
            ((Component)this.starsParentToday.transform.Find("StarMarkerR")).GetComponent<Image>().enabled = false;
        }
        if (this.yesterdayStars >= 1 && this.yesterdayStars != 99)
        {
            ((Component)this.starsParentYesterday.transform.Find("StarL")).GetComponent<Image>().enabled = true;
            ((Component)this.starsParentYesterday.transform.Find("StarMarkerL")).GetComponent<Image>().enabled = false;
        }
        if (this.yesterdayStars >= 2 && this.yesterdayStars != 99)
        {
            ((Component)this.starsParentYesterday.transform.Find("StarM")).GetComponent<Image>().enabled = true;
            ((Component)this.starsParentYesterday.transform.Find("StarMarkerM")).GetComponent<Image>().enabled = false;
        }
        if (this.yesterdayStars >= 3 && this.yesterdayStars != 99)
        {
            ((Component)this.starsParentYesterday.transform.Find("StarR")).GetComponent<Image>().enabled = true;
            ((Component)this.starsParentYesterday.transform.Find("StarMarkerR")).GetComponent<Image>().enabled = false;
        }
        if (this.ereyesterdayStars >= 1 && this.ereyesterdayStars != 99)
        {
            ((Component)this.starsParentEreyesterday.transform.Find("StarL")).GetComponent<Image>().enabled = true;
            ((Component)this.starsParentEreyesterday.transform.Find("StarMarkerL")).GetComponent<Image>().enabled = false;
        }
        if (this.ereyesterdayStars >= 2 && this.ereyesterdayStars != 99)
        {
            ((Component)this.starsParentEreyesterday.transform.Find("StarM")).GetComponent<Image>().enabled = true;
            ((Component)this.starsParentEreyesterday.transform.Find("StarMarkerM")).GetComponent<Image>().enabled = false;
        }
        if (this.ereyesterdayStars >= 3 && this.ereyesterdayStars != 99)
        {
            ((Component)this.starsParentEreyesterday.transform.Find("StarR")).GetComponent<Image>().enabled = true;
            ((Component)this.starsParentEreyesterday.transform.Find("StarMarkerR")).GetComponent<Image>().enabled = false;
        }
        if (DailyDataExists(this.todayFilename))
        {
            this.SetDailyButtonVerb("Play", 0);
            this.bTodayDataExists = true;
            if (this.todayStars == 99)
            {
                this.LoadDailyPuzzleIcon(0, true);
            }
            else
            {
                this.LoadDailyPuzzleIcon(0, false);
            }
        }
        else
        {
            this.SetDailyButtonVerb("Download", 0);
        }
        this.dateNumTodayText.text = this.GetDailyDateDayString(0);
        if (DailyDataExists(this.yesterdayFilename))
        {
            this.SetDailyButtonVerb("Play", -1);
            this.bYesterdayDataExists = true;
            if (this.yesterdayStars == 99)
            {
                this.LoadDailyPuzzleIcon(-1, true);
            }
            else
            {
                this.LoadDailyPuzzleIcon(-1, false);
            }
        }
        else
        {
            this.SetDailyButtonVerb("Download", -1);
        }
        this.dateNumYesterdayText.text = this.GetDailyDateDayString(-1);
        if (DailyDataExists(this.ereyesterdayFilename))
        {
            this.SetDailyButtonVerb("Play", -2);
            this.bEreyesterdayDataExists = true;
            if (this.ereyesterdayStars == 99)
            {
                this.LoadDailyPuzzleIcon(-2, true);
            }
            else
            {
                this.LoadDailyPuzzleIcon(-2, false);
            }
        }
        else
        {
            this.SetDailyButtonVerb("Download", -2);
        }
        this.dateNumEreyesterdayText.text = this.GetDailyDateDayString(-2);
        //this.leaderBoardScript = base.GetComponent<LeaderboardScript>();
        //if (LevelProgressControl.control.androidSdkLevel < 14)
        //{
        //    this.leaderboardButton.interactable = false;
        //}
		 
        this.resultText.gameObject.SetActive(false);
        this.dlSpinner.SetActive(false);
        this.bestTimeNewImage.gameObject.SetActive(false);
        TimeSpan timeSpan = TimeSpan.FromSeconds((double)LevelProgressControl.control.dailyTodayPersonalBest);
        string str = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        this.bestTimeText.text = "Today's Personal Best Time is " + str;
         
        base.InvokeRepeating("DailyRolloverCountdown", 0f, 60f);
        if (LevelProgressControl.control.isReturningFromLevel)
        {
            LevelProgressControl.control.isReturningFromLevel = false;
            if (LevelProgressControl.control.dailyTodayPersonalBestBeaten)
            {
                base.StartCoroutine(this.FlashSchedule());
            }
            //LevelProgressControl.control.dailyTodayPersonalBestBeaten = false;
            int num2 = PlayerPrefs.GetInt("PuzzleStateId", -1);
            if (num2 == -1)
            {
                num2 = 287462819;
                PlayerPrefs.SetInt("PuzzleStateId", num2);
            }
            num2 = (num2 - 287462819) / 3;
            if (num2 > LevelProgressControl.adPeriodInSeconds)
            {
                if (!LevelProgressControl.control.iapDisableAds)
                {
                    if (AdsWrapper.Instance.IsVideoAvailable())
                    {
						AdsWrapper.Instance.ShowInter(string.Empty);
                        num2 = 287462819;
                        PlayerPrefs.SetInt("PuzzleStateId", num2);
                        PlayerPrefs.Save();
                    }
                }
                else
                {
                    UnityEngine.Debug.Log("IAP to disable ads detected. Skip Ad.");
                }
            }
            else if (Debug.isDebugBuild)
            {
                UnityEngine.Debug.Log("currAccumTime still accumulating... : " + num2.ToString());
            }
        }
    }

	public static void GetTodayArt(ImagePreview image, Material grayMaterial)
	{
		NewLibraryWindow.UpdateDailyClick(false);
		var identify = GetDailyIdentity(0);

		if (DailyDataExists(identify))
		{
			DataManager.Instance.GetImages(false, (ii) =>
			{
				int index = GetIdentifyIndex(identify, ii.Images.Count);
				var imageInfo = ii.Images[index];

				image.Init(imageInfo);
				image.LoadIcon();
				NewLibraryWindow.UpdateDailyClick(true);
			});  
		} 
	}

    public void PressDownloadorPlay(int dailyOffset)
    {
        UnityEngine.Debug.Log("PressDownloadorPlay()");
		AudioManager.Instance.PlayClick(); 
        if (DailyDataExists(GetDailyIdentity(dailyOffset)))
        {
            UnityEngine.Debug.Log("PLAY");
            LevelProgressControl.control.dailyDayOffset = dailyOffset;
            this.ExecutePuzzle(GetDailyIdentity(dailyOffset));
        }
        else
        {
            UnityEngine.Debug.Log("DOWNLOAD");
            this.ResetDailyDownload();
            this.dlSpinner.SetActive(true);
            string str = GetDailyIdentity(dailyOffset);
            base.StartCoroutine(this.DownloadTextureData(str, dailyOffset)); 
        }
    }

    public void ResetDailyDownload()
    {
        this.bIsDataFetched = false;
        this.bIsImageFetched = false;
    }

    public void ExecutePuzzle()
    {
        PlayerPrefs.SetString("datafile", "daily");
        SceneManager.LoadScene("GenericPuzzle");
    }

	public void ExecutePuzzle(string identify)
	{
		LevelProgressControl.control.puzzleSrcType = LevelProgressControl.PuzzleSourceType.PUZZLE_DAILY;
		LevelProgressControl.control.postcardOrigin = SceneManager.GetActiveScene().name;
		LevelProgressControl.control.puzzleBasename = identify;
		PlayerPrefs.SetString("datafile", identify);

		DataManager.Instance.GetImages(false, (ii) =>
		{
			int index = GetIdentifyIndex(identify, ii.Images.Count);
			var imageInfo = ii.Images[index];

			NewLibraryWindow.ExecutePuzzle(imageInfo);
		});
	}
	private static int GetIdentifyIndex(string str, int maxSize)
	{
		uint number = (uint)str.GetHashCode();
		return (int)((number + (maxSize + 200)) % maxSize);
	}
    private IEnumerator DownloadTextureData(string identify, int dailyOffset)
    {
		if (DataManager.Instance == null)
			yield return null;

		this.LoadDailyPuzzleIcon(dailyOffset, true);
	} 

    private void LoadSilhouette()
    {
        Texture2D texture2D = LoadPNG(Application.persistentDataPath + "/daily.png");
    }
	private IEnumerator LoadSaveCoroutine(ImageInfo imageInfo, Image image, string id)
	{
		var resTex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		resTex.filterMode = (FilterMode)(imageInfo.Is3D ? 1 : 0);
		var swd = MainManager.Instance.SavedWorksList.LoadById(id);
		yield return null;
		resTex.LoadImage(swd.Preview);

		image.sprite = Sprite.Create(resTex, new Rect(0f, 0f, (float)resTex.width, (float)resTex.height), new Vector2(0.5f, 0.5f));
	}
	
	private void LoadDailyPuzzleIcon(Image image, string identify, int dailyOffset, bool isSilhouette)
	{
		if (isSilhouette)
		{
			image.material = grayMaterial;
		}
		else
		{
			image.material = null;
		}

		DataManager.Instance.GetImages(false, (ii) =>
		{
			int index = GetIdentifyIndex(identify, ii.Images.Count);
			var imageInfo = ii.Images[index];

			var saveId = MainManager.Instance.SavedWorksList.LastSaveOfImageId(imageInfo.Id);
			 
			if (imageInfo.Is3D && saveId != null)
			{
				this.bIsImageFetched = true;
				image.gameObject.SetActive(true);
				base.StartCoroutine(LoadSaveCoroutine(imageInfo, image, saveId));
				this.SetDailyButtonVerb("Play", dailyOffset);
				this.dlSpinner.SetActive(false); 

				PlayerPrefsWrapper.SetBool("Daily-" + identify, true);
				if (dailyOffset == 0)
				{
					MainMenu.Instance.RefreshTodayArt();
				}
			}
			else
			{
				DataManager.Instance.GetImageAsset(imageInfo, delegate (bool res, Texture2D tex)
				{
					if (res)
					{
						this.bIsImageFetched = true;
						image.sprite = Sprite.Create(tex, new Rect(0f, 0f, (float)tex.width, (float)tex.height), new Vector2(0.5f, 0.5f));
						if (saveId != null)
						{
							base.StartCoroutine(LoadSaveCoroutine(imageInfo, image, saveId));
						}
						this.SetDailyButtonVerb("Play", dailyOffset);
						this.dlSpinner.SetActive(false);

						PlayerPrefsWrapper.SetBool("Daily-" + identify, true);
						if (dailyOffset == 0)
						{
							MainMenu.Instance.RefreshTodayArt();
						}
					} 
				});
				if (imageInfo.Is3D)
				{
					DataManager.Instance.GetImageAsset3D(imageInfo, null);
				}
			}
		}); 
	}

	private void LoadDailyPuzzleIcon(int todayOffset, bool isSilhouette)
    {
		switch (todayOffset)
		{
			case 0:
				LoadDailyPuzzleIcon(puzzleReadyTodayImg, GetDailyIdentity(todayOffset), todayOffset, isSilhouette);
				break;
			case -1:
				LoadDailyPuzzleIcon(puzzleReadyYesterdayImg, GetDailyIdentity(todayOffset), todayOffset, isSilhouette);
				break;
			case -2:
				LoadDailyPuzzleIcon(puzzleReadyEreyesterdayImg, GetDailyIdentity(todayOffset), todayOffset, isSilhouette);
				break;
		}

		//Texture2D texture2D = LoadPNG(Application.persistentDataPath + "/" + GetDailyIdentity(todayOffset) + ".png");
  //      switch (todayOffset)
  //      {
  //          case 0:
  //              this.puzzleReadyTodayImg.sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
  //              if (isSilhouette)
  //              {
		//			this.puzzleReadyTodayImg.material = grayMaterial; 
  //              }
  //              else
  //              {
		//			this.puzzleReadyTodayImg.material = null; 
  //              }
  //              break;
  //          case -1:
  //              this.puzzleReadyYesterdayImg.sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
  //              if (isSilhouette)
  //              {
  //                  this.puzzleReadyYesterdayImg.material = grayMaterial;
		//		}
  //              else
  //              {
  //                  this.puzzleReadyYesterdayImg.material = null;
		//		}
  //              break;
  //          case -2:
  //              this.puzzleReadyEreyesterdayImg.sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
  //              if (isSilhouette)
  //              {
  //                  this.puzzleReadyEreyesterdayImg.material = grayMaterial;
		//		}
  //              else
  //              {
  //                  this.puzzleReadyEreyesterdayImg.material = null;
		//		}
  //              break;
  //      }
    }

    public static Texture2D LoadPNG(string filePath)
    {
        Texture2D texture2D = null;
        if (File.Exists(filePath))
        {
            byte[] data = File.ReadAllBytes(filePath);
            texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture2D.filterMode = FilterMode.Point;
            texture2D.wrapMode = TextureWrapMode.Clamp;
            texture2D.LoadImage(data);
        }
        return texture2D;
    }

    public static string GetDailyIdentity(int todayOffset)
    {
        DateTime dateTime = DateTime.UtcNow.AddHours(-8.0);
        if (todayOffset != 0)
        {
            dateTime = dateTime.AddDays((double)todayOffset);
        }
        UnityEngine.Debug.Log(dateTime.Date);
        string text = string.Format("{0,3:D3}-{1}", dateTime.DayOfYear, dateTime.Year);
        UnityEngine.Debug.Log("Today's identity string: DayOfYear : Year -> " + text);
        return text;
    }

    private string GetDailyDateString(int todayOffset)
    {
        DateTime dateTime = DateTime.UtcNow.AddHours(-8.0);
        if (todayOffset != 0)
        {
            dateTime = dateTime.AddDays((double)todayOffset);
        }
        return DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(dateTime.Month) + " " + dateTime.Day.ToString("00");
    }

    private string GetDailyDateDayString(int todayOffset)
    {
        DateTime dateTime = DateTime.UtcNow.AddHours(-8.0);
        if (todayOffset != 0)
        {
            dateTime = dateTime.AddDays((double)todayOffset);
        }
        return dateTime.Day.ToString("00");
    }

    private static bool DailyDataExists(string identify)
    {
		return PlayerPrefsWrapper.GetBool("Daily-" + identify);
		//if (DataManager.Instance.AllImages == null || DataManager.Instance.AllImages.Images.Count == 0)
		//	return false;

		//int index = GetIdentifyIndex(identify, DataManager.Instance.AllImages.Images.Count);
		//var imageInfo = DataManager.Instance.AllImages.Images[index];

		//return DataManager.Instance.CheckLocalAsset(imageInfo); 

		//string path = Application.persistentDataPath + "/" + fnameBase + ".png";
  //      string path2 = Application.persistentDataPath + "/" + fnameBase + "-data.txt";
  //      bool result = true;
  //      if (!File.Exists(path))
  //      {
  //          result = false;
  //      }
  //      if (!File.Exists(path2))
  //      {
  //          result = false;
  //      }
  //      return result;
    }

    private void SetDailyButtonVerb(string verb, int dailyOffset)
    {
        switch (dailyOffset)
        {
            case 0:
                ((Component)this.todayButton).GetComponentInChildren<Text>().text = verb + " " + this.GetDailyDateString(0);
                break;
            case -1:
                ((Component)this.yesterdayButton).GetComponentInChildren<Text>().text = verb + " " + this.GetDailyDateString(-1);
                break;
            case -2:
                ((Component)this.ereyesterdayButton).GetComponentInChildren<Text>().text = verb + " " + this.GetDailyDateString(-2);
                break;
        }
    }

    public void CleanDailyPuzzleDataFiles()
    {
        UnityEngine.Debug.Log("*** CleanDailyPuzzleDataFiles ***");
        string[] files = Directory.GetFiles(Application.persistentDataPath, "???-20??*.*", SearchOption.TopDirectoryOnly);
        if (files.Length > 12)
        {
            string[] array = files;
            foreach (string text in array)
            {
                UnityEngine.Debug.Log("Daily data file -> " + text);
                string fileName = Path.GetFileName(text);
                UnityEngine.Debug.Log("Daily data file -> " + fileName);
                if (fileName.Contains(this.todayFilename))
                {
                    UnityEngine.Debug.Log("Daily is today. NOT DELETING!");
                }
                else if (fileName.Contains(this.yesterdayFilename))
                {
                    UnityEngine.Debug.Log("Daily is yesterday. NOT DELETING!");
                }
                else if (fileName.Contains(this.ereyesterdayFilename))
                {
                    UnityEngine.Debug.Log("Daily is ereyesterday. NOT DELETING!");
                }
                else
                {
                    UnityEngine.Debug.Log("DELETING " + text);
                    File.Delete(text);
                }
            }
        }
    }

    public int CalcDailyPrefDateDiff()
    {
        UnityEngine.Debug.Log("CalcDailyDateDiff");
        string[] array = this.todayFilename.Split('-');
        UnityEngine.Debug.Log("today day " + array[0] + " today year " + array[1]);
        int num = Convert.ToInt32(array[0]);
        int num2 = Convert.ToInt32(array[1]);
        int num3 = num2 * 365 + num;
        UnityEngine.Debug.Log("Integer version: today day " + num + " today year " + num2 + " total days " + num3);
        string @string = PlayerPrefs.GetString("dailytodaydata", "nodata");
        if (@string.Equals("nodata"))
        {
            UnityEngine.Debug.Log("CalcDailyPrefDateDiff -> nodata for dailytodaydata. This shouldn't happen");
        }
        string[] array2 = @string.Split('-');
        int num4 = Convert.ToInt32(array2[0]);
        int num5 = Convert.ToInt32(array2[1]);
        int num6 = Convert.ToInt32(array2[2]);
        switch (num3 - (num5 * 365 + num4))
        {
            case 0:
                if (Debug.isDebugBuild)
                {
                    UnityEngine.Debug.Log("daysDiff == 0");
                }
                return 0;
            case 1:
            {
                if (Debug.isDebugBuild)
                {
                    UnityEngine.Debug.Log("daysDiff == 1");
                }
                PlayerPrefs.SetString("dailyereyesterdaydata", PlayerPrefs.GetString("dailyyesterdaydata", "nodata"));
                PlayerPrefs.SetString("dailyyesterdaydata", PlayerPrefs.GetString("dailytodaydata", "nodata"));
                string value4 = this.todayFilename + "-99";
                PlayerPrefs.SetString("dailytodaydata", value4);
                LevelProgressControl.control.dailyTodayPersonalBest = 600f;
                break;
            }
            case 2:
            {
                if (Debug.isDebugBuild)
                {
                    UnityEngine.Debug.Log("daysDiff == 2");
                }
                PlayerPrefs.SetString("dailyereyesterdaydata", PlayerPrefs.GetString("dailytodaydata", "nodata"));
                string value5 = this.yesterdayFilename + "-99";
                PlayerPrefs.SetString("dailyyesterdaydata", value5);
                string value6 = this.todayFilename + "-99";
                PlayerPrefs.SetString("dailytodaydata", value6);
                LevelProgressControl.control.dailyTodayPersonalBest = 600f;
                break;
            }
            default:
            {
                if (Debug.isDebugBuild)
                {
                    UnityEngine.Debug.Log("daysDiff > 2");
                }
                string value = this.todayFilename + "-99";
                PlayerPrefs.SetString("dailytodaydata", value);
                LevelProgressControl.control.dailyTodayPersonalBest = 600f;
                string value2 = this.yesterdayFilename + "-99";
                PlayerPrefs.SetString("dailyyesterdaydata", value2);
                string value3 = this.ereyesterdayFilename + "-99";
                PlayerPrefs.SetString("dailyereyesterdaydata", value3);
                break;
            }
        }
        PlayerPrefs.SetInt("DailyPersonalBest", (int)LevelProgressControl.control.dailyTodayPersonalBest);
        PlayerPrefs.Save();
        return 0;
    }

    private void CheckDailyPrefDataExists()
    {
        string @string = PlayerPrefs.GetString("dailytodaydata", "nodata");
        if (@string.Equals("nodata"))
        {
            string value = this.todayFilename + "-99";
            PlayerPrefs.SetString("dailytodaydata", value);
            if (Debug.isDebugBuild)
            {
                UnityEngine.Debug.Log("today PPref = nodata");
            }
            LevelProgressControl.control.dailyTodayPersonalBest = 600f;
        }
        string string2 = PlayerPrefs.GetString("dailyyesterdaydata", "nodata");
        if (string2.Equals("nodata"))
        {
            string value2 = this.yesterdayFilename + "-99";
            PlayerPrefs.SetString("dailyyesterdaydata", value2);
            if (Debug.isDebugBuild)
            {
                UnityEngine.Debug.Log("yesterday PPref = nodata");
            }
        }
        string string3 = PlayerPrefs.GetString("dailyereyesterdaydata", "nodata");
        if (string3.Equals("nodata"))
        {
            string value3 = this.ereyesterdayFilename + "-99";
            PlayerPrefs.SetString("dailyereyesterdaydata", value3);
            if (Debug.isDebugBuild)
            {
                UnityEngine.Debug.Log("ereyesterday PPref = nodata");
            }
        }
        PlayerPrefs.Save();
    }

    private static int GetDailyStars(string prefname)
    {
        string str = PlayerPrefs.GetString(prefname, "nodata");
        string[] array = str.Split('-');
        return Convert.ToInt32(array[2]);
    } 

    public void PressShowLeaderboardButton()
	{
		AudioManager.Instance.PlayClick();
		this.leaderBoardScript.ShowLeaderboards();
    } 

    public void PressSignoutButton()
	{
		AudioManager.Instance.PlayClick();
		this.leaderBoardScript.LeaderboardLogout();
        this.PressCloseButton();
    }

    public void PressCloseButton()
    { 
        this.SetButtonsInteractable(true);
		AudioManager.Instance.PlayClick();
	}

    private void SetButtonsInteractable(bool bSetting)
    {
        this.todayButton.interactable = bSetting;
        this.yesterdayButton.interactable = bSetting;
        this.ereyesterdayButton.interactable = bSetting;
        this.leaderboardButton.interactable = bSetting;
        //if (LevelProgressControl.control.androidSdkLevel < 14)
        //{
        //    this.leaderboardButton.interactable = false;
        //}
        float a = 1f;
        if (!bSetting)
        {
            a = 0.25f;
        }
        Color color = new Color(1f, 1f, 1f, a);
        ((Component)this.todayButton).GetComponentInChildren<Text>().color = color;
        ((Component)this.yesterdayButton).GetComponentInChildren<Text>().color = color;
        ((Component)this.ereyesterdayButton).GetComponentInChildren<Text>().color = color;
    }

    private void DailyRolloverCountdown()
    {
        DateTime dateTime = DateTime.UtcNow.AddHours(-8.0);
        UnityEngine.Debug.Log("UTC Now today (PST) : " + dateTime);
        DateTime dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
        UnityEngine.Debug.Log("UTC Rollover midnight 24 hrs early (PST) : " + dateTime2);
        DateTime dateTime3 = dateTime2.AddDays(1.0);
        UnityEngine.Debug.Log("UTC Rollover midnight (PST) : " + dateTime3);
        TimeSpan timeSpan = dateTime3.Subtract(dateTime);
        UnityEngine.Debug.Log("Time until roll over " + timeSpan);
        string str = string.Format("{0:D2}:{1:D2}", timeSpan.Hours, timeSpan.Minutes);
        this.rolloverTimeText.text = "New Daily Puzzle in " + str;
    }

    private IEnumerator FlashSchedule()
    {
        while (true)
        {
            yield return (object)new WaitForSeconds(1f);
            this.bestTimeText.gameObject.SetActive(false);
            yield return (object)new WaitForSeconds(0.25f);
            this.bestTimeText.gameObject.SetActive(true);
        }
    }
}



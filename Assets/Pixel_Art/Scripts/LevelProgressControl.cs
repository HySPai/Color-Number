/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class LevelProgressControl : MonoBehaviour
{
    public enum PuzzleSourceType
    {
        PUZZLE_STORY,
        PUZZLE_DAILY,
        PUZZLE_PACK
    }

    public static LevelProgressControl control;

    public static int LevelsPerPack = 12;

    public static int NumberOfPacks = 60;

    public static int NumberOfActivePacks = 18;

    public static int LevelsToUnlock = 6;

    public static long CipherKey = 1273017494670060202L;

    public static int HintPotMax = 999;

    public static int adPeriodInSeconds = 300;

    [HideInInspector]
    public int[] levelOffsetArray;

    [HideInInspector]
    public int[] completeArray;

    [HideInInspector]
    public int[] movesPerfectArray;

    [HideInInspector]
    public int[] movesTakenArray;

    [HideInInspector]
    public int[] starsArray;

    [HideInInspector]
    public int[] timeTakenArray;

    [HideInInspector]
    public int[] attemptsTakenArray;

    [HideInInspector]
    public int[] hintsRemainingArray;

    [HideInInspector]
    public long[] hintsLastTimeArray;

    [HideInInspector]
    public int[] postcardCompleteStateArray;

    [HideInInspector]
    public int currentLevelId;

    [HideInInspector]
    public bool isReturningFromLevel;

    [HideInInspector]
    public int puzzleReturnId = -1;

    [HideInInspector]
    public int postcardReturnId;

    [HideInInspector]
    public int dailyDayOffset;

    [HideInInspector]
    public string postcardOrigin;

    [HideInInspector]
    public string puzzleBasename;

    [HideInInspector]
    public PuzzleSourceType puzzleSrcType;

    [HideInInspector]
    public float dailyTodayPersonalBest = 600f;

    [HideInInspector]
	public bool dailyTodayPersonalBestBeaten
	{
		get { return true; }
	}

    [HideInInspector]
    public int hintPotCount;

    [HideInInspector]
    public long hintPotLastPeriod;

    [HideInInspector]
    public bool iapUnlockLevels;

    [HideInInspector]
    public bool iapDisableAds;

    //[HideInInspector]
    //public int androidSdkLevel;

    private void Awake()
    {
        if (Debug.isDebugBuild)
        {
            UnityEngine.Debug.Log("LevelProgressControl : Awake()");
        }
        if ((UnityEngine.Object)LevelProgressControl.control == (UnityEngine.Object)null)
        {
            UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
            LevelProgressControl.control = this;
            this.levelOffsetArray = new int[LevelProgressControl.LevelsPerPack * LevelProgressControl.NumberOfPacks];
            this.completeArray = new int[LevelProgressControl.LevelsPerPack * LevelProgressControl.NumberOfPacks];
            this.movesPerfectArray = new int[LevelProgressControl.LevelsPerPack * LevelProgressControl.NumberOfPacks];
            this.movesTakenArray = new int[LevelProgressControl.LevelsPerPack * LevelProgressControl.NumberOfPacks];
            this.starsArray = new int[LevelProgressControl.LevelsPerPack * LevelProgressControl.NumberOfPacks];
            this.timeTakenArray = new int[LevelProgressControl.LevelsPerPack * LevelProgressControl.NumberOfPacks];
            this.attemptsTakenArray = new int[LevelProgressControl.LevelsPerPack * LevelProgressControl.NumberOfPacks];
            this.hintsRemainingArray = new int[LevelProgressControl.LevelsPerPack * LevelProgressControl.NumberOfPacks];
            this.hintsLastTimeArray = new long[LevelProgressControl.LevelsPerPack * LevelProgressControl.NumberOfPacks];
            this.postcardCompleteStateArray = new int[LevelProgressControl.NumberOfPacks];
            this.LoadProgressData();


			#if (UNITY_ANDROID || (UNITY_IPHONE && !NO_GPGS))
			PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
				// enables saving game progress.
				//.EnableSavedGames()

				// require access to a player's Google+ social graph (usually not needed)
				//.RequireGooglePlus()
				.Build();
			PlayGamesPlatform.InitializeInstance(config);

			PlayGamesPlatform.DebugLogEnabled = true;
			PlayGamesPlatform.Activate();
			#endif
			//this.androidSdkLevel = this.GetAndroidSDKLevel();
		}
        else if ((UnityEngine.Object)LevelProgressControl.control != (UnityEngine.Object)this)
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
    }

    //private int GetAndroidSDKLevel()
    //{
    //    IntPtr clazz = AndroidJNI.FindClass("android.os.Build$VERSION");
    //    IntPtr staticFieldID = AndroidJNI.GetStaticFieldID(clazz, "SDK_INT", "I");
    //    return AndroidJNI.GetStaticIntField(clazz, staticFieldID);
    //}

    public void SaveProgressData()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.persistentDataPath + "/lpC10.dat");
        PlayerData playerData = new PlayerData(LevelProgressControl.LevelsPerPack, LevelProgressControl.NumberOfPacks);
        Array.Copy(this.levelOffsetArray, playerData.levelOffsetArray, this.levelOffsetArray.Length);
        Array.Copy(this.completeArray, playerData.completeArray, this.completeArray.Length);
        Array.Copy(this.movesPerfectArray, playerData.movesPerfectArray, this.movesPerfectArray.Length);
        Array.Copy(this.movesTakenArray, playerData.movesTakenArray, this.movesTakenArray.Length);
        Array.Copy(this.starsArray, playerData.starsArray, this.starsArray.Length);
        Array.Copy(this.timeTakenArray, playerData.timeTakenArray, this.timeTakenArray.Length);
        Array.Copy(this.attemptsTakenArray, playerData.attemptsTakenArray, this.attemptsTakenArray.Length);
        Array.Copy(this.hintsRemainingArray, playerData.hintsRemainingArray, this.hintsRemainingArray.Length);
        Array.Copy(this.hintsLastTimeArray, playerData.hintsLastTimeArray, this.hintsLastTimeArray.Length);
        Array.Copy(this.postcardCompleteStateArray, playerData.postcardCompleteStateArray, this.postcardCompleteStateArray.Length);
        binaryFormatter.Serialize(fileStream, playerData);
        fileStream.Close();
        this.SaveProgressDataPrefsOnly();
    }

    public void SaveProgressDataPrefsOnly()
    {
        PlayerPrefs.SetString("PuzzleColorId", (this.hintPotLastPeriod ^ LevelProgressControl.CipherKey).ToString());
        PlayerPrefs.SetString("PuzzleColorKey", (this.hintPotCount ^ LevelProgressControl.CipherKey).ToString());
        PlayerPrefs.SetString("PuzzleShapeSeed", (Convert.ToInt64(this.iapUnlockLevels) + 2927 ^ LevelProgressControl.CipherKey).ToString());
        PlayerPrefs.SetString("PuzzleDiffTrackSeed", (Convert.ToInt64(this.iapDisableAds) + 1066 ^ LevelProgressControl.CipherKey).ToString());
        if (this.dailyTodayPersonalBestBeaten)
        {
            PlayerPrefs.SetInt("DailyPersonalBest", (int)this.dailyTodayPersonalBest);
        }
        PlayerPrefs.Save();
    }

    public void _LoadProgressData()
    {
        if (File.Exists(Application.persistentDataPath + "/lp.dat"))
        {
            if (Debug.isDebugBuild)
            {
                UnityEngine.Debug.Log("LoadProgressData() : Data file found. Loading..." + Application.persistentDataPath + "/lp.dat");
            }
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.Open(Application.persistentDataPath + "/lp.dat", FileMode.Open);
            PlayerData playerData = (PlayerData)binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            Array.Copy(playerData.levelOffsetArray, this.levelOffsetArray, playerData.levelOffsetArray.Length);
            Array.Copy(playerData.completeArray, this.completeArray, playerData.completeArray.Length);
            Array.Copy(playerData.movesPerfectArray, this.movesPerfectArray, playerData.movesPerfectArray.Length);
            Array.Copy(playerData.movesTakenArray, this.movesTakenArray, playerData.movesTakenArray.Length);
            Array.Copy(playerData.starsArray, this.starsArray, playerData.starsArray.Length);
            Array.Copy(playerData.timeTakenArray, this.timeTakenArray, playerData.timeTakenArray.Length);
            Array.Copy(playerData.attemptsTakenArray, this.attemptsTakenArray, playerData.attemptsTakenArray.Length);
            Array.Copy(playerData.hintsRemainingArray, this.hintsRemainingArray, playerData.hintsRemainingArray.Length);
            Array.Copy(playerData.hintsLastTimeArray, this.hintsLastTimeArray, playerData.hintsLastTimeArray.Length);
            Array.Copy(playerData.postcardCompleteStateArray, this.postcardCompleteStateArray, playerData.postcardCompleteStateArray.Length);
        }
        else if (Debug.isDebugBuild)
        {
            UnityEngine.Debug.Log("LoadProgressData() : No data file, so assuming a fresh start");
        }
        this.LoadPlayerPrefs();
    }

    private void LoadProgressDataLegacy()
    {
        if (Debug.isDebugBuild)
        {
            UnityEngine.Debug.Log("LoadProgressDataLegacy()");
        }
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Open(Application.persistentDataPath + "/lp.dat", FileMode.Open);
        PlayerData playerData = (PlayerData)binaryFormatter.Deserialize(fileStream);
        fileStream.Close();
        Array.Copy(playerData.levelOffsetArray, this.levelOffsetArray, playerData.levelOffsetArray.Length);
        Array.Copy(playerData.completeArray, this.completeArray, playerData.completeArray.Length);
        Array.Copy(playerData.movesPerfectArray, this.movesPerfectArray, playerData.movesPerfectArray.Length);
        Array.Copy(playerData.movesTakenArray, this.movesTakenArray, playerData.movesTakenArray.Length);
        Array.Copy(playerData.starsArray, this.starsArray, playerData.starsArray.Length);
        Array.Copy(playerData.timeTakenArray, this.timeTakenArray, playerData.timeTakenArray.Length);
        Array.Copy(playerData.attemptsTakenArray, this.attemptsTakenArray, playerData.attemptsTakenArray.Length);
        Array.Copy(playerData.hintsRemainingArray, this.hintsRemainingArray, playerData.hintsRemainingArray.Length);
        Array.Copy(playerData.hintsLastTimeArray, this.hintsLastTimeArray, playerData.hintsLastTimeArray.Length);
        Array.Copy(playerData.postcardCompleteStateArray, this.postcardCompleteStateArray, playerData.postcardCompleteStateArray.Length);
        if (Debug.isDebugBuild)
        {
            UnityEngine.Debug.Log("Legacy array length : " + playerData.completeArray.Length);
            UnityEngine.Debug.Log("C10 array length : " + this.completeArray.Length);
        }
    }

    public void LoadProgressData()
    {
        this.LoadPlayerPrefs();
        if (File.Exists(Application.persistentDataPath + "/lpC10.dat"))
        {
            UnityEngine.Debug.Log("10-CHAPTER FILE EXISTS");
            this.LoadDataStandardStart();
        }
        else if (File.Exists(Application.persistentDataPath + "/lp.dat"))
        {
            UnityEngine.Debug.Log("LEGACY DATA EXISTS");
            PlayerPrefs.SetInt("SuggestHint", 0);
            PlayerPrefs.Save();
            this.LoadDataUpdateStart();
        }
        else
        {
            UnityEngine.Debug.Log("LEGACY DATA DOES NOT EXIST");
            this.LoadDataFreshStart();
        }
    }

    public void LoadDataStandardStart()
    {
        if (Debug.isDebugBuild)
        {
            UnityEngine.Debug.Log("LoadDataStandardStart()");
        }
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Open(Application.persistentDataPath + "/lpC10.dat", FileMode.Open);
        PlayerData playerData = (PlayerData)binaryFormatter.Deserialize(fileStream);
        fileStream.Close();
        Array.Copy(playerData.levelOffsetArray, this.levelOffsetArray, playerData.levelOffsetArray.Length);
        Array.Copy(playerData.completeArray, this.completeArray, playerData.completeArray.Length);
        Array.Copy(playerData.movesPerfectArray, this.movesPerfectArray, playerData.movesPerfectArray.Length);
        Array.Copy(playerData.movesTakenArray, this.movesTakenArray, playerData.movesTakenArray.Length);
        Array.Copy(playerData.starsArray, this.starsArray, playerData.starsArray.Length);
        Array.Copy(playerData.timeTakenArray, this.timeTakenArray, playerData.timeTakenArray.Length);
        Array.Copy(playerData.attemptsTakenArray, this.attemptsTakenArray, playerData.attemptsTakenArray.Length);
        Array.Copy(playerData.hintsRemainingArray, this.hintsRemainingArray, playerData.hintsRemainingArray.Length);
        Array.Copy(playerData.hintsLastTimeArray, this.hintsLastTimeArray, playerData.hintsLastTimeArray.Length);
        Array.Copy(playerData.postcardCompleteStateArray, this.postcardCompleteStateArray, playerData.postcardCompleteStateArray.Length);
    }

    public void LoadDataUpdateStart()
    {
        if (Debug.isDebugBuild)
        {
            UnityEngine.Debug.Log("LoadDataUpdateStart()");
        }
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Open(Application.persistentDataPath + "/lp.dat", FileMode.Open);
        PlayerData playerData = (PlayerData)binaryFormatter.Deserialize(fileStream);
        fileStream.Close();
        Array.Copy(playerData.levelOffsetArray, this.levelOffsetArray, playerData.levelOffsetArray.Length);
        Array.Copy(playerData.completeArray, this.completeArray, playerData.completeArray.Length);
        Array.Copy(playerData.movesPerfectArray, this.movesPerfectArray, playerData.movesPerfectArray.Length);
        Array.Copy(playerData.movesTakenArray, this.movesTakenArray, playerData.movesTakenArray.Length);
        Array.Copy(playerData.starsArray, this.starsArray, playerData.starsArray.Length);
        Array.Copy(playerData.timeTakenArray, this.timeTakenArray, playerData.timeTakenArray.Length);
        Array.Copy(playerData.attemptsTakenArray, this.attemptsTakenArray, playerData.attemptsTakenArray.Length);
        Array.Copy(playerData.hintsRemainingArray, this.hintsRemainingArray, playerData.hintsRemainingArray.Length);
        Array.Copy(playerData.hintsLastTimeArray, this.hintsLastTimeArray, playerData.hintsLastTimeArray.Length);
        Array.Copy(playerData.postcardCompleteStateArray, this.postcardCompleteStateArray, playerData.postcardCompleteStateArray.Length);
    }

    public void LoadDataFreshStart()
    {
        if (Debug.isDebugBuild)
        {
            UnityEngine.Debug.Log("LoadDataFreshStart()");
        }
    }

    public void ResetProgressData()
    {
        Array.Clear(this.levelOffsetArray, 0, this.levelOffsetArray.Length);
        Array.Clear(this.completeArray, 0, this.completeArray.Length);
        Array.Clear(this.movesPerfectArray, 0, this.movesPerfectArray.Length);
        Array.Clear(this.movesTakenArray, 0, this.movesTakenArray.Length);
        Array.Clear(this.starsArray, 0, this.starsArray.Length);
        Array.Clear(this.timeTakenArray, 0, this.timeTakenArray.Length);
        Array.Clear(this.hintsRemainingArray, 0, this.hintsRemainingArray.Length);
        Array.Clear(this.hintsLastTimeArray, 0, this.hintsLastTimeArray.Length);
        Array.Clear(this.postcardCompleteStateArray, 0, this.postcardCompleteStateArray.Length);
        this.SaveProgressData();
    }

    public void LoadPlayerPrefs()
    {
        string str = PlayerPrefs.GetString("PuzzleColorId", "no");
        long num = default(long);
        if (str.Equals("no"))
        {
            long ticks = DateTime.Now.Ticks;
            PlayerPrefs.SetString("PuzzleColorId", (ticks ^ LevelProgressControl.CipherKey).ToString());
            this.hintPotLastPeriod = ticks;
        }
        else if (long.TryParse(str, out num))
        {
            long num2 = this.hintPotLastPeriod = (num ^ LevelProgressControl.CipherKey);
        }
        string string2 = PlayerPrefs.GetString("PuzzleColorKey", "no");
        long num3 = default(long);
        if (string2.Equals("no"))
        {
            PlayerPrefs.SetString("PuzzleColorKey", (3 ^ LevelProgressControl.CipherKey).ToString());
            this.hintPotCount = 3;
        }
        else if (long.TryParse(string2, out num3))
        {
            if (Debug.isDebugBuild)
            {
                UnityEngine.Debug.Log("PuzzleColorKey : " + num3.ToString());
            }
            long num4 = num3 ^ LevelProgressControl.CipherKey;
            if (Debug.isDebugBuild)
            {
                UnityEngine.Debug.Log("decode : " + num4);
            }
            this.hintPotCount = Convert.ToInt32(num4);
        }
        string string3 = PlayerPrefs.GetString("PuzzleShapeSeed", "no");
        long num5 = default(long);
        if (string3.Equals("no"))
        {
            if (Debug.isDebugBuild)
            {
                UnityEngine.Debug.Log("PuzzleShapeSeed NOT SET");
            }
            PlayerPrefs.SetString("PuzzleShapeSeed", (Convert.ToInt64(false) + 2927 ^ LevelProgressControl.CipherKey).ToString());
            this.iapUnlockLevels = false;
        }
        else if (long.TryParse(string3, out num5))
        {
            if (Debug.isDebugBuild)
            {
                UnityEngine.Debug.Log("PuzzleShapeSeed : " + num5.ToString());
            }
            long num6 = (num5 ^ LevelProgressControl.CipherKey) - 2927;
            if (Debug.isDebugBuild)
            {
                UnityEngine.Debug.Log("decode : " + num6);
            }
            this.iapUnlockLevels = Convert.ToBoolean(num6);
        }
        string string4 = PlayerPrefs.GetString("PuzzleDiffTrackSeed", "no");
        long num7 = default(long);
        if (string4.Equals("no"))
        {
            if (Debug.isDebugBuild)
            {
                UnityEngine.Debug.Log("PuzzleDiffTrackSeed NOT SET");
            }
            PlayerPrefs.SetString("PuzzleDiffTrackSeed", (Convert.ToInt64(false) + 1066 ^ LevelProgressControl.CipherKey).ToString());
            this.iapDisableAds = false;
        }
        else if (long.TryParse(string4, out num7))
        {
            if (Debug.isDebugBuild)
            {
                UnityEngine.Debug.Log("PuzzleDiffTrackSeed : " + num7.ToString());
            }
            long num8 = (num7 ^ LevelProgressControl.CipherKey) - 1066;
            if (Debug.isDebugBuild)
            {
                UnityEngine.Debug.Log("decode : " + num8);
            }
            this.iapDisableAds = Convert.ToBoolean(num8);
        }
        int number = PlayerPrefs.GetInt("DailyPersonalBest", 0);
        if (number == 0)
        {
            LevelProgressControl.control.dailyTodayPersonalBest = 600f;
        }
        else
        {
            LevelProgressControl.control.dailyTodayPersonalBest = (float)number;
        }
        PlayerPrefs.Save();
    }

    public bool CheckHintPot()
    {
        DateTime dateTime = new DateTime(this.hintPotLastPeriod);
        DateTime now = DateTime.Now;
        int num = dateTime.CompareTo(DateTime.Now);
        if (num < 0)
        {
            if (Debug.isDebugBuild)
            {
                UnityEngine.Debug.Log("CheckHintPot : earlier");
            }
            TimeSpan timeSpan = now - dateTime;
            if (Debug.isDebugBuild)
            {
                UnityEngine.Debug.Log(now);
                UnityEngine.Debug.Log(dateTime);
                UnityEngine.Debug.Log(timeSpan);
                UnityEngine.Debug.Log("timediff.TotalHours : " + timeSpan.TotalHours);
                UnityEngine.Debug.Log("timediff.Days*24 + timeDiff.Hours : " + (timeSpan.Days * 24 + timeSpan.Hours));
            }
            if (timeSpan.Days * 24 + timeSpan.Hours > 6)
            {
                return true;
            }
        }
        else if (num == 0)
        {
            if (Debug.isDebugBuild)
            {
                UnityEngine.Debug.Log("CheckHintPot : same");
            }
        }
        else if (Debug.isDebugBuild)
        {
            UnityEngine.Debug.Log("CheckHintPot : later");
        }
        return false;
    }

    public void IncrementHintPot()
    {
        if (this.hintPotCount < LevelProgressControl.HintPotMax)
        {
            this.hintPotCount++;
            this.hintPotLastPeriod = DateTime.Now.Ticks;
            long num = this.hintPotLastPeriod;
            PlayerPrefs.SetString("PuzzleColorId", (num ^ LevelProgressControl.CipherKey).ToString());
            PlayerPrefs.SetString("PuzzleColorKey", (this.hintPotCount ^ LevelProgressControl.CipherKey).ToString());
        }
    }

    public void RewardHintPot(int rewards)
    {
        if (this.hintPotCount < LevelProgressControl.HintPotMax)
        {
            this.hintPotCount += rewards;
            this.hintPotCount = Mathf.Clamp(this.hintPotCount, 0, LevelProgressControl.HintPotMax);
            PlayerPrefs.SetString("PuzzleColorKey", (this.hintPotCount ^ LevelProgressControl.CipherKey).ToString());
        }
    }

    public int NumberOfPostcardsUnlocked()
    {
        int num = 0;
        for (int i = 0; i < LevelProgressControl.NumberOfPacks; i++)
        {
            if (this.postcardCompleteStateArray[i] > 0)
            {
                num++;
            }
        }
        return num;
    }
}



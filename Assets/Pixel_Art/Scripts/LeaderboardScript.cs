/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#elif UNITY_IOS
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;
#endif
using System;
using UnityEngine;

public class LeaderboardScript : MonoBehaviour
{
    [Flags]
    public enum AchievementFlags
    {
        COMPLETE_C1 = 1,
        COMPLETE_C2 = 2,
        COMPLETE_C3 = 4,
        THREESTAR_C1 = 8,
        THREESTAR_C2 = 0x10,
        THREESTAR_C3 = 0x20
    }

    private void Start()
    {   
		this.InitLeaderboards(); 
    }

	//public void CheckForPastAchievements()
	//{
	//    if (this.IsAuthenticated())
	//    {
	//        UnityEngine.Debug.Log("CheckForPastAchievements... IsAuthenticated YES");
	//        int @int = PlayerPrefs.GetInt("CheckedPastAch", 0);
	//        AchievementFlags achievementFlags = (AchievementFlags)@int;
	//        UnityEngine.Debug.Log("CheckForPastAchievements : CheckedPastAch Pref : " + @int);
	//        UnityEngine.Debug.Log("Achievement flags : " + achievementFlags);
	//        if ((achievementFlags & AchievementFlags.COMPLETE_C1) == (AchievementFlags)0 && this.TestChapterComplete(1))
	//        {
	//            UnityEngine.Debug.Log("CheckForPastAchievements... awarding CgkIifzNtrcZEAIQAw");
	//            this.ReportAchievementProgress("CgkIifzNtrcZEAIQAw", 100.0);
	//            achievementFlags |= AchievementFlags.COMPLETE_C1;
	//        }
	//        if ((achievementFlags & AchievementFlags.COMPLETE_C2) == (AchievementFlags)0 && this.TestChapterComplete(2))
	//        {
	//            UnityEngine.Debug.Log("CheckForPastAchievements... awarding CgkIifzNtrcZEAIQBA");
	//            this.ReportAchievementProgress("CgkIifzNtrcZEAIQBA", 100.0);
	//            achievementFlags |= AchievementFlags.COMPLETE_C2;
	//        }
	//        if ((achievementFlags & AchievementFlags.COMPLETE_C3) == (AchievementFlags)0 && this.TestChapterComplete(3))
	//        {
	//            UnityEngine.Debug.Log("CheckForPastAchievements... awarding CgkIifzNtrcZEAIQBQ");
	//            this.ReportAchievementProgress("CgkIifzNtrcZEAIQBQ", 100.0);
	//            achievementFlags |= AchievementFlags.COMPLETE_C3;
	//        }
	//        if ((achievementFlags & AchievementFlags.THREESTAR_C1) == (AchievementFlags)0 && this.TestChapter3Starred(1))
	//        {
	//            UnityEngine.Debug.Log("CheckForPastAchievements... awarding CgkIifzNtrcZEAIQBg");
	//            this.ReportAchievementProgress("CgkIifzNtrcZEAIQBg", 100.0);
	//            achievementFlags |= AchievementFlags.THREESTAR_C1;
	//        }
	//        if ((achievementFlags & AchievementFlags.THREESTAR_C2) == (AchievementFlags)0 && this.TestChapter3Starred(2))
	//        {
	//            UnityEngine.Debug.Log("CheckForPastAchievements... awarding CgkIifzNtrcZEAIQBw");
	//            this.ReportAchievementProgress("CgkIifzNtrcZEAIQBw", 100.0);
	//            achievementFlags |= AchievementFlags.THREESTAR_C2;
	//        }
	//        if ((achievementFlags & AchievementFlags.THREESTAR_C3) == (AchievementFlags)0 && this.TestChapter3Starred(3))
	//        {
	//            UnityEngine.Debug.Log("CheckForPastAchievements... awarding CgkIifzNtrcZEAIQCA");
	//            this.ReportAchievementProgress("CgkIifzNtrcZEAIQCA", 100.0);
	//            achievementFlags |= AchievementFlags.THREESTAR_C3;
	//        }
	//        PlayerPrefs.SetInt("CheckedPastAch", (int)achievementFlags);
	//        PlayerPrefs.Save();
	//    }
	//    else
	//    {
	//        UnityEngine.Debug.Log("CheckForPastAchievements... IsAuthenticated NO");
	//    }
	//}

	public void InitLeaderboards()
	{
		if (!this.IsAuthenticated())
		{
			try
			{
				Social.localUser.Authenticate(new Action<bool>(this.ProcessAuthentication));
			}
			catch (Exception ex)
			{
				Debug.LogError(ex);
			}
		}
		else
		{
			UnityEngine.Debug.Log("InitLeaderboards :  Already Authenticated");
		}
	}

    public bool IsAuthenticated()
    {
        if (Social.localUser.authenticated)
        {
            return true;
        }
        return false;
    }

    private void ProcessAuthentication(bool success)
    {
        if (success)
        {
            UnityEngine.Debug.Log("ProcessAuthentication - success");
            //this.CheckForPastAchievements();
        }
        else
        {
            UnityEngine.Debug.Log("ProcessAuthentication - failed");
        }
    }

    public void ShowAchievements()
    {
        if (this.IsAuthenticated())
        {
            Social.ShowAchievementsUI();
        }
    }

    public void ShowLeaderboards()
    {
        if (this.IsAuthenticated())
        {
            Social.ShowLeaderboardUI();
        }
		else
		{
			try
			{
				Social.localUser.Authenticate((b) =>
				{
					if (b)
					{
						Social.ShowLeaderboardUI();
					}
				});
			}
			catch (Exception ex)
			{
				Debug.LogError(ex);
			}
		}
    }

    public void ReportLeaderboardScore(long score, string leaderBoardID)
    {
        if (this.IsAuthenticated())
        {
            Social.ReportScore(score, leaderBoardID, delegate(bool success)
            {
                if (success)
                {
                    UnityEngine.Debug.Log("Reported time successfully");
                }
                else
                {
                    UnityEngine.Debug.Log("Failed to report time " + leaderBoardID);
                }
            });
        }
        else
        {
            UnityEngine.Debug.Log("ReportLeaderboardScore NOT authenticated");
        }
    }

    //public void ReportAchievementProgress(string achievementID, double progress)
    //{
    //    if (this.IsAuthenticated())
    //    {
    //        Social.ReportProgress(achievementID, progress, delegate(bool success)
    //        {
    //            if (success)
    //            {
    //                UnityEngine.Debug.Log("Reported achievement successfully");
    //            }
    //            else
    //            {
    //                UnityEngine.Debug.Log("Failed to report achievement " + achievementID);
    //            }
    //        });
    //    }
    //    else
    //    {
    //        UnityEngine.Debug.Log("ReportAchievementProgress NOT authenticated");
    //    }
    //}

    public void LeaderboardLogout()
	{
#if (UNITY_ANDROID || (UNITY_IPHONE && !NO_GPGS))
		((GooglePlayGames.PlayGamesPlatform)Social.Active).SignOut();
#endif
    }

    public int GetChapterNumber(int postcardIndex)
    {
        return postcardIndex / 6 + 1;
    }

    public bool TestChapterComplete(int num)
    {
        UnityEngine.Debug.Log("TestChapterComplete : " + num);
        int num2 = 0;
        int num3 = (num - 1) * 6;
        for (int i = num3; i < num3 + 6; i++)
        {
            num2 += LevelProgressControl.control.postcardCompleteStateArray[i];
        }
        if (num2 == 12)
        {
            return true;
        }
        return false;
    }

    public bool TestChapter3Starred(int num)
    {
        UnityEngine.Debug.Log("TestChapter3Starred : " + num);
        int num2 = 0;
        int num3 = (num - 1) * 72;
        int num4 = num3 + 72;
        for (int i = num3; i < num4; i++)
        {
            num2 += LevelProgressControl.control.starsArray[i];
        }
        if (num2 == 216)
        {
            return true;
        }
        return false;
    }

    //public void TestChapter3StarredFromPostcard(int pcIdx)
    //{
    //    UnityEngine.Debug.Log("TestChapter3StarredFromPostcard : " + pcIdx);
    //    int @int = PlayerPrefs.GetInt("CheckedPastAch", 0);
    //    AchievementFlags achievementFlags = (AchievementFlags)@int;
    //    int chapterNumber = this.GetChapterNumber(pcIdx);
    //    switch (chapterNumber)
    //    {
    //        default:
    //            return;
    //        case 1:
    //            if ((achievementFlags & AchievementFlags.THREESTAR_C1) == (AchievementFlags)0 && this.TestChapter3Starred(chapterNumber))
    //            {
    //                this.ReportAchievementProgress("CgkIifzNtrcZEAIQBg", 100.0);
    //                achievementFlags |= AchievementFlags.THREESTAR_C1;
    //            }
    //            break;
    //        case 2:
    //            if ((achievementFlags & AchievementFlags.THREESTAR_C2) == (AchievementFlags)0 && this.TestChapter3Starred(chapterNumber))
    //            {
    //                this.ReportAchievementProgress("CgkIifzNtrcZEAIQBw", 100.0);
    //                achievementFlags |= AchievementFlags.THREESTAR_C2;
    //            }
    //            break;
    //        case 3:
    //            if ((achievementFlags & AchievementFlags.THREESTAR_C3) == (AchievementFlags)0 && this.TestChapter3Starred(chapterNumber))
    //            {
    //                this.ReportAchievementProgress("CgkIifzNtrcZEAIQCA", 100.0);
    //                achievementFlags |= AchievementFlags.THREESTAR_C3;
    //            }
    //            break;
    //    }
    //    PlayerPrefs.SetInt("CheckedPastAch", (int)achievementFlags);
    //    PlayerPrefs.Save();
    //}

    //public void TestChapterCompleteFromPostcard(int pcIdx)
    //{
    //    UnityEngine.Debug.Log("TestChapterCompleteFromPostcard : " + pcIdx);
    //    int @int = PlayerPrefs.GetInt("CheckedPastAch", 0);
    //    AchievementFlags achievementFlags = (AchievementFlags)@int;
    //    int chapterNumber = this.GetChapterNumber(pcIdx);
    //    switch (chapterNumber)
    //    {
    //        default:
    //            return;
    //        case 1:
    //            if ((achievementFlags & AchievementFlags.COMPLETE_C1) == (AchievementFlags)0 && this.TestChapterComplete(chapterNumber))
    //            {
    //                this.ReportAchievementProgress("CgkIifzNtrcZEAIQAw", 100.0);
    //                achievementFlags |= AchievementFlags.COMPLETE_C1;
    //            }
    //            break;
    //        case 2:
    //            if ((achievementFlags & AchievementFlags.COMPLETE_C2) == (AchievementFlags)0 && this.TestChapterComplete(chapterNumber))
    //            {
    //                this.ReportAchievementProgress("CgkIifzNtrcZEAIQBA", 100.0);
    //                achievementFlags |= AchievementFlags.COMPLETE_C2;
    //            }
    //            break;
    //        case 3:
    //            if ((achievementFlags & AchievementFlags.COMPLETE_C3) == (AchievementFlags)0 && this.TestChapterComplete(chapterNumber))
    //            {
    //                this.ReportAchievementProgress("CgkIifzNtrcZEAIQBQ", 100.0);
    //                achievementFlags |= AchievementFlags.COMPLETE_C3;
    //            }
    //            break;
    //    }
    //    PlayerPrefs.SetInt("CheckedPastAch", (int)achievementFlags);
    //    PlayerPrefs.Save();
    //}
}



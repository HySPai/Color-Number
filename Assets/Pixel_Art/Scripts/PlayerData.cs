/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System;

[Serializable]
public class PlayerData
{
    public int[] levelOffsetArray;

    public int[] completeArray;

    public int[] movesPerfectArray;

    public int[] movesTakenArray;

    public int[] starsArray;

    public int[] timeTakenArray;

    public int[] attemptsTakenArray;

    public int[] hintsRemainingArray;

    public long[] hintsLastTimeArray;

    public int[] postcardCompleteStateArray;

    public PlayerData(int levelsPerPack, int numPacks)
    {
        this.levelOffsetArray = new int[levelsPerPack * numPacks];
        this.completeArray = new int[levelsPerPack * numPacks];
        this.movesPerfectArray = new int[levelsPerPack * numPacks];
        this.movesTakenArray = new int[levelsPerPack * numPacks];
        this.starsArray = new int[levelsPerPack * numPacks];
        this.timeTakenArray = new int[levelsPerPack * numPacks];
        this.attemptsTakenArray = new int[levelsPerPack * numPacks];
        this.hintsRemainingArray = new int[levelsPerPack * numPacks];
        this.hintsLastTimeArray = new long[levelsPerPack * numPacks];
        this.postcardCompleteStateArray = new int[numPacks];
    }
}



/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

// <copyright file="NativePlayerStats.cs" company="Google Inc.">
// Copyright (C) 2015 Google Inc. All Rights Reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>

#if (UNITY_ANDROID || (UNITY_IPHONE && !NO_GPGS))

namespace GooglePlayGames.Native.PInvoke
{
    using System;
    using System.Runtime.InteropServices;
    using GooglePlayGames;
    using GooglePlayGames.BasicApi;
    using UnityEngine;
    using C = GooglePlayGames.Native.Cwrapper.PlayerStats;

    internal class NativePlayerStats : BaseReferenceHolder
    {
        internal NativePlayerStats(IntPtr selfPointer)
            : base (selfPointer)
        {
        }

        internal bool Valid()
        {
            return C.PlayerStats_Valid(SelfPtr());
        }

        internal bool HasAverageSessionLength()
        {
            return C.PlayerStats_HasAverageSessionLength(SelfPtr());
        }

        internal float AverageSessionLength()
        {
            return C.PlayerStats_AverageSessionLength(SelfPtr());
        }

        internal bool HasChurnProbability()
        {
            return C.PlayerStats_HasChurnProbability(SelfPtr());
        }

        internal float ChurnProbability()
        {
            return C.PlayerStats_ChurnProbability(SelfPtr());
        }

        internal bool HasDaysSinceLastPlayed()
        {
            return C.PlayerStats_HasDaysSinceLastPlayed(SelfPtr());
        }

        internal int DaysSinceLastPlayed()
        {
            return C.PlayerStats_DaysSinceLastPlayed(SelfPtr());
        }

        internal bool HasNumberOfPurchases()
        {
            return C.PlayerStats_HasNumberOfPurchases(SelfPtr());
        }

        internal int NumberOfPurchases()
        {
            return C.PlayerStats_NumberOfPurchases(SelfPtr());
        }

        internal bool HasNumberOfSessions()
        {
            return C.PlayerStats_HasNumberOfSessions(SelfPtr());
        }

        internal int NumberOfSessions()
        {
            return C.PlayerStats_NumberOfSessions(SelfPtr());
        }

        internal bool HasSessionPercentile()
        {
            return C.PlayerStats_HasSessionPercentile(SelfPtr());
        }

        internal float SessionPercentile()
        {
            return C.PlayerStats_SessionPercentile(SelfPtr());
        }

        internal bool HasSpendPercentile()
        {
            return C.PlayerStats_HasSpendPercentile(SelfPtr());
        }

        internal float SpendPercentile()
        {
            return C.PlayerStats_SpendPercentile(SelfPtr());
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            C.PlayerStats_Dispose(selfPointer);
        }

        internal PlayerStats AsPlayerStats()
        {
            PlayerStats playerStats = new PlayerStats();

            playerStats.Valid = Valid();
            if (Valid ()) {
                playerStats.AvgSessonLength = AverageSessionLength();
                playerStats.ChurnProbability = ChurnProbability();
                playerStats.DaysSinceLastPlayed = DaysSinceLastPlayed();
                playerStats.NumberOfPurchases = NumberOfPurchases();
                playerStats.NumberOfSessions = NumberOfSessions();
                playerStats.SessPercentile = SessionPercentile();
                playerStats.SpendPercentile = SpendPercentile();
                playerStats.SpendProbability = -1.0f;
            }

            return playerStats;
        }
    }
}
#endif // (UNITY_ANDROID || UNITY_IPHONE)

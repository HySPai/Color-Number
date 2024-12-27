/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

// <copyright file="PlayerStats.cs" company="Google Inc.">
// Copyright (C) 2014 Google Inc.
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

#if UNITY_ANDROID
//
// ****   GENERATED FILE  DO NOT EDIT !!!  ****//
//
using Google.Developers;
using System;
using UnityEngine;
namespace Com.Google.Android.Gms.Games.Stats
{
    public interface PlayerStats
    {
        float getAverageSessionLength();
        float getChurnProbability();
        int getDaysSinceLastPlayed();
        int getNumberOfPurchases();
        int getNumberOfSessions();
        float getSessionPercentile();
        float getSpendPercentile();
        float getSpendProbability();
        float getHighSpenderProbability();
        float getTotalSpendNext28Days();
    }
}
//
// ****   GENERATED FILE  DO NOT EDIT !!!  ****//
//
#endif

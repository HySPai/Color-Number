/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

// <copyright file="IClientImpl.cs" company="Google Inc.">
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

using System;
using GooglePlayGames.BasicApi;
using GooglePlayGames.Native.PInvoke;

namespace GooglePlayGames
{
    /// <summary>
    /// Interface defining platform specific functionality.
    /// </summary>
    internal interface IClientImpl
    {
        PlatformConfiguration CreatePlatformConfiguration (PlayGamesClientConfiguration clientConfig);

        TokenClient CreateTokenClient (bool reset);

        void GetPlayerStats(IntPtr apiClientPtr, Action<CommonStatusCodes, GooglePlayGames.BasicApi.PlayerStats> callback);

        /// <summary>
        /// Sets the gravity for popups (Android only).
        /// </summary>
        /// <remarks>This can only be called after authentication.  It affects
        /// popups for achievements and other game services elements.</remarks>
        /// <param name="gravity">Gravity for the popup.</param>
        void SetGravityForPopups(IntPtr apiClient, Gravity gravity);
    }
}

#endif

/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

// <copyright file="TokenClient.cs" company="Google Inc.">
// Copyright (C) 2015 Google Inc.
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
//  limitations under the License.
// </copyright>

#if (UNITY_ANDROID || (UNITY_IPHONE && !NO_GPGS))
namespace GooglePlayGames
{
    using System;

    internal interface TokenClient
    {
        /// <summary>
        /// Gets the user's email.
        /// </summary>
        /// <remarks>The email address returned is selected by the user from the accounts present
        /// on the device. There is no guarantee this uniquely identifies the player.
        /// For unique identification use the id property of the local player.
        /// The user can also choose to not select any email address, meaning it is not
        /// available.</remarks>
        /// <returns>The user email or null if not authenticated or the permission is
        /// not available.</returns>
        string GetEmail();
        string GetAuthCode();
        string GetIdToken();

        /// <summary>
        /// Gets another server auth code.
        /// </summary>
        /// <remarks>This method should be called after authenticating, and exchanging
        /// the initial server auth code for a token.  This is implemented by signing in
        /// silently, which if successful returns almost immediately and with a new
        /// server auth code.
        /// </remarks>
        /// <param name="reAuthenticateIfNeeded">Calls Authenticate if needed when
        /// retrieving another auth code. </param>
        /// <param name="callback">Callback.</param>
        void GetAnotherServerAuthCode(bool reAuthenticateIfNeeded,
                                      Action<string> callback);

        void Signout();

        void SetRequestAuthCode(bool flag, bool forceRefresh);

        void SetRequestEmail(bool flag);

        void SetRequestIdToken(bool flag);

        void SetWebClientId(string webClientId);

        void SetAccountName(string accountName);

        void AddOauthScopes(string[] scopes);

        void SetHidePopups(bool flag);

        bool NeedsToRun();

        void FetchTokens(Action<int> callback);
    }
}
#endif

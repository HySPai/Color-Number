/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

// <copyright file="GameServices.cs" company="Google Inc.">
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

#if (UNITY_ANDROID || (UNITY_IPHONE && !NO_GPGS))

namespace GooglePlayGames.Native.PInvoke
{
    using System;
    using System.Runtime.InteropServices;
    using System.Collections.Generic;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using C = GooglePlayGames.Native.Cwrapper.GameServices;

    class GameServices : BaseReferenceHolder
    {

        internal GameServices(IntPtr selfPointer)
            : base(selfPointer)
        {
        }

        internal bool IsAuthenticated()
        {
            return C.GameServices_IsAuthorized(SelfPtr());
        }

        internal void SignOut()
        {
            C.GameServices_SignOut(SelfPtr());
        }

        internal void StartAuthorizationUI()
        {
            C.GameServices_StartAuthorizationUI(SelfPtr());
        }

        public AchievementManager AchievementManager()
        {
            return new AchievementManager(this);
        }

        public LeaderboardManager LeaderboardManager()
        {
            return new LeaderboardManager(this);
        }

        public PlayerManager PlayerManager()
        {
            return new PlayerManager(this);
        }

        public StatsManager StatsManager()
        {
            return new StatsManager(this);
        }

        internal HandleRef AsHandle()
        {
            return SelfPtr();
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            C.GameServices_Dispose(selfPointer);
        }
    }
}


#endif

/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

// <copyright file="PlayerSelectUIResponse.cs" company="Google Inc.">
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
    using C = GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager;
    using Types = GooglePlayGames.Native.Cwrapper.Types;
    using Status = GooglePlayGames.Native.Cwrapper.CommonErrorStatus;
    using MultiplayerStatus = GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    internal class PlayerSelectUIResponse : BaseReferenceHolder, IEnumerable<string>
    {
        internal PlayerSelectUIResponse(IntPtr selfPointer)
            : base(selfPointer)
        {
        }

        internal Status.UIStatus Status()
        {
            return C.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetStatus(SelfPtr());
        }

        private string PlayerIdAtIndex(UIntPtr index)
        {
            return PInvokeUtilities.OutParamsToString(
                (out_string, size) => C.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetPlayerIds_GetElement(
                    SelfPtr(), index, out_string, size));
        }

        public IEnumerator<string> GetEnumerator()
        {
            return PInvokeUtilities.ToEnumerator<string>(
                C.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetPlayerIds_Length(SelfPtr()),
                PlayerIdAtIndex
            );
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal uint MinimumAutomatchingPlayers()
        {
            return C.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetMinimumAutomatchingPlayers(SelfPtr());
        }

        internal uint MaximumAutomatchingPlayers()
        {
            return C.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetMaximumAutomatchingPlayers(SelfPtr());
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            C.TurnBasedMultiplayerManager_PlayerSelectUIResponse_Dispose(selfPointer);
        }

        internal static PlayerSelectUIResponse FromPointer(IntPtr pointer)
        {
            if (PInvokeUtilities.IsNull(pointer))
            {
                return null;
            }

            return new PlayerSelectUIResponse(pointer);
        }
    }
}


#endif

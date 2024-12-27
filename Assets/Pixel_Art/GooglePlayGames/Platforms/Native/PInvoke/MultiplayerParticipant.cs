/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

// <copyright file="MultiplayerParticipant.cs" company="Google Inc.">
// Copyright (C) 2014 Google Inc. All Rights Reserved.
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
    using C = GooglePlayGames.Native.Cwrapper.MultiplayerParticipant;
    using Types = GooglePlayGames.Native.Cwrapper.Types;
    using Status = GooglePlayGames.Native.Cwrapper.CommonErrorStatus;
    using GooglePlayGames.BasicApi.Multiplayer;

    internal class MultiplayerParticipant : BaseReferenceHolder
    {

        private static readonly
        Dictionary<Types.ParticipantStatus, Participant.ParticipantStatus> StatusConversion =
            new Dictionary<Types.ParticipantStatus, Participant.ParticipantStatus>
            {
                { Types.ParticipantStatus.INVITED, Participant.ParticipantStatus.Invited },
                { Types.ParticipantStatus.JOINED, Participant.ParticipantStatus.Joined },
                { Types.ParticipantStatus.DECLINED, Participant.ParticipantStatus.Declined },
                { Types.ParticipantStatus.LEFT, Participant.ParticipantStatus.Left },
                { Types.ParticipantStatus.NOT_INVITED_YET, Participant.ParticipantStatus.NotInvitedYet },
                { Types.ParticipantStatus.FINISHED, Participant.ParticipantStatus.Finished },
                { Types.ParticipantStatus.UNRESPONSIVE, Participant.ParticipantStatus.Unresponsive },
            };

        internal MultiplayerParticipant(IntPtr selfPointer)
            : base(selfPointer)
        {
        }

        internal Types.ParticipantStatus Status()
        {
            return C.MultiplayerParticipant_Status(SelfPtr());
        }

        internal bool IsConnectedToRoom()
        {
            // Check the method, and the status to work around a bug found in Feb 2016
            return C.MultiplayerParticipant_IsConnectedToRoom(SelfPtr()) ||
                    Status() == Types.ParticipantStatus.JOINED;
        }

        internal string DisplayName()
        {
            return PInvokeUtilities.OutParamsToString(
                (out_string, size) => C.MultiplayerParticipant_DisplayName(SelfPtr(), out_string, size)
            );
        }

        internal NativePlayer Player()
        {
            if (!C.MultiplayerParticipant_HasPlayer(SelfPtr()))
            {
                return null;
            }

            return new NativePlayer(C.MultiplayerParticipant_Player(SelfPtr()));
        }

        internal string Id()
        {
            return PInvokeUtilities.OutParamsToString(
                (out_string, size) => C.MultiplayerParticipant_Id(SelfPtr(), out_string, size));
        }

        internal bool Valid()
        {
            return C.MultiplayerParticipant_Valid(SelfPtr());
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            C.MultiplayerParticipant_Dispose(selfPointer);
        }

        internal Participant AsParticipant()
        {
            NativePlayer nativePlayer = Player();

            return new Participant(
                DisplayName(),
                Id(),
                StatusConversion[Status()],
                nativePlayer == null ? null : nativePlayer.AsPlayer(),
                IsConnectedToRoom()
            );
        }

        internal static MultiplayerParticipant FromPointer(IntPtr pointer)
        {
            if (PInvokeUtilities.IsNull(pointer))
            {
                return null;
            }

            return new MultiplayerParticipant(pointer);
        }

        internal static MultiplayerParticipant AutomatchingSentinel()
        {
            return new MultiplayerParticipant(Sentinels.Sentinels_AutomatchingParticipant());
        }
    }
}

#endif

/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

// <copyright file="ParticipantResults.cs" company="Google Inc.">
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
    using C = GooglePlayGames.Native.Cwrapper.ParticipantResults;
    using Types = GooglePlayGames.Native.Cwrapper.Types;
    using Status = GooglePlayGames.Native.Cwrapper.CommonErrorStatus;

    internal class ParticipantResults : BaseReferenceHolder
    {

        internal ParticipantResults(IntPtr selfPointer)
            : base(selfPointer)
        {
        }

        internal bool HasResultsForParticipant(string participantId)
        {
            return C.ParticipantResults_HasResultsForParticipant(SelfPtr(), participantId);
        }

        internal uint PlacingForParticipant(string participantId)
        {
            return C.ParticipantResults_PlaceForParticipant(SelfPtr(), participantId);
        }

        internal Types.MatchResult ResultsForParticipant(string participantId)
        {
            return C.ParticipantResults_MatchResultForParticipant(SelfPtr(), participantId);
        }

        internal ParticipantResults WithResult(string participantId, uint placing,
                                           Types.MatchResult result)
        {
            return new ParticipantResults(C.ParticipantResults_WithResult(
                    SelfPtr(), participantId, placing, result));
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            C.ParticipantResults_Dispose(selfPointer);
        }
    }
}

#endif

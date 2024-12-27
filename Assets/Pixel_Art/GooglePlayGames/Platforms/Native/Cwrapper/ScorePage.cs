/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

// <copyright file="ScorePage.cs" company="Google Inc.">
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


namespace GooglePlayGames.Native.Cwrapper
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    internal static class ScorePage
    {
        [DllImport(SymbolLocation.NativeSymbolLocation)]
        internal static extern void ScorePage_Dispose(
            HandleRef self);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        internal static extern /* from(LeaderboardTimeSpan_t) */ Types.LeaderboardTimeSpan ScorePage_TimeSpan(
            HandleRef self);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        internal static extern /* from(size_t) */ UIntPtr ScorePage_LeaderboardId(
            HandleRef self,
            [In, Out] /* from(char *) */byte[] out_arg,
         /* from(size_t) */UIntPtr out_size);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        internal static extern /* from(LeaderboardCollection_t) */ Types.LeaderboardCollection ScorePage_Collection(
            HandleRef self);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        internal static extern /* from(LeaderboardStart_t) */ Types.LeaderboardStart ScorePage_Start(
            HandleRef self);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern /* from(bool) */ bool ScorePage_Valid(
            HandleRef self);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern /* from(bool) */ bool ScorePage_HasPreviousScorePage(
            HandleRef self);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern /* from(bool) */ bool ScorePage_HasNextScorePage(
            HandleRef self);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        internal static extern /* from(ScorePage_ScorePageToken_t) */ IntPtr ScorePage_PreviousScorePageToken(
            HandleRef self);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        internal static extern /* from(ScorePage_ScorePageToken_t) */ IntPtr ScorePage_NextScorePageToken(
            HandleRef self);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        internal static extern /* from(size_t) */ UIntPtr ScorePage_Entries_Length(
            HandleRef self);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        internal static extern /* from(ScorePage_Entry_t) */ IntPtr ScorePage_Entries_GetElement(
            HandleRef self,
         /* from(size_t) */UIntPtr index);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        internal static extern void ScorePage_Entry_Dispose(
            HandleRef self);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        internal static extern /* from(size_t) */ UIntPtr ScorePage_Entry_PlayerId(
            HandleRef self,
            [In, Out] /* from(char *) */byte[] out_arg,
         /* from(size_t) */UIntPtr out_size);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        internal static extern /* from(uint64_t) */ ulong ScorePage_Entry_LastModified(
            HandleRef self);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        internal static extern /* from(Score_t) */ IntPtr ScorePage_Entry_Score(
            HandleRef self);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern /* from(bool) */ bool ScorePage_Entry_Valid(
            HandleRef self);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        internal static extern /* from(uint64_t) */ ulong ScorePage_Entry_LastModifiedTime(
            HandleRef self);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern /* from(bool) */ bool ScorePage_ScorePageToken_Valid(
            HandleRef self);

        [DllImport(SymbolLocation.NativeSymbolLocation)]
        internal static extern void ScorePage_ScorePageToken_Dispose(
            HandleRef self);
    }
}
#endif // (UNITY_ANDROID || UNITY_IPHONE)

/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

// <copyright file="AndroidPlatformConfiguration.cs" company="Google Inc.">
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

#if UNITY_ANDROID

namespace GooglePlayGames.Native.PInvoke {
    using System;
    using System.Runtime.InteropServices;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using C = GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration;

sealed class AndroidPlatformConfiguration : PlatformConfiguration {

    private delegate void IntentHandlerInternal(IntPtr intent, IntPtr userData);

    private AndroidPlatformConfiguration (IntPtr selfPointer) : base(selfPointer) {
    }

    internal void SetActivity(System.IntPtr activity) {
        C.AndroidPlatformConfiguration_SetActivity(SelfPtr(), activity);
    }

    internal void SetOptionalIntentHandlerForUI(Action<IntPtr> intentHandler)
    {
        Misc.CheckNotNull(intentHandler);
        C.AndroidPlatformConfiguration_SetOptionalIntentHandlerForUI(SelfPtr(),
        InternalIntentHandler, Callbacks.ToIntPtr(intentHandler));
    }

    internal void SetOptionalViewForPopups(IntPtr view)
    {
        C.AndroidPlatformConfiguration_SetOptionalViewForPopups(SelfPtr(), view);
    }

    protected override void CallDispose(HandleRef selfPointer) {
        C.AndroidPlatformConfiguration_Dispose(selfPointer);
    }

    [AOT.MonoPInvokeCallback(typeof(IntentHandlerInternal))]
    private static void InternalIntentHandler(IntPtr intent, IntPtr userData) {
        Callbacks.PerformInternalCallback("AndroidPlatformConfiguration#InternalIntentHandler",
            Callbacks.Type.Permanent, intent, userData);
    }

    internal static AndroidPlatformConfiguration Create() {
        IntPtr p = C.AndroidPlatformConfiguration_Construct();
        return new AndroidPlatformConfiguration(p);
    }
}
}
#endif

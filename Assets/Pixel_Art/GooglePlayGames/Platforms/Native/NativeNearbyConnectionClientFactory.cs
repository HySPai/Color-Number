/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

// <copyright file="NativeNearbyConnectionClientFactory.cs" company="Google Inc.">
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

// Android only feature
#if (UNITY_ANDROID)

namespace GooglePlayGames.Native {

    using UnityEngine;
    using GooglePlayGames.Android;
    using GooglePlayGames.BasicApi.Nearby;
    using GooglePlayGames.Native.PInvoke;
    using GooglePlayGames.OurUtils;
    using System;
    using N = GooglePlayGames.Native.Cwrapper.NearbyConnectionsStatus;
    using GooglePlayGames.BasicApi;

    public class NativeNearbyConnectionClientFactory {

        private static volatile NearbyConnectionsManager sManager;
        private static Action<INearbyConnectionClient> sCreationCallback;

        internal static NearbyConnectionsManager GetManager() {
            return sManager;
        }

        public static void Create(Action<INearbyConnectionClient> callback) {
            if (sManager == null) {
                sCreationCallback = callback;
                InitializeFactory();
            }
            else {
                callback.Invoke(new NativeNearbyConnectionsClient(GetManager()));
            }
        }

        internal static void InitializeFactory() {
            // initialize the callback thread processing
            PlayGamesHelperObject.CreateObject();

            //Read the Service ID
            NearbyConnectionsManager.ReadServiceId();

            NearbyConnectionsManagerBuilder sBuilder = new NearbyConnectionsManagerBuilder();
            // The connection manager needs to be initialized before using it, so
            // wait for initialization.
            sBuilder.SetOnInitializationFinished(OnManagerInitialized);
            PlatformConfiguration cfg = new AndroidClient().CreatePlatformConfiguration(PlayGamesClientConfiguration.DefaultConfiguration);
            Debug.Log("Building manager Now");
            sManager = sBuilder.Build(cfg);
        }

        internal static void OnManagerInitialized(N.InitializationStatus status) {
            Debug.Log("Nearby Init Complete: " + status + " sManager = " + sManager);
            if (status == N.InitializationStatus.VALID) {
                if(sCreationCallback != null) {
                    sCreationCallback.Invoke(new NativeNearbyConnectionsClient(GetManager()));
                    sCreationCallback = null;
                }
            }
            else {
                Debug.LogError("ERROR: NearbyConnectionManager not initialized: " + status);
            }
        }
    }
}
#endif

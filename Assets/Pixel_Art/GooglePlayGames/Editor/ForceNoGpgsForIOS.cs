/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

// <copyright file="ForceNoGpgsForIOS.cs" company="Google Inc.">
// Copyright (C) 2018 Google Inc.
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

namespace GooglePlayGames.Editor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    #if UNITY_2017
    using UnityEditor.Build;
    #endif
    using UnityEditor;

    [InitializeOnLoad]
    public class ForceNoGpgsForIOS 
    #if UNITY_2017
        : IActiveBuildTargetChanged
    #endif
    {
        static ForceNoGpgsForIOS ()
        {
            setNoGPGS ();
        }

        public void OnActiveBuildTargetChanged (BuildTarget previousTarget, BuildTarget newTarget)
        {
            if (newTarget == BuildTarget.iOS) {
                setNoGPGS ();
            }
        }

        public int callbackOrder { get { return 0; } }

        private static void setNoGPGS ()
        {
            Debug.Log ("Forcing NO_GPGS to be defined for iOS builds.");
            string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup (BuildTargetGroup.iOS);
            if (string.IsNullOrEmpty (symbols)) {
                symbols = "NO_GPGS";
            } else if (!symbols.Contains ("NO_GPGS")) {
                symbols += ";NO_GPGS";
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.iOS, symbols);
        }
    }
}

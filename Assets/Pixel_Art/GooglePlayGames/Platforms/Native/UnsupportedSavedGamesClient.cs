/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

// <copyright file="UnsupportedSavedGamesClient.cs" company="Google Inc.">
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

namespace GooglePlayGames.Native
{
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.BasicApi.SavedGame;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections.Generic;

    internal class UnsupportedSavedGamesClient : ISavedGameClient
    {

        private readonly string mMessage;

        public UnsupportedSavedGamesClient(string message)
        {
            this.mMessage = Misc.CheckNotNull(message);
        }

        public void OpenWithAutomaticConflictResolution(string filename, DataSource source,
                                                    ConflictResolutionStrategy resolutionStrategy,
                                                    Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
        {
            throw new NotImplementedException(mMessage);
        }

        public void OpenWithManualConflictResolution(string filename, DataSource source,
                                                 bool prefetchDataOnConflict, ConflictCallback conflictCallback,
                                                 Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback)
        {
            throw new NotImplementedException(mMessage);
        }

        public void ReadBinaryData(ISavedGameMetadata metadata,
                               Action<SavedGameRequestStatus, byte[]> completedCallback)
        {
            throw new NotImplementedException(mMessage);
        }

        public void ShowSelectSavedGameUI(string uiTitle, uint maxDisplayedSavedGames,
                                      bool showCreateSaveUI, bool showDeleteSaveUI, Action<SelectUIStatus, ISavedGameMetadata> callback)
        {
            throw new NotImplementedException(mMessage);
        }

        public void CommitUpdate(ISavedGameMetadata metadata, SavedGameMetadataUpdate updateForMetadata,
                             byte[] updatedBinaryData, Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
        {
            throw new NotImplementedException(mMessage);
        }

        public void FetchAllSavedGames(DataSource source, Action<SavedGameRequestStatus,
        List<ISavedGameMetadata>> callback)
        {
            throw new NotImplementedException(mMessage);
        }

        public void Delete(ISavedGameMetadata metadata)
        {
            throw new NotImplementedException(mMessage);
        }
    }
}
#endif

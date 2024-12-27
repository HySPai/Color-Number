/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

// <copyright file="IEvent.cs" company="Google Inc.">
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

namespace GooglePlayGames.BasicApi.Events
{

    public enum EventVisibility
    {
        Hidden = 1,
        Revealed = 2,
    }

    /// <summary>
    /// Data object representing an Event. <see cref="EventManager"/> for more.
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// The ID of the event.
        /// </summary>
        string Id
        {
            get;
        }

        /// <summary>
        /// The name of the event.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// The description of the event.
        /// </summary>
        string Description
        {
            get;
        }

        /// <summary>
        /// The URL of the image for the event. Empty if there is no image for this event.
        /// </summary>
        /// <value>The image URL.</value>
        string ImageUrl
        {
            get;
        }

        /// <summary>
        /// The current count for this event.
        /// </summary>
        ulong CurrentCount
        {
            get;
        }

        /// <summary>
        /// The visibility of the event.
        /// </summary>
        EventVisibility Visibility
        {
            get;
        }
    }
}

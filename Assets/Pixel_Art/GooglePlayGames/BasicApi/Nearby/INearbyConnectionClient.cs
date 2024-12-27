/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

// <copyright file="INearbyConnectionClient.cs" company="Google Inc.">
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

namespace GooglePlayGames.BasicApi.Nearby
{
    using System;
    using System.Collections.Generic;

    // move this inside IMessageListener and IDiscoveryListener are always declared.
    #if (UNITY_ANDROID || (UNITY_IPHONE && !NO_GPGS))

    public interface INearbyConnectionClient
    {

        int MaxUnreliableMessagePayloadLength();

        int MaxReliableMessagePayloadLength();

        void SendReliable(List<string> recipientEndpointIds, byte[] payload);

        void SendUnreliable(List<string> recipientEndpointIds, byte[] payload);

        void StartAdvertising(string name, List<string> appIdentifiers,
                              TimeSpan? advertisingDuration, Action<AdvertisingResult> resultCallback,
                              Action<ConnectionRequest> connectionRequestCallback);

        void StopAdvertising();

        void SendConnectionRequest(string name, string remoteEndpointId, byte[] payload,
                                   Action<ConnectionResponse> responseCallback, IMessageListener listener);

        void AcceptConnectionRequest(string remoteEndpointId, byte[] payload,
                                     IMessageListener listener);

        void StartDiscovery(string serviceId, TimeSpan? advertisingTimeout,
                            IDiscoveryListener listener);

        void StopDiscovery(string serviceId);

        void RejectConnectionRequest(string requestingEndpointId);

        void DisconnectFromEndpoint(string remoteEndpointId);

        void StopAllConnections();

        string GetAppBundleId();

        string GetServiceId();
    }
#endif

    public interface IMessageListener
    {
        void OnMessageReceived(string remoteEndpointId, byte[] data,
                       bool isReliableMessage);

        void OnRemoteEndpointDisconnected(string remoteEndpointId);
    }

    public interface IDiscoveryListener
    {
        void OnEndpointFound(EndpointDetails discoveredEndpoint);

        void OnEndpointLost(string lostEndpointId);
    }
}


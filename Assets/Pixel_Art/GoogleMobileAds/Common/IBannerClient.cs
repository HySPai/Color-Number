/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

// Copyright (C) 2015 Google, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;

using GoogleMobileAds.Api;

namespace GoogleMobileAds.Common {
    public interface IBannerClient
    {
        // Ad event fired when the banner ad has been received.
        event EventHandler<EventArgs> OnAdLoaded;
        // Ad event fired when the banner ad has failed to load.
        event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;
        // Ad event fired when the banner ad is opened.
        event EventHandler<EventArgs> OnAdOpening;
        // Ad event fired when the banner ad is closed.
        event EventHandler<EventArgs> OnAdClosed;
        // Ad event fired when the banner ad is leaving the application.
        event EventHandler<EventArgs> OnAdLeavingApplication;

        // Creates a banner view and adds it to the view hierarchy.
        void CreateBannerView(string adUnitId, AdSize adSize, AdPosition position);

        // Creates a banner view and adds it to the view hierarchy with a custom position.
        void CreateBannerView(string adUnitId, AdSize adSize, int x, int y);

        // Requests a new ad for the banner view.
        void LoadAd(AdRequest request);

        // Shows the banner view on the screen.
        void ShowBannerView();

        // Hides the banner view from the screen.
        void HideBannerView();

        // Destroys a banner view.
        void DestroyBannerView();

        // Returns the height of the BannerView in pixels.
        float GetHeightInPixels();

        // Returns the width of the BannerView in pixels.
        float GetWidthInPixels();

      // Set the position of the banner view using standard position.
        void SetPosition(AdPosition adPosition);

        // Set the position of the banner view using custom position.
        void SetPosition(int x, int y);

        // Returns the mediation adapter class name.
        string MediationAdapterClassName();
    }
}
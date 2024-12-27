/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

// <copyright file="BaseReferenceHolder.cs" company="Google Inc.">
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

namespace GooglePlayGames.Native.PInvoke
{
    using System;
    using System.Runtime.InteropServices;
    using System.Collections.Generic;
    using GooglePlayGames.OurUtils;

    internal abstract class BaseReferenceHolder : IDisposable
    {

        private static Dictionary<HandleRef, BaseReferenceHolder> _refs =
            new Dictionary<HandleRef, BaseReferenceHolder>();

        private HandleRef mSelfPointer;

        protected bool IsDisposed()
        {
            return PInvokeUtilities.IsNull(mSelfPointer);
        }

        protected HandleRef SelfPtr()
        {
            if (IsDisposed())
            {
                throw new InvalidOperationException(
                    "Attempted to use object after it was cleaned up");
            }

            return mSelfPointer;
        }

        public BaseReferenceHolder(IntPtr pointer)
        {
            mSelfPointer = PInvokeUtilities.CheckNonNull(new HandleRef(this, pointer));
        }

        protected abstract void CallDispose(HandleRef selfPointer);

        ~BaseReferenceHolder ()
        {
            Dispose(true);
        }

        public void Dispose()
        {
            Dispose(false);
            System.GC.SuppressFinalize(this);
        }

        internal IntPtr AsPointer()
        {
            return SelfPtr().Handle;
        }

        private void Dispose(bool fromFinalizer)
        {
            if (fromFinalizer || !_refs.ContainsKey(mSelfPointer))
            {
                if (!PInvokeUtilities.IsNull(mSelfPointer))
                {
                    CallDispose(mSelfPointer);
                    mSelfPointer = new HandleRef(this, IntPtr.Zero);
                }
            }
        }

        internal void ReferToMe()
        {
            _refs[SelfPtr()] =  this;
        }

        internal void ForgetMe()
        {
            if (_refs.ContainsKey(SelfPtr()))
            {
                _refs.Remove(SelfPtr());
                Dispose(false);
            }
        }

    }
}
#endif

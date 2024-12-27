/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

// <copyright file="Misc.cs" company="Google Inc.">
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

namespace GooglePlayGames.OurUtils
{
    using System;

    public static class Misc
    {
        public static bool BuffersAreIdentical(byte[] a, byte[] b)
        {
            if (a == b)
            {
                // not only identical but the very same!
                return true;
            }

            if (a == null || b == null)
            {
                // one of them is null, the other one isn't
                return false;
            }

            if (a.Length != b.Length)
            {
                return false;
            }

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static byte[] GetSubsetBytes(byte[] array, int offset, int length)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (offset < 0 || offset >= array.Length)
            {
                throw new ArgumentOutOfRangeException("offset");
            }

            if (length < 0 || (array.Length - offset) < length)
            {
                throw new ArgumentOutOfRangeException("length");
            }

            if (offset == 0 && length == array.Length)
            {
                return array;
            }

            byte[] piece = new byte[length];
            Array.Copy(array, offset, piece, 0, length);
            return piece;
        }

        public static T CheckNotNull<T>(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }
        
            return value;
        }

        public static T CheckNotNull<T>(T value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }

            return value;
        }
    }
}

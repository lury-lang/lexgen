﻿//
// ExitCode.cs
//
// Author:
//       Tomona Nanase <nanase@users.noreply.github.com>
//
// The MIT License (MIT)
//
// Copyright (c) 2015 Tomona Nanase
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;

namespace Lury.Lexgen
{
    /// <summary>
    /// 終了コードを格納したクラスです。
    /// </summary>
    public static class ExitCode
    {
        /// <summary>
        /// プログラムはエラー無しに終了しました。
        /// </summary>
        public const int Success = 0;

        /// <summary>
        /// パラメータが足りません。
        /// </summary>
        public const int ParameterNotEnough = 1;

        /// <summary>
        /// パラメータが多すぎます。
        /// </summary>
        public const int TooManyParameter = 2;

        /// <summary>
        /// ファイルを作れません。
        /// </summary>
        public const int FileCannotCreated = 3;

        /// <summary>
        /// ファイルを開けません。
        /// </summary>
        public const int FileCannotOpened = 4;

        /// <summary>
        /// スケルトン記述が不正です。
        /// </summary>
        public const int InvalidSkeleton = 5;

        /// <summary>
        /// 不正な JSON ファイルです。
        /// </summary>
        public const int InvalidJson = 6;
    }
}


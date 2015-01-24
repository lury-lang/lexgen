//
// Program.cs
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

using System.Runtime.Serialization;

namespace Lury.Lexgen
{
    /// <summary>
    /// 字句解析ルートを表します。
    /// </summary>
    [DataContract]
    public class LexRoot
    {
        #region -- Public Properties --

        /// <summary>
        /// 字句解析コードを展開する名前空間を取得または設定します。
        /// </summary>
        /// <value>名前空間を表す文字列。</value>
        [DataMember(Name = "namespace")]
        public string Namespace { get; set; }

        /// <summary>
        /// 字句解析コードを展開するクラス名を取得または設定します。
        /// </summary>
        /// <value>クラス名を表す文字列。</value>
        [DataMember(Name = "class")]
        public string ClassName { get; set; }



        /// <summary>
        /// 字句解析カテゴリの配列を取得または設定します。
        /// </summary>
        /// <value>字句解析カテゴリを格納する LexCategory クラスの配列。</value>
        [DataMember(Name = "category")]
        public LexCategory[] Category { get; set; }

        #endregion
    }

    /// <summary>
    /// 字句解析エントリをカテゴリとしてグループ化します。
    /// </summary>
    [DataContract]
    public class LexCategory
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "entry")]
        public LexEntry[] Entry { get; set; }
    }

    [DataContract]
    public class LexEntry
    {
        [DataMember(Name = "regex")]
        public string Regex { get; set; }

        [DataMember(Name = "token")]
        public string Token { get; set; }

        [DataMember(Name = "option")]
        public LexOptions[] Options { get; set; }

        [DataMember(Name = "switchOn")]
        public string[] ContextSwitchOn { get; set; }

        [DataMember(Name = "switchOff")]
        public string[] ContextSwitchOff { get; set; }

        [DataMember(Name = "context")]
        public string[] Context { get; set; }
    }

    [DataContract]
    public class LexOptions
    {
        [DataMember(Name = "ignoreAfterSpace")]
        public bool IgnoreAfterSpace { get; set; }

        [DataMember(Name = "singleline")]
        public bool Singleline { get; set; }
    }
}


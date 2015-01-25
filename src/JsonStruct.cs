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
        /// 使用する追加の名前空間を格納した配列を取得または設定します。
        /// </summary>
        /// <value>名前空間を表す文字列の配列。</value>
        [DataMember(Name = "using")]
        public string[] UsingNamespace { get; set; }

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
        #region -- Public Properties --

        /// <summary>
        /// カテゴリ名を取得または設定します。
        /// </summary>
        /// <value>カテゴリ名を表す文字列。</value>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 字句解析エントリの配列を取得または設定します。
        /// </summary>
        /// <value>字句解析エントリを格納する LexEntry クラスの配列。</value>
        [DataMember(Name = "entry")]
        public LexEntry[] Entry { get; set; }

        #endregion
    }

    /// <summary>
    /// 字句解析エントリを表します。
    /// </summary>
    [DataContract]
    public class LexEntry
    {
        #region -- Public Properties --

        /// <summary>
        /// トークンの抽出に用いる正規表現パターンを表す文字列を取得または設定します。
        /// </summary>
        /// <value>正規表現パターンを表す文字列。</value>
        [DataMember(Name = "regex")]
        public string Regex { get; set; }

        /// <summary>
        /// このエントリが示すトークンオブジェクトのコードを取得または設定します。
        /// </summary>
        /// <value>トークンオブジェクトのコード。</value>
        [DataMember(Name = "token")]
        public string Token { get; set; }

        /// <summary>
        /// 解析に用いられるオプションオブジェクトの配列を取得または設定します。
        /// </summary>
        /// <value>オプションオブジェクトの配列。</value>
        [DataMember(Name = "option")]
        public LexOptions[] Options { get; set; }

        /// <summary>
        /// 有効化するコンテキストスイッチ名を表す文字列の配列を取得または設定します。
        /// </summary>
        /// <value>有効化するコンテキストスイッチ名を表す文字列の配列。</value>
        [DataMember(Name = "switchOn")]
        public string[] ContextSwitchOn { get; set; }

        /// <summary>
        /// 無効化するコンテキストスイッチ名を表す文字列の配列を取得または設定します。
        /// </summary>
        /// <value>無効化するコンテキストスイッチ名を表す文字列の配列。</value>
        [DataMember(Name = "switchOff")]
        public string[] ContextSwitchOff { get; set; }

        /// <summary>
        /// トークンを抽出する条件としてのコンテキストスイッチ名を表す文字列の配列を
        /// 取得または設定します。
        /// </summary>
        /// <value>抽出条件としてのコンテキストスイッチ名を表す文字列の配列。</value>
        [DataMember(Name = "context")]
        public string[] Context { get; set; }

        #endregion
    }

    /// <summary>
    /// 字句解析時のオプションです。
    /// </summary>
    [DataContract]
    public class LexOptions
    {
        #region -- Public Properties --

        /// <summary>
        /// トークンに続く空白文字を無視するかの真偽値を取得または設定します。
        /// </summary>
        /// <value>無視する場合は <c>true</c>、それ以外のとき <c>false</c>。</value>
        [DataMember(Name = "ignoreAfterSpace")]
        public bool IgnoreAfterSpace { get; set; }

        /// <summary>
        /// 正規表現のオプションを単一行モードにするかの真偽値を取得または設定します。
        /// </summary>
        /// <value>単一行モードのとき <c>true</c>、それ以外のとき <c>false</c>。</value>
        [DataMember(Name = "singleline")]
        public bool Singleline { get; set; }

        #endregion
    }
}


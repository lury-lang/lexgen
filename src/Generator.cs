//
// Generator.cs
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
using System.Text;
using System.IO;

namespace Lury.Lexgen
{
    /// <summary>
    /// 字句リストを自動生成するための機能を提供します。
    /// </summary>
    class Generator
    {
        #region -- Public Properties --

        /// <summary>
        /// プログラムのオプションを取得します。
        /// </summary>
        /// <value>プログラムのオプションを表す <see cref="Lury.Lexgen.ProgramOptions"/> オブジェクト。</value>
        public ProgramOptions Options { get; private set; }

        /// <summary>
        /// 字句構造ルートを取得します。
        /// </summary>
        /// <value>字句構造ルートを表す <see cref="Lury.Lexgen.LexRoot"/> オブジェクト。</value>
        public LexRoot LexRoot { get; private set; }

        #endregion

        #region -- Constructors --

        /// <summary>
        /// 新しい <see cref="Lury.Lexgen.Generator"/> クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="options">プログラムのオプションを表す <see cref="Lury.Lexgen.ProgramOptions"/> オブジェクト。</param>
        /// <param name="lexRoot">字句構造ルートを表す <see cref="Lury.Lexgen.LexRoot"/> オブジェクト。</param>
        public Generator(ProgramOptions options, LexRoot lexRoot)
        {
            this.Options = options;
            this.LexRoot = lexRoot;

            this.CheckLexRoot();
        }

        #endregion

        #region -- Public Methods --

        /// <summary>
        /// 書き込みストリームを指定して字句リストを生成します。
        /// </summary>
        /// <param name="stream">書き込み可能な <see cref="System.IO.Stream"/> オブジェクト。</param>
        public void Generate(Stream stream)
        {
            string categoryString = this.CreateCategoryString();
            string classString = ReadSkeletonFile(this.Options.ClassSkeletonPath);

            using (StreamWriter sw = new StreamWriter(stream, Encoding.UTF8))
            {
                sw.Write(string.Format(classString,
                                       this.LexRoot.Namespace,
                                       this.LexRoot.ClassName,
                                       categoryString,
                                       string.Join("\n", this.LexRoot.UsingNamespace)));
            }
        }

        #endregion

        #region -- Private Methods --

        /// <summary>
        /// カテゴリスケルトンを使ってコードを自動生成します。
        /// </summary>
        /// <returns>自動生成コードを表す文字列。</returns>
        private string CreateCategoryString()
        {
            string entryString = ReadSkeletonFile(this.Options.EntrySkeletonPath);
            string categoryString = ReadSkeletonFile(this.Options.CategorySkeletonPath);
            StringBuilder entriesString = new StringBuilder();
            StringBuilder categoriesString = new StringBuilder();

            // categoriesString.Clear();

            foreach (var category in this.LexRoot.Category)
            {
                entriesString.Clear();

                foreach (var entry in category.Entry)
                {
                    entriesString.AppendFormat(entryString,
                                               entry.Name,
                                               entry.Regex,
                                               CreateRegexOptionsString(entry.Option),
                                               entry.Token,
                                               CreateIgnoreAfterSpaceString(entry.Option),
                                               entry.ContextSwitchOn.ToArrayString(),
                                               entry.ContextSwitchOff.ToArrayString(),
                                               entry.Context.ToArrayString());
                }

                categoriesString.AppendFormat(categoryString,
                                              category.Name,
                                              entriesString.ToString());
            }

            return categoriesString.ToString();
        }

        /// <summary>
        /// 字句ルートオブジェクトが有効なプロパティを持っているかを検査します。
        /// </summary>
        private void CheckLexRoot()
        {
            if (this.LexRoot.Category == null)
            {
                Console.WriteLine("カテゴリを null または未指定にすることはできません.");
                Environment.Exit(ExitCode.InvalidJson);
            }

            if (string.IsNullOrWhiteSpace(this.LexRoot.Namespace))
            {
                Console.WriteLine("名前空間名を null、空の文字列または未指定にすることはできません.");
                Environment.Exit(ExitCode.InvalidJson);
            }

            if (string.IsNullOrWhiteSpace(this.LexRoot.ClassName))
            {
                Console.WriteLine("クラス名を null、空の文字列または未指定にすることはできません.");
                Environment.Exit(ExitCode.InvalidJson);
            }
        }

        #endregion

        #region -- Private Static Methods --

        /// <summary>
        /// IgnoreAfterSpace オプションに対する真偽値を表す文字列を生成します。
        /// </summary>
        /// <returns>正規表現オプションを表す文字列。</returns>
        /// <param name="option">IgnoreAfterSpace オプションに対する真偽値を表す文字列。</param>
        private static string CreateIgnoreAfterSpaceString(LexOptions option)
        {
            if (option == null)
                return "false";
            else
                return option.IgnoreAfterSpace.ToString().ToLower();
        }

        /// <summary>
        /// 指定されたスケルトンファイルを読み込み、文字列として取得します。
        /// </summary>
        /// <returns>読み込まれたファイルの内容。</returns>
        /// <param name="path">読み込まれるスケルトンファイルのパス。</param>
        private static string ReadSkeletonFile(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine("スケルトンファイルを読み取れません. {0}", ex.Message);
                Environment.Exit(ExitCode.FileCannotOpened);
            }

            // cannot reach
            return null;
        }

        /// <summary>
        /// 字句オプションに対する正規表現オプションを表す文字列を生成します。
        /// </summary>
        /// <returns>正規表現オプションを表す文字列。</returns>
        /// <param name="option">字句オプションを表す <see cref="Lury.Lexgen.LexOptions"/> オブジェクト。</param>
        private static string CreateRegexOptionsString(LexOptions option)
        {
            if (option == null)
                return "RegexOptions.None";
            else
                return option.Singleline ? "RegexOptions.Singleline" : "RegexOptions.None";
        }

        #endregion
    }
}


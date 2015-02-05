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

using System;
using System.IO;
using System.Linq;

namespace Lury.Lexgen
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                ShowUsage();
                Environment.Exit(ExitCode.ParameterNotEnough);
            }
        }

        #region -- Private Static Methods --

        /// <summary>
        /// 文字列の配列を結合した文字列を生成します。
        /// </summary>
        /// <returns>文字列の配列が結合された文字列。</returns>
        /// <param name="array">結合される文字列の配列。</param>
        private static string CreateStringArrayString(string[] array)
        {
            return string.Join(", ", array.Select(s => "\"" + s + "\""));
        }

        /// <summary>
        /// 字句オプションに対する正規表現オプションを表す文字列を生成します。
        /// </summary>
        /// <returns>正規表現オプションを表す文字列。</returns>
        /// <param name="option">字句オプションを表す <see cref="Lury.Lexgen.LexOptions"/> オブジェクト。</param>
        private static string CreateRegexOptionsString(LexOptions option)
        {
            return option.Singleline ? "RegexOptions.Singleline" : "RegexOptions.None";
        }

        /// <summary>
        /// コマンドラインオプションのファイルを検査します。
        /// </summary>
        /// <param name="options">コマンドラインオプションが格納された <see cref="Lury.Lexgen.ProgramOptions"/> オブジェクト。</param>
        private static void CheckOptions(ProgramOptions options)
        {
            if (!File.Exists(options.InputJsonPath))
            {
                Console.WriteLine("入力ファイル {0} が存在しません.", options.InputJsonPath);
                Environment.Exit(ExitCode.FileCannotOpened);
            }

            if (!File.Exists(options.ClassSkeletonPath))
            {
                Console.WriteLine("クラススケルトン ファイル {0} が存在しません.", options.ClassSkeletonPath);
                Environment.Exit(ExitCode.FileCannotOpened);
            }

            if (!File.Exists(options.CategorySkeletonPath))
            {
                Console.WriteLine("カテゴリスケルトン ファイル {0} が存在しません.", options.CategorySkeletonPath);
                Environment.Exit(ExitCode.FileCannotOpened);
            }

            if (!File.Exists(options.EntrySkeletonPath))
            {
                Console.WriteLine("エントリスケルトン ファイル {0} が存在しません.", options.EntrySkeletonPath);
                Environment.Exit(ExitCode.FileCannotOpened);
            }
        }

        /// <summary>
        /// このプログラムの使用法を表示します。
        /// </summary>
        private static void ShowUsage()
        {
            var executeFilePath = GetExecuteFilePath();
            Console.WriteLine("{0}: {1} [OPTION]... INPUT OUTPUT", "使用法", executeFilePath);
        }

        /// <summary>
        /// このプログラムの実行ファイル名を取得します。
        /// </summary>
        /// <returns>ファイル名を表す文字列。</returns>
        private static string GetExecuteFilePath()
        {
            return System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName;
        }

        /// <summary>
        /// プログラムのヘルプを表示します。
        /// </summary>
        private static void ShowHelp()
        {
            ShowUsage();
            Console.WriteLine();
            ProgramOptions.ShowHelp();
            Console.WriteLine();
            Console.WriteLine("プログラムのリポジトリ: <https://github.com/lury-lang/lexgen>");
        }

        #endregion
    }
}

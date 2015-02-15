﻿//
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
using System.Runtime.Serialization.Json;
using System.Text;
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

            ProgramOptions options = new ProgramOptions(args);

            CheckOptions(options);
            LexRoot root = null;

            try
            {
                using (FileStream fs = new FileStream(options.InputJsonPath, FileMode.Open))
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(LexRoot));
                    root = (LexRoot)ser.ReadObject(fs);
                }
            }
            catch (System.Runtime.Serialization.SerializationException ex)
            {
                Console.WriteLine("入力ファイル {0} を読み取れません. JSONとして読み込めません.", options.InputJsonPath);
                Console.WriteLine(ex.InnerException.Message);
                Environment.Exit(ExitCode.InvalidJson);
            }
            catch (IOException ex)
            {
                Console.WriteLine("入力ファイル {0} を読み取れません.", ex.Message);
                Console.WriteLine(ex.Message);
                Environment.Exit(ExitCode.FileCannotOpened);
            }

            CheckLexRoot(root);
            string categoryString = CreateCategoryString(options, root);
            string classString = ReadSkeletonFile(options.ClassSkeletonPath);

            using (FileStream fs = new FileStream(options.OutputCsPath, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
            {
                sw.Write(string.Format(classString,
                                       root.Namespace,
                                       root.ClassName,
                                       categoryString,
                                       string.Join("\n", root.UsingNamespace)));
            }
        }

        #region -- Private Static Methods --

        /// <summary>
        /// カテゴリスケルトンを使ってコードを自動生成します。
        /// </summary>
        /// <returns>自動生成コードを表す文字列。</returns>
        /// <param name="options">プログラムのオプションを表す <see cref="Lury.Lexgen.ProgramOptions"/> オブジェクト。</param>
        /// <param name="lexRoot"><see cref="Lury.Lexgen.LexRoot"/> オブジェクト。</param>
        private static string CreateCategoryString(ProgramOptions options, LexRoot lexRoot)
        {
            string entryString = ReadSkeletonFile(options.EntrySkeletonPath);
            string categoryString = ReadSkeletonFile(options.CategorySkeletonPath);
            StringBuilder entriesString = new StringBuilder();
            StringBuilder categoriesString = new StringBuilder();

            // categoriesString.Clear();

            foreach (var category in lexRoot.Category)
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

        /// <summary>
        /// 字句ルートオブジェクトが有効なプロパティを持っているかを検査します。
        /// </summary>
        /// <param name="root">字句ルートを表す <see cref="Lury.Lexgen.LexRoot"/> オブジェクト。</param>
        private static void CheckLexRoot(LexRoot root)
        {
            if (root.Category == null)
            {
                Console.WriteLine("カテゴリを null または未指定にすることはできません.");
                Environment.Exit(ExitCode.InvalidJson);
            }

            if (string.IsNullOrWhiteSpace(root.Namespace))
            {
                Console.WriteLine("名前空間名を null、空の文字列または未指定にすることはできません.");
                Environment.Exit(ExitCode.InvalidJson);
            }

            if (string.IsNullOrWhiteSpace(root.ClassName))
            {
                Console.WriteLine("クラス名を null、空の文字列または未指定にすることはできません.");
                Environment.Exit(ExitCode.InvalidJson);
            }
        }

        /// <summary>
        /// コマンドラインオプションのファイルを検査します。
        /// </summary>
        /// <param name="options">コマンドラインオプションが格納された <see cref="Lury.Lexgen.ProgramOptions"/> オブジェクト。</param>
        private static void CheckOptions(ProgramOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.InputJsonPath) ||
                string.IsNullOrWhiteSpace(options.OutputCsPath))
            {
                ShowUsage();
                Environment.Exit(ExitCode.ParameterNotEnough);
            }

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

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
using System.Runtime.Serialization.Json;
using System.Text;
using System.Linq;

namespace Lury.Lexgen
{
    class MainClass
    {
        #region -- Public Static Methods --

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

            #region 入力ファイル読み取り
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
                Console.WriteLine("入力ファイル {0} を読み取れません.", options.InputJsonPath);
                Console.WriteLine(ex.Message);
                Environment.Exit(ExitCode.FileCannotOpened);
            }
            #endregion

            #region 出力ファイル書き込み
            try
            {
                using (FileStream fs = new FileStream(options.OutputCsPath, FileMode.Create))
                {
                    var generator = new Generator(options, root);
                    generator.Generate(fs);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("出力ファイル {0} に書き込めません.", options.OutputCsPath);
                Console.WriteLine(ex.Message);
                Environment.Exit(ExitCode.FileCannotCreated);
            }
            #endregion
        }

        #endregion

        #region -- Private Static Methods --

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

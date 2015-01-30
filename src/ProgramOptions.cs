//
// ProgramOptions.cs
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
using System.Collections.Generic;
using System.Linq;

namespace Lury.Lexgen.SampleRunner
{
    class ProgramOptions
    {
        #region -- Public Properties --

        /// <summary>
        /// 入力するJsonファイルのパスを取得します。
        /// </summary>
        /// <value>入力するJsonファイルのパス。</value>
        public string InputJsonPath { get; private set; }

        /// <summary>
        /// 出力するCSファイルのパスを取得します。
        /// </summary>
        /// <value>出力するCSファイルのパス。</value>
        public string OutputCsPath { get; private set; }

        /// <summary>
        /// クラススケルトンのパスを取得します。
        /// </summary>
        /// <value>クラススケルトンファイルのパス。</value>
        public string ClassSkeletonPath { get; private set; }

        /// <summary>
        /// カテゴリスケルトンのパスを取得します。
        /// </summary>
        /// <value>カテゴリスケルトンファイルのパス。</value>
        public string CategorySkeletonPath { get; private set; }

        /// <summary>
        /// エントリスケルトンのパスを取得します。
        /// </summary>
        /// <value>エントリスケルトンファイルのパス。</value>
        public string EntrySkeletonPath { get; private set; }

        /// <summary>
        /// コンパイル出力以外のメッセージを表示しないかどうかの真偽値を取得します。
        /// </summary>
        /// <value>出力しないとき true、それ以外のとき false。</value>
        public bool SilentMode { get; private set; }

        /// <summary>
        /// エラー出力を表示しないかどうかの真偽値を取得します。
        /// </summary>
        /// <value>エラーを表示しないとき true、それ以外のとき false。</value>
        public bool SuppressError { get; private set; }

        /// <summary>
        /// 出力をカラーにするかの真偽値を取得します。
        /// </summary>
        /// <value>カラー出力するとき true、それ以外のとき false。</value>
        public bool EnableColor { get; private set; }

        /// <summary>
        /// ヘルプを表示するかの真偽値を取得します。
        /// </summary>
        /// <value>ヘルプ表示モードのとき true、それ以外のとき false。</value>
        public bool ShowHelpMode { get; private set; }

        #endregion

        #region -- Constructors --

        /// <summary>
        /// コマンドライン引数を指定して
        /// 新しい <see cref="Lury.SampleRunner.ProgramOptions"/> クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="args">コマンドライン引数。</param>
        public ProgramOptions(string[] args)
        {
            this.SetDefaultValue();
            this.ParseCommandLine(args);
        }

        #endregion

        #region -- Private Methods --

        /// <summary>
        /// 各プロパティにデフォルト値を設定します。
        /// </summary>
        private void SetDefaultValue()
        {
            this.InputJsonPath = null;
            this.OutputCsPath = null;

            this.EnableColor = true;

            // TODO: スケルトンファイルパスのデフォルト値
            // this.ClassSkeletonPath = ...
            // this.CategorySkeletonPath = ...
            // this.EntrySkeletonPath = ...
        }

        /// <summary>
        /// コマンドライン引数を解析して各プロパティに反映します。
        /// </summary>
        /// <param name="args">コマンドライン引数を格納した文字列の配列。</param>
        private void ParseCommandLine(string[] args)
        {
            bool commandSwitch = false;

            foreach (var arg in args)
            {
                if (commandSwitch)
                {
                    this.CheckFilePath(arg);
                    continue;
                }

                string command, parameter;
                SeparateArg(arg, out command, out parameter);

                switch (command)
                {
                    case "--skeleton-class":
                        this.ClassSkeletonPath = arg;
                        break;
                    
                    case "--skeleton-category":
                        this.CategorySkeletonPath = arg;
                        break;

                    case "--skeleton-entry":
                        this.EntrySkeletonPath = arg;
                        break;

                    case "-h":
                    case "--help":
                        this.ShowHelpMode = true;
                        return;

                    case "--":
                        commandSwitch = true;
                        break;

                    case "-s":
                    case "--silent":
                        this.SilentMode = true;
                        break;

                    case "-e":
                    case "--suppress-error":
                        this.SuppressError = true;
                        break;

                    case "--color":
                        this.EnableColor = true;
                        break;

                    case "--disable-color":
                        this.EnableColor = false;
                        break;

                    default:
                        this.CheckFilePath(arg);
                        break;
                }
            }
        }

        private void CheckFilePath(string arg)
        {
            if (this.InputJsonPath == null)
                this.InputJsonPath = arg;
            else if (this.OutputCsPath == null)
                this.OutputCsPath = arg;
            else
            {
                Console.WriteLine("出力CSファイルが多すぎます.");
                Environment.Exit(ExitCode.TooManyParameter);
            }
        }

        #endregion

        #region -- Public Static Methods --

        /// <summary>
        /// 引数に関するヘルプを表示します。
        /// </summary>
        public static void ShowHelp()
        {
            var args = new []
            {
                new {Command = "--", Text = "これ以降のオプションをすべてファイルパスとして認識します."},
                new {Command = "    --color", Text = "カラー表示を有効化します."},
                new {Command = "    --disable-color", Text = "カラー表示を無効化します."},
                new {Command = "    --skeleton-category", Text = "カテゴリスケルトンファイルのパス."},
                new {Command = "    --skeleton-class", Text = "クラススケルトンファイルのパス."},
                new {Command = "    --skeleton-entry", Text = "エントリスケルトンファイルのパス."},
                new {Command = "-e, --suppress-error", Text = "エラー表示を抑制します."},
                new {Command = "-s, --silent", Text = "コンパイル出力以外の表示を抑制します."},

                new {Command = "-h, --help", Text = "このヘルプを表示します."},
            };

            Console.WriteLine("Options:");
            var commandLength = Math.Min(25, args.Max(a => a.Command.Length));
            var format = string.Format("  {{0,{0}}} {{1}}", -commandLength);

            foreach (var arg in args)
                Console.WriteLine(format, arg.Command, arg.Text);
        }

        #endregion

        #region -- Private Static Methods --

        /// <summary>
        /// 文字列をコマンドとパラメータに分割します。
        /// </summary>
        /// <param name="arg">文字列。</param>
        /// <param name="command">コマンドを表す文字列。</param>
        /// <param name="parameter">パラメータを表す文字列。</param>
        private static void SeparateArg(string arg, out string command, out string parameter)
        {
            int commandIndex = -1;
            command = arg.StartsWith("-", StringComparison.Ordinal) ?
                        ((commandIndex = arg.IndexOf(":", StringComparison.Ordinal)) < 0 ?
                            arg : arg.Substring(0, commandIndex)) : string.Empty;
            parameter = commandIndex < 0 ? 　string.Empty : arg.Substring(commandIndex);
        }

        #endregion
    }
}


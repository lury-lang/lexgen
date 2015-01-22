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
using System.Runtime.Serialization;

namespace Lury.Lexgen
{
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


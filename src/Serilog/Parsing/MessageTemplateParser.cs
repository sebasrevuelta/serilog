﻿// Copyright 2013 Serilog Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Text;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Parsing
{
    /// <summary>
    /// Parses message template strings into sequences of text or property
    /// tokens.
    /// </summary>
    public class MessageTemplateParser : IMessageTemplateParser
    {
        /// <summary>
        /// Parse the supplied message template.
        /// </summary>
        /// <param name="messageTemplate">The message template to parse.</param>
        /// <returns>A sequence of text or property tokens. Where the template
        /// is not syntactically valid, text tokens will be returned. The parser
        /// will make a best effort to extract valid property tokens even in the
        /// presence of parsing issues.</returns>
        public MessageTemplate Parse(string messageTemplate)
        {
            if (messageTemplate == null) throw new ArgumentNullException("messageTemplate");
            return new MessageTemplate(messageTemplate, Tokenize(messageTemplate));
        }

        static IEnumerable<MessageTemplateToken> Tokenize(string messageTemplate)
        {
            if (messageTemplate == "")
            {
                yield return new TextToken("");
                yield break;
            }

            var nextIndex = 0;
            while (true)
            {
                var beforeText = nextIndex;
                var tt = ParseTextToken(nextIndex, messageTemplate, out nextIndex);
                if (nextIndex > beforeText)
                    yield return tt;

                if (nextIndex == messageTemplate.Length)
                    yield break;

                var beforeProp = nextIndex;
                var pt =  ParsePropertyToken(nextIndex, messageTemplate, out nextIndex);
                if (beforeProp < nextIndex)
                    yield return pt;

                if (nextIndex == messageTemplate.Length)
                    yield break;
            }
        }

        private static MessageTemplateToken ParsePropertyToken(int startAt, string messageTemplate, out int next)
        {
            var first = startAt;
            startAt++;
            while (startAt < messageTemplate.Length && IsValidInPropertyTag(messageTemplate[startAt]))
                startAt++;

            if (startAt == messageTemplate.Length || messageTemplate[startAt] != '}')
            {
                next = startAt;
                return new TextToken(messageTemplate.Substring(first, next - first));
            }
            
            next = startAt + 1;

            var rawText = messageTemplate.Substring(first, next - first);
            var tagContent = messageTemplate.Substring(first + 1, next - (first + 2));
            if (tagContent.Length == 0 ||
                !IsValidInPropertyTag(tagContent[0]))
                return new TextToken(rawText);

            string propertyNameAndDestructuring, format;
            var formatDelim = tagContent.IndexOf(':');
            if (formatDelim == -1)
            {
                propertyNameAndDestructuring = tagContent;
                format = null;
            }
            else
            {
                propertyNameAndDestructuring = tagContent.Substring(0, formatDelim);
                format = formatDelim == tagContent.Length - 1 ?
                    null :
                    tagContent.Substring(formatDelim + 1);
            }

            var propertyName = propertyNameAndDestructuring;
            bool optional;
            if (TryGetOptionalHint(propertyName[0], out optional))
            {
                propertyName = propertyName.Substring(1);
            }

            Destructuring destructuring;
            if (TryGetDestructuringHint(propertyName[0], out destructuring))
                propertyName = propertyName.Substring(1);

            if (propertyName == "" || !char.IsLetterOrDigit(propertyName[0]))
                return new TextToken(rawText);

            foreach (var c in propertyName)
                if (!IsValidInPropertyName(c))
                    return new TextToken(rawText);

            if (format != null)
            {
                foreach (var c in format)
                    if (!IsValidInFormat(c))
                        return new TextToken(rawText);
            }

            return new PropertyToken(
                propertyName,
                rawText,
                format,
                destructuring,
                optional);
        }

        private static bool IsValidInPropertyTag(char c)
        {
            return IsValidInDestructuringHint(c) ||
                IsValidInPropertyName(c) ||
                IsValidInFormat(c) ||
                c == ':';
                
        }

        private static bool IsValidInPropertyName(char c)
        {
            return char.IsLetterOrDigit(c);
        }

        private static bool TryGetOptionalHint(char c, out bool optional)
        {
            switch (c)
            {
                case '?':
                {
                    optional = true;
                    return true;
                }
                default:
                    optional = false;
                    return false;
            }
        }

        private static bool TryGetDestructuringHint(char c, out Destructuring destructuring)
        {
            switch (c)
            {
                case '@':
                {
                    destructuring = Destructuring.Destructure;
                    return true;
                }
                case '$':
                {
                    destructuring = Destructuring.Stringify;
                    return true;
                }
                default:
                {
                    destructuring = Destructuring.Default;
                    return false;
                }
            }
        }

        private static bool IsValidInDestructuringHint(char c)
        {
            return c == '@' ||
                   c == '$';
        }

        private static bool IsValidInFormat(char c)
        {
            return c != '}' &&
                (char.IsLetterOrDigit(c) ||
                 char.IsPunctuation(c) ||
                 c == ' ');
        }

        private static TextToken ParseTextToken(int startAt, string messageTemplate, out int next)
        {
            var accum = new StringBuilder();
            do
            {
                var nc = messageTemplate[startAt];
                if (nc == '{')
                {
                    if (startAt + 1 < messageTemplate.Length &&
                        messageTemplate[startAt + 1] == '{')
                    {
                        accum.Append(nc);
                        startAt++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    accum.Append(nc);
                    if (nc == '}')
                    {
                        if (startAt + 1 < messageTemplate.Length &&
                            messageTemplate[startAt + 1] == '}')
                        {
                            startAt++;
                        }
                    }
                }

                startAt++;
            } while (startAt < messageTemplate.Length);

            next = startAt;
            return new TextToken(accum.ToString());
        }
    }
}

﻿using System;

namespace Joke.Front.Pony
{
    public class Keywords
    {
        public static bool IsKeyword(ISpan span)
        {
            return IsKeyword(span.Value.ToString());
        }

        public static bool IsKeyword(string maybeKeyword)
        {
            return Array.BinarySearch(keywords, maybeKeyword) >= 0;
        }

        private static string[] keywords =
        {
            "actor",
            "as",
            "be",
            "box",
            "break",
            "class",
            "compile_error",
            "compile_intrinsic",
            "continue",
            "consume",
            "digestof",
            "do",
            "else",
            "elseif",
            "embed",
            "end",
            "error",
            "for",
            "fun",
            "if",
            "ifdef",
            "in",
            "interface",
            "is",
            "isnt",
            "iso",
            "lambda",
            "let",
            "match",
            "new",
            "not",
            "object",
            "primitive",
            "recover",
            "ref",
            "repeat",
            "return",
            "struct",
            "tag",
            "then",
            "this",
            "trait",
            "trn",
            "try",
            "type",
            "until",
            "use",
            "var",
            "val",
            "where",
            "while",
            "with",
        };
    }
}

﻿using System;
using UnityEngine;

namespace Text
{
    public abstract class BaseExcerpt
    {
        protected static readonly string formatErrorMessage = "Could not parse the format string/arguments supplied";

        public readonly Options defaultOptions;
        public readonly Vector2 defaultPosition;
        public readonly string excerptFormatString;

        public BaseExcerpt(Excerpt excerpt) : this(excerpt.excerptFormatString, excerpt.defaultPosition, excerpt.defaultOptions) { }

        public BaseExcerpt(string excerpt, Vector2 position)
        {
            excerptFormatString = excerpt;
            defaultPosition = position;
        }

        public BaseExcerpt(string excerpt, Vector2 position, Options options) : this(excerpt, position)
        {
            defaultOptions = options;
        }

        protected void TestFormatStringValidity()
        {
            try
            {
                ToString();
            }
            catch (FormatException ex)
            {
                throw new ArgumentException(formatErrorMessage, ex);
            }
        }

        public abstract override string ToString();
    }

    public class Excerpt : BaseExcerpt
    {
        public Excerpt(string excerpt, Vector2 position) : base(excerpt, position) { }

        public Excerpt(string excerpt, Vector2 position, Options options) : base(excerpt, position, options) { }

        public override string ToString()
        {
            return excerptFormatString;
        }
    }

    public class Excerpt<T0> : BaseExcerpt
    {
        private T0 arg0;

        public Excerpt(Excerpt excerpt, T0 formatArg0) : base(excerpt)
        {
            arg0 = formatArg0;
            TestFormatStringValidity();
        }

        public override string ToString()
        {
            return string.Format(excerptFormatString, arg0);
        }
    }

    public class Excerpt<T0, T1> : BaseExcerpt
    {
        private T0 arg0;
        private T1 arg1;

        public Excerpt(Excerpt excerpt, T0 formatArg0, T1 formatArg1) : base(excerpt)
        {
            arg0 = formatArg0;
            arg1 = formatArg1;
            TestFormatStringValidity();
        }

        public override string ToString()
        {
            return string.Format(excerptFormatString, arg0, arg1);
        }
    }

    public class Excerpt<T0, T1, T2> : BaseExcerpt
    {
        private T0 arg0;
        private T1 arg1;
        private T2 arg2;


        public Excerpt(Excerpt excerpt, T0 formatArg0, T1 formatArg1, T2 formatArg2) : base(excerpt)
        {
            arg0 = formatArg0;
            arg1 = formatArg1;
            arg2 = formatArg2;
            TestFormatStringValidity();
        }

        public override string ToString()
        {
            return string.Format(excerptFormatString, arg0, arg1, arg2);
        }
    }
}

using System;
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
        protected T0 arg0;

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
        protected T0 arg0;
        protected T1 arg1;

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
        protected T0 arg0;
        protected T1 arg1;
        protected T2 arg2;

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

    public class UpdateableExcerpt<T0, T1, T2> : Excerpt<T0, T1, T2>
    {
        public UpdateableExcerpt(Excerpt excerpt, T0 formatArg0, T1 formatArg1, T2 formatArg2)
            : base(excerpt, formatArg0, formatArg1, formatArg2)
        { }

        public void updateArg0(T0 newValue)
        {
            arg0 = newValue;
            TextManager.ReplaceText(this);
        }

        public void updateArg1(T1 newValue)
        {
            arg1 = newValue;
            TextManager.ReplaceText(this);
        }

        public void updateArg2(T2 newValue)
        {
            arg2 = newValue;
            TextManager.ReplaceText(this);
        }
    }
}

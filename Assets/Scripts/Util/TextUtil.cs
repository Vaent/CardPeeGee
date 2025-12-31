using System;

namespace Util
{
    public class TextUtil
    {
        /** <summary>
         * Constructs a string from the quantity argument, a space character, and either
         * the singular or plural form of the item as appropriate for the quantity.
         * Pluralisation is performed by <see cref="SingleOrPlural(string, double)"/>.
         * </summary> */
        public static string QuantifiedItem(string singularForm, double quantity)
        {
            return $"{quantity} {SingleOrPlural(singularForm, quantity)}";
        }

        /** <summary>
         * Basic pluralisation of English words, to be extended as required for new use cases.
         * <list type="bullet">
         * <item>All words are currently pluralised by appending a lowercase "s".</item>
         * </list>
         * </summary> */
        public static string SingleOrPlural(string singularForm, double quantity)
        {
            if (Math.Abs(quantity).Equals(1)) return singularForm;
            return $"{singularForm}s";
        }
    }
}
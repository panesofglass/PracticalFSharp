namespace TestDataAccess
{
    using System;

    using Microsoft.FSharp.Core;

    static class DelegateExtensionMethods
    {
        /// <summary>
        /// Performs an implicit conversion to the F# FastFunc delegate type.
        /// </summary>
        /// <typeparam name="T">The type upon which the action should occur.</typeparam>
        /// <param name="action">The action.</param>
        /// <returns>A converter from <typeparamref name="T" /> to Unit (i.e. null).</returns>
        /// <remarks>This allows C# to pass Action delegates to F#.</remarks>
        public static Converter<T, Unit> ToConverter<T>(this Action<T> action)
        {
            return new Converter<T, Unit>(value =>
            {
                action(value);
                return null; // Equivalent to F# Unit.
            });
        }
    }
}
using System;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace Pagansoft.Functional
{
    public static class OptionExtensions
    {
        /// <summary>
        /// Invokes the given <paramref name="action"/> with the value of the given <paramref name="option"/>
        /// if the <paramref name="option"/> has a value.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="action">The action to invoke.</param>
        /// <typeparam name="T">The type of the option value.</typeparam>
        public static void Do<T>(this Option<T> option, Action<T> action)
        {
            Contract.Requires(option != null);
            Contract.Requires(action != null);

            if (option.IsSome)
                action(option.Value);
        }

        /// <summary>
        /// Invokes the given <paramref name="action"/>
        /// if the given <paramref name="option"/> has no value.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="action">The action to invoke.</param>
        /// <param name="action">Action.</param>
        public static void OtherwiseDo<T>(this Option<T> option, Action action)
        {
            Contract.Requires(option != null);
            Contract.Requires(action != null);

            if (option.IsNone)
                action();
        }
    }
}


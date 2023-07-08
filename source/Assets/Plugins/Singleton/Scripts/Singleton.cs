//============================================================
// Project: SingleInstances
// Author: Zoranner@ZORANNER
// Datetime: 2019-05-13 22:51:49
// Description: TODO >> This is a script Description.
//============================================================

using System;
using System.Linq;
using System.Reflection;

namespace KimoTech.Singletons
{
    public abstract class Singleton<T> where T : class
    {
        private static readonly Lazy<T> _Instance = new Lazy<T>(() =>
        {
            var ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (ctors.Length != 1)
            {
                throw new InvalidOperationException($"Type {typeof(T)} must have exactly one constructor.");
            }

            var ctor = ctors.SingleOrDefault(c => !c.GetParameters().Any() && c.IsPrivate);
            return ctor == null
                ? throw new InvalidOperationException(
                    $"The constructor for {typeof(T)} must be private and take no parameters.")
                : (T)ctor.Invoke(null);
        });

        public static T I => _Instance.Value;

        public static implicit operator bool(Singleton<T> singleInstance)
        {
            return singleInstance != null;
        }
    }
}
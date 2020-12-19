using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LLkGrammarChecker.Extensions
{
    static class DictionaryExtensions
    {
        public static Dictionary<T, HashSet<HashSet<U>>> DeepCopy<T, U>(this Dictionary<T, HashSet<HashSet<U>>> source)
        {
            var copy = new Dictionary<T, HashSet<HashSet<U>>>();

            foreach (var (sourceKey, sourceSetValues) in source)
            {
                copy[sourceKey] = new HashSet<HashSet<U>>(HashSet<U>.CreateSetComparer());

                foreach (var sourceSetValue in sourceSetValues)
                {
                    var copySetValue = new HashSet<U>();

                    foreach (var sourceValue in sourceSetValue)
                    {
                        copySetValue.Add(sourceValue);
                    }

                    copy[sourceKey].Add(copySetValue);
                }
            }

            return copy;
        }

        public static bool DeepEquals<T, U>(this Dictionary<T, HashSet<HashSet<U>>> one,
            Dictionary<T, HashSet<HashSet<U>>> another)
        {
            if (one.Count != another.Count)
            {
                return false;
            }

            foreach (var (keyInOne, valuesInOne) in one)
            {
                var valuesInAnother = another.GetValueOrDefault(keyInOne);

                if (valuesInAnother == null || valuesInAnother.Count != valuesInOne.Count)
                {
                    return false;
                }

                foreach (var valueInOne in valuesInOne)
                {
                    if (valuesInAnother.Where(v => v.SetEquals(valueInOne)).Count() == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static Dictionary<T, HashSet<U>> DeepCopy<T, U>(this Dictionary<T, HashSet<U>> source)
        {
            var copy = new Dictionary<T, HashSet<U>>();

            foreach (var (sourceKey, sourceValues) in source)
            {
                copy[sourceKey] = new HashSet<U>();

                foreach (var sourceValue in sourceValues)
                {
                    copy[sourceKey].Add(sourceValue);
                }
            }

            return copy;
        }

        public static bool DeepEquals<T, U>(this Dictionary<T, HashSet<U>> one,
            Dictionary<T, HashSet<U>> another)
        {
            if (one.Count != another.Count)
            {
                return false;
            }

            foreach (var (keyInOne, valuesInOne) in one)
            {
                var valuesInAnother = another.GetValueOrDefault(keyInOne);

                if (valuesInAnother == null || !valuesInOne.SetEquals(valuesInAnother))
                {
                    return false;
                }
            }

            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.infosupport.afstuderen.opleidingsplan.generator
{
    public static class Extentions
    {
        public static bool None<TSource>(this IEnumerable<TSource> source)
        {
            return !source.Any();
        }

        public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return !source.Any(predicate);
        }

        public static int ToInt(this string duration)
        {
            string number = duration.Split(' ').First();
            int output;

            if(int.TryParse(number, out output))
            {
                return output;
            }

            return 0;
        }
    }
}

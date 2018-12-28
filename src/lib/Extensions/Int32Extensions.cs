using System.Collections.Generic;
using System.Linq;

namespace CronQuery.Extensions
{
    internal static class Int32Extensions
    {
        public static IEnumerable<int> Step(this IEnumerable<int> source, int step)
        {
            if (step == 0)
            {
                return source;
            }

            return source.Where((value, index) => index % step == 0);
        }

        public static IEnumerable<int> To(this int from, int to)
        {
            if (from <= to)
            {
                while (from <= to)
                {
                    yield return from++;
                }
            }
            else
            {
                while (from >= to)
                {
                    yield return from--;
                }
            }
        }
    }
}

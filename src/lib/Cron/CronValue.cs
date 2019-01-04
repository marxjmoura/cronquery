/* MIT License
 *
 * Copyright (c) 2018 LogiQ System
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CronQuery.Extensions;

namespace CronQuery.Cron
{
    internal sealed class CronValue
    {
        const int NoValue = -1;

        private readonly TimeUnit _timeUnit;

        public CronValue(string expression, TimeUnit timeUnit)
        {
            _timeUnit = timeUnit;

            IsAsterisk = expression == "*";
            IsL = expression == "L";
            HasAsterisk = expression.Contains("*");
            HasDash = expression.Contains("-");
            HasSlash = expression.Contains("/");
            HasHash = expression.Contains("#");
            HasL = expression.Contains("L");
            HasW = expression.Contains("W");

            var symbol = HasSlash ? '/' : HasDash ? '-' : HasHash ? '#' : '\0';
            var parameters = expression.Split(symbol);
            var nestedParameters = HasSlash && HasDash ? parameters.First().Split('-') : null;

            var start =
                IsAsterisk ? timeUnit.MinValue : // Expression is *
                IsL ? NoValue : // Expression is L
                HasL && HasDash ? ToInt32(parameters.Last()) : // Expression is L-00
                HasL && HasW ? NoValue : // Expression is LW
                HasHash ? ToInt32(parameters.First()) : // Expression is 00#0
                HasSlash && HasAsterisk ? timeUnit.MinValue : // Expression is */00
                HasSlash && HasDash ? ToInt32(nestedParameters.First()) : // Expression is 00-00/00
                HasSlash || HasDash || HasW ? ToInt32(parameters.First()) : // Expression is 00/00 or 00-00 or 00W
                Math.Max(ToInt32(expression), timeUnit.MinValue); // Expression is 00

            var end =
                IsAsterisk ? timeUnit.MaxValue : // Expression is *
                IsL ? NoValue : // Expression is L
                HasL && HasDash ? ToInt32(parameters.Last()) : // Expression is L-00
                HasL && HasW ? NoValue : // Expression is LW
                HasHash ? start : // Expression is 00#0
                HasSlash && HasAsterisk ? timeUnit.MaxValue : // Expression is */00
                HasSlash && HasDash ? ToInt32(nestedParameters.Last()) : // Expression is 00-00/00
                HasSlash ? timeUnit.MaxValue : // Expression is 00/00
                HasW ? ToInt32(parameters.First()) : // Expression is 00W
                HasDash ? ToInt32(parameters.Last()) : // Expression is 00-00
                Math.Min(ToInt32(expression), timeUnit.MaxValue); // Expression is 00

            Nth = HasHash ? ToInt32(parameters.Last()) : 0;
            Step = HasSlash ? ToInt32(parameters.Last()) : 1;
            Values = start.To(end).Step(Step)
                .Where(value => value >= _timeUnit.MinValue)
                .Where(value => value <= _timeUnit.MaxValue)
                .ToList();
        }

        public IEnumerable<int> Values { get; private set; }
        public int Nth { get; private set; }
        public int Step { get; private set; }
        public bool IsAsterisk { get; private set; }
        public bool IsL { get; private set; }
        public bool HasAsterisk { get; private set; }
        public bool HasL { get; private set; }
        public bool HasW { get; private set; }
        public bool HasDash { get; private set; }
        public bool HasSlash { get; private set; }
        public bool HasHash { get; private set; }

        private int ToInt32(string value)
        {
            var digits = Regex.Replace(value, @"[^\d]+", string.Empty);
            return Convert.ToInt32(digits);
        }
    }
}

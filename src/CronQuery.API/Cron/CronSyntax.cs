/*
 * MIT License
 *
 * Copyright (c) 2018 Marx J. Moura
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

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CronQuery.Cron
{
    internal sealed class CronSyntax
    {
        const string Asterisk = @"^\*$"; // Matches: *
        const string Dash = @"^\d{1,2}-\d{1,2}$"; // Matches: 00-00
        const string Hash = @"^\d{1,2}#[1-5]$"; // Matches: 00#0
        const string LAndW = @"^(\d)?L$|L-\d{1,2}|^LW$|^\d{1,2}W$"; // Matches: L, 0L, L-00, LW or 00W
        const string Slash = @"^(\*|\d{1,2}(-\d{1,2})?)/\d{1,2}$"; // Matches: */00, 00/00 or 00-00/00
        const string ListValue = @"\d{1,2}|\d{1,2}-\d{1,2}|\d{1,2}(-\d{1,2})?/\d{1,2}"; // Matches: 00, 00-00, 00/00 or 00-00/00

        const int SecondPosition = 0;
        const int MinutePosition = 1;
        const int HourPosition = 2;
        const int DayPosition = 3;
        const int MonthPosition = 4;
        const int DayOfWeekPosition = 5;

        private readonly IEnumerable<string> _expression;

        public CronSyntax(IEnumerable<string> expression)
        {
            _expression = expression;
        }

        public bool IsValid()
        {
            if (_expression.Count() != 6)
            {
                return false;
            }

            if (!AllowedCharacters("*-,/", _expression.ElementAt(SecondPosition))) return false;
            if (!AllowedCharacters("*-,/", _expression.ElementAt(MinutePosition))) return false;
            if (!AllowedCharacters("*-,/", _expression.ElementAt(HourPosition))) return false;
            if (!AllowedCharacters("*-,/LW", _expression.ElementAt(DayPosition))) return false;
            if (!AllowedCharacters("*-,/", _expression.ElementAt(MonthPosition))) return false;
            if (!AllowedCharacters("*-,/L#", _expression.ElementAt(DayOfWeekPosition))) return false;

            return IsWellFormed();
        }

        private bool AllowedCharacters(string allowedCharacters, string subExpression)
        {
            var characters = Regex.Replace(subExpression, @"\d", string.Empty);

            if (!characters.Any())
            {
                return true;
            }

            return !characters.Where(character => !allowedCharacters.Contains(character)).Any();
        }

        private bool IsWellFormed()
        {
            var list = $@"^{ListValue}(,({ListValue}))*$";
            var pattern = $@"{Asterisk}|{Dash}|{Hash}|{Slash}|{LAndW}|{list}";
            var regex = new Regex(pattern, RegexOptions.Compiled);

            return _expression.All(expression => regex.IsMatch(expression));
        }
    }
}

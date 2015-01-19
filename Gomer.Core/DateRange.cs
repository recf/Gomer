using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomer.Core
{
    public struct DateRange
    {
        public static DateRange Parse(string input)
        {
            var parts = input.Replace("...", "..").Replace("..", ",").Split(',');

            var start = string.IsNullOrWhiteSpace(parts[0]) 
                ? (DateTime?) null 
                : DateTime.Parse(parts[0]);
            var end = string.IsNullOrWhiteSpace(parts[1])
                ? (DateTime?)null
                : DateTime.Parse(parts[1]);

            return new DateRange(start,end);
        }

        private readonly DateTime? _start;

        private readonly DateTime? _end;

        public DateRange(DateTime? start, DateTime? end)
        {
            _start = start;
            _end = end;
        }

        public DateTime? Start { get { return _start; } }

        public DateTime? End { get { return _end; } }

        #region Overrides of ValueType

        public override string ToString()
        {
            var format = "yyyy-MM-dd";
            var result = string.Empty;

            if (Start.HasValue)
            {
                result += Start.Value.ToString(format);
            }

            result += "...";

            if (End.HasValue)
            {
                result += End.Value.ToString(format);
            }

            return result;
        }

        #endregion
    }
}

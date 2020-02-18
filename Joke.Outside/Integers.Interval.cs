using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Joke.Outside
{
    partial class Integers
    {
        private struct Interval : IEnumerable<int>
        {
            public Interval(int minmax)
            {
                Min = minmax;
                Max = minmax;
            }

            public Interval(int min, int max)
            {
                Debug.Assert(min <= max);
                Min = min;
                Max = max;
            }

            public int Min { get; private set; }
            public int Max { get; private set; }
            public int Count => Max - Min + 1;

            public bool Contains(int value)
            {
                return Min <= value && value <= Max;
            }

            public bool Overlaps(Interval other)
            {
                return Contains(other.Min) || Contains(other.Max) || other.Contains(Min) || other.Contains(Max);
            }

            public IEnumerator<int> GetEnumerator() => Enumerable.Range(Min, Max - Min + 1).GetEnumerator();

            public string ToIString()
            {
                if (Min == Max)
                {
                    return $"{Min}";
                }
                if (Min == Max - 1)
                {
                    return $"{Min}, {Max}";
                }
                if (Min == Max - 2)
                {
                    return $"{Min}, {Max - 1}, {Max}";
                }
                if (Min == Max - 3)
                {
                    return $"{Min}, {Max - 2}, {Max - 1}, {Max}";
                }

                if (Min == Max - 4)
                {
                    return $"{Min}, {Max - 3}, {Max - 2}, {Max - 1}, {Max}";
                }

                return $"{Min} - {Max}";
            }

            public override string ToString()
            {
                if (Min == Max)
                {
                    return CharRep.InRange(Min);
                }

                return $"{CharRep.InRange(Min)}-{CharRep.InRange(Max)}";
            }

            public static bool TryParse(string str, out Interval range)
            {
                var end = 0;
                while (end < str.Length && char.IsDigit(str, end))
                {
                    end += 1;
                }
                if (end > 0)
                {
                    if (end == str.Length)
                    {
                        if (int.TryParse(str, out var minmax))
                        {
                            range = new Interval(minmax);
                            return true;
                        }
                    }
                    else if (str[end] == '-' && int.TryParse(str.Substring(0, end), out var min))
                    {
                        var start = end += 1;
                        while (end < str.Length && char.IsDigit(str, end))
                        {
                            end += 1;
                        }
                        if (end > start && end == str.Length)
                        {
                            if (int.TryParse(str[start..end], out var max))
                            {
                                range = new Interval(min, max);
                                return true;
                            }
                        }
                    }
                }

                range = default;
                return false;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public override bool Equals(object? obj)
            {
                return obj is Interval other && Min == other.Min && Max == other.Max;
            }

            public override int GetHashCode() => (Min, Max).GetHashCode();
        }
    }
}

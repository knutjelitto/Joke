using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Joke.Outside
{
    public partial class Integers : IEnumerable<int>, IEquatable<Integers>
    {
        public static Integers Empty => new Integers();
        public static Integers Any => new Integers(new Interval(int.MinValue, int.MaxValue));

        private Integers(Interval range)
            : this(Enumerable.Repeat(range, 1))
        {
        }

        private Integers()
            : this(Enumerable.Empty<Interval>())
        {
        }

        private Integers(Integers other)
            : this(other.ranges)
        {

        }

        public Integers(params (int min, int max)[] ranges)
            : this(ranges.Select(p => new Interval(p.min, p.max)))
        {
        }

        private Integers(IEnumerable<Interval> ranges)
        {
            this.ranges = new List<Interval>();
            foreach (var range in ranges)
            {
                Add(range);
            }
        }

        public int Cardinality => ranges.Sum(range => range.Count);

        public bool IsEmpty => ranges.Count == 0;

        public bool IsAny => ranges.Count == 1 && Min == Any.Min && Max == Any.Max;

        public int Max => ranges.Last().Max;

        public int Min => ranges.First().Min;

        public int IntervalCount => ranges.Count;

        public static Integers From(Integers other)
        {
            return new Integers(other);
        }

        public static Integers From(int minmax)
        {
            return new Integers(new Interval(minmax));
        }

        public static Integers From(int min, int max)
        {
            return new Integers(new Interval(min, max));
        }

        public static Integers? Parse(string str)
        {
            if (TryParse(str, out var set))
            {
                return set;
            }
            return null;
        }

        public static bool TryParse(string str, [MaybeNullWhen(false)] out Integers? set)
        {
            if (str.Length == 0 || str[0] != '[')
            {
                set = null;
                return false;
            }

            var start = 1;
            var end = 1;
            set = new Integers();
            while (end < str.Length)
            {
                while (end < str.Length && str[end] != ',' && str[end] != ']')
                {
                    end += 1;
                }
                if (end > start && Interval.TryParse(str[start..end], out var range))
                {
                    set.Add(range);
                    start = end += 1;
                }
                else
                {
                    break;
                }
            }

            if (end == str.Length)
            {
                return true;
            }

            set = null;
            return false;
        }

        public void Add(int value)
        {
            Add(new Interval(value));
        }

        public void Add(params (int min, int max)[] rangesToAdd)
        {
            Add(rangesToAdd.Select(range => new Interval(range.min, range.max)));
        }

        public void Add(Integers other)
        {
            Add(other.ranges);
        }

        public Integers Clone()
        {
            return new Integers(ranges);
        }

        public bool Contains(int value)
        {
            return Find(value, out var _);
        }

        public override bool Equals(object? obj)
        {
            return obj is Integers other && ranges.SequenceEqual(other.ranges);
        }

        public bool Equals(Integers? other)
        {
            return other != null && ranges.SequenceEqual(other.ranges);
        }

        public Integers ExceptWith(Integers other)
        {
            var set = Clone();
            set.Sub(other.ranges);
            return set;
        }

        public Integers IntersectWith(Integers other)
        {
            var union = UnionWith(other);
            var ex1 = ExceptWith(other);
            var ex2 = other.ExceptWith(this);

            return union.ExceptWith(ex1).ExceptWith(ex2);
        }

        public IEnumerator<int> GetEnumerator()
        {
            foreach (var range in ranges)
            {
                foreach (var value in range)
                {
                    yield return value;
                }
            }
        }

        public override int GetHashCode() => ranges.Hash();

        public IEnumerable<(int Min, int Max)> GetIntervals()
        {
            return ranges.Select(r => (r.Min, r.Max));
        }

        public IEnumerable<int> GetValues()
        {
            return ranges.SelectMany(range => range);
        }

        public bool IsProperSubsetOf(Integers other)
        {
            return IsSubsetOf(other) && !Equals(other);
        }

        public bool IsProperSupersetOf(Integers other)
        {
            return IsSupersetOf(other) && !Equals(other);
        }

        public bool IsSubsetOf(Integers other)
        {
            foreach (var range in ranges)
            {
                if (!other.Contains(range))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsSupersetOf(Integers other)
        {
            return other.IsSubsetOf(this);
        }

        public bool Overlaps(Integers other)
        {
            var t = 0;
            var o = 0;

            while (t < ranges.Count && o < other.ranges.Count)
            {
                while (t < ranges.Count && o < other.ranges.Count && ranges[t].Max < other.ranges[o].Min)
                {
                    t += 1;
                }

                while (t < ranges.Count && o < other.ranges.Count && other.ranges[o].Max < ranges[t].Min)
                {
                    o += 1;
                }

                if (t < ranges.Count && o < other.ranges.Count && other.ranges[o].Overlaps(ranges[t]))
                {
                    return true;
                }
            }

            return false;
        }

        public void Sub(int value)
        {
            Sub(new Interval(value));
        }

        public void Sub(params (int min, int max)[] rangesToSub)
        {
            Sub(rangesToSub.Select(range => new Interval(range.min, range.max)));
        }

        public Integers Substract(Integers other)
        {
            return Clone().Sub(other.ranges);
        }

        public Integers Not()
        {
            return Any.Substract(this);
        }

        public override string ToString()
        {
            return $"[{string.Join(", ", ranges)}]";
        }

        public string ToIString()
        {
            return $"[{string.Join(", ", ranges.Select(r => r.ToIString()))}]";
        }

        public Integers UnionWith(Integers other)
        {
            var set = Clone();
            set.Add(other.ranges);
            return set;
        }

        public static Integers operator +(Integers set1, Integers set2)
        {
            return set1.UnionWith(set2);
        }

        public static Integers operator /(Integers set1, Integers set2)
        {
            return set1.ExceptWith(set2);
        }


        public static explicit operator Integers(char ch)
        {
            return From(ch);
        }

        private void Add(Interval add)
        {
            var i = 0;
            while (i < ranges.Count)
            {
                var current = ranges[i];

                if (add.Min > current.Max + 1)
                {
                    ++i;
                    continue;
                }

                if (add.Max + 1 < current.Min)
                {
                    // before current
                    ranges.Insert(i, add);
                    return;
                }

                if (add.Max <= current.Max)
                {
                    // combine with current
                    ranges[i] = new Interval((char) Math.Min(add.Min, current.Min), current.Max);
                    return;
                }

                add = new Interval((char) Math.Min(add.Min, current.Min), add.Max);
                ranges.RemoveAt(i);
            }

            if (i == ranges.Count)
            {
                ranges.Add(add);
            }
        }

        private void Add(IEnumerable<Interval> rangesToAdd)
        {
            foreach (var range in rangesToAdd)
            {
                Add(range);
            }
        }

        private bool Contains(Interval range)
        {
            return Find(range.Min, out var found) && range.Max <= found.Max;
        }

        private bool Find(int value, out Interval range)
        {
            bool Find(int lower, int upper, out Interval found)
            {
                if (upper < lower)
                {
                    found = default;
                    return false;
                }

                var mid = lower + (upper - lower) / 2;

                if (ranges[mid].Contains(value))
                {
                    found = ranges[mid];
                    return true;
                }
                if (value < ranges[mid].Min)
                {
                    return Find(lower, mid - 1, out found);
                }
                return Find(mid + 1, upper, out found);
            }

            return Find(0, ranges.Count - 1, out range);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void Sub(Interval sub)
        {
            var i = 0;

            while (i < ranges.Count)
            {
                var range = ranges[i];

                if (sub.Max < range.Min)
                {
                    i += 1;
                    continue;
                }

                if (range.Max < sub.Min)
                {
                    i += 1;
                    continue;
                }

                if (sub.Min <= range.Min)
                {
                    // cover from below
                    if (sub.Max >= range.Max)
                    {
                        // full cover
                        ranges.RemoveAt(i);
                    }
                    else
                    {
                        ranges[i] = new Interval(sub.Max + 1, range.Max);
                        i += 1;
                    }
                    continue;
                }

                if (range.Max <= sub.Max)
                {
                    // cover from above
                    if (range.Min >= sub.Min)
                    {
                        // full cover
                        ranges.RemoveAt(i);
                    }
                    else
                    {
                        ranges[i] = new Interval(range.Min, sub.Min - 1);
                        i += 1;
                    }
                    continue;
                }

                // inner
                // sub.Min > range.Min && range.Max > sub.Max
                ranges.Insert(i, new Interval(range.Min, sub.Min - 1));
                ranges[i + 1] = new Interval(sub.Max + 1, range.Max);
                // done
                break;
            }
        }

        private Integers Sub(IEnumerable<Interval> rangesToSub)
        {
            foreach (var range in rangesToSub)
            {
                Sub(range);
            }

            return this;
        }

        private readonly List<Interval> ranges;
    }
}

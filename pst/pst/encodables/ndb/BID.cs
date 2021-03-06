﻿using System;

namespace pst.encodables.ndb
{
    class BID : IComparable<BID>, IEquatable<BID>
    {
        public static readonly BID Zero = new BID(0);

        public long Value { get; }

        private BID(long value)
        {
            Value = value;
        }

        public static BID ForInternalBlock(long bidIndex)
        {
            return new BID((1 << 1) & bidIndex << 2);
        }

        public static BID ForExternalBlock(long bidIndex)
        {
            return new BID(bidIndex << 2);
        }

        public static BID OfValue(long value) => new BID(value);

        public bool Equals(BID other)
        {
            return other?.Value == Value;
        }

        public int CompareTo(BID other)
        {
            return Value.CompareTo(other.Value);
        }

        public override bool Equals(object obj)
        {
            var bid = obj as BID;

            return bid?.Value == Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return $"0x{Value:x}".ToLower();
        }
    }
}

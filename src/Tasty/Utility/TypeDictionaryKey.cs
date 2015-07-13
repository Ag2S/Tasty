using System;

namespace Tasty.Utility
{
    internal class TypeDictionaryKey
    {
        public static TypeDictionaryKey From(Type type, string name)
        {
            return new TypeDictionaryKey(type, name);
        }

        public static TypeDictionaryKey From(Type type, int serialNumder)
        {
            return new TypeDictionaryKey(type, serialNumder);
        }

        private TypeDictionaryKey(Type majorKey, object minorKey)
        {
            MajorKey = majorKey;
            MinorKey = minorKey;
        }

        private Type MajorKey { get; set; }
        private object MinorKey { get; set; }
        public override bool Equals(object rhs)
        {
            if (!(rhs is TypeDictionaryKey))
                return false;

            return MajorKey.Equals(((TypeDictionaryKey)rhs).MajorKey) && MinorKey.Equals(((TypeDictionaryKey)rhs).MinorKey);
        }

        public override int GetHashCode()
        {
            return MajorKey.GetHashCode() ^ MinorKey.GetHashCode();
        }
    }
}

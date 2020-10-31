using System;
using System.Collections.Generic;
using System.Text;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;

namespace Maple2Storage.Utils
{
    public static class Serialization
    {
        // Take an Object of data and save it into a byte[].
        public static byte[] Serialize(this object obj)
        {
            throw new NotImplementedException();
        }
        // Take a Dictionary and save it into a byte[].
        public static byte[] SerializeDictionary(this object dictionary)
        {
            throw new NotImplementedException();
        }
        // Take a Collection and save it into a byte[]
        public static byte[] SerializeCollection(this object collection)
        {
            throw new NotImplementedException();
        }

        // Take a Collection<T> and save it into a byte[]
        public static byte[] SerializeCollection<T>(this object collection)
        {
            throw new NotImplementedException();
        }

        // Take a byte[] and convert the data into an object.
        public static T Deserialize<T>(this object obj)
        {
            throw new NotImplementedException();
        }

        // Take a byte[] and convert the data to a IDictionary of 2 parameters, T & A
        public static IDictionary<T, A> DeserializeDictionary<T, A>(this object obj)
        {
            throw new NotImplementedException();
        }

        // Take a byte[] and convert the data into a ICollection of T
        public static ICollection<T> DeserializeCollection<T>(this object obj)
        {
            throw new NotImplementedException();
        }

        // Take a byte[] and convert the data into a Collection of 2 parameters, T & A
        public static T DeserializeCollection<T, A>(this object obj)
        {
            throw new NotImplementedException();
        }
    }
}

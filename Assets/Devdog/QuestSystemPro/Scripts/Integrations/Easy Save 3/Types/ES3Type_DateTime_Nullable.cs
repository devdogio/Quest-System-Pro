#if EASY_SAVE_3
using System;
using UnityEngine;

namespace ES3Types
{
    [UnityEngine.Scripting.Preserve]
    public class ES3Type_DateTime_Nullable : ES3Type
    {
        public static ES3Type Instance = null;

        public ES3Type_DateTime_Nullable() : base(typeof(DateTime?))
        {
            Instance = this;
        }

        public override void Write(object obj, ES3Writer writer)
        {
            writer.WriteProperty("ticks", obj!=null ? ((DateTime)obj).Ticks : 0 , ES3Type_long.Instance);
        }

        public override object Read<T>(ES3Reader reader)
        {
            reader.ReadPropertyName();
            return new DateTime(reader.Read<long>(ES3Type_long.Instance));
        }
    }

    public class ES3Type_DateTimeArray : ES3ArrayType
    {
        public static ES3Type Instance;

        public ES3Type_DateTimeArray() : base(typeof(DateTime?[]), ES3Type_DateTime.Instance)
        {
            Instance = this;
        }
    }
}
#endif
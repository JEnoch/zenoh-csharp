using System;
using System.Runtime.InteropServices;


namespace Zenoh.Net
{
    public class ResKey
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct ZResKey
        {
            internal UInt64 id;       // c_ulong
            internal IntPtr suffix;   // *const c_char
        }

        internal ZResKey _key;

        internal ResKey(ZResKey _key)
        {
            this._key = _key;
        }

        public static ResKey RId(UInt64 id)
        {
            ZResKey zkey;
            zkey.id = id;
            zkey.suffix = IntPtr.Zero;
            return new ResKey(zkey);
        }

        public static ResKey RName(string suffix)
        {
            ZResKey zkey;
            zkey.id = 0;
            zkey.suffix = Marshal.StringToHGlobalAnsi(suffix);
            return new ResKey(zkey);
        }

        public static ResKey RIdWithSuffix(UInt64 id, string suffix)
        {
            ZResKey zkey;
            zkey.id = id;
            zkey.suffix = Marshal.StringToHGlobalAnsi(suffix);
            return new ResKey(zkey);
        }

        public bool IsNumerical()
        {
            return this._key.suffix == IntPtr.Zero;
        }

        public UInt64 Id()
        {
            return this._key.id;
        }

        public string Suffix()
        {
            return Marshal.PtrToStringAnsi(this._key.suffix);
        }

    }

}
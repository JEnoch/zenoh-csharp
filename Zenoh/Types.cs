using System;
using System.Runtime.InteropServices;


namespace Zenoh
{

    public class ResKey
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct ZResKey
        {
            UInt64 id;       // c_ulong
            IntPtr suffix;   // *const c_char
        }

    }





    [StructLayout(LayoutKind.Sequential)]
    internal struct ZString
    {
        public IntPtr val;  // *const c_char
        public IntPtr len;  // size_t
    }

    internal static class ZTypes
    {
        internal static String ZStringToString(ZString zs)
        {
            byte[] managedArray = new byte[(int)zs.len];
            System.Runtime.InteropServices.Marshal.Copy(zs.val, managedArray, 0, (int)zs.len);
            String result = System.Text.Encoding.UTF8.GetString(managedArray, 0, (int)zs.len);
            // TODO Free ZString ???
            return result;
        }
    }

}
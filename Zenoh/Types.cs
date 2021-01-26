using System;
using System.Runtime.InteropServices;


namespace Zenoh
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ZString
    {
        public IntPtr val;  // *const c_char
        public IntPtr len;  // size_t
    }

    internal static class ZTypes
    {
        internal static string ZStringToString(ZString zs)
        {
            byte[] managedArray = new byte[(int)zs.len];
            System.Runtime.InteropServices.Marshal.Copy(zs.val, managedArray, 0, (int)zs.len);
            string result = System.Text.Encoding.UTF8.GetString(managedArray, 0, (int)zs.len);
            // TODO Free ZString ???
            return result;
        }
    }

}
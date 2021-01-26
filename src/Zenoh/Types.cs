using System;
using System.Runtime.InteropServices;


namespace Zenoh
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ZString
    {
        public IntPtr val;
        public IntPtr len;
    }

    internal static class Types
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
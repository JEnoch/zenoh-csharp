// This file has been auto-generated, please do not edit it.

using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Zenoh.Net
{

    public class Session : IDisposable
    {
        private IntPtr _rustSession = IntPtr.Zero;

        private Session(IntPtr rustSession)
        {
            this._rustSession = rustSession;
        }

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (this._rustSession != IntPtr.Zero)
            {
                ZnClose(this._rustSession);
                this._rustSession = IntPtr.Zero;
            }
        }

        public static Session Open(Dictionary<string, string> config)
        {
            // It's simpler to encode the config as a single string to pass it to Rust where is will be decoded
            string configStr = "";
            foreach (KeyValuePair<string, string> kvp in config)
            {
                configStr += kvp.Key + "=" + kvp.Value + ";";
            }
            var props = ZnConfigFromStr(configStr);

            var rustSession = ZnOpen(props);
            // TODO: check errors...
            return new Session(rustSession);
        }

        private static char[] _propSeparator = { ';' };
        private static char[] _kvSeparator = { '=' };

        public Dictionary<string, string> Info()
        {
            var zstr = ZnInfoAsStr(this._rustSession);
            var str = ZTypes.ZStringToString(zstr);

            // Parse the properties from the string
            var properties = str.Split(_propSeparator, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(_kvSeparator, 2))
                .ToDictionary(x => x.First(), x => (x.Length == 2) ? x.Last() : "");
            return properties;
        }

        unsafe public void Write(ResKey reskey, byte[] payload)
        {
            fixed (byte* p = payload)
            {
                ZnWrite(this._rustSession, reskey._key, (IntPtr)p, (uint)payload.Length);
            }
        }

        [DllImport("zenohc", EntryPoint = "zn_config_from_str", CharSet = CharSet.Ansi)]
        internal static extern IntPtr /*zn_properties_t*/
            ZnConfigFromStr([MarshalAs(UnmanagedType.LPStr)] string str);

        [DllImport("zenohc", EntryPoint = "zn_open")]
        internal static extern IntPtr /*zn_session_t*/ ZnOpen(IntPtr /*zn_properties_t*/ config);

        [DllImport("zenohc", EntryPoint = "zn_info_as_str")]
        internal static extern ZString ZnInfoAsStr(IntPtr /*zn_session_t*/ rustSession);

        [DllImport("zenohc", EntryPoint = "zn_close")]
        internal static extern void ZnClose(IntPtr /*zn_session_t*/ rustSession);

        [DllImport("zenohc", EntryPoint = "zn_write")]
        internal static extern Int32 ZnWrite(IntPtr /*zn_session_t*/ rustSession, ResKey.ZResKey zResKey, IntPtr payload, UInt32 len);

    }
}
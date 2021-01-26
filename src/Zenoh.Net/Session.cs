// This file has been auto-generated, please do not edit it.

using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Zenoh.Net
{

    public class Session
    {
        private IntPtr _rustSession;

        private Session(IntPtr rustSession)
        {
            this._rustSession = rustSession;
        }

        public static Session open(Dictionary<string, string> config)
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


        public Dictionary<string, string> info()
        {
            var zstr = ZnInfoAsStr(this._rustSession);
            var str = Types.ZStringToString(zstr);

            // Parse the properties from the string
            var properties = str.Split(_propSeparator, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(_kvSeparator, 2))
                .ToDictionary(x => x.First(), x => (x.Length == 2) ? x.Last() : "");
            return properties;
        }


        [DllImport("zenohc", EntryPoint = "zn_config_from_str")]
        internal static extern IntPtr ZnConfigFromStr(String str);

        [DllImport("zenohc", EntryPoint = "zn_open")]
        internal static extern IntPtr ZnOpen(IntPtr config);

        [DllImport("zenohc", EntryPoint = "zn_info_as_str")]
        internal static extern ZString ZnInfoAsStr(IntPtr rustSession);

    }

}
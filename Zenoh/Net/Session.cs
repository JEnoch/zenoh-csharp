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
        private IntPtr /*zn_session_t*/ _nativePtr = IntPtr.Zero;

        private Session(IntPtr /*zn_session_t*/ nativeSession)
        {
            this._nativePtr = nativeSession;
        }

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (this._nativePtr != IntPtr.Zero)
            {
                ZnClose(this._nativePtr);
                this._nativePtr = IntPtr.Zero;
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

            var nativeSession = ZnOpen(props);
            // TODO: check errors...
            return new Session(nativeSession);
        }

        private static char[] _propSeparator = { ';' };
        private static char[] _kvSeparator = { '=' };

        public Dictionary<string, string> Info()
        {
            var zstr = ZnInfoAsStr(this._nativePtr);
            var str = ZTypes.ZStringToString(zstr);

            // Parse the properties from the string
            var properties = str.Split(_propSeparator, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(_kvSeparator, 2))
                .ToDictionary(x => x.First(), x => (x.Length == 2) ? x.Last() : "");
            return properties;
        }

        public ulong DeclareResource(ResKey reskey)
        {
            return ZnDeclareResource(this._nativePtr, reskey._key);
        }

        unsafe public void Write(ResKey reskey, byte[] payload)
        {
            fixed (byte* p = payload)
            {
                ZnWrite(this._nativePtr, reskey._key, (IntPtr)p, (uint)payload.Length);
            }
        }

        unsafe public void Write(ResKey reskey, byte[] payload, uint encoding, uint kind, CongestionControl congestionControl)
        {
            fixed (byte* p = payload)
            {
                ZnWriteExt(this._nativePtr, reskey._key, (IntPtr)p, (uint)payload.Length, encoding, kind, congestionControl);
            }
        }

        public Subscriber DeclareSubscriber(ResKey reskey, SubInfo subInfo, SubscriberCallback callback)
        {
            // convert the C# user callback as IntPtr to be passed as "arg" to zn_declare_subscriber()
            // and received back in each call to SubscriberCallbackNativeImpl
            IntPtr callbackPtr = Marshal.GetFunctionPointerForDelegate(callback);
            var nativeSubscriber = ZnDeclareSubscriber(this._nativePtr, reskey._key, subInfo._subInfo, SubscriberCallbackNativeImpl, callbackPtr);
            return new Subscriber(nativeSubscriber);
        }

        // Type of the callback function expected by zenoh-c
        internal delegate void SubscriberCallbackNative(IntPtr /* *const zn_sample_t */ samplePtr, IntPtr /* *const c_void */ callBackPtr);

        // Implementation of the zenoh-c callback that will call the C# user's callback passed as IntPtr
        internal void SubscriberCallbackNativeImpl(IntPtr /* *const zn_sample_t */ samplePtr, IntPtr /* *const c_void */ callBackPtr)
        {
            SubscriberCallback callback = Marshal.GetDelegateForFunctionPointer<SubscriberCallback>(callBackPtr);
            Sample s = new Sample(samplePtr);
            callback(s);
        }


        [DllImport("zenohc", EntryPoint = "zn_config_from_str", CharSet = CharSet.Ansi)]
        internal static extern IntPtr /*zn_properties_t*/ ZnConfigFromStr([MarshalAs(UnmanagedType.LPStr)] string str);

        [DllImport("zenohc", EntryPoint = "zn_open")]
        internal static extern IntPtr /*zn_session_t*/ ZnOpen(IntPtr /*zn_properties_t*/ config);

        [DllImport("zenohc", EntryPoint = "zn_info_as_str")]
        internal static extern ZString ZnInfoAsStr(IntPtr /*zn_session_t*/ rustSession);

        [DllImport("zenohc", EntryPoint = "zn_close")]
        internal static extern void ZnClose(IntPtr /*zn_session_t*/ rustSession);

        [DllImport("zenohc", EntryPoint = "zn_declare_resource")]
        internal static extern ulong ZnDeclareResource(IntPtr /*zn_session_t*/ rustSession, ResKey.NativeType zResKey);

        [DllImport("zenohc", EntryPoint = "zn_write")]
        internal static extern int ZnWrite(
            IntPtr /*zn_session_t*/ rustSession,
            ResKey.NativeType zResKey,
            IntPtr payload,
            uint len);

        [DllImport("zenohc", EntryPoint = "zn_write_ext")]
        internal static extern int ZnWriteExt(
            IntPtr /*zn_session_t*/ rustSession,
            ResKey.NativeType zResKey,
            IntPtr payload,
            uint len,
            uint encoding,
            uint kind,
            CongestionControl congestion);

        [DllImport("zenohc", EntryPoint = "zn_declare_subscriber")]
        internal static extern IntPtr /*zn_subscriber_t*/ ZnDeclareSubscriber(
            IntPtr /*zn_session_t*/ rustSession,
            ResKey.NativeType zResKey,
            SubInfo.NativeType zSubInfo,
            SubscriberCallbackNative callback,
            IntPtr /*void* */ arg);

    }
}
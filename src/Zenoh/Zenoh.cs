using System;
using System.Runtime.InteropServices;


namespace Zenoh
{
    public class Zenoh
    {

        [DllImport("zenohc", EntryPoint = "z_init_logger")]
        public static extern void InitLogger();

    }

}
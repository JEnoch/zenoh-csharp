using System;
using System.Diagnostics;
using System.Collections.Generic;

using Zenoh;

class ZNInfo
{

    static void Main(string[] args)
    {
        Zenoh.Zenoh.InitLogger();

        var conf = new Dictionary<string, string>();
        conf.Add("mode", "client");
        conf.Add("peer", "tcp/127.0.0.1:7447");


        Console.WriteLine("Openning session..");
        var s = Zenoh.Net.Session.open(conf);

        var props = s.info();
        foreach (KeyValuePair<string, string> entry in props)
        {
            Console.WriteLine("{0} : {1}", entry.Key, entry.Value);
        }

    }
}

using System;
using System.Diagnostics;
using System.Collections.Generic;
using Zenoh;
using PowerArgs;

class ZNInfo
{
    static void Main(string[] args)
    {
        try
        {
            // initiate logging
            Zenoh.Zenoh.InitLogger();

            // arguments parsing
            var arguments = Args.Parse<ExampleArgs>(args);
            if (arguments == null) return;
            Dictionary<string, string> conf = arguments.GetConf();

            Console.WriteLine("Openning session..");
            var s = Zenoh.Net.Session.open(conf);

            var props = s.info();
            foreach (KeyValuePair<string, string> entry in props)
            {
                Console.WriteLine("{0} : {1}", entry.Key, entry.Value);
            }
        }
        catch (ArgException)
        {
            Console.WriteLine(ArgUsage.GenerateUsageFromTemplate<ExampleArgs>());
        }
    }
}


[ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
public class ExampleArgs
{
    [HelpHook, ArgShortcut("h"), ArgDescription("Shows this help")]
    public Boolean help { get; set; }

    [ArgShortcut("m"), ArgDefaultValue("peer"), ArgDescription("The zenoh session mode. Possible values [peer|client].")]
    public string mode { get; set; }

    [ArgShortcut("e"), ArgDescription("Peer locators used to initiate the zenoh session.")]
    public string peer { get; set; }

    [ArgShortcut("l"), ArgDescription("Locators to listen on.")]
    public string listener { get; set; }

    [ArgShortcut("c"), ArgDescription("A configuration file.")]
    public string config { get; set; }

    public Dictionary<string, string> GetConf()
    {
        var conf = new Dictionary<string, string>();
        conf.Add("mode", this.mode);
        if (this.peer != null)
        {
            conf.Add("peer", this.peer);
        }
        if (this.listener != null)
        {
            conf.Add("listener", this.listener);
        }
        if (this.config != null)
        {
            conf.Add("config", this.config);
        }
        return conf;
    }
}

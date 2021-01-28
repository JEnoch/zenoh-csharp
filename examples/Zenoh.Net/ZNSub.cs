using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Zenoh;
using PowerArgs;

class ZNSub
{

    public static void SubscriberCallback(Zenoh.Net.Sample sample)
    {
        Console.WriteLine(">> [Subscription listener] Received ('{0}': '{1}')",
            sample.ResName,
            Encoding.UTF8.GetString(sample.Payload));
    }

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

            Console.WriteLine("Openning session...");
            var s = Zenoh.Net.Session.Open(conf);

            Console.WriteLine("Declaring Subscriber on '{0}'...", arguments.selector);
            var rkey = Zenoh.Net.ResKey.RName(arguments.selector);
            var subInfo = new Zenoh.Net.SubInfo();
            s.DeclareSubscriber(rkey, subInfo, ZNSub.SubscriberCallback);

            char c = ' ';
            while (c != 'q')
            {
                c = Console.ReadKey().KeyChar;
            }

            s.Dispose();
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

    [ArgShortcut("s"), ArgDefaultValue("/demo/example/**"), ArgDescription("The selection of resources to subscribe.")]
    public string selector { get; set; }

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

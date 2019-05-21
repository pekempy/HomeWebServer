using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using HomeWebServer.BulbControl;
using HomeWebServer.Converter;

namespace HomeWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            TcpListener server = new TcpListener(IPAddress.Parse("0.0.0.0"), 5134);
            server.Start();
            Console.WriteLine("Server started...");
            Console.WriteLine("Listening on port 5134...");
            while (true)
            {
                ClientWorking cw = new ClientWorking(server.AcceptTcpClient());
                new Thread(new ThreadStart(cw.DoSomethingWithClient)).Start();
            }
        }
    }
    class ClientWorking
    {
        public SmartBulb sb = new SmartBulb();
        public BulbColourConverter colourConverter = new BulbColourConverter();
        private NetworkStream ClientStream;
        private TcpClient Client;
        public string part0;
        public string part1;
        public string part2;
        public string bulbIP = "192.168.0.21";
        public double hue;
        public double sat;
        public double val;

        public ClientWorking(TcpClient Client)
        {

            this.Client = Client;
            ClientStream = Client.GetStream();
        }

        public void DoSomethingWithClient()
        {
            StreamWriter sw = new StreamWriter(ClientStream);
            StreamReader sr = new StreamReader(sw.BaseStream);
            sw.WriteLine("Connected to Home Web Server");
            sw.Flush();
            string data;
            try
            {
                //sw.writeline("____") will output to the client
                //console.writelin("___") will output to the server
                //data holds whatever is sent FROM the client
                //if you write "this is input" (case insensitive)
                //you SHOULD get the server say "this is output"
                //this means we can do 
                //if (data.ToLower() == "command3 param1 param2") {
                //    split data into 3 strings
                //    string 1 is method to run
                //    string 2 is parameter 1 for method
                //    string 3 is parameter 2 for method
                //    }

                while ((data = sr.ReadLine()) != "exit")
                {

                    try
                    {
                        if (data.ToLower() == "this is input")
                        {
                            Console.WriteLine("This is output.");
                            //The below would write output on the client side.
                            sw.Flush();
                            sw.Close(); //close stream
                            break; //break out
                        }

                        //"mirror_weather"
                        //"mirror_notification"
                        //"mirror_message_Hello World"
                        if (data.ToLower().StartsWith("mirror"))
                        {
                            string[] split = data.Split('_');
                            part0 = split[0];
                            part1 = split[1];
                            try
                            { //do this if part2 may sometimes be empty
                                part2 = split[2];
                            }
                            catch { part2 = "empty"; }
                            MirrorUpdate(part1, part2);
                            sw.Close();
                            break;
                        }

                        //"bulb_on"
                        //"bulb_off"
                        //"bulb_colourchange_red"
                        if (data.ToLower().StartsWith("bulb"))
                        {
                            string[] split = data.Split('_');
                            part0 = split[0];
                            part1 = split[1];
                            try
                            { //if a param will not always be filled do this
                                part2 = split[2];
                            }
                            catch { part2 = "empty"; }
                            LightbulbChange(part1, part2);
                            sw.Close();
                            break;
                        }
                    }
                    catch (Exception e) { Console.WriteLine("Error: " + e); }
                }
            }
            finally
            {
                try
                {

                    sw.Close();
                }
                catch { }
            }
        }

        public void MirrorUpdate(string type, string extra)
        {
            switch (type)
            {
                case "weather":
                    Console.WriteLine("Mirror - weather?");
                    break;
                case "notification":
                    Console.WriteLine("Mirror - notification?");
                    break;
                case "message":
                    Console.WriteLine("Mirror - message?");
                    break;
            }
        }

        //Example method with case switch
        public void LightbulbChange(string type, string colour)
        {
            colour = "#3369e8";
            switch (type)
            {
                case "on":
                    sb.Bulb_On(bulbIP);
                    Console.WriteLine("Sent command to turn on bulb: " + bulbIP);
                    break;
                case "off":
                    sb.Bulb_Off(bulbIP);
                    Console.WriteLine("Sent command to turn off bulb: " + bulbIP);
                    break;
                case "colourchange":
                    colourConverter.Convert(colour);
                    hue = colourConverter.hue;
                    sat = colourConverter.sat;
                    val = colourConverter.val;
                    sb.Bulb_Colour(bulbIP, hue, sat, val);
                    Console.WriteLine("Set colour to: " + colour);
                    break;
            }
        }
    }
}

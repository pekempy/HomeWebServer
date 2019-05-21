using System;
using EdgeJs;
using System.Threading.Tasks;

namespace HomeWebServer.BulbControl
{
    class TPLink
    {
        public void Node()
        {
            NodeFunc().Wait();
        }
        public static async Task NodeFunc()
        {

            var func = Edge.Func(@"
                return function(data, callback){
                    callback(null, data);
                    tplight on 192.168.0.21;
                }
            ");
            Console.WriteLine(await func("tplight on 192.168.0.21"));

        }

        public async Task Bulb()
        {
        }

        public void ChangeColour()
        {
        }
    }
}

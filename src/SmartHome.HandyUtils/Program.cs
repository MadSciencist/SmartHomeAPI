using System;
using System.Threading.Tasks;
using SmartHome.DeviceController.Rest;

namespace SmartHome.HandyUtils
{
    class Program
    {
        static void Main(string[] args)
        {

            new Program().Run().Wait();
            Console.WriteLine("Hello World!");
        }

        public async Task Run()
        {
            var client = new PersistentHttpClient();
            string response = await client.GetAsync(@"http://192.168.0.100/api/relay/0?apikey=D8F3A6CC12FE9CC9");

            Console.ReadLine();
        }
    }
}

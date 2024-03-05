using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Valutavalto
{
    internal class Program
    {
        static Valutavaltas valto = null;
        static async Task Main(string[] args)
        {
            Console.WriteLine("Mennyit szeretnél átváltani?");
            float osszeg =float.Parse(Console.ReadLine());
            await Valto();
            var euro = osszeg / valto.Rates["HUF"];
            var dollar = (osszeg / valto.Rates["HUF"]) * valto.Rates["USD"];
            Console.WriteLine($"{osszeg}ft={euro}{valto.Base}");
            Console.WriteLine($"{osszeg}ft={dollar}USD");
            Console.ReadLine();
        }

        private static async Task Valto()
        {
            string url = $"https://infojegyzet.hu/webszerkesztes/php/valuta/api/v1/arfolyam/";
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Hiba a lekérdezés során");
            }
            //response.EnsureSuccessStatusCode();
            string jsonString = await response.Content.ReadAsStringAsync();
            valto = Valutavaltas.FromJson(jsonString);
        }
    }
}

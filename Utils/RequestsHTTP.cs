using System.Net.Http;
using System.Threading.Tasks;

namespace ExtraService.Utils
{
    public class RequestsHTTP
    {
        public static async Task GET(string path)
        {
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(path)){}
        }
    }
}
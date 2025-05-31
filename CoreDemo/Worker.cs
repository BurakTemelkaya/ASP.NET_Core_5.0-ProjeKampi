using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;

namespace CoreDemo;

public class Worker : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                string[] urls = new[] { "https://buraktemelkaya.com", "https://drive.buraktemelkaya.com/" };

                using HttpClient client = new();
                for (int i = 0; i < urls.Length; i++)
                {
                    using HttpResponseMessage response = await client.GetAsync(urls[i], cancellationToken);
                    using HttpContent content = response.Content;
                    string result = await content.ReadAsStringAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            await Task.Delay(TimeSpan.FromMinutes(5), cancellationToken);
        }
    }
}
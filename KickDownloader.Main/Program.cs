using KickDownloader.Http.Service;
using PuppeteerSharp;

namespace KickDownloader.Main
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            PuppeteerHttpService service = await PuppeteerHttpService.CreateAsync(new LaunchOptions { Headless = true });
            var data = await service.ProcessRequestAsync("https://kick.com/api/v1/video/1ae78f7b-5d3b-42da-99f5-05d47a35a023");
            Console.WriteLine(data);
        }
    }
}
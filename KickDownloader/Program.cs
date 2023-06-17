using Microsoft.Playwright;

namespace KickDownloader
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });
            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://kick.com/api/v1/video/1ae78f7b-5d3b-42da-99f5-05d47a35a023");
            Console.WriteLine(await page.ContentAsync());
        }
    }
}
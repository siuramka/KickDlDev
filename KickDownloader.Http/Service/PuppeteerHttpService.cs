using PuppeteerExtraSharp;
using PuppeteerExtraSharp.Plugins.ExtraStealth;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KickDownloader.Http.Service
{
    public class PuppeteerHttpService
    {
        private LaunchOptions _launchOptions;
        private PuppeteerExtra _puppeteerExtra;
        private PuppeteerHttpService(LaunchOptions launchOptions)
        {
            _launchOptions = launchOptions;
            _puppeteerExtra = new PuppeteerExtra();
            this.ConfigPuppeteer();
        }
        private void ConfigPuppeteer()
        {
            _puppeteerExtra.Use(new StealthPlugin());
        }
        public static async Task<PuppeteerHttpService> CreateAsync(LaunchOptions launchOptions) 
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            return new PuppeteerHttpService(launchOptions);
        }
        public async Task<string> ProcessRequestAsync(string url)
        {
            // Launch the puppeteer browser with plugins
            using (IBrowser browser = await _puppeteerExtra.LaunchAsync(_launchOptions))
            {
                var page = await browser.NewPageAsync();
                var navigation = new NavigationOptions
                {
                    WaitUntil = new[] {
                    WaitUntilNavigation.DOMContentLoaded }
                };
                await page.GoToAsync(url, navigation);
                var content = await page.GetContentAsync();

                await page.DisposeAsync();
                await browser.DisposeAsync();
                return content;
            }
        }
    }
}

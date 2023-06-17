using Newtonsoft.Json;
using PuppeteerExtraSharp;
using PuppeteerExtraSharp.Plugins.ExtraStealth;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
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
            var browserFetcher = await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            return new PuppeteerHttpService(launchOptions);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Since going to use only get requests, use simple url param.</param>
        /// <returns></returns>
        public async Task<T> ProcessGetAsync<T>(string url)
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
                
                string content = await page.EvaluateExpressionAsync<string>("document.body.innerText"); //get inner text/text as puppeteer serializes html content

                await page.DisposeAsync();
                await browser.DisposeAsync();
                var jsonObject = JsonConvert.DeserializeObject<T>(content);

                return jsonObject;
            }
        }
    }
}

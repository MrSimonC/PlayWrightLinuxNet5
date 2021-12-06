using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace PlayWrightLinuxNet5
{
    public class Function1
    {
        [Function("Function1")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var log = executionContext.GetLogger("Function1");
            log.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);

            log.LogInformation("Sac starting playwright");

            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://playwright.dev/dotnet");
            var screenshot = await page.ScreenshotAsync();

            log.LogInformation("Sac playwright end");

            response.Headers.Add("Content-Type", "image/png");
            response.WriteBytes(await page.ScreenshotAsync());
            return response;
        }
    }
}

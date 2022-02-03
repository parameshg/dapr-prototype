using Dapr.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Frontend.Controllers
{
    [Route("")]
#if DEBUG
    [Authorize]
#endif
    [ApiController]
    public class HomeController : ControllerBase
    {
        private const string CONFIGURATION = "map";

        private const string SECRET = "vault";

        private DaprClient Dapr { get; set; }

        public HomeController()
        {
            var config = new JsonSerializerOptions(JsonSerializerDefaults.Web);

            config.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;

            Dapr = new DaprClientBuilder()
#if DEBUG
                   .UseHttpEndpoint("http://localhost:4001")
                   .UseGrpcEndpoint("http://localhost:4002")
#endif
                   .UseJsonSerializationOptions(config)
                   .Build();
        }

        [HttpGet("")]
        [AllowAnonymous]
        public async Task<bool> Index()
        {
            return await Dapr.CheckHealthAsync();
        }

        [HttpGet("/config/{key}")]
        public async Task<string> GetConfiguration(string key, [FromQuery] bool secret)
        {
            var result = string.Empty;

            if (secret)
            {
                result = (await Dapr.GetSecretAsync(SECRET, key))?.FirstOrDefault().Value;
            }
            else
            {
                //TODO: This is obsolete and not stable. Use gRPC client to call.

                var configuration = await Dapr.GetConfiguration(CONFIGURATION, new List<string>() { key });

                if (configuration != null && configuration.Items != null && configuration.Items.Count.Equals(1))
                {
                    result = configuration.Items[0].Value;
                }
            }

            return result;
        }

        [HttpPost("/config/{key}")]
        public Task<bool> SetConfiguration(string key, [FromBody] string value, [FromQuery] bool secret)
        {
            var result = false;

            if (secret)
            {
                //TODO: This is not yet available
            }
            else
            {
                //TODO: This is not yet available

                //var configuration = await Dapr.SetConfiguration(CONFIGURATION, new List<string>() { key });

                //if (configuration != null && configuration.Items != null && configuration.Items.Count.Equals(1))
                //{
                //    result = configuration.Items[0].Value;
                //}
            }

            return Task.FromResult(result);
        }

        [HttpGet("/series")]
        public async Task<dynamic> GetSeries([FromQuery] int count = 10)
        {
            dynamic result = new { count = 0, sum = 0, mean = 0, variance = 0, deviation = 0 };

            async Task Fibonacci(int firstNumber, int secondNumber, int counter, int length)
            {
                if (counter < length)
                {
                    if (await Dapr.InvokeMethodAsync<int, bool>(HttpMethod.Post, "backend", "input", secondNumber))
                    {
                        await Fibonacci(secondNumber, firstNumber + secondNumber, counter + 1, length);
                    }
                }
            }

            if (await Dapr.InvokeMethodAsync<bool>(HttpMethod.Get, "backend", "reset"))
            {
                if (await Dapr.InvokeMethodAsync<int, bool>(HttpMethod.Post, "backend", "input", 0))
                {
                    await Fibonacci(0, 1, 1, count);

                    result = new
                    {
                        count = await Dapr.InvokeMethodAsync<int>(HttpMethod.Get, "backend", "count"),
                        sum = await Dapr.InvokeMethodAsync<int>(HttpMethod.Get, "backend", "sum"),
                        mean = await Dapr.InvokeMethodAsync<double>(HttpMethod.Get, "backend", "mean"),
                        variance = await Dapr.InvokeMethodAsync<double>(HttpMethod.Get, "backend", "variance"),
                        deviation = await Dapr.InvokeMethodAsync<double>(HttpMethod.Get, "backend", "sd")
                    };
                }
            }

            return result;
        }
    }
}
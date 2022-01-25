using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Frontend.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
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
        public async Task<bool> Index()
        {
            return await Dapr.CheckHealthAsync();
        }

        [HttpGet("/series")]
        public async Task<dynamic> Series([FromQuery] int count = 10)
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
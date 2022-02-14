using Dapr;
using Dapr.Client;
using Backend.Events;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private const string DATABASE = "cache";

        private const string MIDDLEWARE = "middleware";

        private DaprClient Dapr { get; set; }

        public HomeController()
        {
            var config = new JsonSerializerOptions(JsonSerializerDefaults.Web);

            config.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;

            Dapr = new DaprClientBuilder()
#if DEBUG
                   .UseHttpEndpoint("http://localhost:5001")
                   .UseGrpcEndpoint("http://localhost:5002")
#endif
                   .UseJsonSerializationOptions(config)
                   .Build();
        }

        [HttpGet("")]
        public async Task<dynamic> Index()
        {
            return new
            {
                name = "Backend",
                timestamp = DateTime.Now.ToString(),
                health = await Dapr.CheckHealthAsync(),
            };
        }

        [HttpGet("/random")]
        public async Task<int> Random()
        {
            var result = -1;

            var data = await Dapr.GetStateAsync<Dataset>(DATABASE, "data");

            if (data == null)
            {
                data = new Dataset();
            }

            var number = new Random(DateTime.Now.Millisecond).Next(0, 100);

            data.Add(number);

            await Dapr.SaveStateAsync(DATABASE, "data", data);

            await Publish();

            result = number;

            return result;
        }

        [HttpGet("/sum")]
        public async Task<int> Sum()
        {
            var result = -1;

            var data = await Dapr.GetStateAsync<Dataset>(DATABASE, "data");

            if (data != null)
            {
                result = data.Sum;
            }

            return result;
        }

        [HttpGet("/count")]
        public async Task<int> Count()
        {
            var result = -1;

            var data = await Dapr.GetStateAsync<Dataset>(DATABASE, "data");

            if (data != null)
            {
                result = data.Count;
            }

            return result;
        }

        [HttpPost("/input")]
        public async Task<bool> Save([FromBody] int number)
        {
            var result = false;

            var data = await Dapr.GetStateAsync<Dataset>(DATABASE, "data");

            if (data != null)
            {
                data.Add(number);

                await Dapr.SaveStateAsync(DATABASE, "data", data);

                await Publish();

                result = true;
            }

            return result;
        }

        [HttpGet("/mean")]
        public async Task<double> Mean()
        {
            double result = -1;

            var data = await Dapr.GetStateAsync<Dataset>(DATABASE, "data");

            if (data != null)
            {
                result = data.Mean;
            }

            return result;
        }

        [HttpGet("/variance")]
        public async Task<double> Variance()
        {
            double result = -1;

            var data = await Dapr.GetStateAsync<Dataset>(DATABASE, "data");

            if (data != null)
            {
                result = data.Variance;
            }

            return result;
        }

        [HttpGet("/sd")]
        public async Task<double> StandardDeviation()
        {
            double result = -1;

            var data = await Dapr.GetStateAsync<Dataset>(DATABASE, "data");

            if (data != null)
            {
                result = data.StandardDeviation;
            }

            return result;
        }

        [HttpGet("/reset")]
        public async Task<bool> Reset()
        {
            var result = false;

            var stat = await Dapr.GetStateAsync<Dataset>(DATABASE, "data");

            if (stat == null)
                stat = new Dataset();

            stat.Clear();

            await Dapr.SaveStateAsync(DATABASE, "data", stat);

            await Publish();

            result = true;

            return result;
        }

        [HttpGet("/publish")]
        public async Task Publish()
        {
            var data = await Dapr.GetStateAsync<Dataset>(DATABASE, "data");

            if (data != null)
            {
                await Dapr.PublishEventAsync(MIDDLEWARE, "statistics", new StatisticsUpdatedEvent
                {
                    Mean = data.Mean,
                    Variance = data.Variance,
                    StandardDeviation = data.StandardDeviation
                });
            }
        }

        [HttpPost("/subscribe")]
        [Topic(MIDDLEWARE, "statistics")]
        public void Subscribe([FromBody] StatisticsUpdatedEvent @event)
        {
            Console.WriteLine(JsonConvert.SerializeObject(@event));
        }
    }
}
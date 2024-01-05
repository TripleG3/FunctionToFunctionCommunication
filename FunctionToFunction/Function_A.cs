using FunctionToFunctionShared;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FunctionToFunction
{
    public static class Function_Starter1
    {
        [FunctionName(nameof(Function_Starter1))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation(nameof(Function_Starter1));

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var functionInfo = JsonConvert.DeserializeObject<FunctionInfo>(requestBody);

            if (string.IsNullOrWhiteSpace(functionInfo.Name))
            {
                functionInfo.Name += $"{nameof(Function_Starter1)}";
            }
            else
            {
                functionInfo.Name += $", {nameof(Function_Starter1)}";
            }

            functionInfo.Counter++;

            HttpClient client = new HttpClient();

            functionInfo = await CallRestAsync(functionInfo, client, "http://localhost:7071/api/Function_A");
            functionInfo = await CallRestAsync(functionInfo, client, "http://localhost:7071/api/Function_B");
            functionInfo = await CallRestAsync(functionInfo, client, "http://localhost:7071/api/Function_C");
            functionInfo = await CallRestAsync(functionInfo, client, "http://localhost:7071/api/Function_D");
            functionInfo = await CallRestAsync(functionInfo, client, "http://localhost:7071/api/Function_E");
            functionInfo = await CallRestAsync(functionInfo, client, "http://localhost:7071/api/Function_F");
            functionInfo = await CallRestAsync(functionInfo, client, "http://localhost:7071/api/Function_G");

            return new OkObjectResult(functionInfo);
        }

        private static async Task<FunctionInfo> CallRestAsync(FunctionInfo functionInfo, HttpClient newClient, string url)
        {
            var json = JsonConvert.SerializeObject(functionInfo);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            var response = await newClient.SendAsync(request);
            functionInfo = await response.Content.ReadAsAsync<FunctionInfo>();
            return functionInfo;
        }
    }

    public static class Function_A
    {
        [FunctionName(nameof(Function_A))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation(nameof(Function_A));

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var functionInfo = JsonConvert.DeserializeObject<FunctionInfo>(requestBody);

            if (string.IsNullOrWhiteSpace(functionInfo.Name))
            {
                functionInfo.Name += $"{nameof(Function_A)}";
            }
            else
            {
                functionInfo.Name += $", {nameof(Function_A)}";
            }

            functionInfo.Counter++;

            return new OkObjectResult(functionInfo);
        }
    }

    public static class Function_B
    {
        [FunctionName(nameof(Function_B))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation(nameof(Function_B));

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var functionInfo = JsonConvert.DeserializeObject<FunctionInfo>(requestBody);

            if (string.IsNullOrWhiteSpace(functionInfo.Name))
            {
                functionInfo.Name += $"{nameof(Function_B)}";
            }
            else
            {
                functionInfo.Name += $", {nameof(Function_B)}";
            }

            functionInfo.Counter++;

            return new OkObjectResult(functionInfo);
        }
    }

    public static class Function_C
    {
        [FunctionName(nameof(Function_C))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation(nameof(Function_C));

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var functionInfo = JsonConvert.DeserializeObject<FunctionInfo>(requestBody);

            if (string.IsNullOrWhiteSpace(functionInfo.Name))
            {
                functionInfo.Name += $"{nameof(Function_C)}";
            }
            else
            {
                functionInfo.Name += $", {nameof(Function_C)}";
            }

            functionInfo.Counter++;

            return new OkObjectResult(functionInfo);
        }
    }

    public static class Function_D
    {
        [FunctionName(nameof(Function_D))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation(nameof(Function_D));

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var functionInfo = JsonConvert.DeserializeObject<FunctionInfo>(requestBody);

            if (string.IsNullOrWhiteSpace(functionInfo.Name))
            {
                functionInfo.Name += $"{nameof(Function_D)}";
            }
            else
            {
                functionInfo.Name += $", {nameof(Function_D)}";
            }

            functionInfo.Counter++;

            return new OkObjectResult(functionInfo);
        }
    }

    public static class Function_E
    {
        [FunctionName(nameof(Function_E))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation(nameof(Function_E));

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var functionInfo = JsonConvert.DeserializeObject<FunctionInfo>(requestBody);

            if (string.IsNullOrWhiteSpace(functionInfo.Name))
            {
                functionInfo.Name += $"{nameof(Function_E)}";
            }
            else
            {
                functionInfo.Name += $", {nameof(Function_E)}";
            }

            functionInfo.Counter++;

            return new OkObjectResult(functionInfo);
        }
    }

    public static class Function_F
    {
        [FunctionName(nameof(Function_F))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation(nameof(Function_F));

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var functionInfo = JsonConvert.DeserializeObject<FunctionInfo>(requestBody);

            if (string.IsNullOrWhiteSpace(functionInfo.Name))
            {
                functionInfo.Name += $"{nameof(Function_F)}";
            }
            else
            {
                functionInfo.Name += $", {nameof(Function_F)}";
            }

            functionInfo.Counter++;

            return new OkObjectResult(functionInfo);
        }
    }

    public static class Function_G
    {
        [FunctionName(nameof(Function_G))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation(nameof(Function_G));

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var functionInfo = JsonConvert.DeserializeObject<FunctionInfo>(requestBody);

            if (string.IsNullOrWhiteSpace(functionInfo.Name))
            {
                functionInfo.Name += $"{nameof(Function_G)}";
            }
            else
            {
                functionInfo.Name += $", {nameof(Function_G)}";
            }

            functionInfo.Counter++;

            return new OkObjectResult(functionInfo);
        }
    }
}

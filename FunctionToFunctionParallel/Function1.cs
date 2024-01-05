using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FunctionToFunctionParallel
{
    public static class Function1
    {
        private const string StarterFunction = "Function1_HttpStart";
        private const string StarterFunctionParallel = "Function1_HttpStartParallel";
        private const string OrchestratorFunction = "Function1";
        private const string OrchestratorFunctionParallel = "Function2";
        private const string SayHelloFunction = "Function1_Hello";
        private const string ReverseNameFunction = "Function_ReverseName";

        [FunctionName(StarterFunction)]
        public static async Task<IActionResult> HttpStart([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Start")] HttpRequest req,
                                                          [DurableClient] IDurableOrchestrationClient starter,
                                                          ILogger log)
        {
            var startTime = DateTime.Now;
            var maxTime = DateTime.Now.AddSeconds(5);

            string instanceId = await starter.StartNewAsync(OrchestratorFunction, null);

            DurableOrchestrationStatus durableOrchestrationStatus;

            do
            {
                durableOrchestrationStatus = await starter.GetStatusAsync(instanceId);
                await Task.Yield();
            }
            while (!durableOrchestrationStatus.Output.HasValues
                 && maxTime >= DateTime.Now);

            return durableOrchestrationStatus.Output.HasValues
                ? new OkObjectResult(new TimeResult(startTime, DateTime.Now, durableOrchestrationStatus.Output))
                : new OkObjectResult(starter.CreateCheckStatusResponse(req, instanceId));
        }

        [FunctionName(StarterFunctionParallel)]
        public static async Task<IActionResult> HttpStartParallel([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Start/Parallel")] HttpRequestMessage req,
                                                  [DurableClient] IDurableOrchestrationClient starter,
                                                  ILogger log)
        {
            var startTime = DateTime.Now;
            var maxTime = DateTime.Now.AddSeconds(5);
            var instanceId = await starter.StartNewAsync(OrchestratorFunctionParallel, null);

            DurableOrchestrationStatus durableOrchestrationStatus;

            do
            {
                durableOrchestrationStatus = await starter.GetStatusAsync(instanceId);
                await Task.Yield();
            }
            while (!durableOrchestrationStatus.Output.HasValues
                 && maxTime >= DateTime.Now);

            return durableOrchestrationStatus.Output.HasValues
                ? new OkObjectResult(new TimeResult(startTime, DateTime.Now, durableOrchestrationStatus.Output))
                : new OkObjectResult(starter.CreateCheckStatusResponse(req, instanceId));
        }

        [FunctionName(OrchestratorFunction)]
        public static async Task<List<string>> RunOrchestrator([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var names = new List<string> { "Mathew", "Mark", "Luke", "John", "Peter" };
            var outputs = new List<string>();
            foreach (var name in names)
            {
                var reverseName = await context.CallActivityAsync<string>(ReverseNameFunction, name);
                var result = await context.CallActivityAsync<string>(SayHelloFunction, reverseName);
                outputs.Add(result);
            }

            return outputs;
        }

        [FunctionName(OrchestratorFunctionParallel)]
        public static async Task<List<string>> RunOrchestratorParallel([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var names = new List<string> { "Mathew", "Mark", "Luke", "John", "Peter" };

            var reverseTasks = new List<Task<string>>();
            foreach (var name in names)
            {
                reverseTasks.Add(context.CallActivityAsync<string>(ReverseNameFunction, name));
            }
            await Task.WhenAll(reverseTasks);

            var sayHelloTasks = new List<Task<string>>();
            foreach (var reverseName in reverseTasks.Select(x => x.Result))
            {
                sayHelloTasks.Add(context.CallActivityAsync<string>(SayHelloFunction, reverseName));
            }
            await Task.WhenAll(sayHelloTasks);

            var results = sayHelloTasks.Select(x => x.Result).ToList();
            return results;
        }

        [FunctionName(SayHelloFunction)]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            return $"Hello {name}!";
        }

        [FunctionName(ReverseNameFunction)]
        public static async Task<string> ReverseName([ActivityTrigger] string name, ILogger log)
        {
            await Task.Delay(500);
            return new string(name.Reverse().ToArray());
        }

        public class TimeResult
        {
            public TimeResult(DateTime startTime, DateTime endTime, object value)
            {
                StartTime = startTime;
                EndTime = endTime;
                Value = value;
            }

            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public double Difference => (EndTime - StartTime).TotalMilliseconds;
            public object Value { get; set; }
        }
    }
}
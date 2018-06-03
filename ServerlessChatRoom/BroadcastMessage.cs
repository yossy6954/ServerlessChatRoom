
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;

namespace ServerlessChatRoom {
    public static class BroadcastMessage {
        private const string HubName = "chat";

        [FunctionName("BroadcastMessage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req,
            TraceWriter log) {
            var signalR = new AzureSignalR(Environment.GetEnvironmentVariable("AzureSignalRConnectionString"));
            string name = req.Query["name"];
            string message = req.Query["message"];

            // broadcast through SignalR
            await signalR.SendAsync(HubName, "broadcastMessage", name, message);

            return new OkObjectResult($"OK");
        }
    }
}

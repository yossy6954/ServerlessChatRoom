
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net;

namespace ServerlessChatRoom {
    public static class Join {
        private const string HubName = "chat";

        [FunctionName("Join")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req,
            TraceWriter log) {
            try {
                var signalR = new AzureSignalR(Environment.GetEnvironmentVariable("AzureSignalRConnectionString"));
                return new OkObjectResult(new {
                    authInfo = new {
                        serviceUrl = signalR.GetClientHubUrl(HubName),
                        accessToken = signalR.GenerateAccessToken(HubName)
                    }
                }
                );
            } catch(Exception ex) {
                log.Error(ex.Message);
                log.Error(ex.StackTrace);
                throw;
            }
        }
    }
}

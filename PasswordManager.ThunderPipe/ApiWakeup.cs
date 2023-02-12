using System;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PasswordManager.ThunderPipe
{
    public class ApiWakeup
    {
        [FunctionName("ApiWakeup")]
        public void Run([TimerTrigger("0 */30 * * * *")]TimerInfo myTimer, ILogger log, IConfiguration configuration)
        {
            log.LogInformation($"Wakeup function executed at: {DateTime.UtcNow.ToString("S")}");

            try
            {
                var client = HttpClientFactory.Create();

                var request = new HttpRequestMessage(HttpMethod.Get, "heartbeat");
                request.Headers.Add("x-api-key", configuration.GetValue<string>("secret_key"));

                client.BaseAddress = new Uri(configuration.GetValue<string>("api_client"));

                var response = client.SendAsync(request).Result;

                if (!response.IsSuccessStatusCode)
                    Run(myTimer, log, configuration);
            }
            catch (Exception ex)
            {
                log.LogWarning("Exception was thrown", ex);
            }
        }
    }
}

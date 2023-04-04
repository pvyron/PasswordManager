using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace PasswordManager.ThunderPipe;

public class ApiWakeup
{
    private readonly ILogger _logger;

    public ApiWakeup(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ApiWakeup>();
    }

    [Function(nameof(ApiWakeup))]
    public void Run([TimerTrigger("0/5 * * * * *")] MyInfo myTimer, ILogger log, IConfiguration configuration)
    {
        //log.LogInformation($"Wakeup function executed at: {DateTime.UtcNow:S}");

        //try
        //{
        //    var client = new HttpClient();

        //    var request = new HttpRequestMessage(HttpMethod.Get, "heartbeat");
        //    request.Headers.Add("x-api-key", configuration.GetValue<string>("secret_key"));

        //    client.BaseAddress = new Uri(configuration.GetValue<string>("api_client"));

        //    var response = client.SendAsync(request).Result;

        //    if (!response.IsSuccessStatusCode)
        //        Run(myTimer, log, configuration);
        //}
        //catch (Exception ex)
        //{
        //    log.LogWarning("Exception was thrown", ex);
        //}
    }
}

public class MyInfo
{
    public MyScheduleStatus ScheduleStatus { get; set; }

    public bool IsPastDue { get; set; }
}

public class MyScheduleStatus
{
    public DateTime Last { get; set; }

    public DateTime Next { get; set; }

    public DateTime LastUpdated { get; set; }
}
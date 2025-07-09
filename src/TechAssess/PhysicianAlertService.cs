using Newtonsoft.Json.Linq;
using System.Text;

namespace TechAssess.src;

/// <summary>
/// Handles alerting the physician via API.
/// </summary>
public static class PhysicianAlertService
{
    /// <summary>
    /// Sends a DME order to the physician's API endpoint as a JSON payload.
    /// </summary>
    /// <param name="orderJson">
    /// A <see cref="JObject"/> of the DME order to be sent in the request body.
    /// </param>
    public static void SendOrder(JObject orderJson)
    {
        using var httpClient = new HttpClient();
        var apiUrl = "https://alert-api.com/DrExtract";
        var jsonContent = new StringContent(orderJson.ToString(), Encoding.UTF8, "application/json");
        try
        {
            Console.WriteLine($"Sending Order\n\n{orderJson.ToString()}\n\nto api endpoint at {apiUrl}");
            var response = httpClient.PostAsync(apiUrl, jsonContent).GetAwaiter().GetResult();
        }
        catch (Exception)
        {
            Console.Error.WriteLine("The API endpoint does not exist, which is expected for this example code");
        }
    }
}
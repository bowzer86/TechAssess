using OllamaSharp;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace TechAssess;

/// <summary>
/// Parses DME order details from a physician note.
/// </summary>
public static class DmeOrderParser
{
    /// <summary>
    /// Parses a physician note to extract DME  order details.
    /// </summary>
    /// <param name="note">
    /// The full text of the physician note to be parsed for DME order information.
    /// </param>
    /// <returns>
    /// A <see cref="DmeOrder"/> object populated with extracted details.
    /// </returns>
    public static DmeOrder Parse(string note)
    {
        // Synchronous wrapper for async call (for demo purposes)
        Console.WriteLine($"Parsing following note into a DME Order:\n\n{note}");
        string? useAIConfigString = AppConfiguration.AppSettings["AppSettings:UseAI"];
        bool useAI = bool.TryParse(useAIConfigString, out bool result) ? result : true;

        if(!useAI)
        {
            Console.WriteLine("App has been configured to manually parse note");
            return ParseManually(note);
        }

        try
        {
            var aiResult = ParseWithAI(note).GetAwaiter().GetResult();
            return aiResult;
        } catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while parsing the note using an AI assistant: {ex.Message}\n\nResorting back to manual parsing");
            return ParseManually(note);
        }
    }

    /// <summary>
    /// Method to use Ollama to parse a physician note for DME order using AI.
    /// <param name="note">
    /// The full text of the physician note to be parsed for DME order information.
    /// </param>
    /// <returns>
    /// A <see cref="DmeOrder"/> object populated with extracted details.
    /// </returns>
    private static async Task<DmeOrder> ParseWithAI(string note)
    {
        Console.WriteLine($"Parsing note into a DME Order using AI assistant");
        // set up the client
        var uri = new Uri("http://localhost:11434");
        var ollama = new OllamaApiClient(uri);
        ollama.SelectedModel = "llama3.1:8b";

        var prompt = $@"
Extract the following fields from the physician note:
- DeviceType
- MaskType
- AddOns
- Qualifier
- OrderingProvider
- OxygenLiters
- OxygenUsage

For DeviceType, if it mentions 'oxygen', you must return 'Oxygen Tank', if it mentions CPAP, you must return 'CPAP', if it mentions 'wheelchair', you must return 'Wheelchair'
For OxygenLiters, you must include the L in the value if it is mentioned (include the space between the number and the L), but not the per minute, if it is not mentioned, return null
For AddOns, examples are humidifier, if the item is an empty string, return null
For OxygenUsage, if it mentions sleep and exertion, return 'sleep and exertion', if it only mentions sleep, return 'sleep', if it only mentions exertion, return 'exertion', otherwise return null
For MaskType, don't include the word mask, just the type (e.g., full face), if the value would be an empty string, you must return null
For Qualifier, if it mentions a AHI being greater than or less than a number, include AHI with the number and the greater than or less than sign, do not include preposition words like during, if it's not mentioned return empty string
Return the result purely as a JSON object with these fields with no other text, special characters, or formatting before or after the JSON object.

Physician note:
{note}
";

        StringBuilder rawResponseStringBuilder = new();
        await foreach (var stream in ollama.GenerateAsync(prompt))
            rawResponseStringBuilder.Append(stream?.Response);
        string rawResponseString = rawResponseStringBuilder.ToString();

        int start = rawResponseString.IndexOf('{');
        int end = rawResponseString.LastIndexOf('}');

        string cleanedResponse = (start >= 0 && end >= start)
            ? rawResponseString.Substring(start, end - start + 1)
            : string.Empty;

        Console.WriteLine($"Response from AI model:\n\n{cleanedResponse}\n");

        // Parse the JSON returned by the model
        try
        {
            var order = JsonSerializer.Deserialize<DmeOrder>(cleanedResponse.ToString(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return order ?? new DmeOrder();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"The AI assistant was unable to process the doctor's note due to the following error:\n{ex}");
            // Optionally log or handle malformed JSON
            throw;
        }
    }

    /// <summary>
    /// Legacy method to manually parse a physician note for DME order details without using AI.
    /// <param name="note">
    /// The full text of the physician note to be parsed for DME order information.
    /// </param>
    /// <returns>
    /// A <see cref="DmeOrder"/> object populated with extracted details.
    /// </returns>
    public static DmeOrder ParseManually(string note)
    {
        Console.WriteLine($"Manually parsing following note:\n\n{note}\n\nInto a DME Order");
        var order = new DmeOrder();

        if (note.Contains("CPAP", StringComparison.OrdinalIgnoreCase)) order.DeviceType = "CPAP";
        else if (note.Contains("oxygen", StringComparison.OrdinalIgnoreCase)) order.DeviceType = "Oxygen Tank";
        else if (note.Contains("wheelchair", StringComparison.OrdinalIgnoreCase)) order.DeviceType = "Wheelchair";

        order.MaskType = order.DeviceType == "CPAP" && note.Contains("full face", StringComparison.OrdinalIgnoreCase) ? "full face" : null;
        order.AddOns = note.Contains("humidifier", StringComparison.OrdinalIgnoreCase) ? "humidifier" : null;
        order.Qualifier = note.Contains("AHI > 20") ? "AHI > 20" : "";

        int physicianNameIndex = note.IndexOf("Dr.");
        if (physicianNameIndex >= 0)
            order.OrderingProvider = note.Substring(physicianNameIndex).Replace("Ordered by ", "").Trim('.', '\n');

        if (order.DeviceType == "Oxygen Tank")
        {
            Console.WriteLine("Note refers to Oxygen Tank");
            Match literMeasurement = Regex.Match(note, "(\\d+(\\.\\d+)?) ?L", RegexOptions.IgnoreCase);
            if (literMeasurement.Success) order.OxygenLiters = literMeasurement.Groups[1].Value + " L";

            if (note.Contains("sleep", StringComparison.OrdinalIgnoreCase) && note.Contains("exertion", StringComparison.OrdinalIgnoreCase))
                order.OxygenUsage = "sleep and exertion";
            else if (note.Contains("sleep", StringComparison.OrdinalIgnoreCase))
                order.OxygenUsage = "sleep";
            else if (note.Contains("exertion", StringComparison.OrdinalIgnoreCase))
                order.OxygenUsage = "exertion";
        }

        return order;
    }
}
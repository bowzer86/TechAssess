using Newtonsoft.Json.Linq;

namespace TechAssess.src;

/// <summary>
/// Reads physician notes from file or provides a default.
/// </summary>
public static class PhysicianNoteReader
{
    /// <summary>
    /// Attempts to read the contents of a physician note from the specified file path.
    /// </summary>
    /// <param name="filePath">
    /// The path to the physician note file to read.
    /// </param>
    /// <returns>
    /// The contents of the physician note file as a string.
    /// </returns>
    public static string Read(string filePath)
    {
        try
        {
            Console.WriteLine($"Attempting to read physician note from file: {filePath}");
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Could not find physician note: {filePath}.\nProceeding with default physician note");
                return GetDefaultNote();
            }

            var content = File.ReadAllText(filePath);

            // Try to parse as JSON and extract "data" property if present
            try
            {
                var jObj = JObject.Parse(content);
                Console.WriteLine("Physician note is in JSON format");
                var dataProp = jObj["data"];
                return dataProp?.ToString() ?? content;
            }
            catch
            {
                Console.WriteLine("Physician note is in Text format");
                return content;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error reading physician note: {ex.Message}\nProceeding with default physician note");
            return GetDefaultNote();
        }
    }

    private static string GetDefaultNote() =>
        "Patient needs a CPAP with full face mask and humidifier. AHI > 20. Ordered by Dr. Cameron.";
}
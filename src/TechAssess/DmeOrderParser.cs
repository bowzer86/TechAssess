using System.Text.RegularExpressions;

namespace TechAssess.src;

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
        Console.WriteLine($"Parsing following note:\n\n{note}\n\nInto a DME Order");
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
using Newtonsoft.Json.Linq;

namespace TechAssess.src;

/// <summary>
/// Stores DME order details.
/// </summary>
public class DmeOrder
{
    public string DeviceType { get; set; } = "Unknown";
    public string? MaskType { get; set; }
    public string? AddOns { get; set; }
    public string Qualifier { get; set; } = "";
    public string OrderingProvider { get; set; } = "Unknown";
    public string? OxygenLiters { get; set; }
    public string? OxygenUsage { get; set; }

    public JObject ToJson()
    {
        var obj = new JObject
        {
            ["device"] = DeviceType,
            ["mask_type"] = MaskType,
            ["add_ons"] = AddOns != null ? new JArray(AddOns) : null,
            ["qualifier"] = Qualifier,
            ["ordering_provider"] = OrderingProvider
        };
        if (DeviceType == "Oxygen Tank")
        {
            obj["liters"] = OxygenLiters;
            obj["usage"] = OxygenUsage;
        }
        return obj;
    }
}
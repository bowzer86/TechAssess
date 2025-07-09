namespace TechAssess.src;

/// <summary>
/// Main application for DME extraction.
/// Parses DME order details from a physician note, creates a DME order, then sends it to the Doctor via API call.
/// </summary>
class DmeExtrationApp
{
    static int Main(string[] args)
    {
        Console.WriteLine("Starting DME extraction App...");

        string physicianNote = PhysicianNoteReader.Read("data/physician_note.txt");
        var dmeOrder = DmeOrderParser.Parse(physicianNote);
        var orderJson = dmeOrder.ToJson();

        PhysicianAlertService.SendOrder(orderJson);

        return 0;
    }
}

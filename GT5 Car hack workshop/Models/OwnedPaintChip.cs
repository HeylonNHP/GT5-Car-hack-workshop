namespace GT5_Car_hack_workshop.Models
{
    /// <summary>
    /// An owned paint colour together with the number of chips of it the player holds, as read
    /// back from the save's item box.
    /// </summary>
    public sealed record OwnedPaintChip(uint ColourId, int Quantity);
}

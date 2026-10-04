namespace GT5_Car_hack_workshop.Models
{
    /// <summary>
    /// One car the tuning shop can borrow parts from: a row of the catalogue's <c>Cars</c> table.
    /// The picker's items are instances of this, so <see cref="ToString"/> is what a chosen car shows
    /// as, exactly like <see cref="CarBody"/> does for the body-swap boxes.
    /// </summary>
    public sealed class TuningSourceCar
    {
        /// <summary>The catalogue's car id (the <c>Parts.CarId</c> a part is filed under).</summary>
        public int Id { get; init; }

        /// <summary>The car's display name, e.g. <c>Ibiza Cupra R 04</c>.</summary>
        public string Name { get; init; } = "";

        public override string ToString() => Name;
    }
}

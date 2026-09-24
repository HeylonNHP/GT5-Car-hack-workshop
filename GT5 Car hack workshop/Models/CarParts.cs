namespace GT5_Car_hack_workshop.Models
{
    public class CarParts
    {
        public string Name { get; set; }

        public ushort Engine { get; set; }

        public ushort Drivetrain { get; set; }

        public ushort Chassis { get; set; }

        public ushort Transmission { get; set; }

        public ushort Suspension { get; set; }

        public ushort Body { get; set; }

        public ushort Lsd { get; set; }

        public ushort Horn { get; set; }

        // TurbineKit ("Turbo") part id, low 2 bytes at Moff-169/-168.
        public ushort Turbo { get; set; }

        // Muffler ("Exhaust") part id, low 2 bytes at Moff-153/-152.
        public ushort Exhaust { get; set; }

        // Lightweight ("Weight" reduction) part id, low 2 bytes at Moff-189/-188.
        public ushort Weight { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
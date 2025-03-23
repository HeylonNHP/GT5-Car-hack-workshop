namespace GT5_Car_hack_workshop_2.Models
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

        public override string ToString()
        {
            return Name;
        }
    }
}
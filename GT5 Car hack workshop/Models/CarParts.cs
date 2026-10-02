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

        // The remaining part ids, all low 2 bytes of an int32 at the offset shown.
        // Brake, low 2 bytes at Moff-225/-224.
        public ushort Brake { get; set; }

        // Brake controller, low 2 bytes at Moff-221/-220.
        public ushort BrakeController { get; set; }

        // Displacement, low 2 bytes at Moff-181/-180.
        public ushort Displacement { get; set; }

        // Computer (ECU), low 2 bytes at Moff-177/-176.
        public ushort Computer { get; set; }

        // NA tune, low 2 bytes at Moff-173/-172.
        public ushort Natune { get; set; }

        // Flywheel, low 2 bytes at Moff-165/-164.
        public ushort Flywheel { get; set; }

        // Clutch, low 2 bytes at Moff-161/-160.
        public ushort Clutch { get; set; }

        // Propeller shaft, low 2 bytes at Moff-157/-156.
        public ushort PropellerShaft { get; set; }

        // Intercooler, low 2 bytes at Moff-149/-148.
        public ushort Intercooler { get; set; }

        // Supercharger, low 2 bytes at Moff-133/-132.
        public ushort Supercharger { get; set; }

        // Intake manifold, low 2 bytes at Moff-129/-128.
        public ushort IntakeManifold { get; set; }

        // Exhaust manifold, low 2 bytes at Moff-125/-124.
        public ushort ExhaustManifold { get; set; }

        // Catalyst, low 2 bytes at Moff-121/-120.
        public ushort Catalyst { get; set; }

        // Air cleaner, low 2 bytes at Moff-117/-116.
        public ushort AirCleaner { get; set; }

        // NOS, low 2 bytes at Moff-113/-112.
        public ushort Nos { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
using Clio.Utilities;

namespace Cardinal
{
    public enum RepairState
    {
        None,
        Repairing,
        MovingBack
    }

    public class RepairContext
    {
        public RepairContext(AgilSettings settings)
        {
            RepairEnabled = settings.Repair;
            Mender = new Mender();
            MinimumDurability = settings.MinimumDurability;
        }

        public Mender Mender { get; set; }

        public uint ZoneId
        {
            get { return Mender.ZoneId; }
        }

        public Vector3 Location
        {
            get { return State == RepairState.MovingBack ? InitialLocation : Mender.Location; }
        }

        public uint NpcId
        {
            get { return Mender.NpcId; }
        }

        public Vector3 InitialLocation { get; set; }
        public RepairState State { get; set; }
        public bool RepairEnabled { get; set; }
        public float MinimumDurability { get; set; }
        public bool SelfRepair { get; set; }
    }
}
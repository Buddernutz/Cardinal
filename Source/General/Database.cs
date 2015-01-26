using System.Collections.Generic;

namespace Cardinal
{
    public static class Database
    {
        private static List<Mender> menders;
        private static List<StatWeights> statWeights;
        private static Dictionary<uint, SpellShape> shapes;

        public static List<Mender> Menders
        {
            get
            {
                if (menders != null) { return menders; }
                LoadAgilData();
                return menders;
            }
            set { menders = value; }
        }

        public static List<StatWeights> StatWeights
        {
            get
            {
                if (statWeights != null) { return statWeights; }
                LoadAgilData();
                return statWeights;
            }
            set { statWeights = value; }
        }

        public static Dictionary<uint, SpellShape> Shapes
        {
            get
            {
                if (shapes != null) { return shapes; }
                LoadZekkenData();
                return shapes;
            }
            set { shapes = value; }
        }

        public static void LoadAgilData()
        {
            menders = DataLoader.ProtoLoad<List<Mender>>(Directories.MENDERS) ?? new List<Mender>();
            statWeights = DataLoader.ProtoLoad<List<StatWeights>>(Directories.STAT_WEIGHTS) ?? new List<StatWeights>();

            Logger.AgilMessage("Loaded stat weights for {0} classes.", statWeights.Count);
            Logger.AgilMessage("Loaded data for {0} menders.", menders.Count);
        }

        public static void LoadZekkenData()
        {
            shapes = DataLoader.ProtoLoad<Dictionary<uint, SpellShape>>(Directories.SHAPES) ?? new Dictionary<uint, SpellShape>();
            Logger.ZekkenMessage("Loaded shapes for {0} spells.", shapes.Count);
        }
    }
}
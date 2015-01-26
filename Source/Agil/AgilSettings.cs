namespace Cardinal
{
    public class AgilSettings
    {
        public AgilSettings()
        {
            Repair = true;
            OptimizeGear = true;
            MinimumDurability = 60f;
            MinimumItemLevel = 1;
        }

        public bool Repair { get; set; }
        public bool OptimizeGear { get; set; }
        public float MinimumDurability { get; set; }
        public bool OptimizeBySpiritbond { get; set; }
        public byte MinimumItemLevel { get; set; }

        public static AgilSettings Load()
        {
            var settings = DataLoader.YamlLoad<AgilSettings>(Directories.AGIL_SETTINGS);
            if (settings == null)
            {
                settings = new AgilSettings();
                DataLoader.YamlSave(Directories.AGIL_SETTINGS, settings);
                Logger.AgilMessage("Failed to load settings, recreating the file.");
            }

            Logger.AgilMessage("Loaded settings.");
            return settings;
        }
    }
}

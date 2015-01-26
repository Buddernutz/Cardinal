namespace Cardinal
{
    public static class Directories
    {
        public const string DATA_DIR = @"Plugins\Cardinal\Data";

        public const string SHAPES = DATA_DIR + @"\Shapes.bin";
        public const string SHAPES_YAML = DATA_DIR + @"\Shapes.yaml";
        public const string MENDERS = DATA_DIR + @"\Menders.bin";
        public const string MENDERS_YAML = DATA_DIR + @"\Menders.yaml";
        public const string STAT_WEIGHTS = DATA_DIR + @"\StatWeights.bin";
        public const string STAT_WEIGHTS_YAML = DATA_DIR + @"\StatWeights.yaml";

        public const string AGIL_SETTINGS = DATA_DIR + @"\AgilSettings.yaml";
        public const string LISBETH_SETTINGS = DATA_DIR + @"\LisbethSettings.yaml";
        public const string ZEKKEN_SETTINGS = DATA_DIR + @"\ZekkenSettings.yaml";
        public const string CARDINAL_SETTINGS = DATA_DIR + @"\CardinalSettings.yaml";

        public const string SCREENSHOT_DIR = @"Plugins\Cardinal\Screenshots";
        public const string IMAGES_DIR = @"Plugins\Cardinal\Images";
        public const string AGIL_IMAGES = IMAGES_DIR + @"\Agil";
        public const string ZEKKEN_IMAGES = IMAGES_DIR + @"\Zekken";
        public const string LISBETH_IMAGES = IMAGES_DIR + @"\Lisbeth";
    }
}
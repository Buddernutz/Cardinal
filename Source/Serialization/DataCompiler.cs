using System.Collections.Generic;
using System.Linq;

namespace Cardinal
{
    public static class DataCompiler
    {
        public static void CompileStatWeights()
        {
            var weights = DataLoader.YamlLoad<List<StatWeights>>(Directories.STAT_WEIGHTS_YAML);
            if (weights == null) { return; }

            if (!DataLoader.ProtoSave(Directories.STAT_WEIGHTS, weights)) { return; }
            Logger.AgilMessage("Compiled {0} stat weights.", weights.Count);
        }

        public static void MenderDataToYaml()
        {
            DataLoader.YamlSave(Directories.MENDERS_YAML, Database.Menders);
        }

        public static void CompileMenders()
        {
            var menders = DataLoader.YamlLoad<List<Mender>>(Directories.MENDERS_YAML);
            if (menders == null) { return; }

            if (!DataLoader.ProtoSave(Directories.MENDERS, menders)) { return; }
            Logger.AgilMessage("Compiled {0} menders.", menders.Count);
        }

        public static void CompileShapes()
        {
            var shapes = DataLoader.YamlLoad<List<SpellShape>>(Directories.SHAPES_YAML);
            if (shapes == null) { return; }

            foreach (var shape in shapes)
            {
                shape.Flags = shape.Flags.Distinct().ToList();
            }

            var distinctShapes = GetDistinctShapes(shapes).OrderBy(s => s.Id).ToList();
            if (!DataLoader.YamlSave(Directories.SHAPES_YAML, distinctShapes)) { return; }

            var compiledData = new Dictionary<uint, SpellShape>();
            foreach (var shape in distinctShapes) { compiledData[shape.Id] = shape; }

            if (!DataLoader.ProtoSave(Directories.SHAPES, compiledData)) { return; }
            Logger.ZekkenMessage("Compiled {0} spell shapes.", distinctShapes.Count);
        }

        private static IEnumerable<SpellShape> GetDistinctShapes(IEnumerable<SpellShape> shapes)
        {
            var purgedShapes = new List<SpellShape>();

            foreach (var shape in shapes)
            {
                var repeated = purgedShapes.FirstOrDefault(s => s.Id == shape.Id);
                if (repeated != null) { repeated.MergeFlags(shape); }
                else { purgedShapes.Add(shape); }
            }

            return purgedShapes;
        }
    }
}
using System;
using System.IO;
using YamlDotNet.Serialization;

namespace Cardinal
{
    public static class DataLoader
    {
        private static Deserializer yamlDeserializer = new Deserializer();

        private static Serializer yamlSerializer =
            new Serializer();

        public static T ProtoLoad<T>(string path)
        {
            if (!FileTools.PrepareLoad(path)) { return default(T); }

            var loadedObject = default(T);
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                try { loadedObject = ProtoBuf.Serializer.Deserialize<T>(stream); }
                catch { Logger.CardinalMessage("Failed to load data from {0}.", path); }
            }

            return loadedObject;
        }

        public static bool ProtoSave<T>(string path, T data)
        {
            if (!FileTools.PrepareSave(path)) { return false; }

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                try { ProtoBuf.Serializer.Serialize(stream, data); }
                catch
                {
                    Logger.CardinalMessage("Failed to save data at {0}", path);
                    return false;
                }
            }

            return true;
        }

        public static T YamlLoad<T>(string path)
        {
            if (!FileTools.PrepareLoad(path)) { return default(T); }

            var loadedObject = default(T);
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(stream))
            {
                try { loadedObject = yamlDeserializer.Deserialize<T>(reader); }
                catch(Exception e)
                {
                    Logger.CardinalMessage("Failed to load data from {0}.", path);
                    Logger.CardinalMessage(e.ToString());
                }
            }

            return loadedObject;
        }

        public static bool YamlSave<T>(string path, T data)
        {
            if (!FileTools.PrepareSave(path)) { return false; }

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            using (var writer = new StreamWriter(stream))
            {
                try { yamlSerializer.Serialize(writer, data); }
                catch
                {
                    Logger.CardinalMessage("Failed to save data at {0}", path);
                    return false;
                }
            }

            return true;
        }
    }
}
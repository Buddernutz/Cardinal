using System.IO;

namespace Cardinal
{
    public static class FileTools
    {
        public static bool PrepareDirectory(string directory)
        {
            if (Directory.Exists(directory)) { return true; }

            try { Directory.CreateDirectory(directory); }
            catch
            {
                Logger.CardinalMessage("Failed to create directory: {0}", directory);
                return false;
            }

            return true;
        }

        public static bool PrepareLoad(string path)
        {
            if (File.Exists(path)) { return true; }
            Logger.CardinalMessage("Cannot find file: {0}", path);
            return false;
        }

        public static bool PrepareSave(string path)
        {
            var fileInfo = new FileInfo(path);

            if (fileInfo.Directory == null)
            {
                Logger.CardinalMessage("No directory specified: {0}", path);
                return false;
            }

            if (!PrepareDirectory(fileInfo.Directory.FullName)) { return false; }
            if (!fileInfo.Exists) { return true; }

            try { File.Delete(path); }
            catch
            {
                Logger.CardinalMessage("Failed to overwrite spell shape data file.");
                return false;
            }

            return true;
        }
    }
}

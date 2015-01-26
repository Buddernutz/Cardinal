using System.Windows.Media;
using ff14bot.Helpers;

namespace Cardinal
{
    public static class Logger
    {
        private static readonly Color lisbethColor = Color.FromRgb(166, 53, 71);
        private static readonly Color agilColor = Color.FromRgb(109, 139, 225);
        private static readonly Color cardinalColor = Color.FromRgb(96, 194, 114);
        private static readonly Color zekkenColor = Color.FromRgb(201, 115, 255);

        public static void AgilMessage(string text, params object[] args)
        {
            text = "[Agil] " + string.Format(text, args);
            Logging.Write(agilColor, text);
        }

        public static void ZekkenMessage(string text, params object[] args)
        {
            text = "[Zekken] " + string.Format(text, args);
            Logging.Write(zekkenColor, text);
        }

        public static void CardinalMessage(string text, params object[] args)
        {
            text = "[Cardinal] " + string.Format(text, args);
            Logging.Write(cardinalColor, text);
        }

        public static void LisbethMessage(string text, params object[] args)
        {
            text = "[Lisbeth] " + string.Format(text, args);
            Logging.Write(lisbethColor, text);
        }
    }
}
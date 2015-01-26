using System.Collections.Generic;
using ff14bot.RemoteWindows;
using TreeSharp;

namespace Cardinal
{
    public class SetIconString : TimedTaskNode
    {
        private int choiceOffset;

        public SetIconString(int choiceOffset = 1)
        {
            this.choiceOffset = choiceOffset;
            Time = 600;
        }

        protected override bool PreCondition(object context)
        {
            return SelectIconString.IsOpen;
        }

        protected override RunStatus PostAction(object context)
        {
            List<string> lines;
            try { lines = SelectIconString.Lines(); }
            catch { return RunStatus.Failure; }

            if (lines == null) { return RunStatus.Failure; }

            int lineCount = lines.Count;
            int choiceIndex = lineCount - choiceOffset;
            if (choiceIndex >= lineCount || choiceIndex < 0) { return RunStatus.Failure; }

            SelectIconString.ClickSlot((uint)choiceIndex);
            Logger.CardinalMessage("Selected icon string: {0}", lines[choiceIndex]);

            return RunStatus.Success;
        }
    }
}
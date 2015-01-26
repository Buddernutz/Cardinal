using System;
using ff14bot;
using ff14bot.Managers;
using TreeSharp;
using Action = TreeSharp.Action;

namespace Cardinal
{
    public class SetTarget : Action
    {
        private Func<uint> npcRetriever;

        public SetTarget(Func<uint> npcRetriever)
        {
            this.npcRetriever = npcRetriever;
        }

        protected override RunStatus Run(object context)
        {
            var currentTarget = Core.Me.CurrentTarget;
            if (currentTarget != null && currentTarget.NpcId == npcRetriever()) { return RunStatus.Failure; }

            var target = GameObjectManager.GetObjectByNPCId(npcRetriever());
            if (target == null) { return RunStatus.Failure; }

            target.Target();
            return RunStatus.Success;
        }
    }
}

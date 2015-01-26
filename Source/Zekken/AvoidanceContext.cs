using System;
using System.Collections.Generic;
using System.Linq;
using Clio.Utilities;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;
using TreeSharp;

namespace Cardinal
{
    public class AvoidanceContext
    {
        private const float NEARBY_THRESHOLD = 40;
        private List<SpellCast> spellCasts = new List<SpellCast>();
        private List<SpellCast> castsBeingAvoided = new List<SpellCast>();
        private HashSet<uint> ignoredSpells = new HashSet<uint>();
        public SpellCast UnknownCast { get; set; }
        public AvoidanceVector CurrentVector { get; set; }
        public ScreenshotCapture Capture = new ScreenshotCapture();

        public RunStatus Update()
        {
            var characters = GameObjectManager.GetObjectsOfType<BattleCharacter>()
                .ToList();

            spellCasts.RemoveAll(sc => !characters.Contains(sc.Caster) || sc.Caster.IsDead || SpellExpired(sc) || !sc.Caster.IsValid);

            var newCasts = GetNewCasts(characters);
            spellCasts.AddRange(newCasts);
            UnknownCast = GetNewUnknownCast(newCasts);

            var newAvoidableCasts = GetNewAvoidable(newCasts);
            castsBeingAvoided.AddRange(newAvoidableCasts.Select(t => t.Item1));

            var updatedVector = CurrentVector.GetUpdatedVector();
            var newVectors = newAvoidableCasts.Select(t => t.Item2).ToList();
            CurrentVector = newVectors.Count > 0 ? newVectors.Aggregate(updatedVector, VectorTools.Combine) : updatedVector;

            return RunStatus.Success;
        }

        public Vector3 SafeLocation
        {
            get { return CurrentVector.TargetLocation; }
        }

        public bool IsAvoiding
        {
            get { return CurrentVector.IsActive; }
        }

        public bool IsWaiting
        {
            get { return castsBeingAvoided.Count > 0; }
        }

        public void Reset()
        {
            spellCasts.Clear();
            castsBeingAvoided.Clear();
            ignoredSpells.Clear();
            UnknownCast = null;
            CurrentVector = AvoidanceVector.Zero;
        }

        private List<SpellCast> GetNewCasts(IEnumerable<BattleCharacter> characters)
        {
            var previousCasters = spellCasts.Select(sc => sc.Caster).ToList();
            var myLocation = Core.Me.Location;

            return characters
                .Where(c => c.InCombat && c.CanAttack)
                .Where(c => IsNearby(myLocation, c))
                .Where(c => c.IsCasting)
                .Where(c => c.SpellCastInfo != null)
                .Where(c => !previousCasters.Contains(c))
                .Select(c => new SpellCast(c))
                .ToList();
        }

        private static List<Tuple<SpellCast, AvoidanceVector>> GetNewAvoidable(IEnumerable<SpellCast> newCasts)
        {
            return newCasts
                .Where(sc => !sc.IsUnknown)
                .Where(sc => !sc.Shape.CombinedFlag.HasFlag(ShapeFlag.NotTelegraphed))
                .Select(c => new Tuple<SpellCast, AvoidanceVector>(c, VectorCalculator.GetAvoidanceVector(c)))
                .Where(p => p.Item2.IsActive)
                .ToList();
        }

        private SpellCast GetNewUnknownCast(IEnumerable<SpellCast> newCasts)
        {
            var unknown = newCasts
                .Where(c => c.Caster.ObjectId == Core.Target.ObjectId)
                .Where(c => !ignoredSpells.Contains(c.SpellId))
                .FirstOrDefault(c => c.IsUnknown);

            if (unknown != null) { ignoredSpells.Add(unknown.SpellId); }
            return unknown;
        }

        private static bool SpellExpired(SpellCast cast)
        {
            return !cast.Caster.IsCasting
                && (DateTime.Now - cast.StartTime).TotalMilliseconds > cast.TotalDuration;
        }

        private static bool IsNearby(Vector3 self, GameObject character)
        {
            return self.Distance(character.Location) < NEARBY_THRESHOLD;
        }
    }
}

using System;
using System.Linq;
using System.Timers;
using System.Windows.Forms;
using ff14bot;
using ff14bot.AClasses;
using ff14bot.Behavior;
using ff14bot.Interfaces;
using ff14bot.Managers;
using TreeSharp;
using Timer = System.Timers.Timer;

namespace Cardinal.Plugins
{
    public class AgilPlugin : IBotPlugin
    {
        private EquipmentContext equipContext;
        private RepairContext repairContext;
        private AgilSettings settings = new AgilSettings();
        private Composite root;
        private AgilWindow settingsWindow;
        private bool enabled, botRunning, hooked;
        private volatile bool windowClosed = true;
        private Timer timer = new Timer(10000) {AutoReset = true};

        public void OnButtonPress()
        {
            if (settingsWindow == null)
            {
                settingsWindow = new AgilWindow {Plugin = this};
                settingsWindow.FormClosed += WindowClosed;
            }

            if (!windowClosed) { return; }
            windowClosed = false;
            settingsWindow.ShowDialog();
        }

        private void WindowClosed(object sender, FormClosedEventArgs e)
        {
            windowClosed = true;
        }

        public void Reload()
        {
            Unhook();

            DataCompiler.CompileStatWeights();
            settings = AgilSettings.Load();
            Database.LoadAgilData();

            if (!enabled || !botRunning) { return; }
            Hook();
        }

        public void OnDisabled()
        {
            enabled = false;
            TreeRoot.OnStop -= OnBotStop;
            TreeRoot.OnStart -= OnBotStart;
            Unhook();
        }

        public void OnEnabled()
        {
            enabled = true;
            TreeRoot.OnStop += OnBotStop;
            TreeRoot.OnStart += OnBotStart;

            settings = AgilSettings.Load();
            Database.LoadAgilData();
        }

        public void OnPulse()
        {
            if (!enabled || !botRunning || hooked) { return; }
            Hook();
        }

        public void OnShutdown()
        {
            Unhook();
            TreeRoot.OnStop -= OnBotStop;
            TreeRoot.OnStart -= OnBotStart;
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            equipContext.TimerElapsed = true;
        }

        private void OnBotStop(BotBase bot)
        {
            botRunning = false;
            Unhook();
        }

        private void OnBotStart(BotBase bot)
        {
            botRunning = true;
            Hook();
        }

        private void Hook()
        {
            if (hooked) { return; }

            equipContext = new EquipmentContext(settings);
            repairContext = new RepairContext(settings);

            if (hooked) { TreeHooks.Instance.RemoveHook("TreeStart", root); }
            root = Agil.CreateBehavior(equipContext, repairContext);
            TreeHooks.Instance.InsertHook("TreeStart", 0, root);
            Logger.AgilMessage("Hooked.");
            hooked = true;

            timer.Stop();
            timer.Start();
        }

        private void Unhook()
        {
            if (!hooked) { return; }

            TreeHooks.Instance.RemoveHook("TreeStart", root);
            Logger.AgilMessage("Unhooked.");
            hooked = false;
            timer.Stop();
        }

        private void AddMender()
        {
            var target = Core.Me.CurrentTarget;
            var menders = Database.Menders;

            if (target == null) { return; }

            var location = Core.Me.Location;
            uint npcId = target.NpcId;

            var mender = new Mender
            {
                NpcName = target.Name,
                NpcId = npcId,
                Location = location,
                ZoneId = WorldManager.ZoneId
            };

            if (menders.Any(m => m.NpcId == npcId)) { menders.RemoveAll(m => m.NpcId == npcId); }
            else { menders.Add(mender); }

            DataLoader.ProtoSave(Directories.MENDERS, menders);

            Logger.AgilMessage("Added or removed mender: {0}", mender);
            Logger.AgilMessage("Total menders in database: {0}", menders.Count);
        }

        #region Plugin

        public string Author
        {
            get { return "Saga"; }
        }

        public string ButtonText
        {
            get { return "Settings"; }
        }

        public string Description
        {
            get { return "Equipment repair and optimizer."; }
        }

        public string Name
        {
            get { return "Agil"; }
        }

        public Version Version
        {
            get { return new Version(0, 6); }
        }

        public bool WantButton
        {
            get { return true; }
        }

        public bool Equals(IBotPlugin other)
        {
            return Author == other.Author
                   && Name == other.Name
                   && Version == other.Version;
        }

        public void OnInitialize()
        {
            timer.Elapsed += OnTimer;
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Longship.Managers;
using Longship.Plugins;
using Longship.Utils;

namespace LongshipEssentials
{
    public class Essentials : IPlugin
    {
        public override void OnEnable()
        {
            Longship.Longship.Instance.CommandsManager.RegisterCommand(this, "server", ServerCommandListener);
        }

        private bool ServerCommandListener(long sender, string command, string argument)
        {
            Longship.Longship.Instance.Log.LogInfo($"Command got params: {sender}, {command}, {argument}");
            // sender.SendChat($"Longship v{Longship.Longship.BuildTag}");
            return true;
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }
    }
}

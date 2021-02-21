using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Longship.Utils
{
    public static class ValheimExtensions
    {
        public static void SendChat(this Player player, string chat)
        {
            var targetPeerId = player.GetZDOID().userID;
            ZRoutedRpc.instance.InvokeRoutedRPC(targetPeerId, "Say", Talker.Type.Whisper, "Server", chat);
        }
    }
}

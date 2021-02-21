using System;
using System.Text.RegularExpressions;
using HarmonyLib;
using UnityEngine;

namespace Longship.Patches
{
    [HarmonyPatch(typeof(Chat), "RPC_ChatMessage")]
    public class PatchListenToChatMessages
    {
        private static Regex CommandRegex;

        static PatchListenToChatMessages()
        {
            CommandRegex = new Regex(@"\/(?<command>[A-Za-z]+) {0,1}(?<argument>.*)", RegexOptions.Compiled);
        }
        static void Prefix(long sender, Vector3 position, int type, string name, string text)
        {
            Longship.Instance.Log.LogInfo($"[Chat] {name}: {text}");
            var match = CommandRegex.Match(text);
            if (match.Success)
            {
                var command = match.Groups["command"].Value;
                var argument = match.Groups["argument"].Success ? match.Groups["argument"].Value : null;
                Longship.Instance.CommandsManager.OnCommandExecuted(sender, command, argument);
                Longship.Instance.Log.LogInfo($"Player {name}, executed command: /{command} {argument}");
            }
        }
    }
}
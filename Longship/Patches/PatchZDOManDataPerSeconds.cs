using System;
using HarmonyLib;

namespace Longship.Patches
{
    [HarmonyPatch(typeof(ZDOMan), MethodType.Constructor, typeof(int))]
    public class PatchZDOManDataPerSeconds
    {
        static void Postfix(ref int ___m_dataPerSec)
        {
            ___m_dataPerSec = (int) Longship.Instance.ConfigurationManager.Configuration.NetworkDataPerSeconds.Value;
            Longship.Instance.Log.LogInfo($"ZDOMan m_dataPerSec patched to value {___m_dataPerSec}");
        }
    }
}
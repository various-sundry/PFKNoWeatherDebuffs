using System.Reflection;

using HarmonyLib;

using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Root;
using Kingmaker.Controllers;

using UnityModManagerNet;

namespace NoWeatherDebuffs
{
    public static class Main
    {
        internal static UnityModManager.ModEntry Mod;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            Mod = modEntry;

            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            return true;
        }

        [HarmonyPatch(typeof(LibraryScriptableObject), "LoadDictionary")]
        public static class LibraryScriptableObject_LoadDictionary_Patch
        {
            private static bool s_loaded = false;

            public static void Postfix()
            {
                if (s_loaded)
                {
                    return;
                }

                s_loaded = true;

                // The root blueprint has a large number of configuration and default values.
                BlueprintRoot root = ResourcesLibrary.TryGetBlueprint<BlueprintRoot>(
                    "2d77316c72b9ed44f888ceefc2a131f6"
                );

                // Rain and snow have an intensity level, which is converted to an InclemencyType.
                // This is compared with the following three values to see if it triggers a
                // mechanical effect. Setting them to a larger value than the max in the
                // InclemencyType enum makes this comparison always return false.
                var tooBig = (InclemencyType)10;
                root.WeatherSettings.ConcealmentBeginsOn = tooBig;
                root.WeatherSettings.StealthBonusBeginsOn = tooBig;
                root.WeatherSettings.SlowdownBonusBeginsOn = tooBig;

                // These arrays control the buff icons. While the buffs themselves are entirely
                // cosmetic, it would be incredibly confusing to leave them here.
                root.WeatherSettings.RainPartyBuffs[3] = null;
                root.WeatherSettings.RainPartyBuffs[4] = null;
                root.WeatherSettings.SnowPartyBuffs[3] = null;
                root.WeatherSettings.SnowPartyBuffs[4] = null;
            }
        }
    }
}
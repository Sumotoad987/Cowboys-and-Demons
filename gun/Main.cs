using BlueprintCore.Utils;
using gun.Classes.Gunslinger;
using gun.Firearms;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.JsonSystem.Converters;
using Kingmaker.Modding;
using Kingmaker.SharedTypes;
using Kingmaker.Utility;
using Newtonsoft.Json;
using Owlcat.Runtime.Core.Utils;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityModManagerNet;
using static UnityModManagerNet.UnityModManager.Param;

namespace gun;

public static class Main {
    internal static Harmony HarmonyInstance;
    internal static UnityModManager.ModEntry.ModLogger Log;
    public static string ModPath;
    public static readonly HashSet<string> Bundles = new HashSet<string>();

    public static IEnumerable<string> GetFilesFromDirectory(string directory)
    {
        return Directory.GetFiles(System.IO.Path.Combine(ModPath, directory), "*", SearchOption.AllDirectories);
    }
    public static bool Load(UnityModManager.ModEntry modEntry) {
        Log = modEntry.Logger;
        //LogWrapper.EnableInternalVerboseLogs();
        ModPath = modEntry.Path;
        HarmonyInstance = new Harmony(modEntry.Info.Id);
        try {
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        } catch {
            HarmonyInstance.UnpatchAll(HarmonyInstance.Id);
            throw;
        }
        return true;
    }

    [HarmonyPatch(typeof(BlueprintsCache))]
    public static class BlueprintsCaches_Patch {
        private static bool Initialized = false;
        
        [HarmonyPatch(nameof(BlueprintsCache.Init)), HarmonyPostfix]
        public static void Init_Postfix() {
            try {
                if (Initialized) {
                    Log.Log("Already initialized blueprints cache.");
                    return;
                }
                Initialized = true;

                var path = Path.Combine(ModPath, "Bundles\\");
                OwlcatModification owlcatModification = OwlcatModification
                    .LoadFromDirectory(path, path);
                if (owlcatModification == null)
                {
                    Log.Log("Loading gunmod bundle failed, modification is null.");
                }
                else
                {
                    OwlcatModificationManifest manifest = owlcatModification.Manifest;
                    if (manifest == null)
                    {
                        Log.Log("Loading gunmod bundle failed, manifest is null.");
                    }
                    else
                    {
                        Log.Log("Applying gunmod modification.");

                        owlcatModification.Apply();
                        //Log.Log("applied");
                        owlcatModification.Reload();
                        //Log.Log("reloading");
                    }
                }

                Log.Log("Patching blueprints.");
                
                BaseFirearm.Configure();
                Gunslinger.Configure();
                Musket.Configure();
                Pistol.Configure();
                Rifle.Configure();
                Revolver.Configure();

            } catch (Exception e) {
                Log.Log(string.Concat("Failed to initialize.", e));
            }
        }
    }

}

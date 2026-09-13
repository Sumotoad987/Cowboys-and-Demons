using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils;
using gun.Classes.Gunslinger;
using gun.Classes.Spellscar_Drifter;
using gun.Cowgirl;
using gun.Feats;
using gun.Firearms;
using gun.Plot;
using HarmonyLib;
using Kingmaker;
using Kingmaker.AreaLogic;
using Kingmaker.AreaLogic.Cutscenes;
using Kingmaker.AreaLogic.Cutscenes.Commands;
using Kingmaker.Blueprints.CharGen;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.JsonSystem.Converters;
using Kingmaker.Modding;
using Kingmaker.PubSubSystem;
using Kingmaker.SharedTypes;
using Kingmaker.Utility;
using Newtonsoft.Json;
using Owlcat.Runtime.Core.Utils;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityModManagerNet;
using static UnityModManagerNet.UnityModManager;
using static UnityModManagerNet.UnityModManager.Param;

namespace gun;

public static class Main {
    internal static Harmony HarmonyInstance;
    internal static UnityModManager.ModEntry.ModLogger Log;
    public static string ModPath;
    public static readonly HashSet<string> Bundles = new HashSet<string>();
    public static WorldMapEncounter map;

    public static IEnumerable<string> GetFilesFromDirectory(string directory)
    {
        return Directory.GetFiles(System.IO.Path.Combine(ModPath, directory), "*", SearchOption.AllDirectories);
    }
    public static bool Load(UnityModManager.ModEntry modEntry) {
        Log = modEntry.Logger;
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

    public static void OnGUI(UnityModManager.ModEntry modEntry) {
        

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
                LoadGunAssets();
                
                LocalizationTool.LoadLocalizationPacks(Directory.GetFiles(Path.Combine(ModPath, "Localization")));

                Log.Log("Patching blueprints.");
                
                BaseFirearm.Configure();
                Gunslinger.Configure();
                AmateurGunslinger.Configure();
                OldReliable.Configure();
                SpellSevered.Configure();
                ToughAsNails.Configure();
                SpellscarDrifter.Configure();
                OrderOfTheEasternStar.Configure();
                CowgirlUnit.Configure();
                Musket.Configure();
                Pistol.Configure();
                Rifle.Configure();
                Revolver.Configure();
                Shotgun.Configure();
                Blunderbuss.Configure();
                Plot.Flags.Configure();
                Act2.Configure();
                Mythos.Configure();
                EventBus.Subscribe(new WorldMapEncounter(-70,4,-65,6,Flags.MetCowgirl, Act2.CowgirlMeetingEvent));
                MusketMaster.Configure();
                GritFeats.Configure();
                Buccaneer.Configure();
                Act3.Configure();
                Act4.Configure();
                //EventBus.Subscribe(new WeaverLairMapFix());

            } catch (Exception e) {
                Log.Log(string.Concat("Failed to initialize.", e));
            }
        }

        public static void LoadGunAssets()
        {
            OwlcatModificationsManager OwlcatModManager = OwlcatModificationsManager.Instance;
            if (!OwlcatModManager.m_Started)
            {
                OwlcatModManager.Start();
            }

            List<OwlcatModification> list = new List<OwlcatModification>();
            //list.AddRange(OwlcatModManager.m_Modifications);
            list.AddRange(OwlcatModificationsManager.LoadModifications(System.IO.Path.Combine(ModPath, "Bundles\\")));

            List<OwlcatModification> m_Modifications = new List<OwlcatModification>();


            m_Modifications.AddRange(OwlcatModManager.m_Modifications);
            m_Modifications.AddRange(list);
            OwlcatModManager.m_Modifications = m_Modifications.ToArray();
            string[] enabledModifications = { "GunAssets" };
            foreach (string modificationName in enabledModifications)
            {

                OwlcatModification owlcatModification = OwlcatModManager.m_Modifications.FirstItem((OwlcatModification d) => d.Manifest?.UniqueName == modificationName);
                if (owlcatModification == null)
                {
                    PFLog.Mods.Error("Missing modification: " + modificationName);
                    continue;
                }
                Log.Log("Found mod");
                string path = owlcatModification.Path;
                Log.Log("At path:" + path);
                OwlcatModificationManifest manifest = owlcatModification.Manifest;
                Log.Log("got manifest");
                if (manifest == null)
                {
                    PFLog.Mods.Error("Modification can't be loaded: " + modificationName + " (" + path + ")");
                }
                else
                {
                    Log.Log("getting ready to apply");
                    PFLog.Mods.Log("Apply modification: " + manifest.UniqueName + " (" + path + ")");

                    owlcatModification.Apply();
                    Log.Log("applied");
                    //owlcatModification.Reload();

                }
            }
            foreach (OwlcatModification mod in OwlcatModManager.AppliedModifications)
            {
                list.Add(mod);
            }
            
            OwlcatModManager.AppliedModifications = list.ToArray();
            
        }
    }



}

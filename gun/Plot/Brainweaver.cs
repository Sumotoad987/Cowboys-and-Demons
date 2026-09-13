using BlueprintCore.Blueprints.Configurators;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.ResourceManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace gun.Plot
{
    internal static class Brainweaver
    {
        public const string GUID = "a615d1ab2282484dabad455bf04cecff";
        public static void Configure()
        {
            BrainweaverPortrait.Configure();
            UnitConfigurator.New("Brainweaver", GUID)
                .CopyFrom("05ab760ee34af4c44a82514cec76b59b")
                .AddFacts(new List<Blueprint<BlueprintUnitFactReference>>{ BlueprintTool.GetRef<BlueprintUnitFactReference>("525f980cb29bc2240b93e953974cb325")})
                .SetPortrait(BrainweaverPortrait.GUID)
                .Configure();
        }
    }
    internal static class BrainweaverPortrait
    {
        public static string GUID = "edebf9cb2e8741b899d58798f7f54f47";
        public static PortraitData data;
        public static void Configure()
        {
            PortraitConfigurator BrainweaverPortrait = PortraitConfigurator.New("BrainweaverPortrait", GUID);
            data = new PortraitData("BrainweaverPortraitID");
            ResourceStorage<Sprite> storage = CustomPortraitsManager.Instance.Storage;
            data.SmallPortraitHandle = new Kingmaker.ResourceManagement.CustomPortraitHandle(Main.ModPath + "/Media/Portraits/BrainweaverSmall.png", Kingmaker.Enums.PortraitType.SmallPortrait, storage);
            data.HalfPortraitHandle = new Kingmaker.ResourceManagement.CustomPortraitHandle(Main.ModPath + "/Media/Portraits/BrainweaverHalf.png", Kingmaker.Enums.PortraitType.HalfLengthPortrait, storage);
            data.FullPortraitHandle = new Kingmaker.ResourceManagement.CustomPortraitHandle(Main.ModPath + "/Media/Portraits/BrainweaverFull.png", Kingmaker.Enums.PortraitType.FullLengthPortrait, storage);
            BrainweaverPortrait.SetData(data);
            BrainweaverPortrait.Configure();

        }
    }
}

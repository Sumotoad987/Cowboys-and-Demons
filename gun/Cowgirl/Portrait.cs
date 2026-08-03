using BlueprintCore.Blueprints.Configurators;
using Kingmaker.Blueprints;
using Kingmaker.ResourceManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace gun.Cowgirl
{
    internal static class Portrait
    {
        public static string GUID = "ec5ae22c250641bdb16f0c0cd5601f58";
        public static PortraitData data;
        public static void Configure()
        {
            PortraitConfigurator CowgirlPortrait = PortraitConfigurator.New("CowgirlPortrait", GUID);
            data = new PortraitData("CowgirlPortraitID");
            ResourceStorage<Sprite> storage = CustomPortraitsManager.Instance.Storage;
            data.SmallPortraitHandle = new Kingmaker.ResourceManagement.CustomPortraitHandle(Main.ModPath + "/Media/Portraits/CowgirlSmall.png", Kingmaker.Enums.PortraitType.SmallPortrait, storage);
            data.HalfPortraitHandle = new Kingmaker.ResourceManagement.CustomPortraitHandle(Main.ModPath + "/Media/Portraits/CowgirlHalf.png", Kingmaker.Enums.PortraitType.HalfLengthPortrait, storage);
            data.FullPortraitHandle = new Kingmaker.ResourceManagement.CustomPortraitHandle(Main.ModPath + "/Media/Portraits/CowgirlFull.png", Kingmaker.Enums.PortraitType.FullLengthPortrait, storage);
            CowgirlPortrait.SetData(data);
            CowgirlPortrait.Configure();

        }
    }

}

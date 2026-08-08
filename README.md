# Cowboys and Demons

Adds the [Gunslinger](https://aonprd.com/ClassDisplay.aspx?ItemName=Gunslinger) class as well as firearms and some associated feats such as weapon focus and firearm proficiency. 

Features

Firearms

    New kind of weapon requiring firearm proficiency
    You should get one in your starting equipment as a gunslinger or they can be purchased from the exotic weapons provider in the crusader camp and Drezen as well as the chaplain under magical equipment in the roguelike DLC. Early firearms (that is muskets and pistols) can also be purchased from the blacksmith in Kenebres.
    Should match the tabletop rules as best as I could manage.
    Each shot has to be loaded which requires a full round action but can be reduced by feats and equipment (If reduced to a free action allows for a full attack to be completed successfully)
    Bypasses armor, natural armor, and shields (essentially targets touch AC but can still be used for Deadly Aim and similar feats)
    Can misfire potentially causing an damage to the wielder
    For full rules see the pathfinder rules reference

Gunslinger Class

    A class centered around the use of firearms
    Mostly copied from the tabletop
    Some changes to fit the game
    Not all deeds have been implemented as yet

Spellscar Drifter

    A cavalier archetype for firearm users
    As with gunslinger has a fixed list of deeds rather than the full list

Custom Companion

    Added Bell Tarvil a shieldmarshal from Alkenstar as a new companion
    Met on the road in Act 2
    Appears in Drezen in Act 3 and will give you a quest leading to her recruitment


##Instalation Guide
Hopefully Modfinder will be able to sort this out but for manual install it is important to note this mod had to be split into two parts. This repo contains the content of the mod so the code, blueprint, mechanics, feats, etc. The [Cowboy's and Demons Assets](https://github.com/Sumotoad987/Cowboys-and-Demons-Assets) mod contains the 3d model used for the firearms in game however the releases here include both mods in a single combined install.
All this is due to the fact that assets can only be added (with great difficulty) through a Wrath Template mod while much of the content was far easier to implement in a using Blueprint Core.
(I intend to write a tutorial to explain how all this works in case anyone else wants to add 3d models to the game at a later stage)

##This mod is a work in progress and has several known issues but most of the core features are functional.
* Animations: The Musket and Rifle animate fairly well, though the hand positions can be a little fiddly. Pistols and Revolvers are a little off particularly if you try to dual wield them.
* Dual Weilding: Whilst technically you can dual wield pistols and revolvers and mechanically most of that works the animation is dodge and the second pistol floats in the air pointing in random directions.

##Changes from Tabletop
* Removed several gunslinger deeds which were either very hard to implement in game or would have not do anything within the scope of the game.
* Rapid Reload is a single feat for all firearms rather than needing to chose a new type each time
* Deeds which are free actions after you hit have been changed into activatable abilities used before you make the attack.
* Gun Training applies to all firearms not just one at a time
* Misfire has been tweaked. If you roll a misfire you get the damaged firearm condition which lasts 1 hour (the time it would take to fix in tabletop rules). Damaged firearm increases your misfire range. If you misfire with an early firearm and have the damaged firearm condition your firearm deals its weapon damage to you and a 5ft burst around you. The firearm is not destroyed.
* Probably some other things I've forgotten.


##Attributions
* "Flintlock Musket (no hands)" (https://skfb.ly/6TCDI) by Andy Woodhead is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/).
* "Clement Percussion Revolver" (https://skfb.ly/6YHAY) by Feco is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/).
* "Katana and early Japanese rifle" (https://skfb.ly/6U6RL) by patspet is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/).
* "Flintlock pistol" (https://skfb.ly/6TNo8) by Cyril43 is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/).
* "25 CC0 bang / firework SFX" (https://opengameart.org/content/25-cc0-bang-firework-sfx) by rubberduck 

## Thanks to
* Wolfie's [Modding Wiki](https://github.com/WittleWolfie/OwlcatModdingWiki/wiki) for getting me started.
* Kurufinve for the Unity template needed to add assets to the game.
* The many WotR mods out there on Github which I looked at for guidance.

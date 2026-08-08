# Cowboys and Demons

Adds the [Gunslinger](https://aonprd.com/ClassDisplay.aspx?ItemName=Gunslinger) class as well as firearms and some associated feats such as weapon focus and firearm proficiency. 

##Instalation Guide
Hopefully Modfinder will be able to sort this out but for manual install it is important to note this mod had to be split into two parts. This repo contains the content of the mod so the code, blueprint, mechanics, feats, etc. The [Cowboy's and Demons Assets](https://github.com/Sumotoad987/Cowboys-and-Demons-Assets) mod contains the 3d model used for the firearms in game.
All this is due to the fact that assets can only be added (with great difficulty) through a Wrath Template mod while much of the content was far easier to implement in a using Blueprint Core.
(I intend to write a tutorial to explain how all this works in case anyone else wants to add 3d models to the game at a later stage)

##This mod is a work in progress and has several known issues but most of the core features are functional.
* Projectiles not working: Currently there is a bug when a character attacks with the weapon the game tries to fire a projectile but fails for reasons unknown. I've written a work around but it may have some unforeseen issues down the road.
* Animations: The Musket and Rifle animate fairly well, though the hand positions can be a little fiddly. Pistols and Revolvers are a little off particularly if you try to dual wield them.
* Dual Weilding: Whilst technically you can dual wield pistols and revolvers and mechanically most of that works the animation is dodge and the second pistol floats in the air pointing in random directions.
* Capacity: I haven't implemented any firearms with a capacity of more than 1 round this includes if you are trying to dual wield. until you have Rapid Reload and an Advanced Weapon you'll only be able to fire one shot per attack.
* Bleeding Shot will only apply constitution bleed for some reason.
* Misfire the icon for damaged firearm as well as its name do not apear correctly on the player icon.
* Damage firearm's null icon does not disapear when the condition is removed (however it ceases to have a mechanical effect.)

##Changes from Tabletop
* Removed several gunslinger deeds which were either very hard to implement in game or would have not do anything within the scope of the game.
* Rapid Reload is a single feat for all firearms rather than needing to chose a new type each time
* Deeds which ae free actions after you hit have been changed into activatable abilities used before you make the attack.
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

This mod adds 3 main functions to the Nurse:

1. Hotkey for paid Nurse healing + items earned through progression that increase this hotkey's range

2. Recipes to bridge the gap between lesser/regular healing potions and the later potions, and additional options to craft later potions

3. A shop for the Nurse where she sells most types of healing potions

Tl;dr This mod adds: 7 items, 3 recipes, 1 hotkey, and 1 shop

Things to note:

You need to bind quick heal to a button manually in the Settings -> Controls -> Mod Controls menu.

Should be compatible with Calamity, although unchecked for 1.4.4 currently. Uses BossChecklist for stronger references to Calamity bosses currently.

Items that increase the Nurse's range will do so at a price. The first item you get will just let you heal from the same room, the next will increase the range by triple that, but if you heal outside the room prices increase by 3 times the normal rate. Upgrades will increase the quick heals range further, from one screen to the entire surface to the entire world. However, the sweet spot will increase only by small amounts. You can easily tell if you are in either the normal range of the item or the sweet spot as you will have buffs for when you are in range of either. So go ahead and hit that quick heal while you're digging up that hellstone, just check with your financial advisor before you do.

For the Nurse quick heal at range items, King Slime, Eye of Cthulhu, and Brain of Cthulhu/Eater of Worlds all drop component items to upgrade the base Nurse VIP Badge which is sold in the Nurse's shop. You will also be able to buy these items from her shop as you progress, however it is significantly cheaper to build by just killing the bosses. 

IF YOU ARE PLAYING CALAMITY: Prices when fighting a boss are 5x what they are normally, so be careful. Also, the base heal cost increases by a flat amount depending on which bosses you have defeated.

Numbers should always be accurate to the number you see on the Nurse, but since what you are being charged is reverse engineered and manually implemented, if you find a situation where the quick heal doesn't match up to the price or some other weird situation, let me know and I can fix it. That goes for money reporting in the text box, actual money subtracted from your bank, item text. If you find an issue let me know.

Added recipes are balanced against in game recipes and require an Alchemy Table/Bottle crafting station to craft.

Recipes included currently:

Greater Healing Potion = 2x(Healing Potion) + Pixie Dust

Super Healing Potion = 4x(Greater Healing Potion)

CALAMITY ONLY:

Supreme Healing Potion = 4x(Super Healing Potion)

Unlike Nurse Shop, items are not unlocked in stage progression and rather each item is unlocked by killing individual bosses. What this means: If you kill King Slime, the Nurse will start selling Healing Potions. If you kill Eye of Cthulhu before King Slime, Restoration Potion and the Nurse's Walkie Talkie will be unlocked but Healing Potion won't. Here is a full list of the items that the Nurse will sell and their unlock conditions:

None: Mushroom, Bottled Water, Bottled Honey, Lesser Healing Potion, Nurse VIP Badge

King Slime: Healing Potion

Eye of Cthulhu: Restoration Potion, Nurse's Walkie Talkie

Brain of Cthulhu/Eater of Worlds: Nurse's Painted Shirt

Skeletron: Nurse's Painted Shirt

Wall of Flesh: Lifeforce Potion, Greater Healing Potion, Nurse Nourishment Diamond

Ancient Cultist: Super Healing Potion

CALAMITY ONLY:

Moon Lord: Supreme Healing Potion

Devourer of Gods: Omega Healing Potion

If there are healing related items people would find useful to be added to the shop, leave a comment and I'll see if I should add. The old Nurse Shop had Life Crystal's and Life Fruit, but I wasn't a huge fan of that so I skipped those in this mod.

There are also old idea's floating around for a more comprehensive overhaul including quests to kill certain monsters reset every Terraria day and medically inspired combat items.If that sounds cool leave a comment.

Special Thanks

Huge thanks to the legendary NotLe0n for letting me use his UI/UISystem to draw the Nurse's shop. Core UI was lifted with modifications from his mod AnglerShop.

Another big thank you to Solafide Media for making the sprites for the items and one of the buffs. Brought to life what I envisioned for the items and did what I couldn't. Check out his work on Fiverr.

Shout out to another legend, jopojelly, for helping me figure out how to automatically adjust my manual pricing based on the Nurse's happiness and pointing me to catGPT for shop help. Felt insane to google something and see a response from them 7+ years ago then go to the discord and have them answer one of my questions. 

Also shout out to ThomasthePencil and catGPT in the discord for help with shop indexing in 1.4.4 preview. Adding a shop to a vanilla npc without a shop wasn't as straighforward as you might think, but they helped me get it over the finish line.

Gotta mention my boy TheEdster3 for walking with me through the beginning of my journey on this project, decompiling Terraria and planting the fertile seed that would one day become
Nurse Overhaul. He left me with a heal that max healed every game tick you were next to the Nurse and now we're here.

And thanks to the community in general for having so many resources. A lot of work is put into tModLoader and its supporting documentation, and I definitely took it for granted before dipping my toe into this project so if you've worked on any part of tModLoader let me say from a simple player: thank  you.


Recent Changes

1.0.5
Updates will no longer be fully applied to the 1.4.3 version of Nurse Overhaul in lockstep with the live 1.4.4 version moving forward. Significant systemic changes to tmodloader shop framework as well as the localization system being the main input for items/config/buffs etc makes backpatching changes made in this version of Nurse Overhaul more difficult than before. 
Added config options for enabling and disabling Nurse healing items for full customization of your extended healing experience. Disabling an item will also disable the components needed to build said item and the dropping of those items from their corresponding bosses
Added the ability to toggle in Life Crystal's and Life Fruit to the shop. Crystals will be available to purchase for 1 plat a piece after you beat Eye of Cthulhu, while Fruits will be available after you beat all mech bosses for 2 plat a piece. Would still recommend picking up the free ones off the ground, but the option is there for those who want it
Made changes to the ordering of the shop. Items will now be categorized by their general item type (Potions, Nurse Items, Health Increasing items)
Fixed a bug where Omega Healing Potions weren't correctly being added to the shop for Calamity players after they beat Devourer of Gods


1.0.4
Fixed a bug where if you had more than 1 stack of platinum, your money would be calculated as negative and you would not receive a heal
Fixed a main chat output when healing debuffs during Calamity boss fights so that it states the correct amount of debuffs being cured

1.0.3.1 to 1.0.3.6
Icon issues

1.0.3
Removed BossChecklist as a reference
Fixed a bug where Calamity prices were miscalculated due to 1.4.4 changes
Fixed a bug that caused debuffs to be cured but not charged for

1.0.2
Icon resized for mod browser

1.0.1
Readme changes, description changes

1.0
Ready for 1.4.4 release
Better sprites added
Items text, rarity, and names changed
Sweet spot healing functionality added
Sweet spot buff added
Extended healing range numbers changed
Buffs added as nonremovable
Rarities readjusted
Item text changes
Icon added
Readme changes


0.9.3
Further changes to items
Code cleanup. Added comments and more organization to the file structure. Prepped for 1.4.4 publish and 1.4.3 ready once commented out sections re-added

0.9.2
Changed shop from stage progression unlock style to just needing to kill individual bosses 
Nurse items renamed, tooltips changed, rarities adjusted, sell prices adjusted to their corresponding tiers
Readme changes

0.9.1
Readme changes

0.9
Initial release prepared
[quote=tModLoader]Developed By Selystra[/quote]
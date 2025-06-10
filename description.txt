Overview

TL;DR: This mod adds: 7 items, 3 recipes, 1 hotkey for quick healing, and 1 shop.

From what I've seen, the Nurse is a divisive topic in the Terraria community. Some say she's a noob trap, while others believe she trivializes the game if used correctly. However, something everyone seems to agree on is that it can be difficult to get a heal from her when fighting, especially during boss fights. Everyone who has played through Terraria has at least one experience of trying to heal at the Nurse during a boss fight and accidentally killing their entire village of NPCs in the process. In my last playthrough, I wished many times that there was a small way to make it easier to press that heal button, something so it doesn't feel like you're opening a timed aim trainer every time you right-click on the Nurse. This mod solves this problem, attempts to balance the solution, and adds some small additional quality-of-life features centered around the Nurse.

Specifically, this mod adds three things centered around the Nurse and healing items:

1. A shop for the Nurse where she sells most types of healing potions.
2. A hotkey for paid Nurse healing and items earned through progression that increase this hotkey's range.
3. Recipes to bridge the gap between lesser/regular healing potions and later potions, and additional options to craft those later potions.

Things to Note

You need to manually bind the quick heal to a button in the Settings -> Controls -> Mod Controls menu.

Items that increase the Nurse's range will do so at a cost. The first item you get will only allow you to heal from the same room; the next will triple the range, but healing outside the room will increase the cost to three times the normal rate. Upgrades will further increase the quick heal range, from one screen to the entire surface to the entire world. However, the sweet spot will increase only by small amounts. You can easily tell if you are within the normal range of the item or the sweet spot as you will receive buffs for being in either range. So go ahead and use that quick heal while you're digging up that hellstone, but be sure to check with your financial advisor before you do.

For the Nurse quick heal at range items, King Slime, Eye of Cthulhu, and Brain of Cthulhu/Eater of Worlds all drop component items to upgrade the base Nurse VIP Badge, which is sold in the Nurse's shop. You will also be able to buy these items from her shop as you progress, but it is significantly cheaper to obtain them by killing the bosses.

IF YOU ARE PLAYING CALAMITY: Prices when fighting a boss are 5x what they normally are, so be careful. Also, the base heal cost increases by a flat amount depending on which bosses you have defeated.

Numbers should always be accurate to what you see on the Nurse, but since what you are being charged is reverse-engineered and manually implemented, if you find a situation where the quick heal doesn't match the price or there is some other issue, let me know and I can fix it. This applies to money reporting in the text box, actual money subtracted from your bank, and item text. If you find an issue, let me know.

Recipes

The added recipes are balanced against in-game recipes and require an Alchemy Table/Bottle crafting station to craft.

Current recipes:

Greater Healing Potion = 2x(Healing Potion) + Pixie Dust
Super Healing Potion = 4x(Greater Healing Potion)

CALAMITY ONLY:

Supreme Healing Potion = 4x(Super Healing Potion)

Shop

Unlike the Nurse Shop mod, items are not unlocked through stage progression; rather, each item is unlocked by killing individual bosses. This means that if you kill King Slime, the Nurse will start selling Healing Potions. If you kill Eye of Cthulhu before King Slime, Restoration Potion and the Nurse's Walkie Talkie will be unlocked, but Healing Potion won't be. Here is a full list of the items the Nurse will sell and their unlock conditions:

None: Mushroom, Bottled Water, Bottled Honey, Lesser Healing Potion, Nurse VIP Badge
King Slime: Healing Potion
Eye of Cthulhu: Restoration Potion
Brain of Cthulhu/Eater of Worlds: Nurse's Walkie Talkie
Skeletron: Nurse's Painted Shirt
Wall of Flesh: Lifeforce Potion, Greater Healing Potion, Nurse Nourishment Diamond
Ancient Cultist: Super Healing Potion

CALAMITY ONLY:

Moon Lord: Supreme Healing Potion
Devourer of Gods: Omega Healing Potion

TOGGLEABLE (DISABLED BY DEFAULT):

Eye of Cthulhu: Life Crystal
All Mech Bosses: Life Fruit

If there are healing-related items that people would find useful to be added to the shop, leave a comment, and I’ll see if I should add them.

There are also old ideas floating around for a more comprehensive overhaul, including quests to kill certain monsters that reset every Terraria day and medically inspired combat items. If that sounds interesting, leave a comment.

Special Thanks

A huge thank you to the legendary NotLe0n for allowing me to use his UI/UISystem to create the Nurse's shop. Core UI elements were adapted with modifications from his mod AnglerShop.

Another big thank you to Solafide Media for creating the sprites for the items and one of the buffs. They brought my vision for the items to life and did what I couldn’t. Check out his work on Fiverr.

Shout out to another legend, jopojelly, for helping me figure out how to automatically adjust my manual pricing based on the Nurse's happiness and pointing me to catGPT for shop help. It was amazing to Google something and see a response from them from 7+ years ago, then go to the Discord and have them answer one of my questions.

Also, shout out to ThomasthePencil and catGPT in the Discord for help with shop indexing in the 1.4.4 preview. Adding a shop to a vanilla NPC without a shop isn’t as straightforward as you might think, but they helped me get it over the finish line.

Gotta mention my boy TheEdster3 for guiding me through the beginning of my journey on this project, decompiling Terraria and planting the seed that would eventually become Nurse Overhaul. He left me with a heal that max-healed every game tick you were next to the Nurse, and now we’re here.

And thanks to the community in general for having so many resources. A lot of work goes into tModLoader and its supporting documentation, and I definitely took it for granted before diving into this project. If you’ve worked on any part of tModLoader, let me say from a simple player: thank you.

Recent Changes

1.0.9
Added community request for option to store nurse range increase items in bank

1.0.8.3
Edited documentation for errors and uniformity across platforms

1.0.8.2
Fixed a bug that disabled quick heal items by default for new players. If you were affected by this, you can manually turn on the items from the config menu, or just download this update and revert Nurse Overhaul's settings to default

1.0.8.1
When using Dialogue Panel, the Nurse Shop should no longer automatically open on hovering over the button

1.0.8
Reduced default price of Life Crystal to 30g and Life Fruit to 35g (still disabled by default)
Added ability to change price of Life Crystal and Life Fruit (minimum of 5g and 8g respectively)

1.0.7.5
Minor tweaks to text
Preparation for public GitHub release

1.0.7.4
Fixed a text error in the configuration menu

1.0.7.3
Added German Localization

1.0.7.2
Updated text output to reflect last patches bugfix

1.0.7.1
Fixed a bug where if you were in combat with a boss in Calamity, you would be charged to cure 1 debuff as well as health and other debuffs you had

1.0.7
Restructured internal shop code. Testing of old shop was stable but users have reported some issues so I've updated some of the logic so that it's more in line with how other's are coding their shops for vanilla NPCs without shops (shoutout again to NotLe0n and Angler Shop)
Added Russian localization

1.0.6
Further changes to internal money calculation, should be able to handle most extreme values of platinum now (1.4.4 exclusive, may backpatch later)

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
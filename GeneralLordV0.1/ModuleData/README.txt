# Disclaimer

This mod is more of a cheating/tooling mod than a gameplay one.
It can help designing and testing troops with two new shortcuts.
Be sure to back-up your game save before using this mod with it.

# Files and Folder

Keep the following files and folder in the ModuleData folder:
* 'CustomTroopRosterSettings.xml' this file is used when using the 'Ctrl + P' shortcut to retrieve custom roster character ids
* 'CustomTroopRosterOutput.xml' this file is used when using the 'Ctrl + E' shortcut to output equipment
* 'custom_troops' this folder contains the custom troop xmls and is referenced by the SubModule
Removing or renaming one of them could result in crashes/errors during playtime.
If you mess up with the files badly, try retrieving the mod again.

# Shortcuts

## Ctrl + P

This shortcut will open a troop roster containing 500 units of each character specified in 'CustomTroopRosterSettings.xml'.
You will be able to transfer any of them to your own party.

## Ctrl + E

This shortcut will ouput your current equipment in the 'CustomTroopRosterOutput.xml' in the xml format used to create character equipment sets.
For more information about custom unit creation, look inside 'custom_troops\example_troops.xml'.
Note that you can clear 'CustomTroopRosterOutput.xml' content but should not delete or rename the file.


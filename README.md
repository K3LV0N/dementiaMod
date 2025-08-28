# DementiaMod

## Overview

DementiaMod is a Terraria mod that introduces special boss item summoning mechanics. It allows modders to easily add their custom boss summon items to the mod using the `Call()` method during `PostSetupContent()`

## `Call()` methods

### Connecting to the dementia mod
The first thing that needs to be done is to establish communication between the mods. In your class that extends `Mod`, please write the following:
```csharp
public class <YourMod> : Mod {
	public override void PostSetupContent()
	{
	    if (ModLoader.TryGetMod("dementiaMod", out Mod dementiaMod))
		{  
	    }
	}
}
```
This allows you to access the dementia mod from your own mod and make calls from inside the `if` block.
### AddBossSummon
This call allows modders to add their boss summon and boss to the list of forgettable boss summon items. If modders wish to add their item, they need to call
```csharp
dementiaMod.Call("AddBossSummon", <itemID>, new int[] { npcIDs });
```
Where `itemID` is the type of your boss summon, and `npcIDs` are the types of the boss(es) that your item summons. This is to allow custom bosses that function like `NPCID.Retinazer` and `NPCID.Spazmatism` to summon properly. 

**Note:** if your boss summon AI spawns another boss (as in you have 1 main boss and the second gets spawned BY the first boss), only use the MAIN boss in the npcIDs section or else some funky behaviour might occur.
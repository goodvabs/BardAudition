# BardAudition
A keyboard synth controller for Windows that works when out-of-focus, intended for FFXIV Bard Performance self-audition.

## Latency is the enemy

Bard Performance in FFXIV is extremely fun, but the latency you get while using makes it almost unusable! But if you have this running in the background, it'll play back what you're playing to you with far less latency, making it possible to improvise during Bard Performance without getting tripped up by the latency.

On my 2.20 GHz processor I'm able to get consistently sub-30 ms latency even while the game is running.

## Windows only

Windows-specific DLL hooks are being used to detect low-level keypresses so this is an OS-specific application.

## .NET / C# Version

I'm targeting recent stuff with C# 10 and .NET 4.8 (as of writing: 2022-03-28). Dunno how much of the new stuff is actually necessary.

## Soundfont dependency

You can get the "Assets/TimGM6mb.sf2" soundfont dependency [here](http://www.timbrechbill.com/saxguru/Timidity.php). The sf2 file goes in an "Assets" folder next to the executable (or in the Assets folder in the Visual Studio solution, with its property set to copy into the executable directory.)

At some point when I know whether it's OK for me to redistribute that soundfont I can possibly just commit it to the repo.

## Key mappings JSON

There's a JSON file in the Assets directory (`Assets/performance_keybinds.json`) that defines the mapping from keyboard key (by "the character it makes when you press it on a US QWERTY keyboard configured as US QWERTY") to the note it should play when you hit it.

I assume the Bard Performance mapping starts at C3 and stops at C6 (inclusive). At some point I'll add an octave shift to support differing ranges (different instruments might be an octave up or down, not sure).

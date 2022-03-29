# BardAudition
A keyboard synth controller for Windows that works when out-of-focus, intended for FFXIV Bard Performance self-audition.

## Latency is the enemy

Still working on latency. The goal is for very low-latency synth operation so you can use it to hear yourself playing anything at any speed and not get tripped up by high latency. Success is less than 40 ms of delay, but I don't have measurements for current performance. Latency is high right now. 

## Windows only

Windows-specific DLL hooks are being used to detect low-level keypresses so this is an OS-specific application.

## .NET / C# Version

I'm targeting recent stuff with C# 10 and .NET 6 (as of writing: 2022-03-28). Dunno how much of the new stuff is actually necessary.

## Soundfont dependency

You can get the "Assets/TimGM6mb.sf2" soundfont dependency [here](http://www.timbrechbill.com/saxguru/Timidity.php). The sf2 file goes in an "Assets" folder next to the executable (or in the Assets folder in the Visual Studio solution, with its property set to copy into the executable directory.)

At some point when I know whether it's OK for me to redistribute that soundfont I can possibly just commit it to the repo.

## Key mappings JSON

There's a JSON file in the Assets directory (`Assets/performance_keybinds.json`) that defines the mapping from keyboard key (by "the character it makes when you press it on a US QWERTY keyboard configured as US QWERTY") to the note it should play when you hit it.

I assume the Bard Performance mapping starts at C3 and stops at C6 (inclusive). At some point I'll add an octave shift to support differing ranges (different instruments might be an octave up or down, not sure).

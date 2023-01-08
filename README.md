# VoxCharger #

Recharge your KFC Chicken sauce ([Download](https://github.com/SirusDoma/VoxCharger/releases/latest))

Written under C# language, this program allow you to manage music asset files of your KFC installation.  
Additionally, it include built-in converter and encoder to import ksh and it's audio files into vox and 2dx files that can be consumed by KFC.

If you're not familiar with these file formats, then this sauce is not for you.

## Features ##
- Create, manage and delete (omni)mix's in your KFC installation
- Import .vox or .ksh into one of your KFC (omni)mix with ease
- Automatically convert imported audio files (e.g mp3, ogg, flac, etc) to .2dx file
- Deferred asset modifications to keep your Music DB and asset files stay in sync until you save it
- Music DB metadata editor
- Standalone .ksh / .2dx file converter

## Prerequisite ##

### .NET Framework 4.7.2
This program require .NET Framework 4.7.2 in order to run properly.

### Media Pack / Desktop Experience
This program now has its own integrated Wav and 2DX Encoder and downloading [mon's 2dx tools](https://github.com/mon/2dxTools) is no **longer required**.
However, the audio encoder / decoder depends on [Media Foundation & ACM API](https://github.com/NAudio/NAudio) under the hood, which means support for most audio formats is only available in Windows 7 and newer.

Furthermore, you might need to install the following dependencies:
- [Media Feature Pack](https://support.microsoft.com/en-us/topic/media-feature-pack-list-for-windows-n-editions-c1c6fffa-d052-8338-7a79-a4bb980a700a) - Install this if you're running on Windows N or KN version
- [Desktop Experience](https://learn.microsoft.com/en-us/windows-server/get-started/install-options-server-core-desktop-experience) - Install this if you're running on Windows Server

### Latest datecodes only
This program won't load anything that doesn't match with `2020011500` or newer structure.

### Backup
Backup your data before using this program, it able to modify and delete your music assets.  
Keep in mind that the program won't allow you to make any changes against original mix.

### No IFS Support
Use [IFS LayeredFS](https://github.com/mon/ifs_layeredfs) in your KFC installation. This program will not pack your assets into IFS, nor attempt to process existing ones.
When mix with ifs files is selected, the program won't be able load music assets properly.

If you need to pack your music assets into ifs, use another tool that process ifs file (for an instance: [mon's ifs tools](https://github.com/mon/ifstools)).

## Remarks, Restrictions and Limitations ##

### Mix Lock
Original mix is locked to prevent you (yes, you) to break your KFC installation.  
Again, use [IFS LayeredFS](https://github.com/mon/ifs_layeredfs). If you haven't heard this then you're totally missing out!

### Conversion Output
Vox have some sense in it's file format than ksh file, as the result, not all attributes can be mapped precisely and potentially lead into bug in the output file. For FX mapping, user defined FX will be ignored, only basic FX's that will be included into the output.

Remember, stupid input get stupid output. But if you believe it's a bug, feel free to open issue or PR.

### Network Scores
Make sure you are running the game under offline environment and **NOT** connected to any KFC network server while using the output files of this tool. It could break network score table and you may get banned from the network for doing so.

### Music DB
The program also prevent you to modify Music ID and some attributes are kept hidden from editor. For existing songs, the program will try to preserve the original metadata throughout save iteration.
However, attributes that not recognized by the program might be lost.

Make sure to backup your `music_db.xml` / `music_db.merged.xml` if you have non-standard attributes in your music db or attributes that newly introduced after `2022` latest datecode (Note: `radar` is supported!).

### Asset File Modification
Replacing asset files such as vox, 2dx and graphic files are happen immediately after changes are confirmed. In other hand, metadata need to be saved manually by clicking `File -> Save` or `CTRL+S`.

Note that you can postpone asset modification until you save metadata by disabling autosave in `Edit -> Autosave Assets`. This allows the asset files to stay in sync with the metadata, might be useful for certain modding workflow.

# License #

This is an open-sourced application licensed under the [MIT License](http://github.com/SirusDoma/VoxCharger/blob/master/LICENSE)

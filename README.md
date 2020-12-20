# MapleServer2
>An open source MapleStory2 server emulation project created in C#.

[Community Discord](https://discord.gg/eVGMGydwgm)

## Setup

### Requirements
Download and install **[Visual Studio or Visual Studio Code](https://visualstudio.microsoft.com/)**.
>Visual Studio is only for Windows, and make sure you get the community edition if you choose to install Visual Studio.

Have an up-to-date GMS2 client with **[Orion2.dll](https://github.com/EricSoftTM/Orion2-Client)** injected.
>It is somewhat technical to do this step, so you can choose to simply download **[this](https://www.youtube.com/redirect?q=https://drive.google.com/file/d/1DVDryRO3AnkeSH-n1lp7bv9aurVDdPKy/view&v=H8WQWqnVL7U&event=video_description&redir_token=QUFFLUhqazd0ejhLSDRzQlp0UXFibzhPcEpCQUcya0JEZ3xBQ3Jtc0trb3B4MjNIaFdnMEZOSmhLa0VPSzg5UWNFaG1tMVVTTHgzWVZ4TDdGX19DOWZZVjB6WjRJZzZhMF9yeGxUX25wOHk3YUFtY3pYcVFDeWhHcTBudllDUlhKWW9hYmRyLWpJb3ExRFZoVHl1N1F2aTVhSQ==)** already set up client instead **[\[mirror\]](https://www.youtube.com/redirect?q=https://mega.nz/file/oU8GESxC#CdkjdSkODEpcIJ-p7HGrYaID5BrO0_WfbKSynf6Or_E&v=H8WQWqnVL7U&event=video_description&redir_token=QUFFLUhqblJhcXM1czZGWTRtZHU5ZXVJYVBmOUlOUkhUd3xBQ3Jtc0tsUWUtNXBxeXB5X2wweF91cWFwQVVzTWNWVTFyamhnTFoxMWFwcmFPVWlxYlZZNTdzSjdES1V0YTRTYTJRWHIyVUJCancyeUV6T1BONjdHUTFlRGc2YmRuUXluR0RRbWZpR0xwMWRKY3dER0dIeEJnVQ==)**.

>Note that this download includes a version of the server but it is out of date.

**[Download](https://github.com/AlanMorel/MapleServer2/archive/master.zip)** or clone/fork this GitHub repository.
>If you are familiar with Git then it is recommended to clone or fork the repository, otherwise, use the download link.

### Configuration (Visual Studio)
Open the solution in Visual Studio and then go to `Project > Properties > Configuration Properties > Debugging`.

Then change the working directory to `$(SolutionDir)`.

### Configuration (Visual Studio Code)
Open the solution as a workspace and then go to `Extensions` and install the `C#` extension.

Then go to `Run > Open Configurations` and change the value of `"cwd"` to `"${workspaceFolder}"`.

### Need to be implemented
- Achieve
- Quest (All types)
- Mobs, from Npc (diferent behavior, moving around, spawning on field, more )
- Attributes on Player
- Dungeons (Instance of dungeons, rewards, more)
- Exp System (Level up, get exp from quest, mob exp, more)
- Battle System (Do damage, taking damage, skills damage, affecting stats, more)
- BlackMarket (selling item, buying item, list on market, retrieving currency, more)
- Pets (affecting stats, more)
- Mounts
- Cutscenes
- Gathering
- Housing
- Crafting
- Prestige System
- Adventure System
- Reputation
- Traits
- Lapenshards
- World Map
- Minimap
- CharacterInfo (Title, insignia, Mastery, more)
- Networking (needs to be rewritten)

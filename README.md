# SF-Platformer-Package
This is Shatter Fantasy Platformer Unity package that can be used to create any game needing platformer like controls. 

## Future Alpha Two Release
Currently the second alpha is being worked on a seperate branch from the main branch. 

### Already Completed Upcoming Features:

#### Physics
- Quite of a bit of improvements brought to the custom collision detection calculations in the Controller2D for physics.
- Physics improvements brought by the implementation of ColliderDistance2D struct usage.
- Added a list of CollisionHits that can be read from in any scenario they are needed in.

#### Event System
- Added a GameEvent EventListener for triggering events that change the control of the game like when pausing the game.
- Added a ApplicationEvent EventListener for listening to application events like shutting down when quitting the game.
- Added a GameMenu EventListener for triggering events when users interact with UI elements or press an input shortcut for brining up UI like the in game menu.

### Currently In Progress Features
- AIState for giving characters the ability to do detection systems like seeing while patrolling an area in games.
- AIState for NPC characters to patroll an area back and forth.
- Options menu system for volume control and some graphic setting event listeners.
   - Fullscreen, windowed, borderless window
   - Background music, master, sound effect, and ambient sound volume settings.

## Demo Videos
There will be demo videos as soon as I finished the options menu events.

## WIP API Documentation.
Please note the documentation is very early wip. The manual link at the top left is not ready yet. So clicking it does nothing. We are working on videos currently for the manual. 
Currently somethings are not in the final location for namespaces. In the root SF namespace you will see some classes, structs, or interfaces needing moved to their proper namespaces.

[WIP SF Platformer Documentation](https://crowhound.github.io/SF-Platformer/api/SF.Physics.CollisionInfo.html)

## Install Instructions
This package was built with the idea of using Unity's built in package manager to help make installing and choosing which version of the package you want to use easier.
Here is the official Unity documentation page if you want to do a full read through instead of reading the short answer below.

[Unity Custom Git Package Documentation](https://docs.unity3d.com/6000.0/Documentation/Manual/upm-git.html#extended)

1. Open up Unity's package manager editor window and click the button with a plus sign at the top left of the package manager window. It should have a small dropdown arrow icon by the button with a plus sign on it.

![install-instructions-1](https://github.com/user-attachments/assets/de316cc8-5498-4496-b702-221b6f2b73f7)

   
2. Choose install package from git url and paste in the following.

https://github.com/crowhound/SF-Platformer.git

Optional for choosing a specific version of the SF Platformer package to install.
Unity supports git revision syntax allowing you to add options at the end of the Git url to customize your package download.
All options are added onto the git url after the pound symbol # is added to the end.

The options are as followed:
1. Specific branch  - #name-of-branch
2. Specific version for package release. Note the letter v before the numbers - #v0.0.1 would give you the first alpha release while for pre-alphas you would type #pre-alpha.9
3. A specific commit if you want to try out a commit with a feature that hasn't been published in a release yet - #git-commit-hash
Example for the specific commit hash #76c6efb35ac8d4226a22f974939f300231a3637f. This is the hash for the commit added right before pre-alpha 9 release.

Full example for wanting to get the SF Package that is release version alpha 1
https://github.com/crowhound/SF-Platformer.git#v0.0.1

Full example for wanting to get the SF Package that is being worked on in the alpha-two branch
https://github.com/crowhound/SF-Platformer.git#alpha-two


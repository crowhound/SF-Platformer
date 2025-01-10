# SF-Platformer-Package
This is Shatter Fantasy Platformer Unity package that can be used to create any game needing platformer like controls. 

## Pre-alpha repo management warning
Heads up during the pre-alpha most things were just committed directly to the main branch of the repo.
Starting with alpha one we will have a branch for each development release cycle and we will be grouping feature sets into pull requests for organization.
Example the future audio features will be in a pull-request called Audio-Manager-1. When it is finished, tested, and reviwed than it will be merged into the current release cycle being worked on.

## Demo Videos
There will be demo videos being recorded as progress is made on the package to show off how to do a few things.
Here is an example of using the pre-alpha 10 version being worked on to make a crushing ai enemy.

[SF Platformer Crushing AI Demo](https://www.youtube.com/watch?v=jRTSwovfHqQ)


## WIP Documentation.
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


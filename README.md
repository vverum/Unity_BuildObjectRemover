# Build Object Remover

The **Build Object Remover** allow you to automatically remove object with selected tag from scenes from a build.

## Overview

By default Unity Editor removes objects with tag 'EditorOnly' from build and runtime.<br/>
**Build Object Remover** allow you to add more tags that will be removed from selected platforms and build types.

![Tool view in editor][ViewImage]

## Installation

### Via Package manager as gitURL
1. Copy https link to this repository<br/>
https://github.com/vverum/Unity_BuildObjectRemover.git
2. Open package manager in unity 
3. Press Add button (+ in the upper left corner) and select `Add package from git URL...`
4. Paste link and press `add`

### Via Package manager as local file
1. Download package (tgz file) from [releases](https://github.com/vverum/Unity_BuildObjectRemover/releases)
2. Move file to project location
3. Open package manager in unity 
4. Press Add button (+ in the upper left corner) and select `Add package from tarball...`
5. Select package file

### Unity Asset Store
1. install normally like any unity asset from
[Unity Asset Store][AssetStoreLink]

## Setup **Build Object Remover**

**Build Object Remover** view is located in ***Project Settings***

### Adding and removing tags<br/>
1. Adding and removing tags is as simple as pressing the appropriate button.
![Add and remove][AddRemoveImage]
2. Select correct tag from drop-down list with tags in new created row.
3. Enable the row with selected tag.
4. Select correct build type.
5. Done.

### Seting build type
In `Remove from build types` you are selecting the type of build when the object will be removed.

![Build Types][BuildTypeImage]
* Nothing - does not remove from any runtime or build
* Everything - remove from any runtime or build
* Editor Play Time - remove from play time in editor 
* Development - remove from builds that have checked Development option
* Release - remove from build that is not development

For example:<br/>
If only 'Development' is selected, then objects in "Development" build will remove, but objects in the release build will not be removed.

### Add platform
1. Press `Add Platform` button.
2. Select the platform you want to add options to.
3. Press `Add Platform` again.

A tag set on a specific platform will not override the tag setted in `All platforms`!

### Do not forget to save changes
Press `Apply` button to save current changes.<br/>
Press `Revert` button to discard currently unsaved changes.<br/>

Changes that are not applied before build will not take effect!

## Debug
Tags marked to remove will be listed in log in console after building process.
![Add and remove][BuildLogImage]

## This tool is Open Sourced
[github.com/vverum/Unity_BuildObjectRemover][GitRepoLink]

## LICENSE: Modified MIT License (MIT)
This project's license is available in the provided "[LICENSE.md](LICENSE.md)" file.


[ViewImage]: Documentation~/BuildObjRemoverScreenshot.png?raw=true "Tool view in editor"
[AddRemoveImage]: Documentation~/AddRemoveTagScreenshot.png
[BuildTypeImage]: Documentation~/BuildTypeScreenshot.png
[BuildLogImage]: Documentation~/LogScreenshot.png
[GitRepoLink]: https://github.com/vverum/Unity_BuildObjectRemover
[AssetStoreLink]: https://assetstore.unity.com/packages/tools/utilities/build-object-remover-185552
# Unity Fog Of War

[中文传送门](http://oldking.wang/2ed40ca0-3616-11e9-a000-1f6ee05e8fdd/)

inspire by [AsehesL](https://github.com/AsehesL/FogOfWar) and [smilehao](https://github.com/smilehao/fog-of-war) and [Ultimate Fog of War](https://assetstore.unity.com/packages/tools/utilities/ultimate-fog-of-war-76011)

### Preview

![](Img/FogOfWar.gif)


### How To Use

#### Init

 fist CopyEngine Fog of War (CEFow) is not extend MonoBehaviour. so you need call 
 
 ```
  CEFowFacade.instance.InitAsNew($propery);
 ```

when the game start.

the $propery you pass in is defined in `CEFowProperty.cs` file. check for more detail in that file.

#### Update

as the CEFow is not extend MonoBehaviour, so you need call 

```
CEFowFacade.instance.Update();
``` 

the frequency of this function called is up to you, you can call it once pre frame or once pre second.

even you can call it only when some explorer changed.

it will not hit any performance issue for call it once pre frame. CEFow will only repaint the Fow texture when something really changed.

#### Explorer

there are two type of explorer , static and not static. check more detail with the demo project.

#### Stalker

CEFow provide a function 

```
CEFowFacade.IsWorldPosInView($worldPosition)
```

let you check is the point in fog or not.

check more detail in `DebugStalker.cs` file


### More

if you curiousness on how i made this,please check more info on my [Blog](http://oldking.wang/2ed40ca0-3616-11e9-a000-1f6ee05e8fdd/)



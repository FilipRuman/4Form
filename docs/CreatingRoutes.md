# Before creating maps please setup your scene as described [in map export documentation](./)
If you want to test out your new map you just need to press the export button on whole map export script.![image](https://github.com/user-attachments/assets/cbfa2531-b892-4ab7-b2fe-acf52c504d7d)

Then after re opening the game you will be able to open your new map!
# Configuring map settings
Map script has multiple properties, most important ones are:
* Name: value of this will be used to name: folder that contains exported map, name that will be used to import this map with "manual map load name", name that will be displayed in game. 
* Speed scale: This value will affect: displayed scale of player, calculated stats of route(length, height), slope. the main usage of it are: when you make the map to big by accident or when bike model is to small.
* Drag coefficient: Increases the total drag on this map. 
* Routes: you need to place here all routes that you want to export. 
* Can edit bike models: whether the user will be able to edit bike models in game.
## Creating Bike models
# Creating terrain
The terrain tool that is used by this project is [Terrain3D](https://github.com/TokisanGames/Terrain3D) so please refer to their [tutorials](https://terrain3d.readthedocs.io/en/stable/docs/tutorial_videos.html) for instructions.
Everything from Instance meshes, to textures should be exported smoothly. 
> [!Caution]
>  **Exporting [``Terrain3DMaterial``](https://terrain3d.readthedocs.io/en/stable/api/class_terrain3dmaterial.html) is not currently  supported**
> The default one on main scene's terrain 3D true export will be used on all imported scenes in game.
# 3D meshes
You can export any 3D mesh, lights and other things supported by [GLTF format](https://www.khronos.org/files/gltf20-reference-guide.pdf)
> [!Caution]
> **Sometimes materials might not be imported correctly**
> 99% of the time your materials will be exported correctly, but if you try to do something weird then it might not be supported by gltf and Godot.
> 
> [Custom shaders are not supported! (We might think about adding them in the future if it could be done safely)](https://docs.godotengine.org/en/stable/classes/class_gltfdocument.html#class-gltfdocument) 


# Routes
## Setup:
Structure all of your routes like this: 

![image](https://github.com/user-attachments/assets/94b81186-b0f8-4010-aed8-c48abc41857e)

Ensure that you have collision mode set to `` Dynamic / Editor`` with reasonable collision radius(this will depend on your hardware).

![image](https://github.com/user-attachments/assets/9893a8bb-37dd-4562-a3e2-45c8ce01af65)

You should be able to see the generated collisions like this:

![image](https://github.com/user-attachments/assets/f5f6cfdf-f68d-4031-ade6-ca6cd468210c)

Fill references of ``RouteCreationTool`` like this:

![image](https://github.com/user-attachments/assets/ebb21f28-edd7-45b3-9717-566eb021dd63)

You should copy the  ``Slope color gradient`` from routes in included maps.

Check the ``Run`` and ``Highlight Path`` properties

## Making new route

You use the ``rough`` path to tell the output path where it should go. 

You can edit it just by selecting it and using default Godot tools

![image](https://github.com/user-attachments/assets/b7546882-e463-4bc8-b939-040835d99f3a)

``output`` path should automatically follow the terrain3D, in the part that has generated collision mesh(Or any other object with colider).

To generate output path at certain point, just move your camera over it! 

To change resolution of output path change the bake interval on your rough path.
 
![image](https://github.com/user-attachments/assets/04c4e4ec-6844-459f-bcb8-448543726be2)

> [!Note]
> After changing bake interval you will need to go over and regenerate all of output path

The points that are generated on top of the terrain show path of your output path and color of them shows slope.

![image](https://github.com/user-attachments/assets/5f69d831-1efd-48f5-abb9-319194b0b65c)

## Configuring your route
You can also change settings of your route. 

![image](https://github.com/user-attachments/assets/c4ef2dff-6490-464e-a7f3-523563fb5e0a)

Route stats show automatically calculated values of your route
# [Example video](./ExportingMaps.md)

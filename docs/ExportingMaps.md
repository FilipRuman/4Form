# Importing map just to play it
### all of this should be done by steam workshop in the future :D
1. Get folder with map that you want to import- it's name should be the name of the map that you want to import
2. drag it to: ``*user folder/Maps/``
 *user paths on different os: https://docs.godotengine.org/en/stable/tutorials/io/data_paths.html#editor-data-paths
### Done. you can now run game and you should be able to find this map.
If you have any problems please write bug report under the issues page
# For map developement
##🛠️Setup
### Setup your scene similar to blank map scene (if you case your scene of [blank map](./../godot/Map/blank_map.tsc) and eaverything works, you can skip this!)
 0. place ``Map.cs`` at the root of the scene and add fill it's references
 1. place ``terrain3DTrueExport.gd`` script on node under terrain3D node and fill it's references (look at blank map scene)
 2. add ``res://addons/terrain_3d/tools/importer.gd`` script to terrain 3D node
 3. add ``WholeMapExport.cs`` and ``RouteExport.cs`` and fill it's references (look at blank map scene)
### Final setup
 0. Make sure all routes are referenced by map script(root of the scene)
 1. Ensure that all 3D models are under the node with ``WholeMapExport.cs``. not routes, terrain3D. only mesh instances (and lights)

## Export
> [!Warning]
> **Read before proceding**
> Runing export will override all files saved under the directory with maps name.

* you can now run export on ``WholeMapExport.cs`` 
your exports should be under: *userDirectory/Maps/**MapName/Terrain3D/
 *user paths on different os: https://docs.godotengine.org/en/stable/tutorials/io/data_paths.html#editor-data-paths
 **MapName- name found on Map script that this scrpt has reference to
* to share your map just copy whole folder with map's name and paste it (in the same place) on the other device under Maps folder
* it should be automatically discovered next time you open the game
## Import
> [!Warning]
> **Read before proceding**
> Runing import will override current terrain 3d, routes and mesh instances
* set ``manualMapLoadName`` in ``WholeMapExport.cs`` to be name of map(folder) that you want to import.
* click ``manualMapLoad``
If you have any problems please write bug report under the issues page

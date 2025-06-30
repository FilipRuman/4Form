@icon("res://Script icons/publish.png")
@tool
extends Node3D
# usage:
# TODO add this to documentation
# 1. place this script on node under terrain3D node
# 2. add res://addons/terrain_3d/tools/importer.gd script to terrain 3D node
# 3. fill all references for this script
# 4. you can run import / export
# 5. your exports should be under: *userDirectory/Maps/**MapName/Terrain3D/
# *user paths on different os: https://docs.godotengine.org/en/stable/tutorials/io/data_paths.html#editor-data-paths
# **MapName- name found on Map script that this scrpt has reference to
@export var  terrain : Terrain3D
@export var  map : Node 
@export var  export : bool
@export var  import : bool
@export var  default_material: Terrain3DMaterial
## this is potentialy unsafe because of use of .tres files
## more safe way is to use default material
## later in the future could mannualy create .tres file from json
## i have to do this currently because setting shader parameter function doesn't work D:
@export var  should_import_material: bool


func  _process(delta: float) -> void:
	if export :
		run_export()
		export = false
	if import :
		run_import()
		import = false

#region export
var  map_name : String
func  run_export() -> void:
	map_name = map.get("name")
	DirAccess.open("user://Maps/").make_dir(map_name)
	DirAccess.open("user://Maps/%s/" % [map_name]).make_dir("Terrain3D")
	DirAccess.open("user://Maps/%s/Terrain3D" % [map_name]).make_dir("Assets")
	DirAccess.open("user://Maps/%s/Terrain3D" % [map_name]).make_dir("Material")
	DirAccess.open("user://Maps/%s/Terrain3D" % [map_name]).make_dir("Regions")
	
	export_regions()
	handle_assets_export()
	handle_material_export()

func export_regions():
	# mark all regions modified
	for regionPosition in ($'..'.data as Terrain3DData).get_regions_all().keys():
		($'..'.data as Terrain3DData).set_region_modified(regionPosition,true)
	
	$'..'.destination_directory = "user://Maps/%s/Terrain3D/Regions/" % [map_name]
	$'..'.save_to_disk = true
	DirAccess.remove_absolute("user://Maps/%s/Terrain3D/Regions/assets.tres");
	DirAccess.remove_absolute("user://Maps/%s/Terrain3D/Regions/nav_mesh.res");

func handle_assets_export():
	DirAccess.open("user://Maps/%s/Terrain3D/Assets/" % [map_name]).make_dir("Textures");

	for texture_class in terrain.assets.texture_list:
		DirAccess.open("user://Maps/%s/Terrain3D/Assets/Textures" % [map_name]).make_dir(texture_class.name);
		var asset_dir = "user://Maps/%s/Terrain3D/Assets/Textures/%s" % [map_name,texture_class.name]
		texture_class.albedo_texture.get_image().save_png(asset_dir+"/Albedo.png");
		texture_class.normal_texture.get_image().save_png(asset_dir+"/Normal.png");
		var other_data = {
		"Name": texture_class.name,
		"ID" : texture_class.id,
		"Color":texture_class.albedo_color.to_html(false),
		"Normal":texture_class.normal_depth,
		"AO":texture_class.ao_strength,
		"Roughness":texture_class.roughness,
		"UV":texture_class.uv_scale,
		"DetailingRot":texture_class.detiling_rotation,
		"DetailingShift":texture_class.detiling_shift}
		var json_string = JSON.stringify(other_data)
		var file = FileAccess.open(asset_dir+"/OtherData.json", FileAccess.WRITE)
		file.store_string(json_string)
	

	DirAccess.open("user://Maps/%s/Terrain3D/Assets/" % [map_name]).make_dir("Meshes");
	for mesh_class in terrain.assets.mesh_list:
		DirAccess.open("user://Maps/%s/Terrain3D/Assets/Meshes" % [map_name]).make_dir(mesh_class.name)
		var asset_dir = "user://Maps/%s/Terrain3D/Assets/Meshes/%s" % [map_name,mesh_class.name]
		
		var gltfDocumentSave = GLTFDocument.new()
		var gltfStateSave = GLTFState.new()
		gltfDocumentSave.append_from_scene(mesh_class.scene_file.instantiate(), gltfStateSave)
		gltfDocumentSave.write_to_filesystem(gltfStateSave, asset_dir + "/Scene.glb")

		var other_data = {
		"name": mesh_class.name,
		"id" : mesh_class.id,
		"enabled": mesh_class.enabled,
		"generated_type" : mesh_class.generated_type,
		"height_offset" : mesh_class.height_offset,
		"density" : mesh_class.density,
		"cast_shadows" : mesh_class.cast_shadows,
		"lod_count" : mesh_class.lod_count,
		"last_lod": mesh_class.last_lod,
		"last_shadow_lod": mesh_class.last_shadow_lod,
		"shadow_impostor": mesh_class.shadow_impostor, # amogus
		"lod0_range": mesh_class.lod0_range,
		"lod1_range": mesh_class.lod1_range,
		"lod2_range": mesh_class.lod2_range,
		"lod3_range": mesh_class.lod3_range,
		"lod4_range": mesh_class.lod4_range,
		"lod5_range": mesh_class.lod5_range,
		"lod6_range": mesh_class.lod6_range,
		"lod7_range": mesh_class.lod7_range,
		"lod8_range": mesh_class.lod8_range,
		"lod9_range": mesh_class.lod9_range,
		}

		var json_string = JSON.stringify(other_data)
		var file = FileAccess.open(asset_dir+"/Data.json", FileAccess.WRITE)
		file.store_string(json_string)
		
func handle_material_export() :
	var material : Terrain3DMaterial=$'..'.material
		#var material_json = {
			#"world_background": material.world_background,
			#"texture_filtering" : material.texture_filtering,
			#"auto_shader":material.auto_shader,
			#"dual_scaling":material.dual_scaling,
			#"shader_override_enabled":material.shader_override,
			#"auto_slope":material.auto_slope,
			#"auto_height_reduction":material.auto_height_reduction,
			#"auto_base_texture":material.auto_base_texture,
			#"auto_overlay_texture":material.auto_overlay_texture,
			#"dual_scale_texture":material.dual_scale_texture,
			#"dual_scale_reduction":material.dual_scale_reduction,
			#"tri_scale_reduction":material.tri_scale_reduction,
			#"dual_scale_far":material.dual_scale_far,
			#"dual_scale_near":material.dual_scale_near,
			#"height_blending":material.height_blending,
			#"world_space_normal_blend":material.world_space_normal_blend,
			#"enable_projection":material.enable_projection,
			#"projection_threshold":material.projection_threshold,
			#"projection_angular_division":material.projection_angular_division,
			#"mipmap_bias":material.mipmap_bias,
			#"depth_blur":material.depth_blur,
			#"bias_distance":material.bias_distance,
			#"enable_macro_variation":material.enable_macro_variation,
			#"macro_variation1":material.macro_variation1.to_html(false),
			#"macro_variation2":material.macro_variation2.to_html(false),
			#"macro_variation_slope":material.macro_variation_slope,
			#"noise1_scale":material.noise1_scale,
			#"noise1_angle":material.noise1_angle,
			#"noise1_offset_x":material.noise1_offset.x,
			#"noise1_offset_y":material.noise1_offset.y,
			#"noise2_scale":material.noise2_scale,
			#"noise3_scale":material.noise3_scale,
			#"world_noise_fragment_normals" : material.world_noise_fragment_normals,
			#"world_noise_region_blend":material.world_noise_region_blend,
			#"world_noise_max_octaves":material.world_noise_max_octaves,
			#"world_noise_min_octaves":material.world_noise_min_octaves,
			#"world_noise_lod_distance":material.world_noise_lod_distance,
			#"world_noise_scale":material.world_noise_scale,
			#"world_noise_height":material.world_noise_height,
			#"world_noise_offset_x":material.world_noise_offset.x,
			#"world_noise_offset_y":material.world_noise_offset.y,
			#"world_noise_offset_z":material.world_noise_offset.z,
			#}
		#
		#var json_string = JSON.stringify(material_json)
		#var file = FileAccess.open("user://Maps/%s/Terrain3D/Material/Material.json" % [map_name], FileAccess.WRITE)
		#file.store_string(json_string)
		#material.noise_texture.get_image().save_png("user://Maps/%s/Terrain3D/Material/Noise.png" % [map_name]);
	ResourceSaver.save(material,"user://Maps/%s/Terrain3D/Material/Material.tres" % [map_name],ResourceSaver.FLAG_NONE)

#endregion
#region import
func  run_import() :	
	map_name = map.get("name")
	
	$'..'.clear_all = true
	
	if should_import_material :
		$'..'.material = import_material()
	else : 
		$'..'.material = default_material
	$'..'.data_directory = "user://Maps/%s/Terrain3D/Regions/" % [map_name]
	
	DirAccess.remove_absolute("user://Maps/%s/Terrain3D/Regions/assets.tres");
	DirAccess.remove_absolute("user://Maps/%s/Terrain3D/Regions/nav_mesh.res");
	
	var assets = Terrain3DAssets.new()
	assets.set_texture_list(handle_texture_assets())
	
	assets.mesh_list = handle_mesh_assets()
	$'..'.set_assets(assets);
func handle_mesh_assets() -> Array[Terrain3DMeshAsset]:
	var dir = DirAccess.open("user://Maps/%s/Terrain3D/Assets/Meshes" % [map_name])
	var mesh_assets = Array([], TYPE_OBJECT, "Terrain3DMeshAsset", Terrain3DMeshAsset)  
	if dir:
		dir.list_dir_begin()
		var file_name = dir.get_next()
		while file_name != "":
			if dir.current_is_dir():
				mesh_assets.append(import_mesh_asset(file_name))
				
			file_name = dir.get_next()
	return mesh_assets;
	
func handle_texture_assets() -> Array[Terrain3DTextureAsset]:
	var dir = DirAccess.open("user://Maps/%s/Terrain3D/Assets/Textures" % [map_name])
	var texture_assets = Array([], TYPE_OBJECT, "Terrain3DTextureAsset", Terrain3DTextureAsset)
	if dir:
		dir.list_dir_begin()
		var file_name = dir.get_next()
		while file_name != "":
			if dir.current_is_dir():
				texture_assets.append(import_texutre_asset(file_name))
				
			file_name = dir.get_next()
	return texture_assets;
func import_mesh_asset(asset_name:String) -> Terrain3DMeshAsset:
	var asset_dir = "user://Maps/%s/Terrain3D/Assets/Meshes/%s" % [map_name,asset_name]
	var mesh_class = Terrain3DMeshAsset.new()
	var gltfDocumentLoad = GLTFDocument.new();
	var gltfStateLoad = GLTFState.new();
	var error =gltfDocumentLoad.append_from_file(asset_dir + "/Scene.glb", gltfStateLoad);
	print("import_mesh_asset error: ", error);
	var gltfSceneRootNode = gltfDocumentLoad.generate_scene(gltfStateLoad); 
	var packed  =PackedScene.new()
	packed.pack(gltfSceneRootNode);
	add_child(packed.instantiate())
	mesh_class.scene_file = packed;    
		
	var file = FileAccess.open(asset_dir+"/Data.json", FileAccess.READ)
	var content = file.get_as_text()
	
	var json = JSON.new()
	var errorJson = json.parse(content)
	
	if errorJson == OK:
		var data = json.data
		if typeof(data) == TYPE_DICTIONARY:
			mesh_class.name = data["name"]
			mesh_class.id = data["id"]
			mesh_class.enabled = data["enabled"]
			mesh_class.generated_type = data["generated_type"]
			mesh_class.height_offset = data["height_offset"]
			mesh_class.density = data["density"]
			mesh_class.cast_shadows = data["cast_shadows"]
			mesh_class.last_lod = data["last_lod"]
			mesh_class.last_shadow_lod = data["last_shadow_lod"]
			mesh_class.shadow_impostor = data["shadow_impostor"] # amogus
			for i in data["lod_count"]:
				mesh_class.set_lod_range(i as int,data["lod%s_range"% [i as int]])

		else:
			print("Unexpected data")
	else:
		print("JSON Parse Error: ", json.get_error_message(), " in ",content, " at line ", json.get_error_line())
	return mesh_class;
func  import_texutre_asset(asset_name:String) -> Terrain3DTextureAsset:
		var  texture_asset = Terrain3DTextureAsset.new()
		var asset_dir = "user://Maps/%s/Terrain3D/Assets/Textures/%s" % [map_name,asset_name]
		var file = FileAccess.open(asset_dir+"/OtherData.json", FileAccess.READ)
		var content = file.get_as_text()
		
		var json = JSON.new()
		var error = json.parse(content)
		
		if error == OK:
			var data = json.data
			if typeof(data) == TYPE_DICTIONARY:
				texture_asset.name = data["Name"]
				texture_asset.id = data["ID"] as int
				
				texture_asset.albedo_color = Color( data["Color"]) 
				texture_asset.normal_depth = data["Normal"] as float
				#texture_asset.ao_strength = data["AO"] as float
				#texture_asset.roughness = data["Roughnes"] as float
				texture_asset.uv_scale = data["UV"] as float
				texture_asset.detiling_rotation= data["DetailingRot"] as float
				texture_asset.detiling_shift=data["DetailingShift"] as float
				var albedo = Image.load_from_file(asset_dir+"/Albedo.png");
				albedo.generate_mipmaps()
				texture_asset.albedo_texture =ImageTexture.create_from_image(albedo)
				var  normal = Image.load_from_file(asset_dir+"/Normal.png");
				normal.generate_mipmaps()
				texture_asset.normal_texture =ImageTexture.create_from_image(normal)
			else:
				print("Unexpected data")
		else:
			print("JSON Parse Error: ", json.get_error_message(), " in ",content, " at line ", json.get_error_line())
		
		return texture_asset;

func import_material():
	var material = Terrain3DMaterial.new()
	material = ResourceLoader.load("user://Maps/%s/Terrain3D/Material/Material.tres" % [map_name])
	return material;

#endregion 

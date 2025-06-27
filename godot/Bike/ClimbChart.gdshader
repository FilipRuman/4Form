shader_type canvas_item;
// if chart cuts off weirdly at the end, you need to increase resolution
const int RESOLUTION = 2000;
uniform int points_count;
uniform float[RESOLUTION] slope_map;// -1 -> 1 script edits this
uniform float[RESOLUTION] height_map;//  script edits this
uniform float max_height;//  script edits this
uniform float min_height;//  script edits this
// color based on slope % -> left is -100% right 100%
uniform sampler2D slope_color;// you can edit this
uniform sampler2D alpha_based_on_distance_from_line; // you can edit this
void fragment() {

	int map_index =int(UV.x * float(points_count));
	int next_map_index = min( map_index  +1 ,points_count - 1);
	float next_map_index_percentage = float(map_index) - UV.x * float(points_count);

	float pixel_height = min_height + UV.y *(max_height - min_height);
	float distance_from_line =  pixel_height - mix(height_map[map_index],height_map[next_map_index],1. - next_map_index_percentage ) ;
	float normalized_distance_from_line = distance_from_line / (max_height - min_height) /2.0 + .5;
	float alpha = texture(alpha_based_on_distance_from_line,vec2(normalized_distance_from_line,0)).r;

	vec3 albedo = texture(slope_color,vec2(slope_map[map_index] /2.0 + .5f,0.0)).rgb;
	COLOR = vec4(albedo.r,albedo.g,albedo.b,alpha);

}

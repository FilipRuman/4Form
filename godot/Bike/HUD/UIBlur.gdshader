shader_type canvas_item;

uniform float blur_amount : hint_range(-2.0, 10.0);
uniform float mix_amount : hint_range(0.0, 1.0);
uniform vec4 color_over : source_color;
uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_linear_mipmap;

void fragment() {
   vec4 blurred = textureLod(SCREEN_TEXTURE, SCREEN_UV, blur_amount);
   blurred.a = 255.0;
   vec4 fin = mix(blurred, color_over, mix_amount);
   COLOR = fin;
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}

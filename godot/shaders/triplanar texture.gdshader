shader_type spatial;

uniform float scale;
uniform float brightness;
uniform float saturation;

uniform sampler2D albedo;
uniform sampler2D normal;
uniform sampler2D roughness;
uniform sampler2D metallic;


vec4 Triplanar (vec3 position, vec3 worldNormal, sampler2D sampler){
	vec3 blendAxes = abs(worldNormal);
	blendAxes /= blendAxes.x + blendAxes.y + blendAxes.z;

	vec4 xProjection = blendAxes.x * texture(sampler,position.yz );
	vec4 yProjection = blendAxes.y * texture(sampler,position.xz );
	vec4 zProjection = blendAxes.z * texture(sampler,position.xy );

	return xProjection + yProjection + zProjection;
}
float InvLerp(float a, float b, float v){
	return clamp((v-a) / (b-a),0,1);
}

void fragment() {
	vec3 worldPosition = ( INV_VIEW_MATRIX * vec4(VERTEX,1.0) ).xyz;
	vec3 worldNormal = (INV_VIEW_MATRIX * vec4(NORMAL, 0.0)).xyz;
	worldPosition /= scale;

	ALBEDO = brightness * Triplanar(worldPosition,worldNormal,albedo).rgb;
	ALBEDO -= vec3(1) *saturation;
	ROUGHNESS =  Triplanar(worldPosition,worldNormal,normal).r;
	NORMAL_MAP = Triplanar(worldPosition,worldNormal,roughness).rgb;
	METALLIC = Triplanar(worldPosition,worldNormal,metallic).r;
}

Shader "testShaders/lighting"{
	Properties{

	_MainTex("Texture", 2D) = "white"{}
	_Color("Color",Color) = (0,0,0,1)
	_Ramp("Toon Ramp", 2D) = "white" {}

	}
		Subshader{

		Tags{ "RenderType" = "Opaque" "Queue" = "Geometry" }
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM

#pragma surface surf Custom fullforwardShadows
#pragma target 3.0



	sampler2D _MainTex;
	sampler2D _Ramp;
	fixed4 _Color;

	struct Input {

		float2 uv_MainTex;
		float3 worldNormal;
	};

	float4 LightingCustom(SurfaceOutput s, float3 lightDir, float atten) {
		// how much does the normal point towards the light ?
		float towardsLight = dot(s.Normal, lightDir);
		towardsLight = towardsLight * 0.5 + 0.5;
		float3 lightIntensity = tex2D(_Ramp, towardsLight).rgb;
		
		float4 col;
		//intensity we calculated previously, diffuse color, light falloff and shadowcasting, color of the light
		col.rgb = lightIntensity * s.Albedo * atten * _LightColor0.rgb;
		//in case we want to make the shader transparent in the future - irrelevant right now
		col.a = s.Alpha;

		return col;
	}

	void surf(Input i, inout SurfaceOutput o) {

		fixed4 col = tex2D(_MainTex, i.uv_MainTex);
		col *= _Color;
		o.Albedo = col.rgb;

	}
	ENDCG



	}
		FallBack "Standard"
}

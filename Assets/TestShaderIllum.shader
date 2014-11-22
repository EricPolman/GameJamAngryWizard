Shader "Custom/TestShaderIllum" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_EmissiveColor("Emissive Color", Color) = (1,1,1,1)
		_EmissiveIntensity("_EmissiveIntensity", Range(0,1) ) = 0.5
		level("level", int) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;

		float4 _EmissiveColor;
		float _EmissiveIntensity;

		int level;

		struct Input {
			float2 uv_MainTex;
			float4 screenPos;
		};

		struct LevelData
		{
			float maxSight;
			float ratioGrayscale;
			float ratioColoured;
		};


		void surf (Input IN, inout SurfaceOutput o) 
		{
			LevelData data;
			data.maxSight = level * 3;
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			float depth =  IN.screenPos.z / level / 3;
			float gray = (o.Albedo.r + o.Albedo.g + o.Albedo.b) / 3;
			float3 gray3 = float3(gray, gray, gray);
			
			if(depth > 1.25)
			{
				if((o.Albedo.r + o.Albedo.g + o.Albedo.b) / 3 < 0.6)
				{
					o.Albedo = 0.01;
				}
				else
				{
					o.Albedo = 0.9;
				}
			}
			else if(depth > 1.0)
			{
				float subtraction = depth - 1.0;
				float diff = 1.25 - 1.0;
				float fade = subtraction / diff;
				float3 col;
				if((o.Albedo.r + o.Albedo.g + o.Albedo.b) / 3 < 0.6)
				{
					col = 0.01;
				}
				else
				{
					col = 0.9;
				}
				o.Albedo = col * (fade) + gray3 * (1-fade);
			}
			else if(depth > 0.75)
			{
				o.Albedo = gray3;
			}
			else if(depth > 0.5)
			{
				//o.Albedo = gray3;
				float subtraction = depth - 0.5;
				float diff = 0.75 - 0.5;
				float fade = subtraction / diff;
				o.Albedo = o.Albedo * (1-fade) + gray3 * fade;
			}
			else if(depth > 0.25)
			{
				
			}
			o.Alpha = c.a;
			o.Emission = _EmissiveColor * _EmissiveIntensity;
		}

		void pixel()
		{
		}
		ENDCG
	} 
	FallBack "Diffuse"
}

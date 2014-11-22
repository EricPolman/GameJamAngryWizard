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
			float overlapRatio;
		};


		void surf (Input IN, inout SurfaceOutput o) 
		{
			if(level < 1) level = 1;
			LevelData data;
			switch(level)
			{
			case 1:
				data.maxSight = 3;
				data.ratioGrayscale = 0.5;
				data.ratioColoured = 0.25;
				data.overlapRatio = 0.1;
			break;
			case 2:
				data.maxSight = 5;
				data.ratioGrayscale = 0.5;
				data.ratioColoured = 0.25;
				data.overlapRatio = 0.1;
			break;
			case 3:
				data.maxSight = 7;
				data.ratioGrayscale = 0.75;
				data.ratioColoured = 0.375;
				data.overlapRatio = 0.1;
			break;
			default:
			case 4:
				data.maxSight = 9;
				data.ratioGrayscale = 1;
				data.ratioColoured = 0.5;
				data.overlapRatio = 0.1;
			break;
			}
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			float depth =  IN.screenPos.z / data.maxSight;
			float gray = (o.Albedo.r + o.Albedo.g + o.Albedo.b) / 3;
			float3 gray3 = float3(gray, gray, gray);
			
			if(depth < data.ratioColoured)
			{
				
			}
			else if(depth < data.ratioColoured + data.overlapRatio)
			{
				float subtraction = depth - data.ratioColoured;
				float diff = data.overlapRatio;
				float fade = subtraction / diff;
				o.Albedo = o.Albedo * (1-fade) + gray3 * fade;
			}
			else if(depth < data.ratioGrayscale)
			{
				o.Albedo = gray3;
			}
			else if(depth < data.ratioGrayscale + data.overlapRatio)
			{
				float subtraction = depth - data.ratioGrayscale;
				float diff = data.overlapRatio;
				float fade = subtraction / diff;
				float3 col;
				if((o.Albedo.r + o.Albedo.g + o.Albedo.b) / 3 < 0.6)
				{
					col = 0.00;
				}
				else
				{
					col = 0.9;
				}
				o.Albedo = col * (fade) + gray3 * (1-fade);
			}
			else if(depth < 1)
			{
				float3 col;
				if((o.Albedo.r + o.Albedo.g + o.Albedo.b) / 3 < 0.6)
				{
					col = 0.00;
				}
				else
				{
					col = 0.9;
				}
				o.Albedo = col;
			}
			else if(depth < 1 + data.overlapRatio)
			{
				float3 col;
				float subtraction = depth - 1;
				float diff = data.overlapRatio;
				float fade = subtraction / diff;

				if((o.Albedo.r + o.Albedo.g + o.Albedo.b) / 3 < 0.6)
				{
					col = 0.00;
				}
				else
				{
					col = 0.9;
				}
				o.Albedo = col * (1-fade) + float3(0.1,0.1,0.1) * (fade);
			}
			else
			{
				o.Albedo = float3(0,0,0);
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

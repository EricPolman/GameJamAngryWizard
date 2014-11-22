Shader "Custom/TestShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float4 screenPos;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			float depth =  IN.screenPos.z / 10;
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
				float subtraction = depth - 0.5;
				float diff = 0.75 - 0.5;
				float fade = subtraction / diff;
				o.Albedo = o.Albedo * (1-fade) + gray3 * fade;
			}
			else if(depth > 0.25)
			{
				//if(depth > 0.75)
				//{
				//	float subtraction = depth - 0.75;
				//	float diff = 1.0 - 0.75;
				//	float fade = subtraction / diff;
				//	o.Albedo.r = fade;
				//}
				//else if(depth > 0.5)
				//{
				//	float subtraction = depth - 0.5;
				//	float diff = 0.75 - 0.5;
				//	float fade = subtraction / diff;
				//	o.Albedo.g = fade;
				//}
				//else if(depth > 0.25)
				//{
				//	float subtraction = depth - 0.25;
				//	float diff = 0.5 - 0.25;
				//	float fade = subtraction / diff;
				//	o.Albedo.b = fade;
				//}
			}
			o.Alpha = c.a;
		}

		void pixel()
		{
		}
		ENDCG
	} 
	FallBack "Diffuse"
}

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// File: Lava.shader 
// Author: Ross Brown - r.brown@qut.edu.au
// Updated: 20/04/2015
// Description: Procedural lava shader modified from an nVIDIA demo by Sergey A. Makovkin (sergeymak@pisem.net)
//

Shader "Lava" 
{
	Properties 
	{
		VolumeTexture("Base (RGB)", 3D) = "white" {}
		BarkSampler("Base (RGB)", 2D) = "white" {}
	}
	
	SubShader
	{
		Pass 
		{
		
		CGPROGRAM
		#pragma vertex mainVS
		#pragma fragment mainPS
		#pragma target 3.0

		float  TexelIncrement = 1.0f / 64.0f;

		/* data from application vertex buffer */
		struct appdata 
		{
		    float3 Position	: POSITION;
		    float4 UV		: TEXCOORD0;
		    float4 Normal	: NORMAL;
		};

		/* data passed from vertex shader to pixel shader */
		struct vertexOutput 
		{
		    float4 HPosition	: POSITION;
		    float4 TexCoord		: TEXCOORD0;    
		    float3 WorldNormal	: TEXCOORD2;
		    float3 WorldPos		: TEXCOORD3;
		    float3 WorldEyePos	: TEXCOORD4;
		};

		/* Output pixel values */
		struct pixelOutput 
		{
		  float4 col : COLOR;
		};

		uniform sampler3D VolumeTexture;
		uniform sampler2D BarkSampler;
		
		uniform float4x4 _ViewI;

		/*********** vertex shader ******/
		vertexOutput mainVS(appdata IN)
		{
		    vertexOutput OUT;
		    
		    OUT.WorldNormal = mul(transpose(unity_WorldToObject), IN.Normal).xyz;
		    float3 WorldSpacePos = mul(unity_ObjectToWorld, float4(IN.Position, 1.0f)).xyz;
		    OUT.WorldPos = WorldSpacePos;
		    OUT.TexCoord = IN.UV;
		    OUT.WorldEyePos = _ViewI[3].xyz;
		    OUT.HPosition = UnityObjectToClipPos(float4(IN.Position, 1.0f));
		    
		    return OUT;
		}

		/*********** pixel shader ******/
		pixelOutput mainPS(vertexOutput IN)
		{
		    pixelOutput OUT; 
			float4 LavaColor1 = {0.8f, 0.8f, 0.4f, 1.0f};
			float4 LavaColor2 = {0.5f, 0.0f, 0.0f, 0.0f};
			float  LavaFactor = 0.1f;
		         
			// Sample 4 octaves of noise
			
			float rnd = 0.0f;
			float f = 1.0f;
			float3 Coord = IN.WorldPos + (_Time.w * 0.03f);
			
			for (int i = 0; i < 4; i++)
			{
				half4 fnoise = tex3D(VolumeTexture, Coord * 0.2f * f) / 2.0f + 0.4f;
				fnoise -= 0.5f;
				fnoise *= 4.0f;
				rnd += (fnoise.r) / f;
				f *= 4.17;	
			}
			    
			float3 coord = IN.WorldPos;
			coord.x += rnd * LavaFactor;
			coord.y += rnd * LavaFactor;
			float4 tex = tex2D(BarkSampler, coord);
			
			// Add the terms
			OUT.col = tex * LavaColor1 * (rnd + 0.1) * 10 + LavaColor2;
		    return OUT;
		}

		ENDCG
		}
	} 
}

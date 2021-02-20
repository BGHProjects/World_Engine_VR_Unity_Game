// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Clouds" 
{
 	Properties 
 	{
		_MainTex("Base (RGB)", 3D) = "white" {}    
	}	
    
    SubShader 
	{
  		Pass 
  		{ 
  			Blend SrcAlpha OneMinusSrcAlpha
  			
			CGPROGRAM
			
	        #pragma vertex VS_Texture 
	        #pragma fragment PS_Texture
	        #pragma glsl
	        
			#include "UnityCG.cginc"
			
			// used to transform vertices from local space into homogenous clipping space
			sampler3D _MainTex;

			// vertex shader input structure
			struct VSInput_PosTex 
			{
				float4 pos: POSITION;
				float2 tex: TEXCOORD0;
			};
			
			// vertex shader output structure
			struct VSOutput_PosTex 
			{
				float4 pos: SV_POSITION;
				float3 tex: TEXCOORD0;
			};
			
			float4 VSnoise3D(sampler3D NoiseMap, float3 P) // Cannot do 3D vertex textures in Cg!!!
			{
				return float4(1.0, 1.0, 1.0, 1.0); //tex3Dlod(NoiseMap, float4(P, 0)); //create signed noise
			}

			float4 PSnoise3D(sampler3D NoiseMap, float3 P:SV_POSITION)
			{
				return tex3D(NoiseMap, P); //create signed noise
			}

			float4 turbulence4(sampler3D NoiseMap, float3 P:SV_POSITION, bool VS)
			{
				float4 sum;
				
				if(VS == true)
					sum =	VSnoise3D(NoiseMap, P) * 0.5 +
			 				VSnoise3D(NoiseMap, P * 2) * 0.25 +
			 				VSnoise3D(NoiseMap, P * 4) * 0.125 +
							VSnoise3D(NoiseMap, P * 8) * 0.0625;
				else
					sum =	PSnoise3D(NoiseMap, P) * 0.5 +
			 				PSnoise3D(NoiseMap, P * 2) * 0.25 +	
			 				PSnoise3D(NoiseMap, P * 4) * 0.125 + 
			 				PSnoise3D(NoiseMap, P * 8) * 0.0625;
							
				return sum;
			}
			
			// position and texture vertex shader
			VSOutput_PosTex VS_Texture(VSInput_PosTex a_Input)
			{
				VSOutput_PosTex Output;
			
				// compute vertex transformation
				Output.pos = UnityObjectToClipPos(a_Input.pos);
				
				// adjust texture coordinates based on inputs
				Output.tex = a_Input.pos.xyz;
			
				return Output;
			}

			float4 PS_Texture(VSOutput_PosTex a_Input) : COLOR
			{
				float3 colour = turbulence4(_MainTex, a_Input.tex / 20.0f, false).rgb;
				return float4(colour.rgb, colour.r * 3.0f);
			}
						
			ENDCG
		}
	} 
}

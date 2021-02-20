Shader "Custom/TestShader"
{
     Properties 
 	{
		_Tex1("Base (RGB)", 2D) = "bump" {}
		
		_DisplaceHeight("DisplaceHeight", Range(0.0, 1.0)) = 0.0

		_DisplaceFactor("DisplaceFactor", Range(0.0, 1.0)) = 0.0

		_WaterLevel("WaterLevel", Range(0.0, 1.0)) = 0.2

		_SurfaceColor("COLOR", Color) = (0.0, 0.0, 0.0)

		
	}	
    
    SubShader 
	{
  		Pass 
  		{ 
			CGPROGRAM
			
			#pragma glsl
	        #pragma target 		3.0
	        #pragma vertex 		VS_Displacement 
	        #pragma fragment 	PS_Displacement
	        
			#include "UnityCG.cginc"
			
			// used to transform vertices from local space into homogenous clipping space
			sampler2D _Tex1;
			sampler2D _Tex2;

			//custom values
			float3 _SurfaceColor;
			float _DisplaceHeight;
			float _WaterLevel;
			float _DisplaceFactor;
			

			// vertex shader input structure
			struct VSInput 
			{
				float3 pos: POSITION;
				float3 norm: NORMAL;
				float2 tex: TEXCOORD0;
			};
			
			// vertex shader output structure
			struct VSOutput 
			{
				float4 pos: SV_POSITION;
				float3 col: COLOR;
				float2 tex: TEXCOORD0;
			};
			
			VSOutput VS_Displacement(VSInput a_Input) 
			{
				VSOutput Output;
				float g_fDispFactor = 0.2f;	// displacement factor
				
				// Create some hacekd texture coords from the x, y, z points.
				a_Input.tex.x = 0.5f + atan2(a_Input.pos.z, a_Input.pos.x)/(2.0f * 3.14159f);
				a_Input.tex.y = 0.5f + asin(a_Input.pos.y) / 3.14159f;
			
				// retrieve offset value
				//float displaceVal = tex2Dlod(_Tex1, float4(a_Input.tex.x, a_Input.tex.y, 0, 0)).x;

				_DisplaceHeight = tex2Dlod(_Tex1, float4(a_Input.tex.x, a_Input.tex.y, 0, 0)).x * _DisplaceFactor;
				//_DisplaceHeight = 0.2f;
				
				// retrieve offset value - no can do 3D in the Cg vertex shader, BAAAH!
				//float displaceVal = turbulence4(g_textureNoise, a_Input.pos, true).x;
			
				// displace vertex along normal
				float3 displacePos;
				
				if (_DisplaceHeight > 0.6)
				{
					displacePos = a_Input.pos + a_Input.norm * _DisplaceHeight * g_fDispFactor;
					//White
					Output.col = float3(0.5, 0.5, 0.5);
				}
				else if (_DisplaceHeight > 0.5)
				{
					displacePos = a_Input.pos + a_Input.norm * _DisplaceHeight * g_fDispFactor;
					//Brown
					Output.col = float3(0.3, 0.2, 0.0);
				}
				else if (_DisplaceHeight > 0.4)
				{
					displacePos = a_Input.pos + a_Input.norm * _DisplaceHeight * g_fDispFactor;
					//Middle Brown
					Output.col = float3(0.25,0.15,0.0);
				}
				else if (_DisplaceHeight > 0.3)
				{
					displacePos = a_Input.pos + a_Input.norm * _DisplaceHeight * g_fDispFactor;
					//Dark Brown
					Output.col = float3(0.2,0.1,0.0);
				}
				else if (_DisplaceHeight > 0.2)
				{
					displacePos = a_Input.pos + a_Input.norm * _DisplaceHeight * g_fDispFactor;
					//Green
					Output.col = float3(0.0, 0.3, 0.0);
				}
				else if (_DisplaceHeight > 0.12)
				{
					displacePos = a_Input.pos + a_Input.norm * _DisplaceHeight * g_fDispFactor;
					//Light green
					Output.col = float3(0.0, 0.4, 0.0);
				}
				else if (_DisplaceHeight > 0.08)
				{
					displacePos = a_Input.pos + a_Input.norm * _DisplaceHeight * g_fDispFactor;
					//Light green
					Output.col = float3(0.0, 0.15, 0.5);
				}
				else
				{
					displacePos = a_Input.pos + a_Input.norm * _WaterLevel * g_fDispFactor;
					//displacePos = a_Input.pos + a_Input.norm * displaceVal * g_fDispFactor;
					//Output.col = float3(0.0, 0.0, 0.5);
					Output.col = _SurfaceColor;
				}
			
				// set vertex position
				Output.pos = UnityObjectToClipPos(float4(displacePos, 1.0f));
				
				// copy across texture coordinates
				Output.tex = a_Input.tex;
				//Output.col.rg = a_Input.tex.xy;
				//Output.col.b  = 1.0f;
				
				return Output;
			}
			
			float4 PS_Displacement(VSOutput a_Input) : COLOR
			{
				return float4(a_Input.col, 1.0f);
			}
						
			ENDCG
		}
	} 
}

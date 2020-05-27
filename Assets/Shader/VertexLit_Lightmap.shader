Shader "Zero/BG/VertexLit_Lightmap"
{
	Properties{
		_MainColor("Color", color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_LightmapTex("Lightmap", 2D) = "black" {}
		_Brightness("Brightness", float) = 1.25
		[Header(RenderState)]
		[Enum(RGB,14,RGBA,15)] _ColorMask("Color Mask", float) = 14 //Alpha = 1,Blue = 2,Green = 4,Red = 8,All = 15
		[Header(Stencil)][IntRange] _StencilValue("Stencil ID", Range(0,8)) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _StencilComparison("Stencil Comparison",float) = 0 //Disabled = 0,Never = 1,Less = 2,Equal = 3,LessEqual = 4,Greater = 5,NotEqual = 6,GreaterEqual = 7,Always = 8		
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilOperation("Stencil Option",float) = 0 //Keep = 0,Zero = 1,Replace = 2,IncrementSaturate = 3,DecrementSaturate = 4,Invert = 5,IncrementWrap = 6,DecrementWrap = 7
	}

		SubShader{
			Tags { "RenderType" = "Opaque" "DisableBatching" = "False" }
			//LOD 100
			ColorMask[_ColorMask]

			Stencil
			{
				Ref[_StencilValue]
				Comp[_StencilComparison]
				Pass[_StencilOperation]
			}

			Pass {
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma target 2.0
					#pragma multi_compile_fog
					#pragma multi_compile_instancing
					#include "UnityCG.cginc"

					struct appdata_t {
						float4 vertex : POSITION;
						fixed2 texcoord : TEXCOORD0;
						fixed2 texcoord1 : TEXCOORD1;
						UNITY_VERTEX_INPUT_INSTANCE_ID
					};

					struct v2f {
						float4 vertex : SV_POSITION;
						fixed2 texcoord : TEXCOORD0;
						fixed2 lightmapUV : TEXCOORD1;
						UNITY_FOG_COORDS(2)
						//UNITY_VERTEX_OUTPUT_STEREO
					};

					sampler2D _MainTex;
					fixed4 _MainTex_ST;
					fixed4 _MainColor;
					sampler2D _LightmapTex;
					fixed4 _LightmapTex_ST;
					float _Brightness;
					v2f vert(appdata_t v)
					{
						v2f o;
						UNITY_SETUP_INSTANCE_ID(v);
						//UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
						o.vertex = UnityObjectToClipPos(v.vertex);
						o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
						o.lightmapUV = TRANSFORM_TEX(v.texcoord1, _LightmapTex);
						UNITY_TRANSFER_FOG(o,o.vertex);
						return o;
					}

					fixed4 frag(v2f i) : SV_Target
					{
						fixed4 col = tex2D(_MainTex, i.texcoord)*_MainColor;
						fixed3 lightmap = tex2D(_LightmapTex, i.lightmapUV.xy).xyz*_Brightness*2;
						col.xyz *= lightmap;
						UNITY_APPLY_FOG(i.fogCoord, col);
						//UNITY_OPAQUE_ALPHA(col.a);
						return col;
					}
				ENDCG
			}
		}

}

Shader "Zero/BG/Diffuse"
{
	Properties{
		_MainColor("Color", color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		[Header(RenderState)]
		[Enum(RGB,14,RGBA,15)] _ColorMask("Color Mask", float) = 14 //Alpha = 1,Blue = 2,Green = 4,Red = 8,All = 15
		[Header(Stencil)][IntRange] _StencilValue("Stencil ID", Range(0,8)) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _StencilComparison("Stencil Comparison",float) = 0 //Disabled = 0,Never = 1,Less = 2,Equal = 3,LessEqual = 4,Greater = 5,NotEqual = 6,GreaterEqual = 7,Always = 8		
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilOperation("Stencil Option",float) = 0 //Keep = 0,Zero = 1,Replace = 2,IncrementSaturate = 3,DecrementSaturate = 4,Invert = 5,IncrementWrap = 6,DecrementWrap = 7
		[Header(BSC)]
		[Enum(Open, 1, Close, 0)]_OpenBSC("Open BSC", int) = 0
		_Brightness("Brightness", float) = 1
		_Saturation("Saturation", float) = 1
		_Contrast("Contrast", float) = 1
	}

		SubShader{
		Tags{ "RenderType" = "Opaque" "DisableBatching" = "False" }
		//LOD 100
		ColorMask[_ColorMask]

		Stencil
		{
			Ref[_StencilValue]
			Comp[_StencilComparison]
			Pass[_StencilOperation]
		}

		Pass{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_fog
		#pragma multi_compile LIGHTMAP_OFF  LIGHTMAP_ON
		#pragma multi_compile_instancing
		#include "UnityCG.cginc"

		struct appdata_t {
			float4 vertex : POSITION;
			fixed2 texcoord : TEXCOORD0;
			fixed2 texcoord1 : TEXCOORD1;
			fixed3 normal : NORMAL;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct v2f {
			float4 vertex : SV_POSITION;
			fixed2 texcoord : TEXCOORD0;
			fixed2 texcoord1 : TEXCOORD1;
			UNITY_FOG_COORDS(2)
		};

		sampler2D _MainTex;
		fixed4 _MainTex_ST;
		fixed4 _MainColor;
		fixed _Brightness;
		fixed _Saturation;
		fixed _Contrast;
		fixed _OpenBSC;

		v2f vert(appdata_t v)
		{
			v2f o;
			UNITY_SETUP_INSTANCE_ID(v);
			//UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.texcoord1 = fixed2(0, 0);
	#ifndef LIGHTMAP_OFF 
			o.texcoord1 = v.texcoord1 * unity_LightmapST.xy + unity_LightmapST.zw;
	#endif
			UNITY_TRANSFER_FOG(o,o.vertex);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			fixed4 col = tex2D(_MainTex, i.texcoord)*_MainColor;
		#ifndef LIGHTMAP_OFF 
			col.xyz *= DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.texcoord1.xy)).xyz;
		#endif
			
			fixed3 finalColor = col.rgb;
			if (_OpenBSC > 0) {
				finalColor *= _Brightness;
				fixed lum = dot(finalColor, fixed3(0.2125f, 0.7154f, 0.0721f));
				finalColor = lerp(fixed3(lum, lum, lum), finalColor, _Saturation);
				finalColor = lerp(fixed3(0.5f, 0.5f, 0.5f), finalColor, _Contrast);
				finalColor = saturate(finalColor);
			}
			col.rgb = finalColor.rgb;

			UNITY_APPLY_FOG(i.fogCoord, col);
			//UNITY_OPAQUE_ALPHA(col.a);
			return col;
		}
		ENDCG
	}
	}
}

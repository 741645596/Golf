﻿Shader "Custom/GolfHole"
{
	Properties
	{
		_Color("Color", COLOR) = (0,0,0,0)
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry"}

		Pass
		{

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			float4 _Color;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = _Color;
				return col;
			}
			ENDCG
		}
	}
}

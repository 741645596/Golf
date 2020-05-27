Shader "Custom/ForwardDecal"
{
	Properties
	{
		_MainTex("Decal Texture", 2D) = "white" {}
	}

		SubShader
	{
		Tags{ "Queue" = "Geometry+1" }

		Pass
		{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 screenUV : TEXCOORD0;
				float3 ray : TEXCOORD1;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.screenUV = ComputeScreenPos(o.pos);
				o.ray = UnityObjectToViewPos(v.vertex).xyz * float3(-1,-1,1);
				return o;
			}

			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;
			float4 frag(v2f i) : SV_Target
			{
				i.ray = i.ray * (_ProjectionParams.z / i.ray.z);
				float2 uv = i.screenUV.xy / i.screenUV.w;

				float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv);

				// 要转换成线性的深度值 //
				depth = Linear01Depth(depth);

				float4 vpos = float4(i.ray * depth,1);
				float3 wpos = mul(unity_CameraToWorld, vpos).xyz;
				float3 opos = mul(unity_WorldToObject, float4(wpos,1)).xyz;
				clip(float3(0.5,0.5,0.5) - abs(opos.xyz));

				// 转换到 [0,1] 区间 //
				float2 texUV = opos.xz + 0.5;

				float4 col = tex2D(_MainTex, texUV);
				return col;
			}
			ENDCG
		}
	}
	Fallback Off
}

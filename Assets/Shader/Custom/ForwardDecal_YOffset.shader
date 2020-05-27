Shader "Custom/ForwardDecal_YOffset"
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
			#pragma fragment frag2

			#include "UnityCG.cginc"

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 screenUV : TEXCOORD1;
				float3 ray : TEXCOORD2;
				float3 orientation : TEXCOORD3;
				float3 worldNormal : TEXCOORD4;
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
			sampler2D _CameraDepthNormalsTexture;

			float4 frag2(v2f i) : SV_Target
			{
				i.ray = i.ray * (_ProjectionParams.z / i.ray.z);
				float2 uv = i.screenUV.xy / i.screenUV.w;

				float depth;
				float3 viewNormal;
				float4 encode = tex2D(_CameraDepthNormalsTexture, uv);

				// 返回一个视空间深度值 和 一个视空间法线 //
				// 该深度值是一个线性深度值, 范围是0到1, 精度小于使用SAMPLE_DEPTH_TEXTURE方法获得的深度值 //
				// 因为DecodeDepthNormal方法中的深度值用16位存储，而SAMPLE_DEPTH_TEXTURE中的深度值用32位存储 //
				DecodeDepthNormal(encode, depth, viewNormal);

				viewNormal = normalize(viewNormal);

				float4 vpos = float4(i.ray * depth,1);
				float3 wpos = mul(unity_CameraToWorld, vpos).xyz;
				float3 opos = mul(unity_WorldToObject, float4(wpos,1)).xyz;
				clip(float3(0.5,0.5,0.5) - abs(opos.xyz));

				float3 objectNormal = mul(UNITY_MATRIX_T_MV, viewNormal);
				objectNormal = normalize(objectNormal);

				float3 upDir = float3(0,1,0);

				float NDotU = dot(objectNormal, upDir);
				// float offsetScale = sin(acos(NDotU));
				float offsetScale = 1 - NDotU;				// 效果一样，计算更少 //

				// 先只考虑XZ平面 //
				float2 texUV = opos.xz + float2(0.5, 0.5) + offsetScale * float2(0, opos.y);

				float4 col = tex2D(_MainTex, texUV);
				return col;
			}

			ENDCG
		}
	}

		Fallback Off
}

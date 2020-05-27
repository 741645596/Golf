// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UI/UIGrayScale"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_ColorMask("_ColorMask", Vector) = (1,1,1,1)
		_GreyPow("_GreyPower", Range(0,10)) = 1

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
	}

		SubShader
	{
		Tags
		{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
		}

		Stencil
		{
			Ref[_Stencil]
			Comp Lequal
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}
		// Դrgba*Դa + ����rgba*(1-ԴAֵ)   
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
	{
		CGPROGRAM
#pragma vertex vert     
#pragma fragment frag     
#include "UnityCG.cginc"    
#include "UnityUI.cginc"

		struct appdata_t
	{
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex   : SV_POSITION;
		fixed4 color : COLOR;
		half2 texcoord  : TEXCOORD0;
		float4 worldPosition : TEXCOORD1;

	};

	sampler2D _MainTex;
	fixed4 _Color;

	bool _UseClipRect;
	float4 _ClipRect;
	float4 _ColorMask;
	float _GreyPow;

	v2f vert(appdata_t IN)
	{
		v2f OUT;
		OUT.worldPosition = IN.vertex;
		OUT.vertex = UnityObjectToClipPos(IN.vertex);
		OUT.texcoord = IN.texcoord;
#ifdef UNITY_HALF_TEXEL_OFFSET     
		OUT.vertex.xy -= (_ScreenParams.zw - 1.0);
#endif     
		OUT.color = IN.color * _Color;
		return OUT;
	}

	fixed4 frag(v2f IN) : SV_Target
	{
		half4 color = tex2D(_MainTex, IN.texcoord) * IN.color;

		if (_UseClipRect)
			color *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

		float grey = dot(color.rgb, fixed3(0.3, 0.6, 0.1));
		grey = pow(grey, _GreyPow);
		return half4(grey,grey,grey,color.a);
	}
		ENDCG
	}
	}
}
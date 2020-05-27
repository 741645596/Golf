// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/UVAnimation" {
	Properties
	{
		//要播放的UV动画Texture
		_SpriteTex("SpriteTexture (RGB)", 2D) = "white" {}
		//播放速度
		_Speed("AnimationSpeed",Range(0.01,2)) = 1.0
	}
		SubShader
	{
		Pass
		{
		//不透明物体
		 Tags { "RenderType" = "Opaque" "Queue" = "Transparent"}
		 Blend SrcAlpha OneMinusSrcAlpha
		 Cull Off

		 //CG开始
		 CGPROGRAM
		 #pragma vertex vert
		 #pragma fragment frag

		 #include "UnityCG.cginc"
		//引用变量值
		uniform sampler2D _SpriteTex;
		fixed4 _SpriteTex_ST;
		uniform float _Speed;

		//顶点输出结构体
		struct VertexOutput
		{
			float4 pos:SV_POSITION;
			float2 uv:TEXCOORD0;
		};

		//appdate_base Unity预定义的输入结构体 包括了很多信息,可以直接用作为顶点着色程序的输入
		 VertexOutput vert(appdata_base input)
		 {
			 VertexOutput v_output;
			 v_output.pos = UnityObjectToClipPos(input.vertex);
			 //v_output.uv = input.texcoord;
			 v_output.uv.x = input.texcoord.x;
			 v_output.uv.y = 1- input.texcoord.y + frac(_Time.y * _Speed);
			 ///v_output.uv = TRANSFORM_TEX(v_output.uv, _SpriteTex);
			 return v_output;
		 }

		 float4 frag(VertexOutput input) :COLOR
		 {
			//计算当前索引所在的精灵的UV坐标
			 float2 spriteUV = input.uv;

			 //spriteUV.y = input.uv.y + frac(_Time.y * _Speed);

			//获取二维文理坐标color 显示到屏幕      
			float4 col = tex2D(_SpriteTex,spriteUV);
			return col;
	     }

			 //CG结束
			 ENDCG
		 }
	}
}
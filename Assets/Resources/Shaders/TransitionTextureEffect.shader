Shader "Hidden/TransitionEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_TransitionTex("Transition Texture", 2D) = "white"{}
		_OverlayTex("Overlay Texture", 2D) = "white"{}
		_Color("Transition Color", Color) = (1,1,1,1)
		_MainColor("Screen Color", Color) = (1,1,1,1)
		_Fade("Fade", Range(0,1)) = 0
		_Cutoff("Cutoff", Range(0,1)) = 0
		_DoTransition("Do Transition", Range(0,1)) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv1: TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			float4 _MainTex_TexelSize;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				o.uv1 = v.uv;

				#if UNITY_UV_STARTS_AT_TOP
				if(_MainTex_TexelSize.y < 0){
					o.uv1.y = 1 - o.uv1.y;
				}
				#endif

				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _TransitionTex;
			sampler2D _OverlayTex;
			fixed4 _Color;
			fixed4 _MainColor;
			float _Fade;
			float _Cutoff;
			float _DoTransition;


			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 o_col = tex2D(_OverlayTex, i.uv1);
				col = lerp(col, fixed4(0,0,0,1), 1 - o_col.b);

				if(_DoTransition > 0){
					fixed4 transition = tex2D(_TransitionTex, i.uv1);
					if(transition.b < _Cutoff){
						fixed4 tCol = lerp(_MainColor, _Color, 1 -_Cutoff);
						col = lerp(col, tCol, _Fade);
					}
				}

				return col;
			}
			ENDCG
		}
	}
}

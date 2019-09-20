// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Devdog/WeightedTexturePreview"
{
	Properties{
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_Channel("Channel", Int) = 0
	}

		SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass{

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 2.0

#include "UnityCG.cginc"

		struct appdata_t {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f {
		float4 vertex : SV_POSITION;
		half2 texcoord : TEXCOORD0;
		UNITY_FOG_COORDS(1)
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;
	fixed _Channel;

	v2f vert(appdata_t v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 col = tex2D(_MainTex, i.texcoord);
		if (_Channel == 0)
		{
			return fixed4(col.r, col.r, col.r, col.r);
		}

		if (_Channel == 1)
		{
			return fixed4(col.g, col.g, col.g, col.g);
		}

		if (_Channel == 2)
		{
			return fixed4(col.b, col.b, col.b, col.b);
		}

		if (_Channel == 3)
		{
			return fixed4(col.a, col.a, col.a, col.a);
		}

		return col;
	}
		ENDCG
	}
	}

}

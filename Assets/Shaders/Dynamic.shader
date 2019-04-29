Shader "Dynamic"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
	_SectionSize ("Section Size", int) = 30
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
      float4 vertex : SV_POSITION;
    };

    v2f vert (appdata v)
    {
      v2f o;
      o.vertex = UnityObjectToClipPos(v.vertex);
      o.uv = v.uv;
      return o;
    }

    sampler2D _MainTex;
	float _SectionSize;
    const static int colors = 18;

    float4 frag (v2f i) : SV_Target
    {
      float4 originalColor = tex2D(_MainTex, i.uv);
      
	  float4 augmentedColor = originalColor;
	  augmentedColor[0] = (floor((255 * augmentedColor[0]) / _SectionSize) * _SectionSize)/255;
	  augmentedColor[1] = (floor((255 * augmentedColor[1]) / _SectionSize) * _SectionSize)/255;
	  augmentedColor[2] = (floor((255 * augmentedColor[2]) / _SectionSize) * _SectionSize)/255;
	  augmentedColor[3] = originalColor[3];
	  return augmentedColor;
    }
    ENDCG
    }
  }
}
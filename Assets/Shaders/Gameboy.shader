Shader "Gameboy"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
    _Blue1 ("Blue1", color) = (0,0,0)
    _Blue2 ("Blue2", color) = (0,0,0)
    _Blue3 ("Blue3", color) = (0,0,0)
    _Blue4 ("Blue4", color) = (0,0,0)
    _Green1 ("Green1", color) = (0,0,0)
    _Green2 ("Green2", color) = (0,0,0)
    _Green3 ("Green3", color) = (0,0,0)
    _Green4 ("Green4", color) = (0,0,0)
    _Dead1 ("Dead1", color) = (0,0,0)
    _Dead2 ("Dead2", color) = (0,0,0)
    _Dead3 ("Dead3", color) = (0,0,0)
    _Dead4 ("Dead4", color) = (0,0,0)
	_Black1("Black1", color) = (0,0,0)
	_Black2("Black2", color) = (0,0,0)
    _White1 ("White1", color) = (0,0,0)
    _White2 ("White2", color) = (0,0,0)
    _Yellow1 ("Yellow1", color) = (0,0,0)
    _Yellow2 ("Yellow2", color) = (0,0,0)
	_Player("Player", color) = (0,0,0)
	
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
	float4 _Blue1, _Blue2, _Blue3, _Blue4, _Green1, _Green2, _Green3, _Green4, _Dead1, _Dead2, _Dead3, _Dead4, _Black1, _Black2,
		_White1, _White2, _Yellow1, _Yellow2;
    const static int colors = 18;

    float4 frag (v2f i) : SV_Target
    {
      float4 originalColor = tex2D(_MainTex, i.uv);
      

      const float4 _Distances[colors] = 
      {
		  _Blue1, _Blue2, _Blue3, _Blue4, _Green1, _Green2, _Green3, _Green4, _Dead1, _Dead2, _Dead3, _Dead4, _Black1, _Black2,
		  _White1, _White2, _Yellow1, _Yellow2
      };
      
      int best = 0;
      for(int i=0; i < colors; ++i) {
        if(abs(length(originalColor.rgb - _Distances[i])) < abs(length(originalColor.rgb - _Distances[best]))) {
          best = i;
        }
      }
      return _Distances[best];
    }
    ENDCG
    }
  }
}
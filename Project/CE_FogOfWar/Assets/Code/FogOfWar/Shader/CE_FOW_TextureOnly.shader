// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Eran/FOW/TextureOnly"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 wpos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            sampler2D _FowTex;
            half4 _FowColor;
            float _FowInvisibleAreaAlpha;
            
            float _FowWorldWidth;
            float _FowWorldHeight;
            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
  
                //计算Fow的坐标 
                //TODO: 当World有Offset时候需要计算Offset
                o.wpos =  mul (unity_ObjectToWorld, v.vertex).xz;
                o.wpos.x = o.wpos.x / _FowWorldWidth;
                o.wpos.y = o.wpos.y / _FowWorldHeight;
               
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 fow = tex2D(_FowTex,i.wpos);
                //fixed4 a = lerp(_FowColor,fixed4(1,1,1,1),(fow.r+fow.g) * 0.5);
                
                fixed4 a = lerp(_FowColor,fixed4(1,1,1,1) * _FowInvisibleAreaAlpha,fow.r);
                fixed4 b = lerp(_FowColor,fixed4(1,1,1,1),fow.g);
                
                
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb *= min((a+b),fixed4(1,1,1,1));
                return col;
                
            }
            ENDCG
        }
    }
}

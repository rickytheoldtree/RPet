Shader "Unlit/Char3D"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ShadowCol("Color", color) = (0,0,0,1)
        _StencilID("StencilID", float) = 2
        _Plane("Plane", vector) = (0,1,0,-25)
        _LightDir("LightDir", vector) = (-0.29,1.24,-0.25,1)
        _AlphaDesc("Alpha Desc", float) = 11.82
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Cull Off
        Lighting off
        ZWrite On
        ZTest LEqual
        Blend SrcAlpha OneMinusSrcAlpha
        
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

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
        Pass
        {
            Stencil{
                Ref[_StencilID]
                Comp NotEqual
                Pass replace
            }
            zwrite off
            blend srcalpha oneminussrcalpha
            offset -1,-1
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 col : COLOR;
            };

            float4 _ShadowCol;
            half4 _Plane;
            half4 _LightDir;
            fixed _AlphaDesc;

            v2f vert(appdata v)
            {
                v2f o;
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                float t = (_Plane.w - dot(worldPos.xyz, _Plane.xyz)) / dot(_LightDir.xyz, _Plane.xyz);
                worldPos.xyz = worldPos.xyz + t * _LightDir.xyz;
                o.vertex = mul(unity_MatrixVP, worldPos);

                /*o.col.rgb = _ShadowCol * -t;
                o.col.a = 1 / (-t + 0.5);*/
                //o.col.rgb = 1 / (-t + 1);
                o.col.rgb = _ShadowCol * 1 / (-t + 1);
                //o.col.a = 1 / (-t + 1);
                o.col.a = _AlphaDesc / (-t + _AlphaDesc) * 0.6;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = i.col;
                return col;
            }
            ENDCG
        }
    }
}

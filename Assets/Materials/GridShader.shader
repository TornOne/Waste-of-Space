Shader "Unlit/GridShader"
{
    Properties
    {
        _MainTex    ("Background", 2D) = "white" {}
        _GridTex    ("Grid", 2D) = "white" {}
        _GridSize   ("Grid Size", float) = 0.001
        _GridOffset ("Grid Offset", Vector) = (0, 0, 0, 0)
        _Speed      ("Speed", float) = 0
        _BGSize     ("Background Scale", float) = 1
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _GridTex;
            float _GridSize;
            float4 _GridOffset;
            float _BGSize;
            float _Speed;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed2 pos = i.uv + float2(0, -_Speed * _Time.x);
                fixed4 bg = tex2D(_MainTex, pos * _BGSize);
                fixed4 grid = tex2D(_GridTex, i.uv * _GridSize + _GridOffset.xy);
                // apply fog
                fixed4 col = lerp(bg, grid, grid.a);
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}

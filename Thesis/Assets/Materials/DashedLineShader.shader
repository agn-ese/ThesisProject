Shader "Unlit/DashedLineShader"
{
    Properties
    {
        _Color ("Line Color", Color) = (1,1,1,1)
        _DashSize ("Dash Size", Float) = 1.0
        _GapSize ("Gap Size", Float) = 1.0
        _LineWidth ("Line Width", Range(0.01, 0.5)) = 0.1
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
                float3 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float dashPatternCoord : TEXCOORD1; // To store the distance along the line
            };

            float _DashSize;
            float _GapSize;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                // Calculate the distance from the origin in world space
                o.dashPatternCoord = length(o.worldPos); // Use the world position length for tiling

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float dashPatternLength = _DashSize + _GapSize;
                float patternPosition = i.dashPatternCoord / dashPatternLength;
                float dashFrac = frac(patternPosition); // Get the fractional part of the pattern length

                if (dashFrac < _DashSize / dashPatternLength)
                {
                    // Render the dash
                    return _Color;
                }
                else
                {
                    // Render the gap (discard fragment)
                    discard;
                    return fixed4(0, 0, 0, 0);
                }
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}


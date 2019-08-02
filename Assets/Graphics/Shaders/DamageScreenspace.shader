Shader "Shader Forge/DamageScreenspace" {
    Properties {
        _CenterDistMulti ("CenterDistMulti", Float ) = 1
        _MainTex ("MainTex", 2D) = "white" {}
        _HitTex ("HitTex", 2D) = "white" {}
        _ColorLerp ("ColorLerp", Float ) = 0
        [HDR]_HitColor ("HitColor", Color) = (1,1,1,1)
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Overlay+1"
            "RenderType"="Overlay"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            ZTest Always
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _CenterDistMulti;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _HitTex; uniform float4 _HitTex_ST;
            uniform float _ColorLerp;
            uniform float4 _HitColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 _HitTex_var = tex2D(_HitTex,TRANSFORM_TEX(i.uv0, _HitTex));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float node_1058 = 1.0;
                float node_4314 = (1.0 - _CenterDistMulti);
                float node_7650 = saturate(lerp(node_1058,((distance(i.uv0.r,_CenterDistMulti)*distance(i.uv0.r,node_4314)*distance(i.uv0.g,_CenterDistMulti)*distance(i.uv0.g,node_4314))*3.0+0.0),saturate(_ColorLerp)));
                float3 emissive = lerp((_HitTex_var.rgb*_HitColor.rgb),_MainTex_var.rgb,node_7650);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
}

// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:33271,y:32511,varname:node_4795,prsc:2|emission-1134-OUT,clip-2056-OUT;n:type:ShaderForge.SFN_Multiply,id:2393,x:32286,y:32406,varname:node_2393,prsc:2|A-2053-RGB,B-797-RGB,C-1422-OUT;n:type:ShaderForge.SFN_VertexColor,id:2053,x:30172,y:32365,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Color,id:797,x:31844,y:32483,ptovrint:True,ptlb:Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:True,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Tex2d,id:4461,x:30671,y:32894,ptovrint:False,ptlb:DissolvePattern,ptin:_DissolvePattern,varname:node_4461,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ComponentMask,id:8319,x:31111,y:32858,varname:node_8319,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-6740-OUT;n:type:ShaderForge.SFN_Add,id:6740,x:30923,y:32858,varname:node_6740,prsc:2|A-5351-OUT,B-4461-RGB;n:type:ShaderForge.SFN_Multiply,id:1422,x:31844,y:32641,varname:node_1422,prsc:2|A-2053-A,B-2137-OUT;n:type:ShaderForge.SFN_Vector1,id:2137,x:31844,y:32769,varname:node_2137,prsc:2,v1:1.5;n:type:ShaderForge.SFN_Multiply,id:2940,x:30418,y:32584,varname:node_2940,prsc:2|A-2053-A,B-5185-OUT;n:type:ShaderForge.SFN_Vector1,id:5185,x:30207,y:32675,varname:node_5185,prsc:2,v1:3;n:type:ShaderForge.SFN_RemapRange,id:5351,x:30617,y:32584,varname:node_5351,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:0.5|IN-2940-OUT;n:type:ShaderForge.SFN_RemapRange,id:8502,x:31301,y:32858,varname:node_8502,prsc:2,frmn:0,frmx:1,tomn:-3,tomx:3|IN-8319-OUT;n:type:ShaderForge.SFN_Clamp01,id:2056,x:31499,y:32858,varname:node_2056,prsc:2|IN-8502-OUT;n:type:ShaderForge.SFN_Tex2d,id:2229,x:32084,y:32905,ptovrint:False,ptlb:ColorRamp,ptin:_ColorRamp,varname:node_2229,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-4057-OUT;n:type:ShaderForge.SFN_Append,id:4057,x:31901,y:32905,varname:node_4057,prsc:2|A-2056-OUT,B-6937-OUT;n:type:ShaderForge.SFN_Vector1,id:6937,x:31901,y:33033,varname:node_6937,prsc:2,v1:0;n:type:ShaderForge.SFN_Multiply,id:1134,x:33003,y:32614,varname:node_1134,prsc:2|A-2393-OUT,B-7454-OUT;n:type:ShaderForge.SFN_Multiply,id:606,x:32278,y:33007,varname:node_606,prsc:2|A-2229-RGB,B-2653-OUT;n:type:ShaderForge.SFN_Vector1,id:2653,x:32084,y:33072,varname:node_2653,prsc:2,v1:1.5;n:type:ShaderForge.SFN_Vector3,id:9114,x:32278,y:33156,varname:node_9114,prsc:2,v1:0.7607844,v2:0.145098,v3:0.1137255;n:type:ShaderForge.SFN_Lerp,id:7454,x:32527,y:32933,varname:node_7454,prsc:2|A-2229-RGB,B-606-OUT,T-9114-OUT;proporder:797-4461-2229;pass:END;sub:END;*/

Shader "Shader Forge/LogoParticle" {
    Properties {
        [HDR]_TintColor ("Color", Color) = (0.5,0.5,0.5,1)
        _DissolvePattern ("DissolvePattern", 2D) = "white" {}
        _ColorRamp ("ColorRamp", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _TintColor;
            uniform sampler2D _DissolvePattern; uniform float4 _DissolvePattern_ST;
            uniform sampler2D _ColorRamp; uniform float4 _ColorRamp_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 _DissolvePattern_var = tex2D(_DissolvePattern,TRANSFORM_TEX(i.uv0, _DissolvePattern));
                float node_2056 = saturate(((((i.vertexColor.a*3.0)*1.5+-1.0)+_DissolvePattern_var.rgb).r*6.0+-3.0));
                clip(node_2056 - 0.5);
////// Lighting:
////// Emissive:
                float2 node_4057 = float2(node_2056,0.0);
                float4 _ColorRamp_var = tex2D(_ColorRamp,TRANSFORM_TEX(node_4057, _ColorRamp));
                float3 node_606 = (_ColorRamp_var.rgb*1.5);
                float3 node_9114 = float3(0.7607844,0.145098,0.1137255);
                float3 emissive = ((i.vertexColor.rgb*_TintColor.rgb*(i.vertexColor.a*1.5))*lerp(_ColorRamp_var.rgb,node_606,node_9114));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0,0,0,1));
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}

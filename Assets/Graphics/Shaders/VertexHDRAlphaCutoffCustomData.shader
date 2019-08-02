// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "VertexHDRAlphaCutoffCustomData"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		[HDR]_Color("Color", Color) = (1,1,1,1)
		_CutOff("CutOff", Range( 0 , 1)) = 0
		_AlphaMask("Alpha Mask", 2D) = "white" {}
		_Secondaryalphamask("Secondary alpha mask", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}


	Category 
	{
		SubShader
		{
			Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask RGB
			Cull Off
			Lighting Off 
			ZWrite Off
			ZTest LEqual
			
			Pass {
			
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_particles
				#pragma multi_compile_fog
				

				#include "UnityCG.cginc"

				struct appdata_t 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
					
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					#ifdef SOFTPARTICLES_ON
					float4 projPos : TEXCOORD2;
					#endif
					UNITY_VERTEX_INPUT_INSTANCE_ID
					UNITY_VERTEX_OUTPUT_STEREO
					
				};
				
				
				#if UNITY_VERSION >= 560
				UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
				#else
				uniform sampler2D_float _CameraDepthTexture;
				#endif

				//Don't delete this comment
				// uniform sampler2D_float _CameraDepthTexture;

				uniform sampler2D _MainTex;
				uniform fixed4 _TintColor;
				uniform float4 _MainTex_ST;
				uniform float _InvFade;
				uniform float4 _Color;
				uniform sampler2D _AlphaMask;
				uniform float4 _AlphaMask_ST;
				uniform sampler2D _Secondaryalphamask;
				uniform float4 _Secondaryalphamask_ST;
				uniform float _CutOff;
				float4 Cutoff6( float4 ColIn , float CutOff )
				{
					if (ColIn[3] < CutOff) return float4(0, 0, 0, 0);
					return ColIn;
				}
				

				v2f vert ( appdata_t v  )
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					

					v.vertex.xyz +=  float3( 0, 0, 0 ) ;
					o.vertex = UnityObjectToClipPos(v.vertex);
					#ifdef SOFTPARTICLES_ON
						o.projPos = ComputeScreenPos (o.vertex);
						COMPUTE_EYEDEPTH(o.projPos.z);
					#endif
					o.color = v.color;
					o.texcoord = v.texcoord;
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag ( v2f i  ) : SV_Target
				{
					#ifdef SOFTPARTICLES_ON
						float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
						float partZ = i.projPos.z;
						float fade = saturate (_InvFade * (sceneZ-partZ));
						i.color.a *= fade;
					#endif

					float2 uv_MainTex = i.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					float2 uv_AlphaMask = i.texcoord.xy * _AlphaMask_ST.xy + _AlphaMask_ST.zw;
					float2 uv_Secondaryalphamask = i.texcoord.xy * _Secondaryalphamask_ST.xy + _Secondaryalphamask_ST.zw;
					float4 break11 = ( i.color * _Color * tex2D( _MainTex, uv_MainTex ) * tex2D( _AlphaMask, uv_AlphaMask ).a * tex2D( _Secondaryalphamask, uv_Secondaryalphamask ).a );
					float clampResult16 = clamp( i.texcoord.z , 0.0 , 1.0 );
					float4 appendResult13 = (float4(break11.r , break11.g , break11.b , ( break11.a * clampResult16 )));
					float4 ColIn6 = appendResult13;
					float CutOff6 = _CutOff;
					float4 localCutoff6 = Cutoff6( ColIn6 , CutOff6 );
					

					fixed4 col = localCutoff6;
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG 
			}
		}	
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=16400
1927;7;1906;1005;1669.003;465.3458;1.3;True;True
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;9;-1282.731,134.9218;Float;False;0;0;_MainTex;Shader;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-1005,151.5;Float;True;Property;_Texture;Texture;1;0;Create;True;0;0;False;0;None;1c32e629e2346a045b0b544a2d94f9b7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;2;-923,-266.5;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-1000,374.5;Float;True;Property;_AlphaMask;Alpha Mask;2;0;Create;True;0;0;False;0;None;4080f25f8b76c364eadb0ff2757f9e26;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;15;-994.5451,600.8929;Float;True;Property;_Secondaryalphamask;Secondary alpha mask;3;0;Create;True;0;0;False;0;None;d96e41a067526d54282092e94adf783c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;3;-961,-70.5;Float;False;Property;_Color;Color;0;1;[HDR];Create;True;0;0;False;0;1,1,1,1;135.587,135.587,135.587,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;10;-543.1838,191.7361;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-614,-93.67167;Float;True;5;5;0;COLOR;1,1,1,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;11;-405.1838,-122.2639;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ClampOpNode;16;-292.9155,162.3426;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-136.1838,25.73615;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;13;35.81616,-107.2639;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;8;28.14587,81.50528;Float;False;Property;_CutOff;CutOff;1;0;Create;True;0;0;False;0;0;0.122;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;6;318,-87.5;Float;False;if (ColIn[3] < CutOff) return float4(0, 0, 0, 0)@$return ColIn@;4;False;2;True;ColIn;FLOAT4;0,0,0,0;In;;Float;True;CutOff;FLOAT;0;In;;Float;Cutoff;True;False;0;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;571.5458,-91.35957;Float;False;True;2;Float;ASEMaterialInspector;0;7;VertexHDRAlphaCutoffCustomData;0b6a9f8b4f707c74ca64c0be8e590de0;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;True;2;False;-1;True;True;True;True;False;0;False;-1;False;True;2;False;-1;True;3;False;-1;False;True;4;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;False;0;False;False;False;False;False;False;False;False;False;False;True;0;0;;0;0;Standard;0;0;1;True;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;4;0;9;0
WireConnection;7;0;2;0
WireConnection;7;1;3;0
WireConnection;7;2;4;0
WireConnection;7;3;5;4
WireConnection;7;4;15;4
WireConnection;11;0;7;0
WireConnection;16;0;10;3
WireConnection;12;0;11;3
WireConnection;12;1;16;0
WireConnection;13;0;11;0
WireConnection;13;1;11;1
WireConnection;13;2;11;2
WireConnection;13;3;12;0
WireConnection;6;0;13;0
WireConnection;6;1;8;0
WireConnection;1;0;6;0
ASEEND*/
//CHKSM=638A143E27EB3F0FB6BB92B1FB86F2C78FFA5499
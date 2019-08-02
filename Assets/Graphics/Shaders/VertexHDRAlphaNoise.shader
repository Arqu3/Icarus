// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "VertexHDRAlphaNoise"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		[HDR]_HDRColor("HDRColor", Color) = (1,1,1,1)
		_Tiling("Tiling", Vector) = (1,1,0,0)
		_Speed("Speed", Vector) = (0,0,0,0)
		_Alpha("Alpha", 2D) = "white" {}
		_TilingAlpha("TilingAlpha", Vector) = (1,1,0,0)
		_AlphaSpeed("AlphaSpeed", Vector) = (0,0,0,0)
		_Alphacutoff("Alpha cutoff", Range( 0 , 1)) = 0
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
				#include "UnityShaderVariables.cginc"


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
				uniform float2 _Speed;
				uniform float2 _Tiling;
				uniform sampler2D _Alpha;
				uniform float2 _AlphaSpeed;
				uniform float2 _TilingAlpha;
				uniform float4 _HDRColor;
				uniform float _Alphacutoff;
				float4 Cutoff35( float4 ColIn , float CutOff )
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

					float2 uv013 = i.texcoord.xy * _Tiling + float2( 0,0 );
					float2 panner17 = ( _Time.y * _Speed + uv013);
					float2 uv015 = i.texcoord.xy * _TilingAlpha + float2( 0,0 );
					float2 panner22 = ( _Time.y * _AlphaSpeed + uv015);
					float4 ColIn35 = ( ( tex2D( _MainTex, panner17 ) * tex2D( _Alpha, panner22 ).a ) * i.color * _HDRColor );
					float CutOff35 = _Alphacutoff;
					float4 localCutoff35 = Cutoff35( ColIn35 , CutOff35 );
					

					fixed4 col = localCutoff35;
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
1927;7;1906;1004;428.8219;523.7816;1.085253;True;True
Node;AmplifyShaderEditor.Vector2Node;14;-1276.487,-232.9428;Float;False;Property;_Tiling;Tiling;1;0;Create;True;0;0;False;0;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;16;-1327.857,701.7882;Float;False;Property;_TilingAlpha;TilingAlpha;4;0;Create;True;0;0;False;0;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;23;-1022.969,1003.951;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-1071.487,-228.9428;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;34;-1098.858,864.5831;Float;False;Property;_AlphaSpeed;AlphaSpeed;5;0;Create;True;0;0;False;0;0,0;1,0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;21;-1036.944,-100.3727;Float;False;Property;_Speed;Speed;2;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;20;-1041.528,42.70337;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;-1068.779,715.5512;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;3;-797.5166,418.156;Float;True;Property;_Alpha;Alpha;3;0;Create;True;0;0;False;0;None;d96e41a067526d54282092e94adf783c;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.PannerNode;17;-741.1857,-246.9624;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;37;-1012.664,-461.5437;Float;False;0;0;_MainTex;Shader;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;22;-764.8259,747.5821;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;7;-494.6979,-433.5936;Float;True;Global;TextureSample1;Texture Sample 1;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-395.0038,527.1161;Float;True;Global;TextureSample0;Texture Sample 0;3;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;18;279.7258,123.3256;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;276.3021,-97.59363;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;24;240.918,323.2742;Float;False;Property;_HDRColor;HDRColor;0;1;[HDR];Create;True;0;0;False;0;1,1,1,1;2.270603,2.270603,2.270603,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;523.7268,40.70038;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;36;535.3375,197.0519;Float;False;Property;_Alphacutoff;Alpha cutoff;6;0;Create;True;0;0;False;0;0;0.03;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;35;779.6256,75.67732;Float;False;if (ColIn[3] < CutOff) return float4(0, 0, 0, 0)@$return ColIn@;4;False;2;True;ColIn;FLOAT4;0,0,0,0;In;;Float;True;CutOff;FLOAT;0;In;;Float;Cutoff;True;False;0;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;33;1033.2,54.2;Float;False;True;2;Float;ASEMaterialInspector;0;7;VertexHDRAlphaNoise;0b6a9f8b4f707c74ca64c0be8e590de0;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;True;2;False;-1;True;True;True;True;False;0;False;-1;False;True;2;False;-1;True;3;False;-1;False;True;4;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;False;0;False;False;False;False;False;False;False;False;False;False;True;0;0;;0;0;Standard;0;0;1;True;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;13;0;14;0
WireConnection;15;0;16;0
WireConnection;17;0;13;0
WireConnection;17;2;21;0
WireConnection;17;1;20;0
WireConnection;22;0;15;0
WireConnection;22;2;34;0
WireConnection;22;1;23;0
WireConnection;7;0;37;0
WireConnection;7;1;17;0
WireConnection;4;0;3;0
WireConnection;4;1;22;0
WireConnection;8;0;7;0
WireConnection;8;1;4;4
WireConnection;19;0;8;0
WireConnection;19;1;18;0
WireConnection;19;2;24;0
WireConnection;35;0;19;0
WireConnection;35;1;36;0
WireConnection;33;0;35;0
ASEEND*/
//CHKSM=09500C59B9C33A7022EF659356E64996D320AEC2
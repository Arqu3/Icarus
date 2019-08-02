// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "HDRParticleUVPanner"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		[HDR]_Color0("Color 0", Color) = (1,1,1,1)
		_UVNoise("UVNoise", 2D) = "white" {}
		_SecondaryUVNoise("Secondary UVNoise", 2D) = "white" {}
		_UVLerp("UVLerp", Range( 0 , 1)) = 0.8
		_NoiseSpeed1("NoiseSpeed1", Vector) = (-1,1,0,0)
		_NoiseSpeed2("NoiseSpeed2", Vector) = (-1,0,0,0)
		_SpeedMulti("SpeedMulti", Float) = 1
		_NoiseExponent("NoiseExponent", Range( 0.01 , 2)) = 0.2
		_DropdownAlphaExponent("DropdownAlphaExponent", Float) = 3
		_Dropdowndistance("Dropdown distance", Range( 0 , 1)) = 1
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
				uniform float4 _Color0;
				uniform sampler2D _UVNoise;
				uniform float _SpeedMulti;
				uniform float2 _NoiseSpeed1;
				uniform sampler2D _SecondaryUVNoise;
				uniform float2 _NoiseSpeed2;
				uniform float _NoiseExponent;
				uniform float _UVLerp;
				uniform float _Dropdowndistance;
				uniform float _DropdownAlphaExponent;

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

					float4 temp_cast_0 = (0.0).xxxx;
					float4 temp_cast_1 = (1.0).xxxx;
					float2 uv022 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float mulTime66 = _Time.y * _SpeedMulti;
					float2 uv010 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float2 panner8 = ( mulTime66 * _NoiseSpeed1 + uv010);
					float2 panner77 = ( _SinTime.w * float2( 1,0 ) + panner8);
					float2 panner12 = ( mulTime66 * _NoiseSpeed2 + uv010);
					float4 temp_cast_3 = (_NoiseExponent).xxxx;
					float4 temp_cast_4 = (0.0).xxxx;
					float4 temp_cast_5 = (1.0).xxxx;
					float4 clampResult32 = clamp( pow( ( tex2D( _UVNoise, panner77 ) * tex2D( _SecondaryUVNoise, panner12 ) ) , temp_cast_3 ) , temp_cast_4 , temp_cast_5 );
					float4 lerpResult21 = lerp( float4( uv022, 0.0 , 0.0 ) , clampResult32 , ( _UVLerp * i.texcoord.w ));
					float4 smoothstepResult38 = smoothstep( temp_cast_0 , temp_cast_1 , lerpResult21);
					float4 break51 = ( i.color * _Color0 * tex2D( _MainTex, smoothstepResult38.rg ) );
					float2 uv045 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float clampResult55 = clamp( i.texcoord.z , 0.0 , 1.0 );
					float lerpResult42 = lerp( break51.a , ( break51.a * pow( distance( uv045.y , _Dropdowndistance ) , _DropdownAlphaExponent ) ) , clampResult55);
					float4 appendResult53 = (float4(break51.r , break51.g , break51.b , lerpResult42));
					

					fixed4 col = appendResult53;
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
1927;7;1906;1005;3658.686;306.3893;1.850822;True;True
Node;AmplifyShaderEditor.CommentaryNode;72;-2992.054,294.8832;Float;False;1814.302;767.1448;;20;12;7;62;13;9;39;14;15;8;10;31;30;70;71;66;67;73;74;77;78;Noise panner;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-2942.054,617.8252;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;67;-2654.915,697.7394;Float;False;Property;_SpeedMulti;SpeedMulti;6;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;66;-2617.379,610.2187;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;30;-2752.718,362.3834;Float;False;Property;_NoiseSpeed1;NoiseSpeed1;4;0;Create;True;0;0;False;0;-1,1;0,-1.4;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.WireNode;70;-2720.715,783.7394;Float;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;8;-2505.431,338.1577;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;31;-2595.402,909.5032;Float;False;Property;_NoiseSpeed2;NoiseSpeed2;5;0;Create;True;0;0;False;0;-1,0;0.1,-1.7;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SinTimeNode;78;-2464.112,464.4815;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;71;-2432.715,823.7394;Float;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;77;-2313.911,341.3815;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;12;-2296.289,904.6312;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;62;-2301.973,702.7812;Float;True;Property;_SecondaryUVNoise;Secondary UVNoise;2;0;Create;True;0;0;False;0;None;cd460ee4ac5c1e746b7a734cc7cc64dd;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;7;-2301.532,485.7454;Float;True;Property;_UVNoise;UVNoise;1;0;Create;True;0;0;False;0;None;4c7a6f1807b2692498adcbf2346eb0fa;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-1890.522,931.3564;Float;False;Property;_NoiseExponent;NoiseExponent;7;0;Create;True;0;0;False;0;0.2;0.4;0.01;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;73;-1484.286,851.9457;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;9;-1977.137,440.8404;Float;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;13;-1969.125,717.8329;Float;True;Property;_TextureSample2;Texture Sample 2;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;59;-1107.545,297.0167;Float;False;824.2182;799.2738;;9;20;41;33;34;32;22;40;21;38;UV lerp, texcoord;1,1,1,1;0;0
Node;AmplifyShaderEditor.WireNode;74;-1471.286,665.9457;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-1648.54,596.2177;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1000.916,580.2059;Float;False;Constant;_Float0;Float 0;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;15;-1438.753,596.1931;Float;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.2;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-1057.545,754.7748;Float;False;Property;_UVLerp;UVLerp;3;0;Create;True;0;0;False;0;0.8;0.088;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;41;-1045.855,894.2903;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;34;-994.9158,663.2059;Float;False;Constant;_Float1;Float 1;5;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-764.1547,841.7902;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;32;-826.4156,494.7061;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;22;-941.3511,359.8091;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;60;-319.7412,-437.996;Float;False;1176.407;687.4909;;6;3;2;6;4;51;5;Texture & color;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;21;-650.0231,450.5471;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;5;-269.7412,-8.719023;Float;False;0;0;_MainTex;Shader;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;61;536.4081,307.2738;Float;False;1417.863;778.0881;;10;45;57;46;56;44;54;55;42;53;64;Dropdown alpha gradient, texcoord;1,1,1,1;0;0
Node;AmplifyShaderEditor.SmoothstepOpNode;38;-483.3266,545.1389;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;64;561.939,661.6833;Float;False;Property;_Dropdowndistance;Dropdown distance;9;0;Create;True;0;0;False;0;1;0.856;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;2;4.845215,-176.9672;Float;False;Property;_Color0;Color 0;0;1;[HDR];Create;True;0;0;False;0;1,1,1,1;191.749,6.02353,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;3;15.1808,-387.9961;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;45;563.408,508.336;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;6;-37.72899,19.49469;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;57;785.3534,806.5164;Float;False;Property;_DropdownAlphaExponent;DropdownAlphaExponent;8;0;Create;True;0;0;False;0;3;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;46;847.214,579.5342;Float;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;448.5462,-162.554;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;56;1078.46,582.624;Float;True;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;44;1079.357,809.3613;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;51;585.666,-164.2339;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;1327.239,524.317;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;55;1318.839,828.7167;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;42;1513.723,479.5501;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;53;1714.271,357.2738;Float;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;2130.363,292.1147;Float;False;True;2;Float;ASEMaterialInspector;0;7;HDRParticleUVPanner;0b6a9f8b4f707c74ca64c0be8e590de0;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;True;2;False;-1;True;True;True;True;False;0;False;-1;False;True;2;False;-1;True;3;False;-1;False;True;4;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;False;0;False;False;False;False;False;False;False;False;False;False;True;0;0;;0;0;Standard;0;0;1;True;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;66;0;67;0
WireConnection;70;0;10;0
WireConnection;8;0;10;0
WireConnection;8;2;30;0
WireConnection;8;1;66;0
WireConnection;71;0;70;0
WireConnection;77;0;8;0
WireConnection;77;1;78;4
WireConnection;12;0;71;0
WireConnection;12;2;31;0
WireConnection;12;1;66;0
WireConnection;73;0;39;0
WireConnection;9;0;7;0
WireConnection;9;1;77;0
WireConnection;13;0;62;0
WireConnection;13;1;12;0
WireConnection;74;0;73;0
WireConnection;14;0;9;0
WireConnection;14;1;13;0
WireConnection;15;0;14;0
WireConnection;15;1;74;0
WireConnection;40;0;20;0
WireConnection;40;1;41;4
WireConnection;32;0;15;0
WireConnection;32;1;33;0
WireConnection;32;2;34;0
WireConnection;21;0;22;0
WireConnection;21;1;32;0
WireConnection;21;2;40;0
WireConnection;38;0;21;0
WireConnection;38;1;33;0
WireConnection;38;2;34;0
WireConnection;6;0;5;0
WireConnection;6;1;38;0
WireConnection;46;0;45;2
WireConnection;46;1;64;0
WireConnection;4;0;3;0
WireConnection;4;1;2;0
WireConnection;4;2;6;0
WireConnection;56;0;46;0
WireConnection;56;1;57;0
WireConnection;51;0;4;0
WireConnection;54;0;51;3
WireConnection;54;1;56;0
WireConnection;55;0;44;3
WireConnection;42;0;51;3
WireConnection;42;1;54;0
WireConnection;42;2;55;0
WireConnection;53;0;51;0
WireConnection;53;1;51;1
WireConnection;53;2;51;2
WireConnection;53;3;42;0
WireConnection;1;0;53;0
ASEEND*/
//CHKSM=8CDFF2E04DA1329C0CB8FED47D82C7D36D75A207
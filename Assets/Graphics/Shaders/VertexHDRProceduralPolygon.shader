// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "VertexHDRProceduralPolygon"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		[HDR]_Color("Color", Color) = (1,1,1,1)
		_Sides("Sides", Float) = 5
		_Size1("Size 1", Vector) = (1,1,0,0)
		_Size2("Size 2", Vector) = (0.9,0.9,0,0)
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
				uniform float _Sides;
				uniform float2 _Size1;
				uniform float2 _Size2;
				uniform float4 _Color;
				float ProceduralPolygon4( float2 UV , float Sides , float Width , float Height )
				{
					const float pi = 3.14159265359;
					float aWidth = Width * cos(pi /  Sides);
					float aHeight = Height * cos(pi / Sides);
					float2 uv = (UV * 2 - 1) / float2(aWidth, aHeight);
					    uv.y *= -1;
					    float pCoord = atan2(uv.x, uv.y);
					    float r = 2 * pi / Sides;
					    float distance = cos(floor(0.5 + pCoord / r) * r - pCoord) * length(uv);
					    return saturate((1 - distance) / fwidth(distance));
				}
				
				float ProceduralPolygon14( float2 UV , float Sides , float Width , float Height )
				{
					const float pi = 3.14159265359;
					float aWidth = Width * cos(pi /  Sides);
					float aHeight = Height * cos(pi / Sides);
					float2 uv = (UV * 2 - 1) / float2(aWidth, aHeight);
					    uv.y *= -1;
					    float pCoord = atan2(uv.x, uv.y);
					    float r = 2 * pi / Sides;
					    float distance = cos(floor(0.5 + pCoord / r) * r - pCoord) * length(uv);
					    return saturate((1 - distance) / fwidth(distance));
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

					float2 uv07 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float2 UV4 = uv07;
					float Sides4 = _Sides;
					float Width4 = _Size1.x;
					float Height4 = _Size1.y;
					float localProceduralPolygon4 = ProceduralPolygon4( UV4 , Sides4 , Width4 , Height4 );
					float2 UV14 = uv07;
					float Sides14 = _Sides;
					float Width14 = _Size2.x;
					float Height14 = _Size2.y;
					float localProceduralPolygon14 = ProceduralPolygon14( UV14 , Sides14 , Width14 , Height14 );
					

					fixed4 col = ( abs( ( localProceduralPolygon4 - localProceduralPolygon14 ) ) * ( i.color * _Color ) );
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
1927;7;1906;1005;1305.374;744.1185;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;15;-487.6685,-197.9155;Float;False;Property;_Sides;Sides;1;0;Create;True;0;0;False;0;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;16;-497.6685,-321.9155;Float;False;Property;_Size1;Size 1;2;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;17;-531.6685,-53.91547;Float;False;Property;_Size2;Size 2;3;0;Create;True;0;0;False;0;0.9,0.9;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-511.3115,-455.8931;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CustomExpressionNode;4;-166,-390.5;Float;False;const float pi = 3.14159265359@$float aWidth = Width * cos(pi /  Sides)@$float aHeight = Height * cos(pi / Sides)@$float2 uv = (UV * 2 - 1) / float2(aWidth, aHeight)@$    uv.y *= -1@$    float pCoord = atan2(uv.x, uv.y)@$    float r = 2 * pi / Sides@$    float distance = cos(floor(0.5 + pCoord / r) * r - pCoord) * length(uv)@$    return saturate((1 - distance) / fwidth(distance))@;1;False;4;True;UV;FLOAT2;0,0;In;;Float;True;Sides;FLOAT;5;In;;Float;True;Width;FLOAT;1;In;;Float;True;Height;FLOAT;1;In;;Float;Procedural Polygon;True;False;0;4;0;FLOAT2;0,0;False;1;FLOAT;5;False;2;FLOAT;1;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;14;-196.8455,-190.0419;Float;False;const float pi = 3.14159265359@$float aWidth = Width * cos(pi /  Sides)@$float aHeight = Height * cos(pi / Sides)@$float2 uv = (UV * 2 - 1) / float2(aWidth, aHeight)@$    uv.y *= -1@$    float pCoord = atan2(uv.x, uv.y)@$    float r = 2 * pi / Sides@$    float distance = cos(floor(0.5 + pCoord / r) * r - pCoord) * length(uv)@$    return saturate((1 - distance) / fwidth(distance))@;1;False;4;True;UV;FLOAT2;0,0;In;;Float;True;Sides;FLOAT;5;In;;Float;True;Width;FLOAT;1;In;;Float;True;Height;FLOAT;1;In;;Float;Procedural Polygon;True;False;0;4;0;FLOAT2;0,0;False;1;FLOAT;5;False;2;FLOAT;1;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;3;-489.4998,301.4002;Float;False;Property;_Color;Color;0;1;[HDR];Create;True;0;0;False;0;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;2;-498.4998,105.4002;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;18;119.5661,-182.8359;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-168.5001,184.4002;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.AbsOpNode;19;323.5661,-138.8359;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;548.3138,24.72945;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;1052.795,-27.99216;Float;False;True;2;Float;ASEMaterialInspector;0;7;VertexHDRProceduralPolygon;0b6a9f8b4f707c74ca64c0be8e590de0;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;True;2;False;-1;True;True;True;True;False;0;False;-1;False;True;2;False;-1;True;3;False;-1;False;True;4;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;False;0;False;False;False;False;False;False;False;False;False;False;True;0;0;;0;0;Standard;0;0;1;True;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;4;0;7;0
WireConnection;4;1;15;0
WireConnection;4;2;16;1
WireConnection;4;3;16;2
WireConnection;14;0;7;0
WireConnection;14;1;15;0
WireConnection;14;2;17;1
WireConnection;14;3;17;2
WireConnection;18;0;4;0
WireConnection;18;1;14;0
WireConnection;9;0;2;0
WireConnection;9;1;3;0
WireConnection;19;0;18;0
WireConnection;10;0;19;0
WireConnection;10;1;9;0
WireConnection;1;0;10;0
ASEEND*/
//CHKSM=65BCD4CD623D21968ACDF94E0A2B761600709AD0
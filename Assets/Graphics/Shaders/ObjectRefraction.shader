// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ObjectRefraction"
{
	Properties
	{
		_TimeScale("Time Scale", Float) = 1
		_Intensity("Intensity", Range( -0.1 , 0.1)) = 0.1
		_Noise("Noise", 2D) = "white" {}
		_Tiling("Tiling", Vector) = (1,1,0,0)
		_DropoffDistance("DropoffDistance", Float) = 55
	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
		LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		GrabPass{ }

		Pass
		{
			Name "Unlit"
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord : TEXCOORD0;
				float3 ase_normal : NORMAL;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
			};

			uniform sampler2D _GrabTexture;
			uniform sampler2D _Noise;
			uniform float2 _Tiling;
			uniform float _TimeScale;
			uniform float _Intensity;
			uniform float _DropoffDistance;
			inline float4 ASE_ComputeGrabScreenPos( float4 pos )
			{
				#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
				#else
				float scale = 1.0;
				#endif
				float4 o = pos;
				o.y = pos.w * 0.5f;
				o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
				return o;
			}
			
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.ase_texcoord1.xyz = ase_worldPos;
				float3 ase_worldNormal = UnityObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord2.xyz = ase_worldNormal;
				float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord4 = screenPos;
				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				o.ase_texcoord3 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				o.ase_texcoord1.w = 0;
				o.ase_texcoord2.w = 0;
				float3 vertexValue =  float3(0,0,0) ;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				fixed4 finalColor;
				float2 uv02 = i.ase_texcoord.xy * _Tiling + float2( 0,0 );
				float mulTime4 = _Time.y * _TimeScale;
				float cos3 = cos( mulTime4 );
				float sin3 = sin( mulTime4 );
				float2 rotator3 = mul( uv02 - float2( 0.5,0.5 ) , float2x2( cos3 , -sin3 , sin3 , cos3 )) + float2( 0.5,0.5 );
				float3 ase_worldPos = i.ase_texcoord1.xyz;
				float3 ase_worldViewDir = UnityWorldSpaceViewDir(ase_worldPos);
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = i.ase_texcoord2.xyz;
				float fresnelNdotV27 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode27 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV27, 5.0 ) );
				float clampResult35 = clamp( ( 1.0 - fresnelNode27 ) , 0.0 , 1.0 );
				float3 worldSpaceViewDir47 = WorldSpaceViewDir( float4( i.ase_texcoord3.xyz , 0.0 ) );
				float clampResult50 = clamp( length( worldSpaceViewDir47 ) , 1.0 , _DropoffDistance );
				float4 screenPos = i.ase_texcoord4;
				float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( screenPos );
				float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
				float4 screenColor10 = tex2D( _GrabTexture, ( (( tex2D( _Noise, rotator3 ) * clampResult35 )*( _Intensity * ( 1.0 - ( clampResult50 / _DropoffDistance ) ) ) + 0.0) + ase_grabScreenPosNorm ).rg );
				
				
				finalColor = screenColor10;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=16400
1927;7;1906;1005;4079.703;1289.278;3.778776;True;True
Node;AmplifyShaderEditor.CommentaryNode;59;-1414.399,637.5494;Float;False;1121.843;310.9994;;7;58;47;48;51;50;49;57;View distance dropoff;1,1,1,1;0;0
Node;AmplifyShaderEditor.PosVertexDataNode;58;-1364.399,689.1929;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;61;-1734.807,-473.9479;Float;False;1509.141;499.5399;;8;17;6;4;2;3;7;5;36;Noise rotator;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;60;-1894.227,132.6296;Float;False;965.64;406.9997;;5;28;29;27;37;35;Fresnel;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldSpaceViewDirHlpNode;47;-1161.068,688.5496;Float;False;1;0;FLOAT4;0,0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;29;-1838.226,355.6293;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;5;-1553.862,-101.1154;Float;False;Property;_TimeScale;Time Scale;0;0;Create;True;0;0;False;0;1;1.48;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;48;-920.1272,689.0118;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-1071.067,833.549;Float;False;Property;_DropoffDistance;DropoffDistance;4;0;Create;True;0;0;False;0;55;55;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;17;-1684.807,-423.9479;Float;False;Property;_Tiling;Tiling;3;0;Create;True;0;0;False;0;1,1;1.2,1.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.WorldNormalVector;28;-1844.227,182.6296;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ClampOpNode;50;-779.0661,687.5496;Float;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;27;-1597.223,251.63;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1415.358,-417.408;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;4;-1360.358,-84.40799;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;6;-1389.358,-234.4078;Float;False;Constant;_Vector0;Vector 0;1;0;Create;True;0;0;False;0;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleDivideOpNode;49;-617.0661,749.5494;Float;False;2;0;FLOAT;0;False;1;FLOAT;20;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;37;-1312.836,236.5012;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;3;-1093.357,-283.4081;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-471.7997,129.2043;Float;False;Property;_Intensity;Intensity;1;0;Create;True;0;0;False;0;0.1;0.0125;-0.1;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;57;-479.5546,741.5717;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-844.8555,-368.1342;Float;True;Property;_Noise;Noise;2;0;Create;True;0;0;False;0;d96e41a067526d54282092e94adf783c;d96e41a067526d54282092e94adf783c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;35;-1103.587,234.4331;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-394.6663,-207.9201;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;62;100.127,-144.885;Float;False;779.2209;390.2894;;4;8;11;9;10;Screen space, scale;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-102.646,234.1166;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;8;171.3306,38.40438;Float;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScaleAndOffsetNode;11;150.1271,-94.88496;Float;False;3;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;9;494.0441,-66.813;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;10;677.3478,-71.54041;Float;False;Global;_GrabScreen0;Grab Screen 0;2;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;32;1041.729,-61.87128;Float;False;True;2;Float;ASEMaterialInspector;0;1;ObjectRefraction;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;False;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;True;0;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;47;0;58;0
WireConnection;48;0;47;0
WireConnection;50;0;48;0
WireConnection;50;2;51;0
WireConnection;27;0;28;0
WireConnection;27;4;29;0
WireConnection;2;0;17;0
WireConnection;4;0;5;0
WireConnection;49;0;50;0
WireConnection;49;1;51;0
WireConnection;37;0;27;0
WireConnection;3;0;2;0
WireConnection;3;1;6;0
WireConnection;3;2;4;0
WireConnection;57;0;49;0
WireConnection;7;1;3;0
WireConnection;35;0;37;0
WireConnection;36;0;7;0
WireConnection;36;1;35;0
WireConnection;53;0;12;0
WireConnection;53;1;57;0
WireConnection;11;0;36;0
WireConnection;11;1;53;0
WireConnection;9;0;11;0
WireConnection;9;1;8;0
WireConnection;10;0;9;0
WireConnection;32;0;10;0
ASEEND*/
//CHKSM=0C05F6B4D3AF79D43277595302BC70EB1F12CDE1
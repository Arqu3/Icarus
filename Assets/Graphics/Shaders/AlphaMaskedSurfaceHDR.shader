// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "AlphaMaskedSurfaceHDR"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.46
		_AlbedoColor("AlbedoColor", Color) = (1,1,1,1)
		[HDR]_Emission("Emission", Color) = (1,1,1,1)
		_EmissionExponent("Emission Exponent", Float) = 2
		_Emissionmask("Emission mask", 2D) = "white" {}
		_Secondaryemissionmask("Secondary emission mask", 2D) = "white" {}
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 uv_tex4coord;
		};

		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float4 _AlbedoColor;
		uniform float4 _Emission;
		uniform sampler2D _Emissionmask;
		uniform float4 _Emissionmask_ST;
		uniform sampler2D _Secondaryemissionmask;
		uniform float4 _Secondaryemissionmask_ST;
		uniform float _EmissionExponent;
		uniform float _Cutoff = 0.46;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 tex2DNode21 = tex2D( _Albedo, uv_Albedo );
			o.Albedo = ( tex2DNode21 * _AlbedoColor ).rgb;
			float2 uv_Emissionmask = i.uv_texcoord * _Emissionmask_ST.xy + _Emissionmask_ST.zw;
			float2 uv_Secondaryemissionmask = i.uv_texcoord * _Secondaryemissionmask_ST.xy + _Secondaryemissionmask_ST.zw;
			float clampResult34 = clamp( i.uv_tex4coord.z , 0.0 , 1.0 );
			float smoothstepResult28 = smoothstep( 0.0 , 1.0 , pow( ( tex2D( _Emissionmask, uv_Emissionmask ).a * tex2D( _Secondaryemissionmask, uv_Secondaryemissionmask ).a * clampResult34 ) , _EmissionExponent ));
			float clampResult33 = clamp( smoothstepResult28 , 0.0 , 1.0 );
			o.Emission = ( _Emission * clampResult33 ).rgb;
			o.Alpha = 1;
			float clampResult35 = clamp( i.uv_tex4coord.w , 0.0 , 1.0 );
			clip( ( tex2DNode21.a * clampResult35 ) - _Cutoff );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
1927;7;1906;1005;663.1691;287.533;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;30;-1681.004,483.6863;Float;False;1691.835;817.0845;;13;10;11;22;9;26;4;28;2;3;19;32;33;34;Glow;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;10;-1631.004,870.9983;Float;True;Property;_Secondaryemissionmask;Secondary emission mask;6;0;Create;True;0;0;False;0;3a5a96df060a5cf4a9cc0c59e13486b7;3a5a96df060a5cf4a9cc0c59e13486b7;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;2;-1611.457,623.7223;Float;True;Property;_Emissionmask;Emission mask;5;0;Create;True;0;0;False;0;8d4dde8d35308a543a4065301a32001d;6d9223843543d554994658935619779f;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;22;-1423.442,1105.382;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;11;-1341.833,878.0489;Float;True;Property;_TextureSample1;Texture Sample 1;4;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-1323.457,622.7223;Float;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;34;-1176.776,1108.59;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-950.2394,885.2288;Float;True;3;3;0;FLOAT;1;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-971.3008,1148.631;Float;False;Property;_EmissionExponent;Emission Exponent;4;0;Create;True;0;0;False;0;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;26;-695.4006,970.738;Float;True;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;31;-936.6642,-362.2007;Float;False;918.7772;748.4216;;7;20;21;24;25;29;23;35;Albedo + opacity;1,1,1,1;0;0
Node;AmplifyShaderEditor.SmoothstepOpNode;28;-457.2567,822.2955;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;29;-731.6465,176.2207;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;20;-886.6641,-312.2007;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;None;087de3c65e1b6d34b94227fec7d49b0c;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.ColorNode;24;-535.2647,-24.086;Float;False;Property;_AlbedoColor;AlbedoColor;2;0;Create;True;0;0;False;0;1,1,1,1;0.7264151,0.5771476,0.5036935,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;21;-600.464,-298.9007;Float;True;Property;_TextureSample2;Texture Sample 2;4;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;33;-282.8297,700.1446;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;4;-626.093,533.6865;Float;False;Property;_Emission;Emission;3;1;[HDR];Create;True;0;0;False;0;1,1,1,1;191.749,31.12157,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;35;-438.9941,199.9944;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-208.2648,-125.086;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-158.1692,546.5493;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-186.887,95.27831;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;801.9002,113;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;AlphaMaskedSurfaceHDR;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.46;True;False;0;True;TransparentCutout;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;4;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;11;0;10;0
WireConnection;3;0;2;0
WireConnection;34;0;22;3
WireConnection;9;0;3;4
WireConnection;9;1;11;4
WireConnection;9;2;34;0
WireConnection;26;0;9;0
WireConnection;26;1;32;0
WireConnection;28;0;26;0
WireConnection;21;0;20;0
WireConnection;33;0;28;0
WireConnection;35;0;29;4
WireConnection;25;0;21;0
WireConnection;25;1;24;0
WireConnection;19;0;4;0
WireConnection;19;1;33;0
WireConnection;23;0;21;4
WireConnection;23;1;35;0
WireConnection;0;0;25;0
WireConnection;0;2;19;0
WireConnection;0;10;23;0
ASEEND*/
//CHKSM=ACD6792EBDB56C083BC239FA0216A54D88F63426
// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "lightning surface"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_cutoff("cutoff", Float) = 0
		[HDR]_Color0("Color 0", Color) = (1,1,1,0)
		_speed("speed", Float) = 1
		_size("size", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Color0;
		uniform sampler2D _TextureSample0;
		uniform float _size;
		uniform float _speed;
		uniform float _cutoff;


		float4 Cutoff3( float4 ColIn , float CutOff )
		{
			if (ColIn[3] < CutOff) return float4(0, 0, 0, 0);
			return ColIn;
		}


		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float mulTime29 = _Time.y * _speed;
			float cos5 = cos( mulTime29 );
			float sin5 = sin( mulTime29 );
			float2 rotator5 = mul( ( i.uv_texcoord * _size ) - float2( -1,-1 ) , float2x2( cos5 , -sin5 , sin5 , cos5 )) + float2( -1,-1 );
			float4 ColIn3 = tex2D( _TextureSample0, rotator5 );
			float CutOff3 = _cutoff;
			float4 localCutoff3 = Cutoff3( ColIn3 , CutOff3 );
			float4 temp_output_17_0 = ( _Color0 * localCutoff3 );
			o.Emission = temp_output_17_0.rgb;
			o.Alpha = temp_output_17_0.a;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
1927;1;1906;1010;829.717;490.0485;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;28;-1358.711,-43.61506;Float;False;Property;_speed;speed;3;0;Create;True;0;0;False;0;1;6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;45;-1451.406,-287.1896;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;48;-1471.156,-137.4017;Float;False;Property;_size;size;4;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;29;-1170.131,-37.97767;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-1156.555,-223.2015;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode;5;-964.2778,-80.9806;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;-1,-1;False;2;FLOAT;1.22;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;2;-748.1822,-103.5769;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;None;6120a09c0bb4abc43a61da17cde3e80e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;4;-641.0847,138.7751;Float;False;Property;_cutoff;cutoff;1;0;Create;True;0;0;False;0;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;15;-738.6291,-327.133;Float;False;Property;_Color0;Color 0;2;1;[HDR];Create;True;0;0;False;0;1,1,1,0;31.62679,26.49365,9.438363,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CustomExpressionNode;3;-379.4802,-41.24228;Float;False;if (ColIn[3] < CutOff) return float4(0, 0, 0, 0)@$return ColIn@;4;False;2;True;ColIn;FLOAT4;0,0,0,0;In;;Float;True;CutOff;FLOAT;0;In;;Float;Cutoff;True;False;0;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-139.8302,-122.6281;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;51;86.28296,-10.04849;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;50;390.3561,-173.4787;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;lightning surface;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;29;0;28;0
WireConnection;47;0;45;0
WireConnection;47;1;48;0
WireConnection;5;0;47;0
WireConnection;5;2;29;0
WireConnection;2;1;5;0
WireConnection;3;0;2;0
WireConnection;3;1;4;0
WireConnection;17;0;15;0
WireConnection;17;1;3;0
WireConnection;51;0;17;0
WireConnection;50;2;17;0
WireConnection;50;9;51;3
ASEEND*/
//CHKSM=7CCA4E5EDDB6F400996461A6555837A792AFC748
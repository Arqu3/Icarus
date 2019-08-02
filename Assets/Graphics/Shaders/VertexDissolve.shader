// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "VertexDissolve"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 1
		[HDR]_Color("Color", Color) = (1,1,1,1)
		_Noise("Noise", 2D) = "white" {}
		_ColorRamp("ColorRamp", 2D) = "white" {}
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 vertexColor : COLOR;
			float4 uv_tex4coord;
			float2 uv_texcoord;
		};

		uniform float4 _Color;
		uniform sampler2D _ColorRamp;
		uniform sampler2D _Noise;
		uniform float4 _Noise_ST;
		uniform float _Cutoff = 1;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Noise = i.uv_texcoord * _Noise_ST.xy + _Noise_ST.zw;
			float temp_output_13_0 = ( i.uv_tex4coord.z + (-5.0 + (( 1.0 - tex2D( _Noise, uv_Noise ).r ) - 0.0) * (10.0 - -5.0) / (1.0 - 0.0)) );
			float2 temp_cast_0 = (temp_output_13_0).xx;
			o.Emission = ( _Color * i.vertexColor.a * tex2D( _ColorRamp, temp_cast_0 ) ).rgb;
			o.Alpha = 1;
			clip( temp_output_13_0 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
1927;1;1906;1010;672.1772;291.7415;1;True;True
Node;AmplifyShaderEditor.SamplerNode;5;-658.5309,157.8246;Float;True;Property;_Noise;Noise;2;0;Create;True;0;0;False;0;d96e41a067526d54282092e94adf783c;d96e41a067526d54282092e94adf783c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;20;-258.198,283.4361;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;21;71.60038,348.7357;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-5;False;4;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;41;51.82275,153.2585;Float;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;13;485.9005,219.9001;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;33;210.5818,-317.5813;Float;False;Property;_Color;Color;1;1;[HDR];Create;True;0;0;False;0;1,1,1,1;72.65929,8.749548,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;3;219.7908,-140.7544;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;39;650.3127,-35.12144;Float;True;Property;_ColorRamp;ColorRamp;3;0;Create;True;0;0;False;0;None;271f5ee3273dd7f4fae6e204d4f8c4bf;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;990.5818,-145.5813;Float;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;28;1232.761,-23.57352;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;VertexDissolve;False;False;False;False;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;1;True;True;0;True;Transparent;;AlphaTest;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;2;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;20;0;5;1
WireConnection;21;0;20;0
WireConnection;13;0;41;3
WireConnection;13;1;21;0
WireConnection;39;1;13;0
WireConnection;37;0;33;0
WireConnection;37;1;3;4
WireConnection;37;2;39;0
WireConnection;28;2;37;0
WireConnection;28;10;13;0
ASEEND*/
//CHKSM=7A987EE8447F15B3A7616CB8FC69E5E6E902BC56
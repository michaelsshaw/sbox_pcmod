
HEADER
{
	Description = "";
}

FEATURES
{
	#include "common/features.hlsl"
}

MODES
{
	VrForward();
	Depth(); 
	ToolsVis( S_MODE_TOOLS_VIS );
	ToolsWireframe( "vr_tools_wireframe.shader" );
	ToolsShadingComplexity( "tools_shading_complexity.shader" );
}

COMMON
{
	#ifndef S_ALPHA_TEST
	#define S_ALPHA_TEST 0
	#endif
	#ifndef S_TRANSLUCENT
	#define S_TRANSLUCENT 0
	#endif
	
	#include "common/shared.hlsl"
	#include "procedural.hlsl"

	#define S_UV2 1
	#define CUSTOM_MATERIAL_INPUTS
}

struct VertexInput
{
	#include "common/vertexinput.hlsl"
	float4 vColor : COLOR0 < Semantic( Color ); >;
};

struct PixelInput
{
	#include "common/pixelinput.hlsl"
	float3 vPositionOs : TEXCOORD14;
	float3 vNormalOs : TEXCOORD15;
	float4 vTangentUOs_flTangentVSign : TANGENT	< Semantic( TangentU_SignV ); >;
	float4 vColor : COLOR0;
	float4 vTintColor : COLOR1;
};

VS
{
	#include "common/vertex.hlsl"

	PixelInput MainVs( VertexInput v )
	{
		PixelInput i = ProcessVertex( v );
		i.vPositionOs = v.vPositionOs.xyz;
		i.vColor = v.vColor;

		ExtraShaderData_t extraShaderData = GetExtraPerInstanceShaderData( v );
		i.vTintColor = extraShaderData.vTint;

		VS_DecodeObjectSpaceNormalAndTangent( v, i.vNormalOs, i.vTangentUOs_flTangentVSign );

		return FinalizeVertex( i );
	}
}

PS
{
	#include "common/pixel.hlsl"
	
	SamplerState g_sSampler0 < Filter( ANISO ); AddressU( BORDER ); AddressV( BORDER ); >;
	CreateInputTexture2D( GlyphTexture, Srgb, 8, "None", "_color", "Textures,1/,0/0", Default4( 1.00, 1.00, 1.00, 1.00 ) );
	Texture2D g_tGlyphTexture < Channel( RGBA, Box( GlyphTexture ), Srgb ); OutputFormat( DXT5 ); SrgbRead( True ); >;
	float g_flTerminalWidth < UiType( Slider ); UiStep( 1 ); UiGroup( ",0/,0/0" ); Default1( 80 ); Range1( 0, 512 ); >;
	float g_flTerminalHeight < UiStep( 1 ); UiGroup( ",0/,0/0" ); Default1( 23 ); Range1( 0, 512 ); >;
	
	float4 MainPs( PixelInput i ) : SV_Target0
	{
		Material m = Material::Init();
		m.Albedo = float3( 1, 1, 1 );
		m.Normal = float3( 0, 0, 1 );
		m.Roughness = 1;
		m.Metalness = 0;
		m.AmbientOcclusion = 1;
		m.TintMask = 1;
		m.Opacity = 1;
		m.Emission = float3( 0, 0, 0 );
		m.Transmission = 0;
		
		float l_0 = g_flTerminalWidth;
		float l_1 = floor( l_0 );
		float l_2 = l_1 / 12;
		float2 l_3 = i.vTextureCoords.xy * float2( 1, 1 );
		float l_4 = l_3.x;
		float l_5 = l_2 * l_4;
		float l_6 = l_5 % 0.083333;
		float l_7 = g_flTerminalHeight;
		float l_8 = floor( l_7 );
		float l_9 = l_8 / 12;
		float l_10 = l_3.y;
		float l_11 = l_9 * l_10;
		float l_12 = l_11 % 0.083333;
		float2 l_13 = float2( l_6, l_12);
		float2 l_14 = i.vTextureCoords.xy * float2( 1, 1 );
		float l_15 = l_14.x;
		float l_16 = l_15 * l_1;
		float l_17 = floor( l_16 );
		float l_18 = l_14.y;
		float l_19 = l_18 * l_8;
		float l_20 = floor( l_19 );
		float l_21 = l_20 * l_1;
		float l_22 = l_17 + l_21;
		float l_23 = l_22 % 12;
		float l_24 = l_23 / 12;
		float l_25 = l_22 / 10.6667;
		float l_26 = floor( l_25 );
		float l_27 = l_26 / 12;
		float2 l_28 = float2( l_24, l_27);
		float2 l_29 = l_13 + l_28;
		float4 l_30 = Tex2DS( g_tGlyphTexture, g_sSampler0, l_29 );
		
		m.Albedo = l_30.xyz;
		m.Opacity = 1;
		m.Roughness = 1;
		m.Metalness = 0;
		m.AmbientOcclusion = 1;
		
		m.AmbientOcclusion = saturate( m.AmbientOcclusion );
		m.Roughness = saturate( m.Roughness );
		m.Metalness = saturate( m.Metalness );
		m.Opacity = saturate( m.Opacity );

		// Result node takes normal as tangent space, convert it to world space now
		m.Normal = TransformNormal( m.Normal, i.vNormalWs, i.vTangentUWs, i.vTangentVWs );

		// for some toolvis shit
		m.WorldTangentU = i.vTangentUWs;
		m.WorldTangentV = i.vTangentVWs;
        m.TextureCoords = i.vTextureCoords.xy;
		
		return ShadingModelStandard::Shade( i, m );
	}
}

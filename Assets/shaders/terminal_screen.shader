
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
	int g_intTerminalWidth < UiType( Slider ); UiStep( 1 ); UiGroup( ",0/,0/0" ); Default1( 80 ); Range1( 0, 512 ); >;
	int g_intTerminalHeight < UiStep( 1 ); UiGroup( ",0/,0/0" ); Default1( 23 ); Range1( 0, 512 ); >;

	
	int CharIndexFromUV(float2 uv) {
		int rowOffset = floor(uv.x * g_intTerminalWidth);
		int colOffset = floor(uv.y * g_intTerminalHeight) * g_intTerminalWidth;

		return rowOffset + colOffset;
	}
	
	// Get the topmost UV position of in the glyph texture for a given ascii index.
	// I honestly don't remember how this works in the slightest.
	float2 IndexToFrameCoords(int index) {
		float x = (index % 12) / 12.0;
		float y = floor(index / 10.666667) / 12.0;
		return float2(x, y);
	}

	float4 MainPs(PixelInput i) : SV_Target0
	{
		Material m = Material::Init();
		float2 uv = i.vTextureCoords.xy;

		// The width and height of a single frame in UV space
		float w = g_intTerminalWidth / 12.0f;
		float h = g_intTerminalHeight / 12.0f;

		// Compute local UV within frame
		float u = (w * uv.x) % 0.083333; // % (1/12)
		float v = (h * uv.y) % 0.083333;

		float2 localUV = float2(u, v);

		int charIndex = CharIndexFromUV(uv);

		// TODO: extract the ascii code for the character from the input string.
		
		float2 charUV = IndexToFrameCoords(charIndex);
		float4 color = Tex2DS(g_tGlyphTexture, g_sSampler0, charUV + localUV);
		m.Albedo = color;

		
		return ShadingModelStandard::Shade( i, m );
	}

}

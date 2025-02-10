// Copyright Andrew Betson.
// SPDX-License-Identifier: MIT

HEADER
{
	Description = "Color blindness assistance shader";
	DevShader = true;
}

MODES
{
	Default();
	Forward();
}

COMMON
{
	#include "postprocess/shared.hlsl"

	struct VS_INPUT
	{
		float3 PositionOS : POSITION < Semantic( PosXyz ); >;
		float2 TexCoord : TEXCOORD0 < Semantic( LowPrecisionUv ); >;
	};

	struct PS_INPUT
	{
		float2 TexCoord : TEXCOORD0;

		// VS only
	#if ( PROGRAM == VFX_PROGRAM_VS )
		float4 PositionPS : SV_Position;
	#endif

	// PS only
	#if ( ( PROGRAM == VFX_PROGRAM_PS ) )
		float4 PositionSS : SV_Position;
	#endif
	};
}

VS
{
	PS_INPUT MainVs( VS_INPUT i )
	{
		PS_INPUT o;

		o.PositionPS = float4( i.PositionOS.xy, 0.0, 1.0 );
		o.TexCoord = i.TexCoord;

		return o;
	}
}

PS
{
	#include "postprocess/common.hlsl"
	#include "postprocess/functions.hlsl"

	#include "ThirdParty/TunableColorBlindnessSolution.hlsl"

	RenderState( DepthWriteEnable, false );
	RenderState( DepthEnable, false );

	Texture2D gColorBuffer < Attribute( "ColorBuffer" ); SrgbRead( true ); >;

	float4 MainPs( PS_INPUT i ) : SV_Target0
	{
		float2 ScreenCoord = CalculateViewportUv( i.PositionSS.xy );
		return AccessibilityPostProcessing( gColorBuffer.Sample( g_sBilinearMirror, ScreenCoord ) );
	}
}

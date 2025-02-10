// Copyright Andrew Betson.
// SPDX-License-Identifier: MIT

using Sandbox.Rendering;

namespace AndrewBetson;

[Title( "Color Blind Assistance" )]
[Category( "Post Processing" )]
[Icon( "visibility" )]
[Description( "Applies color blindness assistance to the camera" )]
public sealed class ColorBlindnessAssistance : PostProcess, Component.ExecuteInEditor
{
	[ConVar(
		Help = "Enable color blindness assistance.",
		Flags = ConVarFlags.Saved
	)]
	private static bool r_colorblind_enable { get; set; } = false;

	[ConVar(
		Help = "Type of color blindness to compensate for. 0: Protanopia, 1: Deuteranopia, 2: Tritanopia",
		Flags = ConVarFlags.Saved,
		Min = 0.0f,
		Max = 2.0f
	)]
	private static int r_colorblind_type { get; set; } = 0;

	[ConVar(
		Help = "Intensity of the daltonization effect. Recommended range is 0.0 to 0.9.",
		Flags = ConVarFlags.Saved,
		Min = 0.0f,
		Max = 1.0f
	)]
	private static float r_colorblind_daltonize_factor { get; set; } = 0.0f;

	[ConVar(
		Help = "Amount to bias the final image's brightness by. Recommended values are -0.10, -0.05, 0.00, 0.05, and 0.11.",
		Flags = ConVarFlags.Saved,
		Min = -1.0f,
		Max = 1.0f
	)]
	private static float r_colorblind_brightness_factor { get; set; } = 0.05f;

	[ConVar(
		Help = "Amount to bias the final image's contrast by. Recommended values are -0.25, -0.12, 0.20, and 0.40.",
		Flags = ConVarFlags.Saved,
		Min = -1.0f,
		Max = 1.0f
	)]
	private static float r_colorblind_contrast_factor { get; set; } = 0.20f;

	private IDisposable mRenderHook;

	protected override void OnEnabled()
	{
		base.OnEnabled();

		mRenderHook = Camera.AddHookBeforeOverlay( "Color Blindness Assistance", 7000 /* AfterPostProcess */, RenderEffect );
	}

	protected override void OnDisabled()
	{
		base.OnDisabled();

		mRenderHook?.Dispose();
		mRenderHook = null;
	}

	RenderAttributes mAttributes = new();

	public void RenderEffect( SceneCamera SceneCamera )
	{
		if ( !SceneCamera.EnablePostProcessing )
		{
			return;
		}

		if ( !r_colorblind_enable )
		{
			return;
		}

		float ProtanopiaFactor = 0.0f;
		float DeuteranopiaFactor = 0.0f;
		float TritanopiaFactor = 0.0f;

		switch ( r_colorblind_type )
		{
			case 0:
			{
				ProtanopiaFactor = 1.0f;
				break;
			}
			case 1:
			{
				DeuteranopiaFactor = 1.0f;
				break;
			}
			case 2:
			{
				TritanopiaFactor = 1.0f;
				break;
			}
		}

		mAttributes.Set( "ProtanopiaFactor", ProtanopiaFactor );
		mAttributes.Set( "DeuteranopiaFactor", DeuteranopiaFactor );
		mAttributes.Set( "TritanopiaFactor", TritanopiaFactor );

		mAttributes.Set( "DaltonizeFactor", r_colorblind_daltonize_factor );
		mAttributes.Set( "BrightnessFactor", r_colorblind_brightness_factor );
		mAttributes.Set( "ContrastFactor", r_colorblind_contrast_factor );

		Graphics.GrabFrameTexture( "ColorBuffer", mAttributes );

		Graphics.Blit( Material.FromShader( "Shaders/PostProcess_ColorBlindnessAssistance.shader" ), mAttributes );
	}
}

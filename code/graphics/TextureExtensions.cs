using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pcmod.Graphics;

public static class TextureExtensions
{
	private static ComputeShader DownscaleShader { get; set; } = new ComputeShader("downscale_cs");

	/// <summary>
	/// Downscale this texture.
	/// </summary>
	/// <param name="texture">Texture to downscale</param>
	/// <param name="target">Texture to put the dowscaled copy in</param>
	public static void Downscale(this Texture texture, Texture target)
	{
		DownscaleShader.Attributes.Set("InputTexture", texture);
		DownscaleShader.Attributes.Set("OutputTexture", target);
		DownscaleShader.Dispatch(target.Width, target.Height, 1);
	}
}
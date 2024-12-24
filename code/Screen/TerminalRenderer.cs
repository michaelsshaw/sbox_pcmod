using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TerminalRenderer
{
	static ComputeShader TerminalShader { get; set; } = new ComputeShader( "terminal" );

	public static Texture GlyphTexture = Texture.Load( FileSystem.Mounted, "textures/terminal_glyphs" );

	public static void DrawTerminal(TerminalScreen screen, Texture outputTexture)
	{
		using (var buffer = new GpuBuffer<TerminalChar>(screen.Contents.Length))
		{
			buffer.SetData( screen.Contents );

			TerminalShader.Attributes.Set( "OutputTexture", outputTexture );
			TerminalShader.Attributes.Set( "GlyphTexture", GlyphTexture );
			TerminalShader.Attributes.Set( "TerminalContents", buffer );
			TerminalShader.Attributes.Set( "Width", screen.Width );
			TerminalShader.Attributes.Set( "Height", screen.Height );

			TerminalShader.Dispatch();
		}

	}
}

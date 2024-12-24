#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class ComputerItem : Component, Component.IPressable
{
	public Texture ScreenTexture { get; private set; }

	public ComputerItem()
	{
		ScreenTexture = Texture.Create( 854, 480 )
			.WithUAVBinding()
			.WithFormat( ImageFormat.BGR888 )
			.Finish();	
	}

	public bool Press( IPressable.Event e )
	{
		throw new NotImplementedException();
	}
}

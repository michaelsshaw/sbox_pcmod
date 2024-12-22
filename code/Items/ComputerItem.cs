using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class ComputerItem : Component, Component.IPressable
{
	public bool Press( IPressable.Event e )
	{
		throw new NotImplementedException();
	}
}

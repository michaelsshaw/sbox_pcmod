using pcmod.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pcmod.GameIntegration;

public interface IScreenProvider
{
	public WritableTexture ScreenTexture { get; }
}

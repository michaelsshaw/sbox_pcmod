/* SPDX-License-Identifier: GPL-2.0-only */
/* Copyright 2023 Igrium */

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

/* SPDX-License-Identifier: GPL-2.0-only */
/* Copyright 2023 Igrium */
#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

namespace pcmod.GameIntegration;

public static class ExtensionMethods
{
	private static Trace CreateTrace(Entity entity)
	{
		var ray = entity.AimRay;
		bool underwater = Trace.TestPoint(ray.Position, "water");

		var trace = Trace.Ray(ray, 2048)
			.WithAnyTags("solid", "player", "npc", "glass")
			.Ignore(entity);

		return trace;
	}

	public static TraceResult GetViewTarget(this Entity entity)
	{
		var trace = CreateTrace(entity);
		return trace.Run();
	}

}
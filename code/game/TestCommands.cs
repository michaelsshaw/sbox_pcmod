/* SPDX-License-Identifier: GPL-2.0-only */
/* Copyright 2023 Igrium */
#nullable enable

using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pcmod.GameIntegration;

public static class TestCommands
{
	static Entity? Caller
	{
		get
		{
			var client = ConsoleSystem.Caller;

			if (client == null)
			{
				var clients = Game.Clients;
				if (clients.Any())
				{
					client = clients.First();
				}
			}

			if (client == null) return null;

			if (client.Pawn is Entity p)
			{
				return p;
			}
			else
			{
				return null;
			}
		}
	}

	[ConCmd.Server("ent_create_computer")]
	public static void SpawnComputer()
	{
		var SpawnPos = Caller?.GetViewTarget().EndPosition;
		//var SpawnPos = Caller?.Position;

		if (!SpawnPos.HasValue)
		{
			Log.Error("Unable to resolve spawn position.");
			return;
		}

		var entity = new ComputerEntity();
		entity.Position = SpawnPos.Value.WithZ(SpawnPos.Value.z + 32);
	}
}
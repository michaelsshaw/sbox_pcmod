/* SPDX-License-Identifier: GPL-2.0-only */
/* Copyright 2023 Michael Shaw */

/* PC-mod main source */

using Sandbox;
using System;

public partial class pcmod_addon
{
	[ConCmd.Client("65_load")]
	public static void loadfile(string fname)
	{
		Span<byte> file = null; 
		try {
			file = FileSystem.Mounted.ReadAllBytes(fname);
		} catch (Exception e) {
			Log.Error(e.Message);
		}

		cpu_6502.cpu cpu = new cpu_6502.cpu();
		file.CopyTo(cpu.mem);

		long i = 0;
		cpu.reset();

		cpu.echo = true;
		while (i < 40000) {
			cpu.cycle();
			i++;
		}

		cpu.echo = false;
		while (i < 40_000_000) {
			cpu.cycle();
			i++;
		}
	}
}

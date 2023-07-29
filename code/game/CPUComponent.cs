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

public partial class CPUComponent : EntityComponent<ComputerEntity>, ISingletonComponent
{
	//public pcmod.ap2.bus bus = new pcmod.ap2.bus();
	public pcmod.ap2.bus? Bus { get; private set; }

	public int ScreenWidth => Entity.ScreenTexture.Width;
	public int ScreenHeight => Entity.ScreenTexture.Height;

	byte[]? data = null;

	public void OnSpawn()
	{
		Log.Info("booting CPU");
		Bus = new pcmod.ap2.bus(Entity.ScreenTexture);
		loadfile("bin/65/screen.rom");
	}

	protected override void OnActivate()
	{
		base.OnActivate();
		data = new byte[ScreenWidth * ScreenHeight * 3];
	}

	public void WriteToScreen(ReadOnlySpan<byte> data)
	{
		Entity.ScreenTexture?.Update(data);
	}

	private int _frameInterval = 0;

	//[GameEvent.Tick.Client]
	//public void WriteDummyTexture()
	//{
	//	if (data == null) return;

	//	if (data == null) return;

	//	if (_frameInterval > 0)
	//	{
	//		_frameInterval--;
	//		return;
	//	}

	//	int time = (int)(Time.Now * 1000);
	//	for (int i = 0; i < ScreenWidth * ScreenHeight * 3; i += 3)
	//	{
	//		byte b = (byte)(time & 0xFF + i);
	//		byte g = (byte)((time >> 8) & 0xFF + i);
	//		byte r = (byte)((time >> 16) & 0xFF + i);

	//		data[i] = b;
	//		data[i + 1] = g;
	//		data[i + 2] = r;
	//	}

	//	WriteToScreen(data.AsSpan());
	//	_frameInterval = 4;
	//}

	public void loadfile(string fname)
	{
		Span<byte> file = null; 
		try {
			file = FileSystem.Mounted.ReadAllBytes(fname);
		} catch (Exception e) {
			Log.Error(e.Message);
		}
		if (Bus == null)
			throw new InvalidOperationException("The computer must be on to load a rom");

		//cpu_6502.cpu cpu = new cpu_6502.cpu();
		var cpu = Bus.cpu;
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
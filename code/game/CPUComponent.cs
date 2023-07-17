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
    public cpu_6502.cpu CPU { get; private set; } = new cpu_6502.cpu();

    public int ScreenWidth => Entity.WritableTexture.Width;
    public int ScreenHeight => Entity.WritableTexture.Height;

    byte[] data = new byte[512 * 512 * 3];

    public void OnSpawn()
    {
        Log.Info("booting CPU");
    }

    public void LoadRom(Span<byte> data)
    {
        data.CopyTo(CPU.mem);
    }

    public void WriteToScreen(ReadOnlySpan<byte> data)
    {
        Entity.WritableTexture?.Update(data);
    }

    private int _frameInterval = 0;

    //[GameEvent.Tick.Client]
    public void WriteDummyTexture()
    {
        DebugOverlay.ScreenText("Buffer: " + data.Length);
        if (data == null) return;

        if (_frameInterval > 0)
        {
            _frameInterval--;
            return;
        }

        int time = (int) (Time.Now * 1000);
        for (int i = 0; i < ScreenWidth * ScreenHeight * 3; i += 3)
        {
            byte b = (byte)(time & 0xFF + i);
            byte g = (byte)((time >> 8) & 0xFF + i);
            byte r = (byte)((time >> 16) & 0xFF + i);

            data[i] = b;
            data[i + 1] = g;
            data[i + 2] = r;
        }

        WriteToScreen(data.AsSpan());
        _frameInterval = 4;
    }
}
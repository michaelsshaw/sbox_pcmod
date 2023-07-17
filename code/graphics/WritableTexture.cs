/* SPDX-License-Identifier: GPL-2.0-only */
/* Copyright 2023 Igrium */
#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

namespace pcmod.Graphics;

/// <summary>
/// A wrapper around a texture that permits easy writing.
/// </summary>
public class WritableTexture
{
    /// <summary>
    /// The underlying texture being written.
    /// </summary>
    public Texture Texture { get; }

    public int Width => Texture.Width;
    public int Height => Texture.Height;

    public WritableTexture(int width, int height)
    {
        ThreadSafe.AssertIsMainThread();

        Texture = new Texture2DBuilder()
            .WithSize(new Vector2(width, height))
            .WithFormat(ImageFormat.BGR888)
            .WithAnonymous(true)
            .WithDynamicUsage()
            .WithUAVBinding()
            .Finish();

        Update(new byte[Width * Height * 3]);
    }

    /// <summary>
    /// Update this texture.
    /// </summary>
    /// <param name="data">The new texture. Should BGR-888 formated and the right size.param>
    public void Update(ReadOnlySpan<byte> data)
    {
        ThreadSafe.AssertIsMainThread();
        Texture.Update(data, 0, 0, Width, Height);
    }

    /// <summary>
    /// Update this texture.
    /// </summary>
    /// <param name="data">The new texture. Should BGR-888 formated and the right size.param>
    public void Update(byte[] data)
    {
        Update(data.AsSpan());
    }
}
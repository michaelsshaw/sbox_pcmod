/* SPDX-License-Identifier: GPL-2.0-only */
/* Copyright 2023 Igrium */
#nullable enable

using pcmod.Graphics;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pcmod.GameIntegration;

public partial class BounceLightComponent : EntityComponent<ComputerEntity>, ISingletonComponent
{
	public Texture DownscaledTexture { get; private set; }

	/// <summary>
	/// The current color of the screen bounce.
	/// </summary>
	public Color32 Color { get; private set; } = Color32.Black;

	public Texture? ScreenTexture { get; set; }

	public PointLightEntity? LightEntity { get; private set; }

	/// <summary>
	/// Number of frames to wait between light updates.
	/// </summary>
	public int UpdateInterval { get; set; } = 1;

	public float Brightness { get; set; } = 10;

	public BounceLightComponent()
	{
		DownscaledTexture = new Texture2DBuilder()
			.WithSize(1, 1)
			.WithFormat(ImageFormat.RGBA8888)
			.WithAnonymous(true)
			.WithDynamicUsage()
			.WithUAVBinding()
			.Finish();
	}

	protected override void OnActivate()
	{
		base.OnActivate();
		LightEntity = CreateLight();
	}

	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		LightEntity?.Delete();
	}

	private int _interval;

	[GameEvent.Tick.Client]
	protected virtual void TickFrame()
	{
		if (_interval > 0)
		{
			_interval--;
		} else
		{
			UpdateColor();
			_interval = 0;
		}
	}

	

	private PointLightEntity CreateLight()
	{
		var light = new PointLightEntity()
		{
			Brightness = Brightness,
			Transform = Entity.Transform,
			Range = 64
		};
		light.SetParent(Entity);
		return light;
	}

	private void UpdateColor()
	{
		Color32 color;
		if (ScreenTexture != null)
		{
			ScreenTexture.Downscale(DownscaledTexture);
			Color = DownscaledTexture.GetPixel(0, 0);
			color = Color;
		}
		else
		{
			color = Color32.Black;
		}

		if (LightEntity != null)
		{
			LightEntity.Color = color.ToColor();
			LightEntity.RenderDirty();
		}
		DebugOverlay.Texture(DownscaledTexture, new Rect(0, 0, 64, 64));


	}
}
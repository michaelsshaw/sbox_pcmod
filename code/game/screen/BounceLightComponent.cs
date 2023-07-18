/* SPDX-License-Identifier: GPL-2.0-only */
/* Copyright 2023 Igrium */
#nullable enable

using pcmod.Graphics;
using Sandbox;
using Sandbox.ModelEditor.Nodes;
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

	//public PointLightEntity? LightEntity { get; private set; }
	public PointLightEntity? LightEntity { get; private set; }

	/// <summary>
	/// Number of frames to wait between light updates.
	/// </summary>
	public int UpdateInterval { get; set; } = 1;

	public float Brightness { get; set; } = .01f;

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
	protected virtual void TickClient()
	{
		if (LightEntity == null) return;
		//DebugOverlay.Axis(LightEntity.Transform.Position, LightEntity.Transform.Rotation, depthTest: false);
		//DebugOverlay.Text($"Size: [{LightEntity.LightSize}, {LightEntity.PlaneHeight}]", LightEntity.Transform.Position + new Vector3(0, 0, 32));

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
		var plane = GetLightPlane();
		var light = new RectangleLightEntity()
		{
			Brightness = Brightness,
			Transform = Entity.Transform.ToWorld(plane.Transform),
			LightSize = plane.Width / 2f,
			PlaneHeight = plane.Height / 2f,
			Range = 64,
			EnableShadowCasting = false
		};
		light.SetParent(Entity);
		return light;
	}

	private LimitedPlane GetLightPlane()
	{

		if ( Entity.Model.TryGetData(out ModelComputerScreenLight[] lights ))
		{
			var plane = lights[0].Plane;
			plane = plane.WithRotation(plane.Transform.RotationToWorld(Rotation.FromPitch(-90)));
			return plane;
		} else
		{
			Log.Warning($"no computer screen light node found in {Entity.Model.Name}");
			return new LimitedPlane(new Transform(new Vector3()), new Vector2(16, 16));
		}
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

	}
}
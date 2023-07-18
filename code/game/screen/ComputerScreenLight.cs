/* SPDX-License-Identifier: GPL-2.0-only */
/* Copyright 2023 Igrium */
#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.ModelEditor;
using Sandbox.ModelEditor.Internal;
using Sandbox.ModelEditor.Nodes;

namespace pcmod.GameIntegration;

/// <summary>
/// An area light will be spawned with the aggregate contents of the screen colors.
/// </summary>
[GameData("pcmod_screen_light", AllowMultiple = true)]
[Box("dimensions", Bone = "bonename", Origin = "offset_origin", Angles = "offset_angles")]
[Axis(Bone = "bonename", Origin = "offset_origin", Angles = "offset_angles")]
public class ModelComputerScreenLight
{
	[FGDType("model_bone")]
	public string BoneName { get; set; }

	private Vector3 _dimensions;

	[ScaleBoneRelative]
	public Vector3 Dimensions
	{
		get => _dimensions; set
		{
			_dimensions = value.WithZ(0);
		}
	}
	[JsonPropertyName("offset_origin"), ScaleBoneRelative]
	public Vector3 OriginOffset { get; set; }

	[JsonPropertyName("offset_angles")]
	public Angles Angles { get; set; }

	/// <summary>
	/// The position of the plane relative to the parent bone.
	/// </summary>
	public LimitedPlane Plane
	{
		get
		{
			Transform transform = new(OriginOffset, Angles.ToRotation());
			Vector2 dimensions = new(Dimensions.x, Dimensions.y);
			return new LimitedPlane(transform, dimensions);
		}
	}

	/// <summary>
	/// The position of the plane relative to the model root.
	/// </summary>
	/// <param name="model">Model to use.</param>
	/// <returns></returns>
	public LimitedPlane LocalPlane(Model model)
	{
		var bone = model.Bones.GetBone(BoneName);
		if (bone != null)
		{
			var plane = Plane;
			return plane.WithTransform(bone.LocalTransform.ToWorld(plane.Transform));
		}
		else
		{
			return Plane;
		}

	}
}
/* SPDX-License-Identifier: GPL-2.0-only */
/* Copyright 2023 Igrium */
#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pcmod.GameIntegration;

/// <summary>
/// A planar object with an origin (center) and dimensions.
/// </summary>
public struct LimitedPlane
{
	public Transform Transform { get; init; }
	public Vector2 Dimensions { get; init; }

	public Vector3 TopLeft => Transform.TransformVector(new Vector3(Dimensions.x / -2f, Dimensions.y / -2f));
	public Vector3 TopRight => Transform.TransformVector(new Vector3(Dimensions.x / 2f, Dimensions.y / -2f));
	public Vector3 BottomLeft => Transform.TransformVector(new Vector3(Dimensions.x / -2f, Dimensions.y / 2f));
	public Vector3 BottomRight => Transform.TransformVector(new Vector3(Dimensions.x / -2f, Dimensions.y / -2f));
	public Vector3 Normal => Transform.NormalToWorld(new Vector3(0, 0, 1));
	public Plane Plane => new Plane(Transform.Position, Normal);
	public float Width => Dimensions.x;
	public float Height => Dimensions.y;
	public Vector3 Origin => Transform.Position;
	public Rotation Rotation => Transform.Rotation;

	public LimitedPlane(Transform origin, Vector2 dimensions)
	{
		Transform = origin;
		Dimensions = dimensions;
	}

	public LimitedPlane(Transform origin, Vector3 dimensions)
	{
		Transform = origin;
		Dimensions = new Vector2(dimensions.x, dimensions.y);
	}

	public LimitedPlane WithTransform(Transform transform)
	{
		return new LimitedPlane(transform, Dimensions);
	}

	public LimitedPlane WithOrigin(Vector3 origin)
	{
		return new LimitedPlane(Transform.WithPosition(origin), Dimensions);
	}

	public LimitedPlane WithRotation(Rotation rotation)
	{
		return new LimitedPlane(Transform.WithRotation(rotation), Dimensions);
	}

	public LimitedPlane WithDimensions(Vector2 dimensions)
	{
		return new LimitedPlane(Transform, dimensions);
	}

}
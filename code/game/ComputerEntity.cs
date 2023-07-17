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

// TODO: Wait for Facepunch to fix their whitelist code so we can use BasePhysics
public partial class ComputerEntity : ModelEntity, IUse
{
    // The second material in the first material group is targeted.

    public WritableTexture WritableTexture { get; private set; } = new WritableTexture(512, 512);

    public Material? ScreenMaterial { get; set; }

    /// <summary>
    /// The running CPU of this computer.
    /// Not null if the computer is on.
    /// </summary>
    [BindComponent] public CPUComponent? CPU { get; }

    public override void Spawn()
    {
        base.Spawn();
        SetModel("models/computer/apple2.vmdl");

        PhysicsEnabled = true;
        UsePhysicsCollision = true;

        Tags.Add("solid");
        Log.Info("spawning");
    }

    public override void ClientSpawn()
    {
        base.ClientSpawn();

        foreach(var n in Model.Materials.Select(m => m.Name))
        {
        }

        WritableTexture = new WritableTexture(512, 512);
        ScreenMaterial = Material.Load("materials/screen/screen_overlay.vmat").CreateCopy();
    }

    /// <summary>
    /// Create a new CPU component and boot it.
    /// </summary>
    /// <returns></returns>
    public CPUComponent Boot()
    {
        var c = Components.Create<CPUComponent>();
        c.OnSpawn();
        return c;
    }

    [GameEvent.Client.Frame]
    public void Frame()
    {
        if (ScreenMaterial == null || WritableTexture == null) return;

        ScreenMaterial.Set("SelfIllumMask", WritableTexture.Texture);
        SetMaterialOverride(ScreenMaterial, "ScreenContent");
    }

    public bool OnUse(Entity user)
    {
        if (CPU == null) Boot();
        return true;
    }

    public bool IsUsable(Entity user)
    {
        return true;
    }
}
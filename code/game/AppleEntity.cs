using pcmod.Graphics;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pcmod.GameIntegration;

// TODO: Wait for Facepunch to fix their whitelist code so we can use BasePhysics
public partial class AppleEntity : ModelEntity
{
    // The second material in the first material group is targeted.

    public WritableTexture Texture { get; private set; }

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

        Texture = new WritableTexture(512, 512);

        foreach (var m in Model.Materials)
        {
            //Log.Info("attr: " + m.Attributes.GetTexture("colorAttr")?.ImageFormat);
            //Log.Info("tex: " + Texture.Texture.ImageFormat);
            //Log.Info("material: " + m.Name);
            m.Attributes.Set("colorAttr", Texture.Texture);
            //Log.Info("attr: " + m.Attributes.GetTexture("colorAttr")?.ImageFormat);

        }
    }
}
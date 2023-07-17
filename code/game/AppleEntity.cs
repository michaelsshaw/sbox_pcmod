using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pcmod.GameIntegration;

public partial class AppleEntity : BasePhysics
{
    public override void Spawn()
    {
        base.Spawn();
        SetModel("models/computer/apple2.vmdl");
        Tags.Add("solid");
    }
}
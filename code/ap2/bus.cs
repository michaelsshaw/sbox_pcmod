﻿/* SPDX-License-Identifier: GPL-2.0-only */
/* Copyright 2023 Michael Shaw */

/* Apple II bus
 *
 * This is a simple bus that connects the CPU to the RAM as well as any MMIO
 * devices.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pcmod.ap2;
public partial class bus
{
	public const ushort AP2_DBG_SCREEN_ZONE = 0xE900; 

        public cpu_6502.cpu cpu = new cpu_6502.cpu();
        public byte[] ram = new byte[64 * 1024];
	
	public bus()
	{
                cpu.mem = ram;
        }
}

public class cpu : cpu_6502.cpu
{
	public override byte read_mem(ushort addr)
	{
		cycles++;
		return mem[addr];
	}

	public override void write_mem(ushort addr, byte val)
	{
		cycles += 1;
		mem[addr] = val;
	}
}
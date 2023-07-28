/* SPDX-License-Identifier: GPL-2.0-only */
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
	const ushort AP2_DMA_BEGIN = 0xC000;
	const ushort PWOF_ADDR = 0xC001;

	public override byte read_mem(ushort addr)
	{
		cycles += 1;
		return mem[addr];
	}

	public override void write_mem(ushort addr, byte val)
	{
		if (addr == AP2_DMA_BEGIN) {
			byte[] vmem = new byte[256 * 256];

			for (int i = 0; i < vmem.Length; i++) {
				vmem[i] = mem[0x2900 + i];
			}
		} else if (addr == PWOF_ADDR) {
			
		} else {
                        cycles += 1;
                        mem[addr] = val;
		}
	}
}

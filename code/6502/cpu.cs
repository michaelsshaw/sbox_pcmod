/* SPDX-License-Identifer: GPL-2.0-only */
/* Copyright 2023 Michael Shaw */

/* 6502 CPU */

namespace cpu_6502;

public partial class cpu
{
	byte A;
	byte X;
	byte Y;

	byte flags;
	ushort PC;
	
	byte *mem;

	int cycles;

	public cpu()
	{
		A = 0;
		X = 0;
		Y = 0;
		flags = 0;
		cycles = 0;
	}

	public void attach_mem(byte *mem)
	{
		this.mem = mem;
	}

	public byte read_mem(ushort addr)
	{
		cycles += 1;
		return mem[addr];
	}

	public void set_flag(byte flag, bool cond)
	{
		if (cond)
			flags |= flag;
		else
			flags &= (~flag);
	}

	public bool read_flag(byte flag)
	{
		return (flags & flag) == flag;
	}
}

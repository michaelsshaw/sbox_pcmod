/* SPDX-License-Identifer: GPL-2.0-only */
/* Copyright 2023 Michael Shaw */

/* 6502 CPU */

namespace cpu_6502;

enum adm6502
{
	ADM_A,
	ADM_ABS,
	ADM_ABX,
	ADM_ABY,
	ADM_IMM,
	ADM_IMP,
	ADM_IND,
	ADM_INX,
	ADM_INY,
	ADM_REL,
	ADM_ZPG,
	ADM_ZPX,
	ADM_ZPY
};

public partial class cpu
{
	byte A;
	byte X;
	byte Y;

	byte flags;
	ushort PC;
	byte SP;
	
	byte[] mem;

	int cycles;

	/* Addressing mode */
	adm6502 adm;

	public cpu()
	{
		mem = new byte[0x10000];
	}

	public byte read_mem(ushort addr)
	{
		cycles++;
		return mem[addr];
	}

	public void write_mem(ushort addr, byte val)
	{
		cycles += 1;
		mem[addr] = val;
	}

	public void set_flag(byte flag, bool cond)
	{
		if (cond)
			flags |= flag;
		else
			flags &= (byte)(~flag);
	}

	public bool read_flag(byte flag)
	{
		return (flags & flag) == flag;
	}

	public void push(byte val)
	{
		write_mem((ushort)(0x0100 | SP), val);
		SP -= 1;
	}

	public byte pop()
	{
		SP += 1;
		return read_mem((ushort)(0x0100 | SP));
	}
}

/* SPDX-License-Identifer: GPL-2.0-only */
/* Copyright 2023 Michael Shaw */

/* 6502 Addressing Modes */

namespace cpu_6502;

public partial class cpu
{
	delegate ushort addrmode();

	ushort a()
	{
		adm = adm6502.ADM_A;
		return 0;
	}

	ushort abs()
	{
		byte l = read_mem(PC);
		ushort h = read_mem((ushort)(PC + 1));
		
		h <<= 8;
		h |= l;

		PC += 2;
		adm = adm6502.ADM_ABS;

		return h;
	}

	ushort abx()
	{
		byte l = read_mem(PC);
		ushort h = read_mem((ushort)(PC + 1));

		h <<= 8;
		h |= l;
		h += X;

		PC += 2;
		
		adm = adm6502.ADM_ABX;

		if ((ushort)(l + X) > 0xFF)
			cycles += 1;

		return h;
	}

	ushort imm()
	{
		PC += 1;
		adm = adm6502.ADM_IMM;

		return (ushort)(PC - 1);
	}

	ushort imp()
	{
		adm = adm6502.ADM_IMP;
		return 0;
	}

	ushort ind()
	{
		byte l = read_mem(PC);
		ushort h = read_mem((ushort)(PC + 1));

		h <<= 8;
		h |= l;

		ushort r;

		if (l == 0xFF) 
			r = (ushort)((read_mem((ushort)(h & 0xFF00)) << 8) | read_mem(h));
		else
			r = (ushort)(((read_mem((ushort)(h + 1))) << 8) | read_mem(h));

		PC += 2;
		
		adm = adm6502.ADM_IND;
		return r;
	}

	ushort inx()
	{
		byte l = read_mem(PC);
		byte a1 = (byte)(l + X + 1);
		byte a = (byte)(l + X);

		ushort r = (ushort)((read_mem((ushort)(a1)) << 8) | read_mem(a));

		cycles += 1;
		PC += 1;

		adm = adm6502.ADM_INX;
		return r;
	}

	ushort iny()
	{
		ushort t = read_mem(PC);

		ushort l = (ushort)(read_mem((ushort)(t & 0xFF)) & 0xFF);
		ushort h = read_mem((ushort)((t + 1) & 0xFF));

		h <<= 8;
		h |= l;

		h += Y;

		/* Page boundary cross */
		if ((l + Y) > 0xFF)
			cycles += 1;

		PC += 1;
		
		adm = adm6502.ADM_INY;
		return h;
	}

	ushort rel()
	{
		byte b = read_mem(PC);
		PC += 1;

		ushort r = PC;


		adm = adm6502.ADM_REL;

		if (neg8(b))
			return (ushort)(r - (((~b) & 0x00FF) + 1));
		else
			return (ushort)(r + b);
	}

	ushort zpg()
	{
		byte b = read_mem(PC);
		PC += 1;

		adm = adm6502.ADM_ZPG;
		return (ushort)(0x00FF & b);
	}

	ushort zpx()
	{
		byte b = read_mem(PC);
		ushort r = (ushort)(0x00FF & (b + X));

		PC += 1;
		cycles += 1;

		adm = adm6502.ADM_ZPG;
		return r;
	}

	ushort zpy()
	{
		byte b = read_mem(PC);
		ushort r = (ushort)(0x00FF & (b + Y));

		PC += 1;
		cycles += 1;

		adm = adm6502.ADM_ZPY;
		return r;
	}
}

/* SPDX-License-Identifier: GPL-2.0-only */
/* Copyright 2023 Michael Shaw */

/* 6502 CPU */

namespace cpu_6502;

public partial class cpu
{
	public byte A;
	public byte X;
	public byte Y;

	public byte flags;
	public ushort PC;
	public byte SP;

	public byte[] mem;

	public int cycles;

	/* Addressing mode */
	public adm6502 adm;

	/* Number of bytes used by the current addressing mode */
	public int pbytes;

	public cpu()
	{
		mem = new byte[0x10000];
		opcodes_populate();
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

	/* cycle: executes an entire instruction at once
	 *
	 * cycles are added to the next call, so it retains cycle accuracy
	 * without splitting a single instruction over several calls
	 */
	public void cycle()
	{
		if (cycles == 0) {
			byte ins = read_mem(PC);

			opcode op = opcodes[ins];
			
			PC += 1;

			ushort addr = op.adm();
			op.ins(addr);
			print_instruction(ins);

			cycles += 1;
		} else {
			cycles -= 1;
		}
	}

	public void nmi()
	{
		push((byte)(PC >> 8));
		push((byte)(PC));

		set_flag(FLAG_B, false);
		set_flag(FLAG_5, true);
		set_flag(FLAG_I, true);

		push(flags);

		byte l = read_mem(0xFFFA);
		ushort h = read_mem(0xFFFB);

		h <<= 8;
		h |= l;

		PC = h;
		cycles = 7;
	}

	public void irq()
	{
		if (!read_flag(FLAG_I)) {
			push((byte)(PC >> 8));
			push((byte)(PC));

			set_flag(FLAG_B, false);
			set_flag(FLAG_5, true);
			set_flag(FLAG_I, true);

			push(flags);

			byte l = read_mem(0xFFFE);
			ushort h = read_mem(0xFFFF);

			h <<= 8;
			h |= l;

			PC = h;
			cycles = 7;
		}
	}

	public void reset()
	{
		byte l = read_mem(0xFFFC);
		ushort h = read_mem(0xFFFD);

		h <<= 8;
		h |= l;

		PC = h;
		flags = 0x34;
		A = 0;
		X = 0;
		Y = 0;
		SP = 0xFD;

		cycles = 8;
	}
}

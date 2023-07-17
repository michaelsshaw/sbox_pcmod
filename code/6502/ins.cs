/* SPDX-License-Identifier: GPL-2.0-only */
/* Copyright 2023 Michael Shaw */

/* 6502 Instructions */

namespace cpu_6502;

public partial class cpu
{
	private bool invert = false;

	byte FLAG_N = 0x80;
	byte FLAG_V = 0x40;
	byte FLAG_5 = 0x20;
	byte FLAG_B = 0x10;
	byte FLAG_D = 0x08;
	byte FLAG_I = 0x04;
	byte FLAG_Z = 0x02;
	byte FLAG_C = 0x01;

	public delegate void instruction(ushort addr);

	static bool neg8(byte a)
	{
		return (a & 0x80) > 0;
	}

	void invalid(ushort addr)
	{
	}

	void adc(ushort addr)
	{
		byte m = read_mem(addr);

		if (invert) {
			invert = false;
			m = (byte)(~m);
		}

		byte c = 0;

		if (read_flag(FLAG_C)) 
			c = 1;

		ushort sum = (ushort)(A + m + c);


		set_flag(FLAG_C, sum > 0xFF);
		set_flag(FLAG_V, ((~(A ^ m) & (A ^ sum)) & 0x80) > 0);

		A = (byte)sum;

		set_flag(FLAG_Z, A == 0);
		set_flag(FLAG_N, neg8(A));
	}

	void and(ushort addr)
	{
		byte m = read_mem(addr);

		A &= m;

		set_flag(FLAG_Z, A == 0);
		set_flag(FLAG_N, neg8(A));
	}

	void asl(ushort addr)
	{
		byte m = read_mem(addr);
		set_flag(FLAG_C, (flags & 0x80) > 0);

		cycles += 1;
		m <<= 0x01;

		write_mem(addr, m);

		if (adm == adm6502.ADM_ABX)
			cycles = 7;

		set_flag(FLAG_Z, m == 0);
		set_flag(FLAG_N, neg8(m));
	}	

	void asl_a(ushort addr)
	{
		set_flag(FLAG_C, neg8(A));

		A <<= 0x01;
		cycles += 1;

		set_flag(FLAG_Z, A == 0);
		set_flag(FLAG_N, neg8(A));
	}

	void bcc(ushort addr)
	{
		if (!read_flag(FLAG_C)) {
			cycles += 1;

			if (((addr)&0xFF00) != (PC & 0xFF00)) {
				/* Page boundary cross */
				cycles += 1;
			}

			PC = addr;
		}
	}

	void bcs(ushort addr)
	{
		if (read_flag(FLAG_C)) {
			cycles += 1;

			if (((addr)&0xFF00) != (PC & 0xFF00)) {
				/* Page boundary cross */
				cycles += 1;
			}

			PC = addr;
		}
	}

	void beq(ushort addr)
	{
		if (read_flag(FLAG_Z)) {
			cycles += 1;

			if (((addr)&0xFF00) != (PC & 0xFF00)) {
				/* Page boundary cross */
				cycles += 1;
			}

			PC = addr;
		}
	}

	void bit(ushort addr)
	{
		byte m = read_mem(addr);

		set_flag(FLAG_N, neg8(m));
		set_flag(FLAG_V, (m & 0x40) > 0);

		m &= A;

		set_flag(FLAG_Z, m == 0);
	}

	void bmi(ushort addr)
	{
		if (read_flag(FLAG_N)) {
			cycles += 1;

			if (((addr)&0xFF00) != (PC & 0xFF00)) {
				/* Page boundary cross */
				cycles += 1;
			}

			PC = addr;
		}
	}

	void bne(ushort addr)
	{
		if (!read_flag(FLAG_Z)) {
			cycles += 1;

			if (((addr)&0xFF00) != (PC & 0xFF00)) {
				/* Page boundary cross */
				cycles += 1;
			}

			PC = addr;
		}
	}

	void bpl(ushort addr)
	{
		if (!read_flag(FLAG_N)) {
			cycles += 1;

			if (((addr)&0xFF00) != (PC & 0xFF00)) {
				/* Page boundary cross */
				cycles += 1;
			}

			PC = addr;
		}
	}

	void brk(ushort addr)
	{
		ushort p = (ushort)(PC + 1);
		
		push((byte)((p >> 0x08) & 0xFF));
		push((byte)(p & 0xFF));
		push((byte)(flags | FLAG_B | FLAG_5));

		set_flag(FLAG_I, true);
		byte ll = read_mem(0xFFFE);
		ushort hh = (ushort)read_mem(0xFFFF);

		hh <<= 0x08;
		hh |= ll;

		cycles += 1;

		PC = hh;
	}

	void bvc(ushort addr)
	{
		if (!read_flag(FLAG_V)) {
			cycles += 1;

			if (((addr)&0xFF00) != (PC & 0xFF00)) {
				/* Page boundary cross */
				cycles += 1;
			}

			PC = addr;
		}
	}

	void bvs(ushort addr)
	{
		if (read_flag(FLAG_V)) {
			cycles += 1;

			if (((addr)&0xFF00) != (PC & 0xFF00)) {
				/* Page boundary cross */
				cycles += 1;
			}

			PC = addr;
		}
	}

	void clc(ushort addr)
	{
		set_flag(FLAG_C, false);
		cycles += 1;
	}

	void cld(ushort addr)
	{
		set_flag(FLAG_D, false);
		cycles += 1;
	}

	void cli(ushort addr)
	{
		set_flag(FLAG_I, false);
		cycles += 1;
	}

	void clv(ushort addr)
	{
		set_flag(FLAG_V, false);
		cycles += 1;
	}

	void cmp(ushort addr)
	{
		byte m = read_mem(addr);

		byte a = (byte)(A - m);

		set_flag(FLAG_N, neg8(a));
		set_flag(FLAG_Z, A == m);
		set_flag(FLAG_C, A >= m);
	}

	void cpx(ushort addr)
	{
		byte m = read_mem(addr);
		byte a = (byte)(X - m);

		set_flag(FLAG_N, neg8(a));
		set_flag(FLAG_Z, X == m);
		set_flag(FLAG_C, X >= m);
	}

	void cpy(ushort addr)
	{
		byte m = read_mem(addr);
		byte a = (byte)(Y - m);

		set_flag(FLAG_N, neg8(a));
		set_flag(FLAG_Z, Y == m);
		set_flag(FLAG_C, Y >= m);
	}

	void dec(ushort addr)
	{
		byte m = read_mem(addr);
		m--;

		cycles += 1;

		write_mem(addr, m);

		if (adm == adm6502.ADM_ABX)
			cycles = 7;

		set_flag(FLAG_N, neg8(m));
		set_flag(FLAG_Z, m == 0);
	}

	void dex(ushort addr)
	{
		X--;
		cycles += 1;

		set_flag(FLAG_N, neg8(X));
		set_flag(FLAG_Z, X == 0);
	}

	void dey(ushort addr)
	{
		Y--;
		cycles += 1;

		set_flag(FLAG_N, neg8(Y));
		set_flag(FLAG_Z, Y == 0);

	}

	void eor(ushort addr)
	{
		byte m = read_mem(addr);

		A ^= m;

		set_flag(FLAG_N, neg8(A));
		set_flag(FLAG_Z, A == 0);
	}

	void inc(ushort addr)
	{
		byte m = read_mem(addr);

		m += 1;
		cycles += 1;

		write_mem(addr, m);

		if (adm == adm6502.ADM_ABX)
			cycles = 7;

		set_flag(FLAG_N, neg8(m));
		set_flag(FLAG_Z, m == 0);
	}

	void inx(ushort addr)
	{
		X += 1;
		cycles += 1;

		set_flag(FLAG_N, neg8(X));
		set_flag(FLAG_Z, X == 0);
	}

	void iny(ushort addr)
	{
		Y += 1;
		cycles += 1;

		set_flag(FLAG_N, neg8(Y));
		set_flag(FLAG_Z, Y == 0);
	}

	void jmp(ushort addr)
	{
		PC = addr;
	}

	void jsr(ushort addr)
	{
		push((byte)(((PC - 1) >> 0x08) & 0xFF));
		push((byte)((PC - 1) & 0xFF));
		PC = addr;

		cycles += 1;
	}

	void lda(ushort addr)
	{
		A = read_mem(addr);

		set_flag(FLAG_N, neg8(A));
		set_flag(FLAG_Z, A == 0);
	}

	void ldx(ushort addr)
	{
		X = read_mem(addr);

		set_flag(FLAG_N, neg8(X));
		set_flag(FLAG_Z, X == 0);
	}

	void ldy(ushort addr)
	{
		Y = read_mem(addr);

		set_flag(FLAG_N, neg8(Y));
		set_flag(FLAG_Z, Y == 0);
	}

	void lsr(ushort addr)
	{
		byte m = read_mem(addr);

		set_flag(FLAG_C, (m & 0x01) > 0);
		
		m >>= 1;
		cycles += 1;

		set_flag(FLAG_Z, m == 0);
		set_flag(FLAG_C, false);

		write_mem(addr, m);

		if (adm == adm6502.ADM_ABX)
			cycles = 7;
	}

	void lsr_a(ushort addr)
	{
		set_flag(FLAG_C, (A & 0x01) > 0);

		A >>= 1;
		cycles += 1;

		set_flag(FLAG_Z, A == 0);
		set_flag(FLAG_N, false);
	}

	void nop(ushort addr)
	{
		cycles += 1;
	}

	void ora(ushort addr)
	{
		byte m = read_mem(addr);

		A |= m;

		set_flag(FLAG_N, neg8(A));
		set_flag(FLAG_Z, A == 0);
	}

	void pha(ushort addr)
	{
		push(A);
		cycles += 1;
	}

	void php(ushort addr)
	{
		push((byte)(flags | FLAG_B | FLAG_5));
	}

	void pla(ushort addr)
	{
		A = pop();

		set_flag(FLAG_N, neg8(A));
		set_flag(FLAG_Z, A == 0);
	}

	void plp(ushort addr)
	{
		flags = (byte)(pop() & (byte)(~(FLAG_5 | FLAG_B)));
	}

	void rol(ushort addr)
	{
		byte m = read_mem(addr);

		bool neg = neg8(m);

		m <<= 1;
		
		m |= (byte)(flags & FLAG_C);
		cycles += 1;

		write_mem(addr, m);

		if (adm == adm6502.ADM_ABX)
			cycles = 7;

		set_flag(FLAG_C, neg);
		set_flag(FLAG_N, neg8(m));
		set_flag(FLAG_Z, m == 0);
	}

	void rol_a(ushort addr)
	{
		bool neg = neg8(A);

		A <<= 1;
		A |= (byte)(flags & FLAG_C);

		cycles += 1;

		set_flag(FLAG_C, neg);
		set_flag(FLAG_N, neg8(A));
		set_flag(FLAG_Z, A == 0);
	}

	void ror(ushort addr)
	{
		byte m = read_mem(addr);

		bool c = (m & 0x01) > 0;

		m >>= 0x01;
		if (read_flag(FLAG_C))
			m |= 0x80;

		cycles += 1;

		write_mem(addr, m);

		if (adm == adm6502.ADM_ABX)
			cycles = 7;

		set_flag(FLAG_C, c);
		set_flag(FLAG_N, neg8(m));
		set_flag(FLAG_Z, m == 0);
	}

	void ror_a(ushort addr)
	{
		bool c = (A & 0x01) > 0;

		A >>= 1;

		if (read_flag(FLAG_C))
			A |= 0x80;

		set_flag(FLAG_C, c);
		set_flag(FLAG_N, neg8(A));
		set_flag(FLAG_Z, A == 0);
	}

	void rti(ushort addr)
	{
		flags = (byte)(pop() & (~(FLAG_5 | FLAG_B)));

		byte l = pop();
		ushort h = pop();

		h <<= 8;
		h |= l;

		PC = h;

		cycles += 2;
	}

	void rts(ushort addr)
	{
		byte l = pop();
		ushort h = pop();

		h <<= 8;
		h |= l;

		PC = (ushort)(h + 1);

		cycles += 3;
	}

	void sbc(ushort addr)
	{
		invert = true;
		adc(addr);
	}

	void sec(ushort addr)
	{
		set_flag(FLAG_C, true);
		cycles += 1;
	}

	void sed(ushort addr)
	{
		set_flag(FLAG_D, true);
		cycles += 1;
	}

	void sei(ushort addr)
	{
		set_flag(FLAG_I, true);
		cycles += 1;
	}

	void sta(ushort addr)
	{
		write_mem(addr, A);

		if (adm == adm6502.ADM_ABX || adm == adm6502.ADM_ABY)
			cycles = 5;

		if (adm == adm6502.ADM_INX || adm == adm6502.ADM_INY)
			cycles = 6;
	}

	void stx(ushort addr)
	{
		write_mem(addr, X);
	}

	void sty(ushort addr)
	{
		write_mem(addr, Y);
	}

	void tax(ushort addr)
	{
		X = A;
		cycles += 1;

		set_flag(FLAG_N, neg8(X));
		set_flag(FLAG_Z, X == 0);
	}

	void tay(ushort addr)
	{
		Y = A;
		cycles += 1;

		set_flag(FLAG_N, neg8(Y));
		set_flag(FLAG_Z, Y == 0);
	}

	void tsx(ushort addr)
	{
		X = SP;
		cycles += 1;

		set_flag(FLAG_N, neg8(X));
		set_flag(FLAG_Z, X == 0);
	}

	void txa(ushort addr)
	{
		A = X;
		cycles += 1;

		set_flag(FLAG_N, neg8(A));
		set_flag(FLAG_Z, A == 0);
	}

	void txs(ushort addr)
	{
		SP = X;
		cycles += 1;
	}

	void tya(ushort addr)
	{
		A = Y;
		cycles += 1;

		set_flag(FLAG_N, neg8(A));
		set_flag(FLAG_Z, A == 0);
	}
}

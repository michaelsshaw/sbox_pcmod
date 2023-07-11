/* SPDX-License-Identifer: GPL-2.0-only */
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

	void adc(ushort addr)
	{
		byte m = read_mem(addr);

		if (invert) {
			invert = false;
			m = ~m;
		}

		byte c = 0;

		if (read_flag(FLAG_C)) 
			c = 1;

		ushort sum = A + m + c;


		set_flag(FLAG_C, sum > 0xFF);
		set_flag(FLAG_V, ((~(A ^ m) & (A ^ sum)) & 0x80) > 0);

		A = (byte)sum;

		set_flag(FLAG_Z, A == 0);
		set_flag(FLAG_N, (A & 0x80) > 0);
	}

	void and(ushort addr)
	{
		byte m = read_mem(addr);

		A &= m;

		set_flag(FLAG_Z, A == 0);
		set_flag(FLAG_N, (A & 0x80) > 0);
	}

	void asl(ushort addr)
	{
	}	

	void asl_a(ushort addr)
	{
	}

	void bcc(ushort addr)
	{
	}

	void bcs(ushort addr)
	{
	}

	void beq(ushort addr)
	{
	}

	void bit(ushort addr)
	{
	}

	void bmi(ushort addr)
	{
	}

	void bne(ushort addr)
	{
	}

	void bpl(ushort addr)
	{
	}

	void brk(ushort addr)
	{
	}

	void bvc(ushort addr)
	{
	}

	void bvs(ushort addr)
	{
	}

	void clc(ushort addr)
	{
	}

	void cld(ushort addr)
	{
	}

	void cli(ushort addr)
	{
	}

	void clv(ushort addr)
	{
	}

	void cmp(ushort addr)
	{
	}

	void cpx(ushort addr)
	{
	}

	void cpy(ushort addr)
	{
	}

	void dec(ushort addr)
	{
	}

	void dex(ushort addr)
	{
	}

	void dey(ushort addr)
	{
	}

	void eor(ushort addr)
	{
	}

	void inc(ushort addr)
	{
	}

	void inx(ushort addr)
	{
	}

	void iny(ushort addr)
	{
	}

	void jmp(ushort addr)
	{
	}

	void jsr(ushort addr)
	{
	}

	void lda(ushort addr)
	{
	}

	void ldx(ushort addr)
	{
	}

	void ldy(ushort addr)
	{
	}

	void lsr(ushort addr)
	{
	}

	void lsr_a(ushort addr)
	{
	}

	void nop(ushort addr)
	{
	}

	void ora(ushort addr)
	{
	}

	void pha(ushort addr)
	{
	}

	void php(ushort addr)
	{
	}

	void pla(ushort addr)
	{
	}

	void plp(ushort addr)
	{
	}

	void rol(ushort addr)
	{
	}

	void rol_a(ushort addr)
	{
	}

	void ror(ushort addr)
	{
	}

	void ror_a(ushort addr)
	{
	}

	void rti(ushort addr)
	{
	}

	void rts(ushort addr)
	{
	}

	void sbc(ushort addr)
	{
	}

	void sec(ushort addr)
	{
	}

	void sed(ushort addr)
	{
	}

	void sei(ushort addr)
	{
	}

	void sta(ushort addr)
	{
	}

	void stx(ushort addr)
	{
	}

	void sty(ushort addr)
	{
	}

	void tax(ushort addr)
	{
	}

	void tay(ushort addr)
	{
	}

	void tsx(ushort addr)
	{
	}

	void txa(ushort addr)
	{
	}

	void txs(ushort addr)
	{
	}

	void tya(ushort addr)
	{
	}
}

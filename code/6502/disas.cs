/* SPDX-License-Identifier: GPL-2.0-only */
/* Copyright 2023 Michael Shaw */

/* 6502 Disassembler */

using Sandbox;

namespace cpu_6502;

public partial class cpu
{
	public bool echo = false;

	string[] opcode_names = {
		"BRK",  "ORA",  "NULL", "NULL", "NULL", "ORA",  "ASL",  "NULL", "PHP",
		"ORA",  "ASL",  "NULL", "NULL", "ORA",  "ASL",  "NULL", "BPL",  "ORA",
		"NULL", "NULL", "NULL", "ORA",  "ASL",  "NULL", "CLC",  "ORA",  "NULL",
		"NULL", "NULL", "ORA",  "ASL",  "NULL", "JSR",  "AND",  "NULL", "NULL",
		"BIT",  "AND",  "ROL",  "NULL", "PLP",  "AND",  "ROL",  "NULL", "BIT",
		"AND",  "ROL",  "NULL", "BMI",  "AND",  "NULL", "NULL", "NULL", "AND",
		"ROL",  "NULL", "SEC",  "AND",  "NULL", "NULL", "NULL", "AND",  "ROL",
		"NULL", "RTI",  "EOR",  "NULL", "NULL", "NULL", "EOR",  "LSR",  "NULL",
		"PHA",  "EOR",  "LSR",  "NULL", "JMP",  "EOR",  "LSR",  "NULL", "BVC",
		"EOR",  "NULL", "NULL", "NULL", "EOR",  "LSR",  "NULL", "CLI",  "EOR",
		"NULL", "NULL", "NULL", "EOR",  "LSR",  "NULL", "RTS",  "ADC",  "NULL",
		"NULL", "NULL", "ADC",  "ROR",  "NULL", "PLA",  "ADC",  "ROR",  "NULL",
		"JMP",  "ADC",  "ROR",  "NULL", "BVS",  "ADC",  "NULL", "NULL", "NULL",
		"ADC",  "ROR",  "NULL", "SEI",  "ADC",  "NULL", "NULL", "NULL", "ADC",
		"ROR",  "NULL", "NULL", "STA",  "NULL", "NULL", "STY",  "STA",  "STX",
		"NULL", "DEY",  "NULL", "TXA",  "NULL", "STY",  "STA",  "STX",  "NULL",
		"BCC",  "STA",  "NULL", "NULL", "STY",  "STA",  "STX",  "NULL", "TYA",
		"STA",  "TXS",  "NULL", "NULL", "STA",  "NULL", "NULL", "LDY",  "LDA",
		"LDX",  "NULL", "LDY",  "LDA",  "LDX",  "NULL", "TAY",  "LDA",  "TAX",
		"NULL", "LDY",  "LDA",  "LDX",  "NULL", "BCS",  "LDA",  "NULL", "NULL",
		"LDY",  "LDA",  "LDX",  "NULL", "CLV",  "LDA",  "TSX",  "NULL", "LDY",
		"LDA",  "LDX",  "NULL", "CPY",  "CMP",  "NULL", "NULL", "CPY",  "CMP",
		"DEC",  "NULL", "INY",  "CMP",  "DEX",  "NULL", "CPY",  "CMP",  "DEC",
		"NULL", "BNE",  "CMP",  "NULL", "NULL", "NULL", "CMP",  "DEC",  "NULL",
		"CLD",  "CMP",  "NULL", "NULL", "NULL", "CMP",  "DEC",  "NULL", "CPX",
		"SBC",  "NULL", "NULL", "CPX",  "SBC",  "INC",  "NULL", "INX",  "SBC",
		"NOP",  "NULL", "CPX",  "SBC",  "INC",  "NULL", "BEQ",  "SBC",  "NULL",
		"NULL", "NULL", "SBC",  "INC",  "NULL", "SED",  "SBC",  "NULL", "NULL",
		"NULL", "SBC",  "INC",  "NULL"
	};

	void print_instruction(int opc)
	{
		if (!echo)
			return;

		string hh;
		string ll;

		byte l = 0;
		if (pbytes > 0) {
			l = read_mem((ushort)(PC + 1));
			cycles -= 1;
		}

		byte h = 0;
		if (pbytes == 2) {
			h = read_mem((ushort)(PC + 2));
			cycles -= 1;
		}

		ll = l.ToString("X2");
		hh = h.ToString("X2");

		string[] adm_names = {
			$"A",
			$"ABS ${hh}{ll}",
			$"ABX ${hh}{ll},X",
			$"ABY ${hh}{ll},Y",
			$"IMM #{ll}",
			$"IMPL",
			$"IND ${hh}{ll})",
			$"INX ${ll},X)",
			$"INY (${ll}),Y",
			$"REL ${ll}",
			$"ZPG ${ll}",
			$"ZPG ${ll},X",
			$"ZPG ${ll},Y",
			$"INVALID OPCODE"
		};

		string opc_hex = opc.ToString("X2");
		string a = A.ToString("X2");
		string x = X.ToString("X2");
		string y = Y.ToString("X2");
		string pc = PC.ToString("X4");

		Log.Info($"A: {a} X: {x} Y: {y} PC: {pc} | {opc_hex} {opcode_names[opc]} {adm_names[(int)(adm)]}");
	}
}

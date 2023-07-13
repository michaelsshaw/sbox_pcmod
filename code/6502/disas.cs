/* SPDX-License-Idenfiter: GPL-2.0-only */
/* Copyright 2023 Michael Shaw */

/* 6502 Disassembler */

using Sandbox;

namespace cpu_6502;

public partial class cpu
{
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
		Log.Info(opcode_names[opc]);
	}
}

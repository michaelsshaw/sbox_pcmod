; SPDX-License-Identifier: GPL-2.0-only

ap_mon = $C000

.code
_main:
	lda #$42
	ldx #$69
	ldy #$AB

_hlt:
	jmp _hlt

_nmi:
	rti
_irq:
	rti

.segment "VECTORS"
.org $FFFA
.addr _nmi
.addr _main
.addr _irq

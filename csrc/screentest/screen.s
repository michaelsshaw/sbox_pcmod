; SPDX-License-Identifier: GPL-2.0-only

ap_mon_dma = $C000
pwof_addr = $C001

dma_begin = $2900
dma_begin_lo = $00
dma_begin_hi = $29

dma_store_lo = $40
dma_store_hi = $41

.code
_main:
	; init registers
	lda #0
	ldx #0
	ldy #0
	
	; init begin dma address
	pha
	lda #dma_begin_lo
	sta dma_store_lo
	lda #dma_begin_hi
	sta dma_store_hi
	pla

	ldx #$FF

_begin_loop:
	; store 256 bytes into memory then increment the stored dma address
	inx
	sta (dma_store_lo),y
	iny
	beq _inc_dma

_inc_dma:
	inc dma_store_hi
	beq _done
	jmp _begin_loop

_done:
	; initiate dma and end program
	lda #00
	sta ap_mon_dma
	sta pwof_addr
	
	; should never happen
	jmp _hlt

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

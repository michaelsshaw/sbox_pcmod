# SPDX-License-Identifier: GPL-2.0-or-later
# Copyright 2024 Michael Shaw

CC := cc65
LD := ld65

CFLAGS := -MMD
LDFLAGS := -C ig8.cfg

SRC := $(shell find . -type f '*.c')
OBJ := $(SRC:.c=.o)

OUT := out.rom

$(OUT): $(OBJ)
	@$(LD) $(LDFLAGS) $< -o $@
	@echo "  LD     $@"

%.o: %.c
	@$(CC) $(CFLAGS) $< -o $@
	@echo "  CC     $@"

.PHONY: clean
clean:
	rm -rf $(OBJ) $(OUT)


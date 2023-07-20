# SPDX-License-Identifier: GPL-2.0-only

SRC := screen.s
OBJ := screen.o
OUT := screen.rom

%.o: %.s
	@ca65 -t none $< -o $@
	@echo "  CC     $@"

$(OUT): $(OBJ)
	@ld65 -C screen.cfg $^ -o $@
	@echo "  LD     $@"

.PHONY: all
all: $(OUT)

.PHONY: clean
clean:
	rm -rf $(OUT) $(OBJ)

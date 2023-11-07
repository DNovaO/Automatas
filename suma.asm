; Autor: Guillermo Fernandez Romero
; Fecha: 3-Mayo-2023
include 'emu8086.inc'
org 100h
;While: 0
InicioWhile0:
MOV AX, i
PUSH AX
MOV AX, 5
PUSH AX
POP BX
POP AX
CMP AX, BX
JAE FinWhile0
printn 'Hola'
INC i
JMP InicioWhile0
FinWhile0:
int 20h
RET
define_scan_num
define_print_num
define_print_num_uns
; V a r i a b l e s
altura dw 0h
i dw 0h
j dw 0h
k dw 0h
a dw 0h
END

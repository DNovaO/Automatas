; Autor: Guillermo Fernandez Romero
; Fecha: 3-Mayo-2023
include 'emu8086.inc'
org 100h
printn 'Hola, di un numero.'
CALL SCAN_NUM 
MOV AX, CX
printn 'El numero que pusiste es: '
CALL PRINT_NUM
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

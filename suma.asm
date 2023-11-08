; Autor: Guillermo Fernandez Romero
; Fecha: 3-Mayo-2023
include 'emu8086.inc'
org 100h
MOV AX, 258
PUSH AX

;Operador = 
POP AX
MOV a, AX
MOV AX, a
PUSH AX
POP AX
MOV BX, 256
DIV BX
PUSH DX
POP AX

;Operador = 
POP AX
MOV a, AX
MOV AX, 8
PUSH AX

;Operador += 
POP AX
ADD a, AX
MOV AX, 10
PUSH AX

;Operador *= 
POP AX
MUL a
MOV a, AX
MOV AX, 100
PUSH AX

;Operador /=
POP AX
MOV BX, a
MOV DX, 0
DIV BX
MOV a,AX
; Printf: 
print 'Valor Casteado de a: '
CALL PRINT_NUM
; if: 1
MOV AX, a
PUSH AX
MOV AX, 1
PUSH AX
POP BX
POP AX
CMP AX, BX
JNE Eif1
; Printf: 
print 'a es igual a 1'
Eif1:
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
b dw 0h
END

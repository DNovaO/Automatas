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
print 'Valor Casteado de a: '
MOV AX, a
CALL print_num
print ''
printn '' 
print 'Digite el valor de altura: '
CALL SCAN_NUM 
MOV altura, CX
print ''
printn '' 
print 'for:'
printn '' 
print ''

; For: 1
MOV AX, 1
PUSH AX

;Operador = 
POP AX
MOV i, AX
InicioFor1:
MOV AX, i
PUSH AX
MOV AX, altura
PUSH AX

;Condicion
POP BX
POP AX
CMP AX, BX
JG FinFor1
print '' 
print ' '

; For: 2
MOV AX, 250
PUSH AX

;Operador = 
POP AX
MOV j, AX
InicioFor2:
MOV AX, j
PUSH AX
MOV AX, 250
PUSH AX
MOV AX, i
PUSH AX
POP BX
POP AX
ADD AX, BX
PUSH AX

;Condicion
POP BX
POP AX
CMP AX, BX
JGE FinFor2

;If 
MOV AX, j
PUSH AX
MOV AX, 2
PUSH AX
POP BX
POP AX
DIV  BX
PUSH DX
XOR DX, DX
MOV AX, 0
PUSH AX

;Condicion
POP BX
POP AX
CMP AX, BX
JNE EtiquetaIf1
print '-'
JMP EtiquetaElse2
EtiquetaIf1:
;Else
print '+'
EtiquetaElse2:
INC j
JMP InicioFor2
FinFor2:
print ''
printn '' 
print ''
INC i
JMP InicioFor1
FinFor1:
print ''
printn '' 
print 'while:'
printn '' 
print ''
MOV AX, 1
PUSH AX

;Operador = 
POP AX
MOV i, AX

;While: 1
InicioWhile1:
MOV AX, i
PUSH AX
MOV AX, altura
PUSH AX

;Condicion
POP BX
POP AX
CMP AX, BX
JG FinWhile1
print '' 
print ' '
MOV AX, 250
PUSH AX

;Operador = 
POP AX
MOV j, AX

;While: 2
InicioWhile2:
MOV AX, j
PUSH AX
MOV AX, 250
PUSH AX
MOV AX, i
PUSH AX
POP BX
POP AX
ADD AX, BX
PUSH AX

;Condicion
POP BX
POP AX
CMP AX, BX
JGE FinWhile2

;If 
MOV AX, j
PUSH AX
MOV AX, 2
PUSH AX
POP BX
POP AX
DIV  BX
PUSH DX
XOR DX, DX
MOV AX, 0
PUSH AX

;Condicion
POP BX
POP AX
CMP AX, BX
JNE EtiquetaIf11
print '-'
JMP EtiquetaElse12
EtiquetaIf11:
;Else
print '+'
EtiquetaElse12:

;Operador ++
INC j
JMP InicioWhile2
FinWhile2:

;Operador ++
INC i
print ''
printn '' 
print ''
JMP InicioWhile1
FinWhile1:
print ''
printn '' 
print 'do:'
printn '' 
print ''
MOV AX, 1
PUSH AX

;Operador = 
POP AX
MOV i, AX

; Do While:1
InicioDo1:
print '' 
print ' '
MOV AX, 250
PUSH AX

;Operador = 
POP AX
MOV j, AX

; Do While:2
InicioDo2:

;If 
MOV AX, j
PUSH AX
MOV AX, 2
PUSH AX
POP BX
POP AX
DIV  BX
PUSH DX
XOR DX, DX
MOV AX, 0
PUSH AX

;Condicion
POP BX
POP AX
CMP AX, BX
JNE EtiquetaIf21
print '-'
JMP EtiquetaElse22
EtiquetaIf21:
;Else
print '+'
EtiquetaElse22:

;Operador ++
INC j
MOV AX, j
PUSH AX
MOV AX, 250
PUSH AX
MOV AX, i
PUSH AX
POP BX
POP AX
ADD AX, BX
PUSH AX

;Condicion
POP BX
POP AX
CMP AX, BX
JGE FinDo2
; Do While:
JMP InicioDo2
FinDo2:

;Operador ++
INC i
print ''
printn '' 
print ''
MOV AX, i
PUSH AX
MOV AX, altura
PUSH AX

;Condicion
POP BX
POP AX
CMP AX, BX
JG FinDo1
; Do While:
JMP InicioDo1
FinDo1:
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

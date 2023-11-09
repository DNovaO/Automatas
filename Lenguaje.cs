using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

/*
    ✔ Requerimiento 1: Programar scanf 
    ✔ Requerimiento 2: Programar printf
    ✔ Requerimiento 3: Programar ++,--,+=,-=,*=,/=,%=
    Requerimiento 4: Programar else
    ✔ Requerimiento 5: Programar do para que gerenre una sola vez el codigo
    ✔ Requerimiento 6: Programar while para que gerenre una sola vez el codigo
    ✔ Requerimiento 7: Programar el for para que gerenre una sola vez el codigo
    ✔ Requerimiento 8: Programar el CAST
*/

namespace Sintaxis_II
{
    public class Lenguaje : Sintaxis
    {
        List<Variable> lista;
        Stack<float> stack;
        int contIf, contEl, contFor, contWh, contDo;

        Variable.TiposDatos tipoDatoExpresion;
        public Lenguaje()
        {
            lista = new List<Variable>();
            stack = new Stack<float>();
            tipoDatoExpresion = Variable.TiposDatos.Char;
            contIf = contEl = contFor = contWh = contDo = 1;
        }
        public Lenguaje(string nombre) : base(nombre)
        {
            lista = new List<Variable>();
            stack = new Stack<float>();
            tipoDatoExpresion = Variable.TiposDatos.Char;
            contIf = contEl = contFor = contWh = contDo = 1;
        }

        //Programa  -> Librerias? Variables? Main
        public void Programa()
        {
            asm.WriteLine("include 'emu8086.inc'");
            asm.WriteLine("org 100h");
            if (getContenido() == "#")
            {
                Librerias();
            }
            if (getClasificacion() == Tipos.TipoDato)
            {
                Variables();
            }
            Main(true);
            asm.WriteLine("int 20h");
            asm.WriteLine("RET");
            asm.WriteLine("define_scan_num");
            asm.WriteLine("define_print_num");
            asm.WriteLine("define_print_num_uns");
            Imprime();
            asm.WriteLine("END");
        }

        private void Imprime()
        {
            log.WriteLine("-----------------");
            log.WriteLine("V a r i a b l e s");
            log.WriteLine("-----------------");
            asm.WriteLine("; V a r i a b l e s");
            foreach (Variable v in lista)
            {
                log.WriteLine(v.getNombre() + " " + v.getTipoDato() + " = " + v.getValor());
                asm.WriteLine(v.getNombre() + " dw 0h");
            }
            log.WriteLine("-----------------");
        }

        private bool Existe(string nombre)
        {
            foreach (Variable v in lista)
            {
                if (v.getNombre() == nombre)
                {
                    return true;
                }
            }
            return false;
        }
        private void Modifica(string nombre, float nuevoValor)
        {
            foreach (Variable v in lista)
            {
                if (v.getNombre() == nombre)
                {
                    v.setValor(nuevoValor);
                }
            }
        }
        private float getValor(string nombre)
        {
            foreach (Variable v in lista)
            {
                if (v.getNombre() == nombre)
                {
                    return v.getValor();
                }
            }
            return 0;
        }
        private Variable.TiposDatos getTipo(string nombre)
        {
            foreach (Variable v in lista)
            {
                if (v.getNombre() == nombre)
                {
                    return v.getTipoDato();
                }
            }
            return Variable.TiposDatos.Char;
        }
        private Variable.TiposDatos getTipo(float resultado)
        {
            if (resultado % 1 != 0)
            {
                return Variable.TiposDatos.Float;
            }
            else if (resultado < 256)
            {
                return Variable.TiposDatos.Char;
            }
            else if (resultado < 65536)
            {
                return Variable.TiposDatos.Int;
            }
            return Variable.TiposDatos.Float;
        }
        // Libreria -> #include<Identificador(.h)?>
        private void Libreria()
        {
            match("#");
            match("include");
            match("<");
            match(Tipos.Identificador);
            if (getContenido() == ".")
            {
                match(".");
                match("h");
            }
            match(">");
        }
        //Librerias -> Libreria Librerias?
        private void Librerias()
        {
            Libreria();
            if (getContenido() == "#")
            {
                Librerias();
            }
        }
        //Variables -> tipo_dato ListaIdentificadores; Variables?
        private void Variables()
        {
            Variable.TiposDatos tipo = Variable.TiposDatos.Char;
            switch (getContenido())
            {
                case "int": tipo = Variable.TiposDatos.Int; break;
                case "float": tipo = Variable.TiposDatos.Float; break;
            }
            match(Tipos.TipoDato);
            ListaIdentificadores(tipo);
            match(";");
            if (getClasificacion() == Tipos.TipoDato)
            {
                Variables();
            }
        }
        //ListaIdentificadores -> identificador (,ListaIdentificadores)?
        private void ListaIdentificadores(Variable.TiposDatos tipo)
        {
            if (!Existe(getContenido()))
            {
                lista.Add(new Variable(getContenido(), tipo));
            }
            else
            {
                throw new Error("de sintaxis, la variable <" + getContenido() + "> está duplicada", log, linea, columna);
            }
            match(Tipos.Identificador);
            if (getContenido() == ",")
            {
                match(",");
                ListaIdentificadores(tipo);
            }
        }
        //BloqueInstrucciones -> { ListaInstrucciones ? }
        private void BloqueInstrucciones(bool ejecuta, bool primeraVez)
        {
            match("{");
            if (getContenido() != "}")
            {
                ListaInstrucciones(ejecuta, primeraVez);
            }
            match("}");
        }

        //ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones(bool ejecuta, bool primeraVez)
        {
            Instruccion(ejecuta, primeraVez);
            if (getContenido() != "}")
            {
                ListaInstrucciones(ejecuta, primeraVez);
            }
        }
        //Instruccion -> Printf | Scanf | If | While | Do | For | Asignacion
        private void Instruccion(bool ejecuta, bool primeraVez)
        {
            if (getContenido() == "printf")
            {
                Printf(ejecuta, primeraVez);
            }
            else if (getContenido() == "scanf")
            {
                Scanf(ejecuta, primeraVez);
            }
            else if (getContenido() == "if")
            {
                If(ejecuta, primeraVez);
            }
            else if (getContenido() == "while")
            {
                While(ejecuta, primeraVez);
            }
            else if (getContenido() == "do")
            {
                Do(ejecuta, primeraVez);
            }
            else if (getContenido() == "for")
            {
                For(ejecuta, primeraVez);
            }
            else
            {
                Asignacion(ejecuta, primeraVez);
            }
        }
        //Asignacion -> identificador = Expresion;
        private void Asignacion(bool ejecuta, bool primeraVez)
        {
            // Inicializamos una variable para almacenar el resultado de la asignación.
            float resultado = 0;
            tipoDatoExpresion = Variable.TiposDatos.Char;

            // Verificamos si la variable existe en el contexto actual.
            if (!Existe(getContenido()))
            {
                throw new Error("OJO 2 de sintaxis, la variable <" + getContenido() + "> no está declarada", log, linea, columna);
            }

            // Escribimos el nombre de la variable en el registro de log.
            log.Write(getContenido() + " = ");
            string variable = getContenido();
            match(Tipos.Identificador);

            // Entramos en un bucle para procesar operaciones de asignación como '=', '++', '--', etc.
            while (getContenido() == "=" || getContenido() == "++" || getContenido() == "--" ||
                   getContenido() == "+=" || getContenido() == "-=" || getContenido() == "*=" ||
                   getContenido() == "/=" || getContenido() == "%=")
            {
                // Obtenemos el operador actual.
                string operador = getContenido();

                // Realizamos una coincidencia con el operador actual.
                match(operador);

                // Realizamos la asignación o la operación correspondiente según el operador.
                if (operador == "=")
                {
                    // Caso: '='
                    Expresion(primeraVez);
                    resultado = stack.Pop(); // Obtenemos el valor de la expresión y lo asignamos.

                    if (primeraVez)
                    {
                        asm.WriteLine();
                        asm.WriteLine(";Operador = ");
                        asm.WriteLine("POP AX");
                        asm.WriteLine("MOV " + variable + ", AX");
                    }
                }
                else if (operador == "++")
                {
                    // Caso: '++'
                    resultado = getValor(variable) + 1; // Incrementamos el valor de la variable.
                    if (primeraVez)
                    {
                        asm.WriteLine();
                        asm.WriteLine(";Operador ++");
                        asm.WriteLine("INC " + variable);
                    }
                }
                else if (operador == "--")
                {
                    // Caso: '--'
                    resultado = getValor(variable) - 1; // Decrementamos el valor de la variable.

                    if (primeraVez)
                    {
                        asm.WriteLine();
                        asm.WriteLine(";Operador -- ");
                        asm.WriteLine("DEC " + variable);
                    }
                }
                else if (operador == "+=")
                {
                    // Caso: '+='
                    Expresion(primeraVez);
                    resultado = stack.Pop();
                    resultado += getValor(variable); // Sumamos el valor de la expresión a la variable.
                    if (primeraVez)
                    {
                        asm.WriteLine();
                        asm.WriteLine(";Operador += ");
                        asm.WriteLine("POP AX");
                        asm.WriteLine("ADD " + variable + ", AX");
                    }
                }
                else if (operador == "-=")
                {
                    // Caso: '-='
                    Expresion(primeraVez);
                    resultado = stack.Pop();
                    resultado -= getValor(variable); // Restamos el valor de la expresión a la variable.
                    if (primeraVez)
                    {
                        asm.WriteLine();
                        asm.WriteLine(";Operador -= ");
                        asm.WriteLine("POP AX");
                        asm.WriteLine("SUB " + variable + ", AX");
                    }
                }
                else if (operador == "*=")
                {
                    // Caso: '*='
                    Expresion(primeraVez);
                    resultado = stack.Pop();
                    resultado *= getValor(variable); // Multiplicamos el valor de la expresión por la variable.
                    if (primeraVez)
                    {
                        asm.WriteLine();
                        asm.WriteLine(";Operador *= ");
                        asm.WriteLine("POP AX");
                        asm.WriteLine("MUL " + variable);
                        asm.WriteLine("MOV " + variable + ", AX");
                    }
                }
                else if (operador == "/=")
                {
                    // Caso: '/='
                    Expresion(primeraVez);
                    resultado = stack.Pop();
                    resultado /= getValor(variable); // Dividimos el valor de la expresión entre la variable.
                    if (primeraVez)
                    {
                        asm.WriteLine();
                        asm.WriteLine(";Operador /=");
                        asm.WriteLine("POP AX");
                        asm.WriteLine("MOV BX, " + variable);
                        asm.WriteLine("MOV DX, 0");
                        asm.WriteLine("DIV BX");
                        asm.WriteLine("MOV " + variable + ",AX");

                    }
                }
                else if (operador == "%=")
                {
                    // Caso: '%='
                    Expresion(primeraVez);
                    resultado = stack.Pop();
                    resultado %= getValor(variable); // Calculamos el módulo entre la variable y la expresión.
                    if (primeraVez)
                    {
                        asm.WriteLine();
                        asm.WriteLine(";Operador %= ");
                        asm.WriteLine("POP BX");
                        asm.WriteLine("MOV AX, " + variable);
                        asm.WriteLine("DIV BX");
                        asm.WriteLine("MOV " + variable + ",DX");
                    }
                }

                if (ejecuta)
                {

                    Variable.TiposDatos tipoDatoVariable = getTipo(variable);
                    Variable.TiposDatos tipoDatoResultado = getTipo(resultado);

                    // Console.WriteLine(variable + " = "+tipoDatoVariable);
                    // Console.WriteLine(resultado + " = "+tipoDatoResultado);
                    // Console.WriteLine("expresion = "+tipoDatoExpresion);

                    if (tipoDatoExpresion > tipoDatoResultado)
                    {
                        tipoDatoResultado = tipoDatoExpresion;
                    }
                    if (tipoDatoVariable >= tipoDatoResultado)
                    {
                        Modifica(variable, resultado);
                    }
                    else
                    {
                        throw new Error("de semantica, no se puede asignar in <" + tipoDatoResultado + "> a un <" + tipoDatoVariable + ">", log, linea, columna);
                    }
                }
                primeraVez = false;
                log.Write(" = " + resultado);
                // Si encontramos un punto y coma, salimos del bucle.
                if (getContenido() == ";")
                {
                    break;
                }
            }

            // Realizamos una coincidencia con el punto y coma al final de la asignación.
            match(";");
        }

        //While -> while(Condicion) BloqueInstrucciones | Instruccion        
        private void While(bool ejecuta, bool primeraVez)
        {
            if (primeraVez) asm.WriteLine();
            if (primeraVez) asm.WriteLine(";While: " + contWh);

            string etiquetaInicioWhile = "InicioWhile" + contWh;
            string etiquetaFinWhile = "FinWhile" + contWh++;
            int inicia = caracter;
            int lineaInicio = linea;
            string variable = getContenido();

            log.WriteLine("While:" + variable);

            if (primeraVez) asm.WriteLine(etiquetaInicioWhile + ":");

            do
            {
                match("while");
                match("(");
                ejecuta = Condicion(etiquetaFinWhile, primeraVez) && ejecuta;
                match(")");
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(ejecuta, primeraVez);
                }
                else
                {
                    Instruccion(ejecuta, primeraVez);
                }

                if (ejecuta)
                {
                    archivo.DiscardBufferedData();
                    caracter = inicia - 6;
                    archivo.BaseStream.Seek(caracter, SeekOrigin.Begin);
                    nextToken();
                    linea = lineaInicio;
                    caracter = inicia;
                }
                if (primeraVez)
                {
                    asm.WriteLine("JMP " + etiquetaInicioWhile);
                    asm.WriteLine(etiquetaFinWhile + ":");
                }
                primeraVez = false;
            } while (ejecuta);
        }

        //Do -> do BloqueInstrucciones | Instruccion while(Condicion)
        private void Do(bool ejecuta, bool primeraVez)
        {
            if (primeraVez) asm.WriteLine();
            if (primeraVez) asm.WriteLine("; Do While:" + contDo);
            match("do");

            string etiquetaInicioDo = "InicioDo" + contDo;
            string etiquetaFinDo = "FinDo" + contDo++;

            int inicia = caracter;
            int lineaInicio = linea;

            log.WriteLine("Do While: ");
            if (primeraVez) asm.WriteLine(etiquetaInicioDo + ":");

            do
            {
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(ejecuta, primeraVez);
                }
                else
                {
                    Instruccion(ejecuta, primeraVez);
                }

                match("while");
                match("(");
                ejecuta = Condicion(etiquetaFinDo, primeraVez) && ejecuta;
                match(")");
                match(";");

                if (ejecuta)
                {
                    archivo.DiscardBufferedData();
                    caracter = inicia - 2;
                    archivo.BaseStream.Seek(caracter, SeekOrigin.Begin);
                    nextToken();
                    linea = lineaInicio;


                }
                if (primeraVez)
                {
                    asm.WriteLine("; Do While:");
                    asm.WriteLine("JMP " + etiquetaInicioDo);
                    asm.WriteLine(etiquetaFinDo + ":");
                }
                primeraVez = false;
            } while (ejecuta);

        }
        //For -> for(Asignacion Condicion; Incremento) BloqueInstrucciones | Instruccion
        private void For(bool ejecuta, bool primeraVez)
        {
            if (primeraVez) asm.WriteLine();
            if (primeraVez) asm.WriteLine("; For: " + contFor);
            match("for");
            match("(");
            Asignacion(ejecuta, primeraVez);

            string etiquetaInicioFor = "InicioFor" + contFor;
            string etiquetaFinFor = "FinFor" + contFor++;

            int inicia = caracter;
            int lineaInicio = linea;
            float resultado = 0;
            string variable = getContenido();

            log.WriteLine("for: " + variable);
            if (primeraVez) asm.WriteLine(etiquetaInicioFor + ":");

            bool incrementar = false;
            bool decrementar = false;
            do
            {
                ejecuta = Condicion(etiquetaFinFor, primeraVez) && ejecuta;
                match(";");
                resultado = Incremento(ejecuta);
                match(")");

                incrementar = getValor(variable) < resultado;
                decrementar = getValor(variable) > resultado;

                if (getContenido() == "{")
                {
                    BloqueInstrucciones(ejecuta, primeraVez);
                }
                else
                {
                    Instruccion(ejecuta, primeraVez);
                }

                if (primeraVez)
                {
                    if (incrementar)
                    {
                        asm.WriteLine("INC " + variable);
                    }
                    else if (decrementar)
                    {
                        asm.WriteLine("DEC " + variable);
                    }
                }
                if (ejecuta)
                {
                    Variable.TiposDatos tipoDatoVariable = getTipo(variable);
                    Variable.TiposDatos tipoDatoResultado = getTipo(resultado);
                    if (tipoDatoVariable >= tipoDatoResultado)
                    {
                        Modifica(variable, resultado);
                    }
                    else
                    {
                        throw new Error("de semántica, no se puede asignar in <" + tipoDatoResultado + "> a un <" + tipoDatoVariable + ">", log, linea, columna);
                    }
                    archivo.DiscardBufferedData();
                    caracter = inicia - variable.Length - 1;
                    archivo.BaseStream.Seek(caracter, SeekOrigin.Begin);
                    nextToken();
                    linea = lineaInicio;
                }

                if (primeraVez)
                {
                    asm.WriteLine("JMP " + etiquetaInicioFor);
                    asm.WriteLine(etiquetaFinFor + ":");
                }
                primeraVez = false;
            } while (ejecuta);
        }

        //Incremento -> Identificador ++ | --
        private float Incremento(bool ejecuta)
        {
            if (!Existe(getContenido()))
            {
                throw new Error("de sintaxis, la variable <" + getContenido() + "> no está declarada", log, linea, columna);
            }
            string variable = getContenido();
            match(Tipos.Identificador);
            if (getContenido() == "++")
            {
                match("++");
                return getValor(variable) + 1;
            }
            else
            {
                match("--");
                return getValor(variable) - 1;
            }
        }
        //Condicion -> Expresion OperadorRelacional Expresion
        private bool Condicion(string etiqueta, bool primeraVez)
        {
            Expresion(primeraVez);
            string operador = getContenido();
            match(Tipos.OperadorRelacional);
            Expresion(primeraVez);
            float R1 = stack.Pop();  // Expresion 2
            float R2 = stack.Pop();  // Expresion 1


            if (primeraVez)
            {
                asm.WriteLine();
                asm.WriteLine(";Condicion");
                asm.WriteLine("POP BX"); // Expresion 2
                asm.WriteLine("POP AX"); // Expresion 1
                asm.WriteLine("CMP AX, BX");
            }

            switch (operador)
            {
                case "==":
                    if (primeraVez) asm.WriteLine("JNE " + etiqueta);
                    return R2 == R1;
                case ">":
                    if (primeraVez) asm.WriteLine("JLE " + etiqueta);
                    return R2 > R1;
                case ">=":
                    if (primeraVez) asm.WriteLine("JL " + etiqueta);
                    return R2 >= R1;
                case "<":
                    if (primeraVez) asm.WriteLine("JGE " + etiqueta);
                    return R2 < R1;
                case "<=":
                    if (primeraVez) asm.WriteLine("JG " + etiqueta);
                    return R2 <= R1;
                default:
                    if (primeraVez) asm.WriteLine("JE " + etiqueta);
                    return R2 != R1;
            }
        }
        //If -> if (Condicion) BloqueInstrucciones | Instruccion (else BloqueInstrucciones | Instruccion)?
        private void If(bool ejecuta, bool primeraVez)
        {
            if (primeraVez) asm.WriteLine();
            if (primeraVez) asm.WriteLine(";If ");

            match("if");
            match("(");

            string etiquetaIf = "EtiquetaIf" + contIf++;
            string etiquetaElse = "EtiquetaElse" + contIf;
            bool evaluacion = Condicion(etiquetaIf, primeraVez);
            match(")");

            if (getContenido() == "{")
            {
                BloqueInstrucciones(evaluacion && ejecuta, primeraVez);
            }
            else
            {
                Instruccion(evaluacion && ejecuta, primeraVez);
            }

            if (primeraVez)
            {
                asm.WriteLine("JMP " + etiquetaElse);
                asm.WriteLine(etiquetaIf + ":");
            }

            if (getContenido() == "else")
            {
                match("else");

                if (primeraVez)
                {
                    asm.WriteLine(";Else");
                }

                if (getContenido() == "{")
                {
                    BloqueInstrucciones(!evaluacion && ejecuta, primeraVez);
                }
                else
                {
                    Instruccion(!evaluacion && ejecuta, primeraVez);
                }
                if (primeraVez) asm.WriteLine(etiquetaElse + ":");
            }
            primeraVez = false;
        }
        //Printf -> printf(cadena(,Identificador)?);

        private void Printf(bool ejecuta, bool primeraVez)
        {
            match("printf");
            match("(");

            string cadena = getContenido().TrimStart('"');
            string cadenaEnsamblador = cadena;

            cadena = cadena.Remove(cadena.Length - 1);
            cadena = cadena.Replace(@"\n", "\n");
            cadena = cadena.Replace(@"\t", "\t");

            cadenaEnsamblador = cadenaEnsamblador.Replace(@"\n", "'\nPRINTN '' \nPRINT '");
            cadenaEnsamblador = cadenaEnsamblador.Replace(@"\t", "' \nPRINT ' ");

            if (ejecuta)
            {
                Console.Write(cadena);
                if (primeraVez) asm.WriteLine("PRINT '" + cadenaEnsamblador + "'");
            }
            else if (primeraVez)
            {
                asm.WriteLine("PRINT '" + cadenaEnsamblador + "'");
            }

            match(Tipos.Cadena);

            if (getContenido() == ",")
            {
                match(",");
                string variable = getContenido();

                if (!Existe(variable))
                {
                    throw new Error("de sintaxis, la variable <" + variable + "> no está declarada", log, linea, columna);
                }

                if (ejecuta)
                {
                    string valor = getContenido();
                    Console.Write(valor);

                    if (primeraVez)
                    {
                        asm.WriteLine("MOV AX, " + variable);
                        asm.WriteLine("CALL PRINT_NUM");
                    }
                }
                else if (primeraVez)
                {
                    asm.WriteLine("MOV AX, " + variable);
                    asm.WriteLine("CALL PRINT_NUM");
                }

                match(Tipos.Identificador);
            }

            match(")");
            match(";");
        }


        //Scanf -> scanf(cadena,&Identificador);
        private void Scanf(bool ejecuta, bool primeraVez)
        {
            match("scanf");
            match("(");
            match(Tipos.Cadena);
            match(",");
            match("&");

            if (!Existe(getContenido()))
            {
                throw new Error("de sintaxis, la variable <" + getContenido() + "> no está declarada", log, linea, columna);
            }

            string variable = getContenido();
            match(Tipos.Identificador);

            if (ejecuta || !ejecuta)
            {
                string captura = "" + Console.ReadLine();
                float resultado = float.Parse(captura);
                Modifica(variable, resultado);

                if (primeraVez)
                {
                    asm.WriteLine("CALL SCAN_NUM ");
                    asm.WriteLine("MOV " + variable + ", CX");

                }

            }

            match(")");
            match(";");
        }
        //Main -> void main() BloqueInstrucciones
        private void Main(bool ejecuta)
        {
            match("void");
            match("main");
            match("(");
            match(")");
            BloqueInstrucciones(ejecuta, true);
        }
        //Expresion -> Termino MasTermino
        private void Expresion(bool primeraVez)
        {
            Termino(primeraVez);
            MasTermino(primeraVez);
        }
        //MasTermino -> (OperadorTermino Termino)?
        private void MasTermino(bool primeraVez)
        {
            if (getClasificacion() == Tipos.OperadorTermino)
            {
                string operador = getContenido();
                match(Tipos.OperadorTermino);
                Termino(primeraVez);
                log.Write(" " + operador);
                float R2 = stack.Pop();
                float R1 = stack.Pop();
                if (primeraVez)
                {
                    asm.WriteLine("POP BX");
                    asm.WriteLine("POP AX");
                }
                if (operador == "+")
                {
                    stack.Push(R1 + R2);
                    if (primeraVez)
                    {
                        asm.WriteLine("ADD AX, BX");
                        asm.WriteLine("PUSH AX");
                    }
                }
                else
                {
                    stack.Push(R1 - R2);
                    if (primeraVez)
                    {
                        asm.WriteLine("SUB AX, BX");
                        asm.WriteLine("PUSH AX");
                    }
                }
            }
        }
        //Termino -> Factor PorFactor
        private void Termino(bool primeraVez)
        {
            Factor(primeraVez);
            PorFactor(primeraVez);
        }
        //PorFactor -> (OperadorFactor Factor)?
        private void PorFactor(bool primeraVez)
        {
            if (getClasificacion() == Tipos.OperadorFactor)
            {
                string operador = getContenido();
                match(Tipos.OperadorFactor);
                Factor(primeraVez);
                log.Write(" " + operador);
                float R2 = stack.Pop();
                float R1 = stack.Pop();
                if (primeraVez)
                {
                    asm.WriteLine("POP BX");
                    asm.WriteLine("POP AX");
                }
                if (operador == "*")
                {
                    stack.Push(R1 * R2);
                    if (primeraVez)
                    {
                        asm.WriteLine("MUL  BX");
                        asm.WriteLine("PUSH AX");
                        asm.WriteLine("XOR AX, AX");
                    }
                }
                else if (operador == "/")
                {
                    stack.Push(R1 / R2);
                    if (primeraVez)
                    {
                        asm.WriteLine("DIV  BX");
                        asm.WriteLine("PUSH AX");
                        asm.WriteLine("XOR AX, AX");
                    }
                }
                else
                {
                    stack.Push(R1 % R2);
                    if (primeraVez)
                    {
                        asm.WriteLine("DIV  BX");
                        asm.WriteLine("PUSH DX");
                        asm.WriteLine("XOR DX, DX");
                    }
                }
            }
        }
        //Factor -> numero | identificador | (Expresion)
        private void Factor(bool primeraVez)
        {
            if (getClasificacion() == Tipos.Numero)
            {
                log.Write(" " + getContenido());
                if (primeraVez)
                {
                    asm.WriteLine("MOV AX, " + getContenido());
                    asm.WriteLine("PUSH AX");
                }
                stack.Push(float.Parse(getContenido()));
                if (tipoDatoExpresion < getTipo(float.Parse(getContenido())))
                {
                    tipoDatoExpresion = getTipo(float.Parse(getContenido()));
                }
                match(Tipos.Numero);
            }
            else if (getClasificacion() == Tipos.Identificador)
            {
                if (!Existe(getContenido()))
                {
                    throw new Error("de sintaxis, la variable <" + getContenido() + "> no está declarada", log, linea, columna);
                }
                if (primeraVez)
                {
                    asm.WriteLine("MOV AX, " + getContenido());
                    asm.WriteLine("PUSH AX");
                }
                stack.Push(getValor(getContenido()));
                match(Tipos.Identificador);
                if (tipoDatoExpresion < getTipo(getContenido()))
                {
                    tipoDatoExpresion = getTipo(getContenido());
                }
            }
            else
            {
                bool huboCast = false;
                Variable.TiposDatos tipoDatoCast = Variable.TiposDatos.Char;
                match("(");
                if (getClasificacion() == Tipos.TipoDato)
                {
                    huboCast = true;
                    switch (getContenido())
                    {
                        case "int": tipoDatoCast = Variable.TiposDatos.Int; break;
                        case "float": tipoDatoCast = Variable.TiposDatos.Float; break;
                    }
                    match(Tipos.TipoDato);
                    match(")");
                    match("(");
                }
                Expresion(primeraVez);
                match(")");
                if (huboCast)
                {
                    tipoDatoExpresion = tipoDatoCast;
                    stack.Push(castea(stack.Pop(), tipoDatoCast, primeraVez));
                }
            }
        }
        float castea(float resultado, Variable.TiposDatos tipoDato, bool primeraVez)
        {
            switch (tipoDato)
            {
                case Variable.TiposDatos.Char:
                    if (primeraVez)
                    {
                        asm.WriteLine("POP AX");
                        asm.WriteLine("MOV BX, 256");
                        asm.WriteLine("DIV BX");
                        asm.WriteLine("PUSH DX");
                    }
                    return MathF.Round(resultado) % 256;

                case Variable.TiposDatos.Int:
                    if (primeraVez)
                    {
                        asm.WriteLine("POP AX");
                        asm.WriteLine("MOV BX, 65536");
                        asm.WriteLine("DIV BX");
                        asm.WriteLine("PUSH DX");
                    }
                    return MathF.Round(resultado) % 65536;
            }
            return resultado;
        }
    }
}
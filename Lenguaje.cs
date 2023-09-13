using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/* 
   ✓ Requerimiento 1: Mensajes del printf deben salir sin comillas
                     Incluir \n y \t como secuencias de escape
   ✓ Requerimiento 2: Agregar el % al PorFactor
                     Modifcar el valor de una variable con ++,--,+=,-=,*=,/=.%=
    Requerimiento 3: Cada vez que se haga un match(Tipos.Identificador) verficar el
                     uso de la variable. Icremento(), Printf(), Factor()
                     Levantar una excepcion en scanf() cuando se capture un string
   ✓ Requerimiento 4: Implementar la ejecucion del ELSE. 
*/

namespace Sintaxis_II
{
    public class Lenguaje : Sintaxis
    {
        List<Variable> lista;
        Stack<float> stack;
        public Lenguaje()
        {
            lista = new List<Variable>();
            stack = new Stack<float>();
        }
        public Lenguaje(string nombre) : base(nombre)
        {
            lista = new List<Variable>();
            stack = new Stack<float>();
        }

        //Programa  -> Librerias? Variables? Main
        public void Programa()
        {
            if (getContenido() == "#")
            {
                Librerias();
            }
            if (getClasificacion() == Tipos.TipoDato)
            {
                Variables();
            }
            Main(true);
            Imprime();
        }

        private void Imprime()
        {
            log.WriteLine("-----------------");
            log.WriteLine("V a r i a b l e s");
            log.WriteLine("-----------------");
            foreach (Variable v in lista)
            {
                log.WriteLine(v.getNombre() + " " + v.getTiposDatos() + " = " + v.getValor());
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

        //get valor del scanf
        private float GetValor(string nombre)
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
                throw new Error(" OJO 1 de sintaxis, la variable <" + getContenido() + "> está duplicada", log, linea, columna);
            }
            match(Tipos.Identificador);
            if (getContenido() == ",")
            {
                match(",");
                ListaIdentificadores(tipo);
            }
        }
        //BloqueInstrucciones -> { ListaInstrucciones ? }
        private void BloqueInstrucciones(bool ejecuta)
        {
            match("{");
            if (getContenido() != "}")
            {
                ListaInstrucciones(ejecuta);
            }
            match("}");
        }

        //ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones(bool ejecuta)
        {
            Instruccion(ejecuta);
            if (getContenido() != "}")
            {
                ListaInstrucciones(ejecuta);
            }
        }
        //Instruccion -> Printf | Scanf | If | While | Do | For | Asignacion
        private void Instruccion(bool ejecuta)
        {
            if (getContenido() == "printf")
            {
                Printf(ejecuta);
            }
            else if (getContenido() == "scanf")
            {
                Scanf(ejecuta);
            }
            else if (getContenido() == "while")
            {
                While(ejecuta);
            }
            else if (getContenido() == "if")
            {
                If(ejecuta);
            }
            else if (getContenido() == "do")
            {
                Do(ejecuta);
            }
            else if (getContenido() == "for")
            {
                For(ejecuta);
            }
            // ...
            else
            {
                Asignacion(ejecuta);
            }
        }
        //Asignacion -> identificador = Expresion;
        private void Asignacion(bool ejecuta)
        {
            // Inicializamos una variable para almacenar el resultado de la asignación.
            float resultado = 0;

            // Obtenemos el nombre de la variable a la que se realizará la asignación.
            string variable = getContenido();

            // Verificamos si la variable existe en el contexto actual.
            if (!Existe(variable))
            {
                throw new Error("OJO 2 de sintaxis, la variable <" + variable + "> no está declarada", log, linea, columna);
            }

            // Escribimos el nombre de la variable en el registro de log.
            log.Write(variable + " = ");

            // Realizamos una coincidencia con el tipo de token 'Identificador'.
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
                    Expresion();
                    resultado = stack.Pop(); // Obtenemos el valor de la expresión y lo asignamos.
                }
                else if (operador == "++")
                {
                    // Caso: '++'
                    resultado = GetValor(variable) + 1; // Incrementamos el valor de la variable.
                }
                else if (operador == "--")
                {
                    // Caso: '--'
                    resultado = GetValor(variable) - 1; // Decrementamos el valor de la variable.
                }
                else if (operador == "+=")
                {
                    // Caso: '+='
                    Expresion();
                    resultado = stack.Pop();
                    resultado += GetValor(variable); // Sumamos el valor de la expresión a la variable.
                }
                else if (operador == "-=")
                {
                    // Caso: '-='
                    Expresion();
                    resultado = stack.Pop();
                    resultado -= GetValor(variable); // Restamos el valor de la expresión a la variable.
                }
                else if (operador == "*=")
                {
                    // Caso: '*='
                    Expresion();
                    resultado = stack.Pop();
                    resultado *= GetValor(variable); // Multiplicamos el valor de la expresión por la variable.
                }
                else if (operador == "/=")
                {
                    // Caso: '/='
                    Expresion();
                    resultado = stack.Pop();
                    resultado /= GetValor(variable); // Dividimos el valor de la variable por el de la expresión.
                }
                else if (operador == "%=")
                {
                    // Caso: '%='
                    Expresion();
                    resultado = stack.Pop();
                    resultado %= GetValor(variable); // Calculamos el módulo entre la variable y la expresión.
                }

                // Escribimos el operador y el resultado en el registro de log.
                log.Write(" " + operador);
                log.Write(" = " + resultado);

                // Si estamos ejecutando el código, aplicamos la asignación o la operación a la variable.
                if (ejecuta)
                {
                    Modifica(variable, resultado);
                }

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
        private void While(bool ejecuta)
        {
            match("while");
            match("(");
            Condicion();
            match(")");
            if (getContenido() == "{")
            {
                BloqueInstrucciones(ejecuta);
            }
            else
            {
                Instruccion(ejecuta);
            }

        }
        //Do -> do BloqueInstrucciones | Instruccion while(Condicion)
        private void Do(bool ejecuta)
        {
            match("do");
            if (getContenido() == "{")
            {
                BloqueInstrucciones(ejecuta);
            }
            else
            {
                Instruccion(ejecuta);
            }
            match("while");
            match("(");
            Condicion();
            match(")");
            match(";");
        }
        //For -> for(Asignacion Condicion; Incremento) BloqueInstrucciones | Instruccion
        private void For(bool ejecuta)
        {
            match("for");
            match("(");
            Asignacion(ejecuta);
            Condicion();
            match(";");
            Incremento(ejecuta);
            match(")");
            if (getContenido() == "{")
            {
                BloqueInstrucciones(ejecuta);
            }
            else
            {
                Instruccion(ejecuta);
            }
        }
        //Incremento -> Identificador ++ | --
        private void Incremento(bool ejecuta)
        {
            // Verificamos si la variable existe en el contexto actual.
            if (!Existe(getContenido()))
            {
                throw new Error("OJO 3 de sintaxis, la variable <" + getContenido() + "> no está declarada", log, linea, columna);
            }

            // Realizamos una coincidencia con el tipo de token 'Identificador'.
            match(Tipos.Identificador);

            // Obtenemos el nombre de la variable.
            string variable = getContenido();

            // Intentamos convertir el valor de la variable a un número flotante.
            if (!float.TryParse(GetValor(variable).ToString(), out float valorVariable))
            {
                // Si la conversión falla, lanzamos un error.
                throw new Error("OJO 4 de sintaxis, la variable <" + variable + "> no es de tipo numérico", log, linea, columna);
            }

            // Apilamos el valor de la variable en la pila de cálculo.
            stack.Push(valorVariable);

            // Comprobamos si el operador actual es '++' (incremento) o '--' (decremento).
            if (getContenido() == "++")
            {
                // Si es '++', realizamos una coincidencia con '++'.
                match("++");
            }
            else
            {
                // Si es '--', realizamos una coincidencia con '--'.
                match("--");
            }
        }

        //Condicion -> Expresion OperadorRelacional Expresion
        private bool Condicion()
        {
            Expresion();
            string operador = getContenido();
            match(Tipos.OperadorRelacional);
            Expresion();
            float R1 = stack.Pop();
            float R2 = stack.Pop();

            switch (operador)
            {
                case "==": return R2 == R1;
                case ">": return R2 > R1;
                case ">=": return R2 >= R1;
                case "<": return R2 < R1;
                case "<=": return R2 <= R1;
                default: return R2 != R1;
            }
        }
        //If -> if (Condicion) BloqueInstrucciones | Instruccion (else BloqueInstrucciones | Instruccion)?
        private void If(bool ejecuta)
        {
            match("if");
            match("(");
            bool evaluacion = Condicion() && ejecuta;
            Console.WriteLine(evaluacion);
            match(")");
            bool ejecutarBloqueIf = evaluacion; // Bandera para el bloque 'if'
            if (getContenido() == "{")
            {
                BloqueInstrucciones(ejecutarBloqueIf);
            }
            else
            {
                Instruccion(ejecutarBloqueIf);
            }

            bool ejecutarBloqueElse = !evaluacion; // Bandera para el bloque 'else'
            if (getContenido() == "else")
            {
                match("else");

                if (getContenido() == "{")
                {
                    BloqueInstrucciones(ejecutarBloqueElse);
                }
                else
                {
                    Instruccion(ejecutarBloqueElse);
                }
            }
        }

        //Printf -> printf(cadena(,Identificador)?);
        private void Printf(bool ejecuta)
        {
            match("printf"); // Comprueba si la siguiente palabra es "printf"
            match("("); // Comprueba si hay un paréntesis de apertura "("

            // Obtiene el contenido entre paréntesis
            string contenido = getContenido();

            // Verifica si el contenido comienza y termina con comillas dobles
            if (contenido.Length >= 2 && contenido[0] == '"' && contenido[contenido.Length - 1] == '"')
            {
                // Si las comillas están al principio y al final las elimina
                contenido = contenido.Substring(1, contenido.Length - 2);
            }

            // Detectar las secuencias de escape \n y \t y hacer que sean funcionales.
            contenido = contenido.Replace("\\n", "\n").Replace("\\t", "\t");

            if (ejecuta)
            {
                // Imprime el contenido de la cadena sin comillas
                Console.Write(contenido);
            }

            match(Tipos.Cadena); // Realiza una coincidencia con un tipo de cadena

            while (getContenido() == ",")
            {
                match(",");
                if (!Existe(getContenido()))
                {
                    throw new Error(" OJO 5 de sintaxis, la variable <" + getContenido() + "> no está declarada", log, linea, columna);
                }
                string variable = getContenido();
                match(Tipos.Identificador);

                if (!Existe(variable))
                {
                    throw new Error(" OJO 6 de sintaxis, la variable <" + variable + "> no está declarada", log, linea, columna);
                }
                
                if (ejecuta)
                {
                    // Imprime el valor de la variable
                    Console.Write("" + GetValor(variable));
                }
            }

            match(")"); // Comprueba si hay un paréntesis de cierre ")"
            match(";"); // Comprueba si hay un punto y coma ";"
        }


        //Scanf -> scanf(cadena,&Identificador);
        private void Scanf(bool ejecuta)
        {
            match("scanf");
            match("(");
            match(Tipos.Cadena);
            match(",");
            match("&");
            if (!Existe(getContenido()))
            {
                throw new Error(" OJO 8 de sintaxis, la variable <" + getContenido() + "> no está declarada", log, linea, columna);
            }
            string variable = getContenido();
            match(Tipos.Identificador);
            if (ejecuta)
            {
                // Declaramos una variable de tipo anulable
                // La variable cadena, nos ayudará a obtener lo que el usuario introduzca
                string? cadena = Console.ReadLine();
                // Verificamos si se puede convertir en un número flotante.
                if (float.TryParse(cadena, out float resultado))
                {
                    // Si se puede hacer la conversión, llamamos a Modifica
                    Modifica(variable, resultado);
                }
                else
                {
                    throw new Exception("Se capturó una cadena en lugar de un número.");
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
            BloqueInstrucciones(ejecuta);
        }
        //Expresion -> Termino MasTermino
        private void Expresion()
        {
            Termino();
            MasTermino();
        }
        //MasTermino -> (OperadorTermino Termino)?
        private void MasTermino()
        {
            if (getClasificacion() == Tipos.OperadorTermino)
            {
                string operador = getContenido();
                match(Tipos.OperadorTermino);
                Termino();
                log.Write(" " + operador);
                float R2 = stack.Pop();
                float R1 = stack.Pop();
                if (operador == "+")
                    stack.Push(R1 + R2);
                else
                    stack.Push(R1 - R2);
            }
        }
        //Termino -> Factor PorFactor
        private void Termino()
        {
            Factor();
            PorFactor();
        }
        //PorFactor -> (OperadorFactor Factor)?
        private void PorFactor()
        {
            if (getClasificacion() == Tipos.OperadorFactor)
            {
                string operador = getContenido();
                match(Tipos.OperadorFactor);
                Factor();
                log.Write(" " + operador);
                float R2 = stack.Pop();
                float R1 = stack.Pop();
                if (operador == "*")
                    stack.Push(R1 * R2);
                else if (operador == "/")
                    stack.Push(R1 / R2);
                else if (operador == "%")
                    stack.Push(R1 % R2);
            }
        }
        //Factor -> numero | identificador | (Expresion)
        private void Factor()
        {
            if (getClasificacion() == Tipos.Numero)
            {
                log.Write(" " + getContenido());
                stack.Push(float.Parse(getContenido()));
                match(Tipos.Numero);
            }
            else if (getClasificacion() == Tipos.Identificador)
            {
                if (!Existe(getContenido()))
                {
                    throw new Error(" OJO 9 de sintaxis, la variable <" + getContenido() + "> no está declarada", log, linea, columna);
                }
                stack.Push(GetValor(getContenido()));
                match(Tipos.Identificador);
            }
            else
            {
                match("(");
                Expresion();
                match(")");
            }
        }

        // public char convertidorAscci(string caracteres)
        // {
        //     char ascci = caracteres.ToChar(13);
        // }
    }
}
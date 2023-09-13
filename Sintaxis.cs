using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sintaxis_II {
    public class Sintaxis : Lexico {
        public Sintaxis() {
            nextToken();
        }
        public Sintaxis(string nombre) : base(nombre) {
            nextToken();
        }
        public void match(string token_esperado) {
            if (token_esperado == getContenido()) {
                nextToken();
            }
            else {
                throw new Error("OJO 99 de sintaxis, se espera un <" + token_esperado + ">", log, linea, columna);
            }
        }
        public void match(Tipos token_esperado) {
            if (token_esperado == getClasificacion()) {
                nextToken();
            }
            else {
                throw new Error(" OJO 100 de sintaxis, se espera un <" + token_esperado + ">", log, linea, columna);
            }
        }
    }
}
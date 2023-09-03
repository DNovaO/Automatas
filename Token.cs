using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sintaxis_II {
    public class Token {
        //Enumeramos los componentes
        public enum Tipos { Identificador, Numero, Asignacion, Inicializacion,
                            OperadorRelacional, OperadorTermino, OperadorFactor,
                            IncrementoTermino, IncrementoFactor, Cadena, Ternario,
                            FinSentencia, OperadorLogico, Inicio, Fin, Caracter,
                            TipoDato, Zona, Condicion, Ciclo };
        //Guardamos el contenido de token
        private string Contenido;
        //Guardamos si el string pertenece a la enumeracion de Tipos
        private Tipos Clasificacion;

        public Token() {
            //Inicializa la string vacia
            Contenido = "";
            //Inicializa la variable de tipos en 1 = identificador
            Clasificacion = Tipos.Identificador;
        }

        //Recibe la string y la modifica
        public void setContenido(string Contenido) {
            this.Contenido = Contenido;
        }
        //Recibe la clasificacion y la modifica
        public void setClasificacion(Tipos Clasificacion) {
            this.Clasificacion = Clasificacion;
        }
        //usamos get para obtener los contenidos
        public string getContenido() {
            return this.Contenido;
        }
        //usamos get para obtener los contenidos
        public Tipos getClasificacion() {
            return this.Clasificacion;
        }
        /*Esta funcion nos ayudara a eliminar los caracteres que no necesitamos. en este caso las "".
            NOTA: Ya no fue necesaria la funcion.
            public void eliminarCaracter(char caracter) {
            Contenido = Contenido.Replace(caracter.ToString(), string.Empty);
        */ }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XEnRaya
{
    internal class Program
    {
        static int[,] tablero;
        static char[] simbolos = { '.', 'O', 'X' };
        static int nPlayerActual = 1;
        static bool bTerminado = false;
        static int nInput;

        static void Main(string[] args)
        {
            Console.WriteLine("X en Raya app");
            Console.WriteLine(" Generación del tablero de juego ->>");
            Console.WriteLine("     El número que introduzca determinara el tamaño del tablero.");
            Console.WriteLine("     Ejemplo: Si ingresa '3' se generara un tablero 3x3.");
            Console.WriteLine(" Introduce un numero: ");            
            
            while (!int.TryParse(Console.ReadLine(), out nInput))
                Console.WriteLine(" INTRODUCE UN NÚMERO");
            
            tablero = new int[nInput, nInput];

            while (!bTerminado)
            {
                GenerarPantalla();
                ComprobarInputUsuario();
                ComprobarEstado();
            }
        }

        private static string GenerarMarcoTablero(int nNumColumnas)
        {
            string sResultado = "";

            for (int nColumna = 0; nColumna < tablero.GetLength(1); nColumna++)
            {
                if (nColumna == nInput - 1)
                    sResultado = sResultado + "-----";
                else
                    sResultado = sResultado + "----";
            }

            return sResultado;
        }

        private static void GenerarPantalla()
        {
            Console.Clear();
            Console.WriteLine(nInput + " en Raya app");
            Console.WriteLine(" Se ha generado un tablero " + nInput + "x" + nInput + ".");
            Console.WriteLine(" Comienza el juego: ");

            for (int nFila = 0; nFila < tablero.GetLength(0); nFila++)
            {
                Console.WriteLine(GenerarMarcoTablero(nInput));
                Console.Write("|");
                for (int nColumna = 0; nColumna < tablero.GetLength(1); nColumna++)
                {
                    // De nuestros simbolos mostramos el que esta en la posicion del tablero
                    Console.Write(" " + simbolos[tablero[nFila, nColumna]] + " |");
                }
                Console.WriteLine();
                
            }
            Console.WriteLine(GenerarMarcoTablero(nInput));
        }        

        private static void ComprobarInputUsuario()
        {
            bool bCasillaValida = false;
            int nFila;
            int nColumna;

            do
            {
                Console.Write("Jugador " + nPlayerActual + " - Introduce la fila (1 a " + nInput + "): ");                
                while (!int.TryParse(Console.ReadLine(), out nFila))
                    Console.WriteLine(" INTRODUCE UN NÚMERO DE FILA");
                
                Console.Write("Jugador " + nPlayerActual + " - Introduce la columna (1 a " + nInput + "): ");
                while (!int.TryParse(Console.ReadLine(), out nColumna))
                    Console.WriteLine(" INTRODUCE UN NÚMERO DE COLUMNA");
                
                if ((nFila >= 1) && (nFila <= tablero.GetLength(0))
                    && (nColumna >= 1) && (nColumna <= tablero.GetLength(1))
                    && tablero[nFila - 1, nColumna - 1] == 0)
                {
                    bCasillaValida = true;
                }
            }
            while (!bCasillaValida);

            // Colocamos el simbolo del jugador en la columna de la fila seleccionada
            tablero[nFila - 1, nColumna - 1] = nPlayerActual;
            
        }

        private static void ComprobarEstado()
        {
            if (ComprobarFilas() || ComprobarColumnas() 
               || ComprobarDiagonal() || ComprobarDiagonalInversa())
            {
                GenerarPantalla();
                Console.WriteLine(" Has ganado jugador" + nPlayerActual);
                Console.ReadLine();
                bTerminado = true;               
            } 
            else
            {   
                if (tablero.Length > 1) { 
                    if (ComprobarEmpate())
                    {
                        GenerarPantalla();
                        Console.WriteLine(" Empate");
                        bTerminado = true;
                    }

                    // Si no se cumple ninguna condicion anterior cambiamos turno   
                    if (nPlayerActual == 1)
                        nPlayerActual = 2;
                    else
                        nPlayerActual = 1;
                } 
                else
                {
                    GenerarPantalla();
                    Console.WriteLine(" ¡WOW! ");
                    Console.WriteLine(" ¿Has rellenado una casilla?");
                    Console.WriteLine(" ¡Felicidades jugador" + nPlayerActual + "!");
                    bTerminado = true;
                }
            }          
    }               

        private static bool ComprobarFilas()
        {
            bool bPartidaGanada = false;

            for (int nFila = 0; nFila < tablero.GetLength(0); nFila++)
            {
                for (int nColumna = 1; nColumna < tablero.GetLength(1); nColumna++)
                {
                    if (tablero[nFila, nColumna] != 0
                            && tablero[nFila, nColumna] == tablero[nFila, nColumna - 1])
                    {
                        bPartidaGanada = true;
                    }                        
                    else
                    {
                        if (tablero[nFila, nColumna] != tablero[nFila, nColumna - 1])
                        {
                            nColumna = tablero.GetLength(0);
                        }
                        bPartidaGanada = false;
                    }                       
                }
                
                if (bPartidaGanada)
                {   
                    //Si las columnas de la fila comprobada son iguales salimos del bucle
                    nFila = tablero.GetLength(0);
                }
            }

            return bPartidaGanada;
        }

        private static bool ComprobarColumnas()
        {
            bool bPartidaGanada = false;

            for (int nColumna = 0; nColumna < tablero.GetLength(1); nColumna++)
            {
                for (int nFila = 1; nFila < tablero.GetLength(0); nFila++)
                {
                    if (tablero[nFila, nColumna] != 0
                        && tablero[nFila, nColumna] == tablero[nFila -1, nColumna])
                    {
                        bPartidaGanada = true;
                    }
                    else
                    {
                        if (tablero[nFila, nColumna] != tablero[nFila - 1, nColumna])
                        {
                            nFila = tablero.GetLength(0);
                        }
                        bPartidaGanada = false;
                    }                                                             
                }

                if (bPartidaGanada)
                {
                    //Si las columnas de la fila comprobada son iguales salimos del bucle
                    nColumna = tablero.GetLength(0);
                }
            }

            return bPartidaGanada;
        }

        private static bool ComprobarDiagonal()
        {
            bool bPartidaGanada = false;
            int[] nDiagonal = new int[tablero.GetLength(0)];

            for (int nFila = 0; nFila < tablero.GetLength(0); nFila++)
            {
                for (int nColumna = 0; nColumna < tablero.GetLength(1); nColumna++)
                {
                    if(nFila == nColumna)
                    {
                        nDiagonal[nFila] = tablero[nFila, nColumna];                    
                    }
                }
            }

            for (int i = 0; i < nDiagonal.Length; i++)
            {
                if ( i+1 < nDiagonal.Length && nDiagonal[i] > 0) 
                {
                    if (nDiagonal[i] == nDiagonal[i + 1])
                    {
                        bPartidaGanada = true;
                    }
                    else
                    {
                        i = nDiagonal.Length;
                        bPartidaGanada = false;
                    }
                } 
                else
                {
                    i = nDiagonal.Length;
                }
                
            }

            return bPartidaGanada;
        }

        private static bool ComprobarDiagonalInversa()
        {
            bool bPartidaGanada = false;

            int[] nDiagonalInversa = new int[tablero.GetLength(0)];

            for (int nFila = 0; nFila < tablero.GetLength(0); nFila++)
            {
                for (int nColumna = 0; nColumna < tablero.GetLength(1); nColumna++)
                {
                    if ((nFila + nColumna) == tablero.GetLength(0) - 1)
                    {
                        nDiagonalInversa[nFila] = tablero[nFila, nColumna];
                    }                        
                }
            }

            for (int i = 0; i < nDiagonalInversa.Length; i++)
            {
                if ( i+1 < nDiagonalInversa.Length && nDiagonalInversa[i] > 0)
                {
                    if (nDiagonalInversa[i] == nDiagonalInversa[i + 1])
                    {
                        bPartidaGanada = true;
                    }
                    else
                    {
                        i = nDiagonalInversa.Length;
                        bPartidaGanada = false;
                    }
                }
                else
                {
                    i = nDiagonalInversa.Length;
                }

            }

            return bPartidaGanada;
        }

        private static bool ComprobarEmpate()
        {   
            bool bComprobarEmpate = false;
            int nEspaciosLibres = 0;

            for (int nFila = 0; nFila < tablero.GetLength(0); nFila++)
            {
                for (int nColumna = 0; nColumna < tablero.GetLength(1); nColumna++)
                {
                    // De nuestros simbolos mostramos el que esta en la posicion del tablero
                    if (tablero[nFila, nColumna] == 0)
                        nEspaciosLibres++;
                }
            }
            
            if (nEspaciosLibres == 0)
            {
                bComprobarEmpate = true;                
            }                    
            
            return bComprobarEmpate;
        }
    }
}

using CColor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CColor.ColorEquipo;

namespace CFicha
{
    public class Ficha
    {
        public int posX_tabla;
        public int posY_tabla;

        public int pos1X;
        public int pos1Y;

        Colores color;

        bool enCasa;

        public int contadorCasa;

        public bool primeraTirada;
        public int contador;

        public bool llegado;
        public bool safe;

      public Ficha(int posX_tabla, int posY_tabla, int pos1X, int pos1Y, bool safe, bool llegado, bool primeraTirada, int contador, int contadorCasa, Colores color)  
        {
         
            this.posX_tabla = posX_tabla;
            this.posY_tabla = posY_tabla;
            this.color= color;
            this.pos1X = pos1X;
            this.pos1Y = pos1Y;

            this.contadorCasa = contadorCasa;

            this.contador = contador;
            this.primeraTirada = primeraTirada;
            this.llegado = llegado;
            this.safe = safe;

        }

        public void setEnCasa(bool enCasa)
        {
            this.enCasa = enCasa; 
        }

        public bool getEnCasa()
        {
            return enCasa;
        }

        public int getX()
        {
            return this.posX_tabla;
        }
        public int getY()
        {
            return this.posY_tabla;
        }

        public void setX(int x)
        {
            this.posX_tabla = x;
        }
        public void setY(int y)
        {
            this.posY_tabla = y;
        }
        public void setPrimeraTirada(bool primeraTirada)
        {
            this.primeraTirada = primeraTirada; 
        }
        public bool getPrimeraTirada()
        {
            return this.primeraTirada;
        }
        public void setCounter(int c)
        {
            this.contador = c;
        }
        public int getCounter()
        {
            return this.contador;
        }
        public int getPOS1X()
        {
            return this.pos1X;
        }
        public int getPOS1Y()
        {
            return this.pos1Y;
        }

        public void setContadorCasa(int x)
        {
            this.contadorCasa = x;
        }
        public int getContadorCasa()
        {
            return this.contadorCasa;
        }
        public void setLlegada(bool llegado)
        {
            this.llegado= llegado;
        }
        public bool getLlegada()
        {
            return llegado;
        }
        public void setSafe(bool safe)
        {
            this.safe = safe;
        }
        public bool isSafe()
        {
            return this.safe;
        }
        public Colores getColor()
        {
            return color;
        }
    }
}

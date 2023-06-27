using CFicha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CColor
{
    public class ColorEquipo
    {
        public enum Colores
        {
            Rojo,
            Verde,
            Amarillo,
            Azul
        }

        int casillaSalida, casillaLlegada;
        List<Ficha> fichas;
        Colores color;
        bool turno;

        public ColorEquipo(Colores color, List<Ficha> fichas)
        {
            switch (color)
            {
                case Colores.Rojo:
                    this.casillaSalida = 39;
                    this.casillaLlegada = 34;
                    this.turno = true;
                    break;
                case Colores.Verde:
                    this.casillaSalida = 56;
                    this.casillaLlegada = 51;
                    this.turno = false;
                    break;
                case Colores.Amarillo:
                    this.casillaSalida = 5;
                    this.casillaLlegada = 68;
                    this.turno = false;
                    break;
                case Colores.Azul:
                    this.casillaSalida = 22;
                    this.casillaLlegada = 17;
                    this.turno = false;
                    break;
            }

            this.fichas = fichas;
        }

        public Colores getColor()
        {
            return color;
        }

        public int getCasillaSalida()
        {
            return casillaSalida;
        }

        public int getCasillaLlegada()
        {
            return casillaLlegada;
        }

        public List<Ficha> getFichas()
        {
            return fichas;
        }

        public bool getTurno()
        {
            return turno;
        } 

        public void setTurno(bool turno)
        {
            this.turno = turno;
        }


    }
}

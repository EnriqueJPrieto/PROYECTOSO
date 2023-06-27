using CFicha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCasilla
{
    public class Casilla
    {
        public int posX;
        public int posY;

        List<Ficha> fichas;

        public bool safe;

        public int AX;
        public int AY;

        public Casilla(int posX, int posY, bool safe, int AX, int AY)
        {
            this.posX = posX;
            this.posY = posY;
            this.fichas = new List<Ficha>(); 
            this.AX = AX;
            this.AY = AY;
            this.safe = safe;
        }
       
        public List<Ficha> getFichas()
        {
            return fichas;
        }

        public void addFicha(Ficha ficha)
        {
            fichas.Add(ficha);
        }

        public void removeFicha(Ficha ficha)
        {
            fichas.Remove(ficha);
        }

        public bool isSafe()
        { return this.safe; }
        
        public int GetX()
        { return this.posX; }
       
        public int GetY()
        { return this.posY; }

        public int GetAX()
        { return this.AX; }

        public int GetAY()
        { return this.AY; }

    }
}
  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBarrera
{
    public class Barrera
    {
        public bool existe;
        public int posBarrera;

        public int C1;
        public int C2;

        public Barrera(bool existe, int posBarrera, int c1, int c2)
        {
            this.existe = existe;
            this.posBarrera = posBarrera;
            this.C1 = c1;
            this.C2 = c2;
        }
        public void setExiste(bool existe)
        {
            this.existe = existe;
        }
        public bool getExiste()
        {
            return this.existe;
        }
        public void setPos(int i)
        {
            this.posBarrera = i;
        }
        public int getPos()
        {
            return this.posBarrera;
        }
        public void setC1(int i)
        {
            this.C1 = i;
        }
        public int getC1()
        {
            return this.C1;
        }
        public void setC2(int i)
        {
            this.C2 = i;
        }
        public int getC2()
        {
            return this.C2;
        }
    }
}

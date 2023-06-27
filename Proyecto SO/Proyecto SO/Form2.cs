using CFicha;
using CBarrera;
using CCasilla;
using CColor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static CColor.ColorEquipo;
using System.Transactions;

namespace Proyecto_SO
{
    public partial class Form2 : Form
    {
        Form1 f1;

        public string enviar;
        public bool _token;
        public int _n;
        public bool _comido;
        public List<ColorEquipo> colors;

        PictureBox[] pictureBoxes;

        public int valorPrevio;
        public int teamColor;

        public int num_dado;

        public Casilla[] casillas = new Casilla[69];

        public int contador = 0;
        public int minutos = 0;

        List<int> xCasaB = new List<int> { 18, 17, 16, 15, 14, 13, 12, 11 };
        List<int> yCasaB = new List<int> { 9, 9, 9, 9, 9, 9, 9, 9 };

        List<int> xCasaG = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
        List<int> yCasaG = new List<int> { 9, 9, 9, 9, 9, 9, 9, 9 };

        List<int> xCasaR = new List<int> { 9, 9, 9, 9, 9, 9, 9, 9 };
        List<int> yCasaR = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };

        List<int> xCasaY = new List<int> { 9, 9, 9, 9, 9, 9, 9, 9 };
        List<int> yCasaY = new List<int> { 18, 17, 16, 15, 14, 13, 12, 11 };

        List<List<List<int>>> Casa;

        public Form2(Form1 f1)
        {
            this.f1 = f1;

            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            colors = new List<ColorEquipo>();

            List<Ficha> fichasRojas = new List<Ficha>();
            List<Ficha> fichasVerdes = new List<Ficha>();
            List<Ficha> fichasAmarillas = new List<Ficha>();
            List<Ficha> fichasAzules = new List<Ficha>();

            fichasRojas.Add(new Ficha(2, 2, 2, 2, true, false, false, 0, -1, Colores.Rojo));
            fichasRojas.Add(new Ficha(2, 4, 2, 4, true, false, false, 0, -1, Colores.Rojo));
            fichasRojas.Add(new Ficha(4, 2, 4, 2, true, false, false, 0, -1, Colores.Rojo));
            fichasRojas.Add(new Ficha(4, 4, 4, 4, true, false, false, 0, -1, Colores.Rojo));

            fichasVerdes.Add(new Ficha(2, 15, 2, 15, true, false, false, 0, -1, Colores.Verde));
            fichasVerdes.Add(new Ficha(4, 15, 4, 15, true, false, false, 0, -1, Colores.Verde));
            fichasVerdes.Add(new Ficha(2, 17, 2, 17, true, false, false, 0, -1, Colores.Verde));
            fichasVerdes.Add(new Ficha(4, 17, 4, 17, true, false, false, 0, -1, Colores.Verde));

            fichasAmarillas.Add(new Ficha(15, 15, 14, 15, true, false, false, 0, -1, Colores.Amarillo));
            fichasAmarillas.Add(new Ficha(17, 15, 17, 15, true, false, false, 0, -1, Colores.Amarillo));
            fichasAmarillas.Add(new Ficha(15, 17, 15, 17, true, false, false, 0, -1, Colores.Amarillo));
            fichasAmarillas.Add(new Ficha(17, 17, 17, 17, true, false, false, 0, -1, Colores.Amarillo));

            fichasAzules.Add(new Ficha(15, 2, 15, 2, true, false, false, 0, -1, Colores.Azul));
            fichasAzules.Add(new Ficha(17, 2, 17, 2, true, false, false, 0, -1, Colores.Azul));
            fichasAzules.Add(new Ficha(15, 4, 15, 4, true, false, false, 0, -1, Colores.Azul));
            fichasAzules.Add(new Ficha(17, 4, 17, 4, true, false, false, 0, -1, Colores.Azul));

            colors.Add(new ColorEquipo(Colores.Rojo, fichasRojas));
            colors.Add(new ColorEquipo(Colores.Verde, fichasVerdes));
            colors.Add(new ColorEquipo(Colores.Amarillo, fichasAmarillas));
            colors.Add(new ColorEquipo(Colores.Azul, fichasAzules));

            List<List<int>> CasaB = new List<List<int>> { xCasaB, yCasaB };
            List<List<int>> CasaY = new List<List<int>> { xCasaY, yCasaY };
            List<List<int>> CasaR = new List<List<int>> { xCasaR, yCasaR };
            List<List<int>> CasaG = new List<List<int>> { xCasaG, yCasaG };

            Casa = new List<List<List<int>>> { CasaR, CasaG, CasaY, CasaB};

            casillas[0] = new Casilla(11, 19, false, 12, 19);//no es accesible, esta para dar estabilidad
            casillas[1] = new Casilla(11, 19, false, 12, 19);
            casillas[2] = new Casilla(11, 18, false, 12, 18);
            casillas[3] = new Casilla(11, 17, false, 12, 17);
            casillas[4] = new Casilla(11, 16, false, 12, 16);
            casillas[5] = new Casilla(11, 15, true, 12, 15);
            casillas[6] = new Casilla(11, 14, false, 12, 14);
            casillas[7] = new Casilla(11, 13, false, 12, 13);
            casillas[8] = new Casilla(11, 12, false, 12, 12);
            casillas[9] = new Casilla(12, 11, false, 12, 12);
            casillas[10] = new Casilla(13, 11, false, 13, 12);
            casillas[11] = new Casilla(14, 11, false, 14, 12);
            casillas[12] = new Casilla(15, 11, true, 15, 12);
            casillas[13] = new Casilla(16, 11, false, 16, 12);
            casillas[14] = new Casilla(17, 11, false, 17, 12);
            casillas[15] = new Casilla(18, 11, false, 18, 12);
            casillas[16] = new Casilla(19, 11, false, 19, 12);
            casillas[17] = new Casilla(19, 9, true, 19, 10);
            casillas[18] = new Casilla(19, 7, false, 19, 8);
            casillas[19] = new Casilla(18, 7, false, 18, 8);
            casillas[20] = new Casilla(17, 7, false, 17, 8);
            casillas[21] = new Casilla(16, 7, false, 16, 8);
            casillas[22] = new Casilla(15, 7, false, 15, 8);
            casillas[23] = new Casilla(14, 7, true, 14, 8);
            casillas[24] = new Casilla(13, 7, false, 13, 8);
            casillas[25] = new Casilla(12, 7, false, 12, 8);
            casillas[26] = new Casilla(11, 7, false, 12, 7);
            casillas[27] = new Casilla(11, 6, false, 12, 7);
            casillas[28] = new Casilla(11, 5, false, 12, 6);
            casillas[29] = new Casilla(11, 4, false, 12, 5);
            casillas[30] = new Casilla(11, 3, true, 12, 4);
            casillas[31] = new Casilla(11, 2, false, 12, 3);
            casillas[32] = new Casilla(11, 1, false, 12, 2);
            casillas[33] = new Casilla(11, 0, false, 12, 1);
            casillas[34] = new Casilla(9, 0, true, 10, 0);
            casillas[35] = new Casilla(7, 0, false, 8, 0);
            casillas[36] = new Casilla(7, 1, false, 8, 1);
            casillas[37] = new Casilla(7, 2, false, 8, 2);
            casillas[38] = new Casilla(7, 3, false, 8, 3);
            casillas[39] = new Casilla(7, 4, true, 8, 4);
            casillas[40] = new Casilla(7, 5, false, 8, 5);
            casillas[41] = new Casilla(7, 6, false, 8, 6);
            casillas[42] = new Casilla(8, 7, false, 7, 7);
            casillas[43] = new Casilla(7, 8, false, 7, 7);
            casillas[44] = new Casilla(6, 7, false, 6, 8);
            casillas[45] = new Casilla(5, 7, false, 5, 8);
            casillas[46] = new Casilla(4, 7, true, 4, 8);
            casillas[47] = new Casilla(3, 7, false, 3, 8);
            casillas[48] = new Casilla(2, 7, false, 2, 8);
            casillas[49] = new Casilla(1, 7, false, 1, 8);
            casillas[50] = new Casilla(0, 7, false, 0, 8);
            casillas[51] = new Casilla(0, 9, true, 0, 10);
            casillas[52] = new Casilla(0, 11, false, 0, 12);
            casillas[53] = new Casilla(1, 11, false, 1, 12);
            casillas[54] = new Casilla(2, 11, false, 2, 12);
            casillas[55] = new Casilla(3, 11, false, 3, 12);
            casillas[56] = new Casilla(4, 11, true, 4, 12);
            casillas[57] = new Casilla(5, 11, false, 5, 12);
            casillas[58] = new Casilla(6, 11, false, 6, 12);
            casillas[59] = new Casilla(7, 11, false, 7, 12);
            casillas[60] = new Casilla(8, 12, false, 7, 12);
            casillas[61] = new Casilla(7, 13, false, 8, 13);
            casillas[62] = new Casilla(7, 14, false, 8, 14);
            casillas[63] = new Casilla(7, 15, true, 8, 15);
            casillas[64] = new Casilla(7, 16, false, 8, 16);
            casillas[65] = new Casilla(7, 17, false, 8, 17);
            casillas[66] = new Casilla(7, 18, false, 8, 18);
            casillas[67] = new Casilla(7, 19, false, 8, 19);
            casillas[68] = new Casilla(9, 19, true, 10, 19);
        }
        public void setInfo(string enviar)
        {
            this.enviar = enviar;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            turno.BackColor = Color.Red;

            SKIP.Visible = false;

            DadoButton.Visible = false;

            if (teamColor == 0)
            {
                DadoButton.Visible = true;
            }
            pictureBoxes = new PictureBox[16];
            pictureBoxes[0] = ficha0;
            pictureBoxes[1] = ficha1;
            pictureBoxes[2] = ficha2;
            pictureBoxes[3] = ficha3;
            pictureBoxes[4] = ficha4;
            pictureBoxes[5] = ficha5;
            pictureBoxes[6] = ficha6;
            pictureBoxes[7] = ficha7;
            pictureBoxes[8] = ficha8;
            pictureBoxes[9] = ficha9;
            pictureBoxes[10] = ficha10;
            pictureBoxes[11] = ficha11;
            pictureBoxes[12] = ficha12;
            pictureBoxes[13] = ficha13;
            pictureBoxes[14] = ficha14;
            pictureBoxes[15] = ficha15;

            for (int i = 0; i < pictureBoxes.Length; i++)
            {
                pictureBoxes[i].BackgroundImage = Image.FromFile("R" + i.ToString() + ".png");
                pictureBoxes[i].BackgroundImageLayout = ImageLayout.Stretch;
            }
        }
        private void DadoButton_Click(object sender, EventArgs e)
        {
            FIXA1.Visible = true;
            FIXA2.Visible = true;
            FIXA3.Visible = true;
            FIXA4.Visible= true;

            label1.Visible = false;

            Random random = new Random();
            this.num_dado =  random.Next(1,7);

            DadoBox.Image = Image.FromFile("dado" + num_dado.ToString() + ".jpg");

            for (int i = 0; i < colors[teamColor].getFichas().Count; i++)
            {
                if (this.num_dado == 1 || this.num_dado == 5)
                {
                    if (!colors[teamColor].getFichas()[i].getPrimeraTirada())
                    {
                        if (i == 0)
                        {
                            FIXA1.BackColor = Color.Red;
                            FIXA1.Enabled = true;
                        }
                        else if (i == 1)
                        {
                            FIXA2.BackColor = Color.Red;
                            FIXA2.Enabled = true;
                        }
                        else if (i == 2)
                        {
                            FIXA3.BackColor = Color.Red;
                            FIXA3.Enabled = true;
                        }
                        else if (i == 3)
                        {
                            FIXA4.BackColor = Color.Red;
                            FIXA4.Enabled = true;
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            FIXA1.BackColor = Color.White;
                            FIXA1.Enabled = true;
                        }
                        else if (i == 1)
                        {
                            FIXA2.BackColor = Color.White;
                            FIXA2.Enabled = true;
                        }
                        else if (i == 2)
                        {
                            FIXA3.BackColor = Color.White;
                            FIXA3.Enabled = true;
                        }
                        else if (i == 3)
                        {
                            FIXA4.BackColor = Color.White;
                            FIXA4.Enabled = true;
                        }
                    }

                }
                else
                {
                    if (!colors[teamColor].getFichas()[i].getPrimeraTirada())
                    {
                        if (i == 0)
                        {
                            FIXA1.BackColor = Color.LightGray;
                            FIXA1.Enabled = false;
                        }
                        else if (i == 1)
                        {
                            FIXA2.BackColor = Color.LightGray;
                            FIXA2.Enabled = false;
                        }
                        else if (i == 2)
                        {
                            FIXA3.BackColor = Color.LightGray;
                            FIXA3.Enabled = false;
                        }
                        else if (i == 3)
                        {
                            FIXA4.BackColor = Color.LightGray;
                            FIXA4.Enabled = false;
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            FIXA1.BackColor = Color.White;
                            FIXA1.Enabled = true;
                        }
                        else if (i == 1)
                        {
                            FIXA2.BackColor = Color.White;
                            FIXA2.Enabled = true;
                        }
                        else if (i == 2)
                        {
                            FIXA3.BackColor = Color.White;
                            FIXA3.Enabled = true;
                        }
                        else if (i == 3)
                        {
                            FIXA4.BackColor = Color.White;
                            FIXA4.Enabled = true;
                        }
                    }
                }
            }

            Visibilidad();

            if (!FIXA1.Enabled && !FIXA2.Enabled && !FIXA3.Enabled && !FIXA4.Enabled)
            {
                label1.Text = "No tienes movimientos legales";
                label1.Visible = true;
                SKIP.Visible = true;
            }
            DadoButton.Visible = false;
        } 
        private void mandarCasa(Ficha ficha, int id)
        {
            casillas[ficha.getCounter()].removeFicha(ficha);

            ficha.setX(ficha.getPOS1X());
            ficha.setY(ficha.getPOS1Y());

            ficha.setPrimeraTirada(false);
            ficha.setCounter(0);

            Tabla.Controls.Add(pictureBoxes[id + 4*teamColor], ficha.getX(), ficha.getY());
        }
        private void moverFicha(int ficha, int OC, int FC)
        {
            valorPrevio = colors[teamColor].getFichas()[ficha].getCounter();
            if (casillas[OC].getFichas().Count > 0)
            {
                casillas[OC].removeFicha(colors[teamColor].getFichas()[ficha]);
            }
            // Limitación debido a barrera
            if (OC != 0)
            {
                for (int i = OC + 1; i <= FC; i++)
                {
                    if (i > 68)
                    {
                        i = 1;
                        FC -= 68;

                    }
                    if (casillas[i].getFichas().Count >= 2)
                    {
                        FC = i - 1;
                        if (FC == 0)
                        {
                            FC = 68;
                        }
                        break;
                    }
                }
            }
            if (FC > 68)
            {
                FC -= 68;
            }
            if (casillas[FC].getFichas().Count > 0)
            {
                if (!casillas[FC].isSafe() && casillas[FC].getFichas()[0].getColor() != colors[teamColor].getFichas()[ficha].getColor())
                {
                    Ficha buffer = casillas[FC].getFichas()[0];
                    mandarCasa(casillas[FC].getFichas()[0], ficha);
                    f1._n--;
                    enviar3((int)buffer.getColor(), colors[(int)buffer.getColor()].getFichas().IndexOf(buffer));
                    Thread.Sleep(200);

                    moverFicha(ficha, FC, FC + 20);
                }
            }
            // Correccion llegada a casa
            if (FC < OC)
            {
                FC += 68;
            }
            //int diff = FC - colors[teamColor].getCasillaLlegada();
            if (OC <= colors[teamColor].getCasillaLlegada() && colors[teamColor].getCasillaLlegada() < FC && OC != 0)
            {
                valorPrevio = colors[teamColor].getFichas()[ficha].getContadorCasa();
                colors[teamColor].getFichas()[ficha].setContadorCasa(FC - colors[teamColor].getCasillaLlegada());
            }
            if (FC > 68)
            {
                FC -= 68;
            }
            // Lo que va dentro de casa
            if (colors[teamColor].getFichas()[ficha].getContadorCasa() != -1 && valorPrevio != -1)
            {
                if (teamColor == 2)
                {
                    if (FC < OC)
                    {
                        FC += 68;
                    }
                }
                colors[teamColor].getFichas()[ficha].setContadorCasa(FC - colors[teamColor].getCasillaLlegada());
                if (teamColor != 2)
                {
                    colors[teamColor].getFichas()[ficha].setCounter(FC);
                }
                else
                {
                    colors[teamColor].getFichas()[ficha].setContadorCasa(FC);// - colors[teamColor].getCasillaLlegada()
                }
                MoverCasa(teamColor, ficha);
                enviar3(teamColor, ficha);
            }
            // lo que va fuera
            else
            {
                colors[teamColor].getFichas()[ficha].setCounter(FC);
                
                moverFichaGrafica(teamColor, ficha);
                casillas[FC].addFicha(colors[teamColor].getFichas()[ficha]);
                enviar3(teamColor, ficha);
            }
            
        }
        public void moverFichaGrafica(int equipo, int i)
        {
            if (colors[equipo].getFichas()[i].getCounter() != 0)
            {
                if (casillas[colors[equipo].getFichas()[i].getCounter()].getFichas().Count == 0)
                {
                    colors[equipo].getFichas()[i].setX(casillas[colors[equipo].getFichas()[i].getCounter()].GetX());
                    colors[equipo].getFichas()[i].setY(casillas[colors[equipo].getFichas()[i].getCounter()].GetY());
                    if (casillas[colors[equipo].getFichas()[i].getCounter()].isSafe())
                    {
                        colors[equipo].getFichas()[i].setSafe(true);
                    }
                    else
                        colors[equipo].getFichas()[i].setSafe(false);
                }
                else
                {
                    colors[equipo].getFichas()[i].setX(casillas[colors[equipo].getFichas()[i].getCounter()].GetAX());
                    colors[equipo].getFichas()[i].setY(casillas[colors[equipo].getFichas()[i].getCounter()].GetAY());
                    if (casillas[colors[equipo].getFichas()[i].getCounter()].isSafe())
                    {
                        colors[equipo].getFichas()[i].setSafe(true);
                    }
                    else
                        colors[equipo].getFichas()[i].setSafe(false);
                }
                Tabla.Controls.Add(pictureBoxes[i + 4 * equipo], colors[equipo].getFichas()[i].getX(), colors[equipo].getFichas()[i].getY());
            }
            else
            {
                colors[equipo].getFichas()[i].setX(colors[equipo].getFichas()[i].getPOS1X());
                colors[equipo].getFichas()[i].setY(colors[equipo].getFichas()[i].getPOS1Y());
                Tabla.Controls.Add(pictureBoxes[i + 4 * equipo], colors[equipo].getFichas()[i].getX(), colors[equipo].getFichas()[i].getY());
            }
        }
        public void MoverCasa(int equipo, int i)
        {
            colors[equipo].getFichas()[i].setX(Casa[equipo][0][colors[equipo].getFichas()[i].getContadorCasa()]);
            colors[equipo].getFichas()[i].setY(Casa[equipo][1][colors[equipo].getFichas()[i].getContadorCasa()]);
            
            Tabla.Controls.Add(pictureBoxes[i + 4 * equipo], colors[equipo].getFichas()[i].getX(), colors[equipo].getFichas()[i].getY());

            if (colors[equipo].getFichas()[i].getContadorCasa() > 8)
            {
                colors[equipo].getFichas()[i].setX(Casa[equipo][0][8]);
                Tabla.Controls.Add(pictureBoxes[i + 4 * equipo], colors[equipo].getFichas()[i].getX(), colors[equipo].getFichas()[i].getY());
            }
        }
        public void enviar3(int equipoColor, int ficha)
        {
            if (colors[equipoColor].getFichas()[ficha].getContadorCasa() != -1)
            {
                enviar = equipoColor + "/" + ficha + "/" + (colors[equipoColor].getCasillaLlegada() + colors[equipoColor].getFichas()[ficha].getContadorCasa()) + "/True";
            }
            else
            {
                enviar = equipoColor + "/" + ficha + "/" + colors[equipoColor].getFichas()[ficha].getCounter() + "/False";
            }
            f1.jugada();
        }
        public void Visibilidad()
        {
            if (colors[teamColor].getFichas()[0].getLlegada())
            {
                FIXA1.Visible = false;
                FIXA1.Enabled = false;
            }
            if (colors[teamColor].getFichas()[1].getLlegada())
            {
                FIXA2.Visible = false;
                FIXA2.Enabled = false;
            }
            if (colors[teamColor].getFichas()[2].getLlegada())
            {
                FIXA3.Visible = false;
                FIXA3.Enabled = false;
            }
            if (colors[teamColor].getFichas()[3].getLlegada())
            {
                FIXA4.Visible = false;
                FIXA4.Enabled = false;
            }
        }
        private void FIXA1_Click(object sender, EventArgs e)
        {
            _comido = false;
            FIXA1.Visible = false;
            FIXA2.Visible = false;
            FIXA3.Visible = false;
            FIXA4.Visible = false;

            if(colors[teamColor].getFichas()[0].getPrimeraTirada() && colors[teamColor].getFichas()[0].getContadorCasa() == -1)
            {
                if (colors[teamColor].getFichas()[0].getCounter() != 0)
                {
                    moverFicha(0, colors[teamColor].getFichas()[0].getCounter(), colors[teamColor].getFichas()[0].getCounter() + num_dado);
                }
                else
                    moverFicha(0, 0, colors[teamColor].getCasillaSalida());
            }
            else if (colors[teamColor].getFichas()[0].getContadorCasa() != -1 && teamColor == 2)
            {
                moverFicha(0, colors[teamColor].getFichas()[0].getContadorCasa(), colors[teamColor].getFichas()[0].getContadorCasa() + num_dado);
            }
            else
            {
                colors[teamColor].getFichas()[0].setPrimeraTirada(true);
                moverFicha(0, 0, colors[teamColor].getCasillaSalida());
            }
            Ganador();
        }
        private void FIXA2_Click(object sender, EventArgs e)
        {
            _comido = false;
            FIXA1.Visible = false;
            FIXA2.Visible = false;
            FIXA3.Visible = false;
            FIXA4.Visible = false;

            if(colors[teamColor].getFichas()[1].getPrimeraTirada())
            {
                
                if (colors[teamColor].getFichas()[1].getCounter() != 0 && colors[teamColor].getFichas()[1].getContadorCasa() == -1)
                {
                    moverFicha(1, colors[teamColor].getFichas()[1].getCounter(), colors[teamColor].getFichas()[1].getCounter() + num_dado);
                }
                else
                    moverFicha(1, 0, colors[teamColor].getCasillaSalida());
            }
            else if (colors[teamColor].getFichas()[1].getContadorCasa() != -1 && teamColor == 2)
            {
                moverFicha(1, colors[teamColor].getFichas()[1].getContadorCasa(), colors[teamColor].getFichas()[1].getContadorCasa() + num_dado);
            }
            else
            {
                colors[teamColor].getFichas()[1].setPrimeraTirada(true);
                moverFicha(1, 0, colors[teamColor].getCasillaSalida());
            }
            Ganador();
        }
        private void FIXA3_Click(object sender, EventArgs e)
        {
            _comido = false;
            FIXA1.Visible = false;
            FIXA2.Visible = false;
            FIXA3.Visible = false;
            FIXA4.Visible = false;

            if (colors[teamColor].getFichas()[2].getPrimeraTirada() && colors[teamColor].getFichas()[2].getContadorCasa() == -1)
            {
                
                if (colors[teamColor].getFichas()[2].getCounter() != 0)
                {
                    moverFicha(2, colors[teamColor].getFichas()[2].getCounter(), colors[teamColor].getFichas()[2].getCounter() + num_dado);
                }
                else
                    moverFicha(2, 0, colors[teamColor].getCasillaSalida());
            }
            else if (colors[teamColor].getFichas()[2].getContadorCasa() != -1 && teamColor == 2)
            {
                moverFicha(2, colors[teamColor].getFichas()[2].getContadorCasa(), colors[teamColor].getFichas()[2].getContadorCasa() + num_dado);
            }
            else
            {
                colors[teamColor].getFichas()[2].setPrimeraTirada(true);
                moverFicha(2, 0, colors[teamColor].getCasillaSalida());
            }
            Ganador();
        }
        private void FIXA4_Click(object sender, EventArgs e)
        {
            _comido = false;
            FIXA1.Visible = false;
            FIXA2.Visible = false;
            FIXA3.Visible = false;
            FIXA4.Visible = false;

            if (colors[teamColor].getFichas()[3].getPrimeraTirada() && colors[teamColor].getFichas()[3].getContadorCasa() == -1)
            {
                if (colors[teamColor].getFichas()[3].getCounter() != 0)
                {
                    moverFicha(3, colors[teamColor].getFichas()[3].getCounter(), colors[teamColor].getFichas()[3].getCounter() + num_dado);
                }
                else
                    moverFicha(3, 0, colors[teamColor].getCasillaSalida());
            }
            else if (colors[teamColor].getFichas()[3].getContadorCasa() != -1 && teamColor == 2)
            {
                moverFicha(3, colors[teamColor].getFichas()[3].getContadorCasa(), colors[teamColor].getFichas()[3].getContadorCasa() + num_dado);
            }
            else
            {
                colors[teamColor].getFichas()[3].setPrimeraTirada(true);
                moverFicha(3, 0, colors[teamColor].getCasillaSalida());
            }
            Ganador();
        }
        public void Ganador()
        {
            for(int i = 0; i < colors.Count; i++)
            {
                if (colors[teamColor].getFichas()[0].getLlegada() && colors[teamColor].getFichas()[1].getLlegada() && colors[teamColor].getFichas()[2].getLlegada() && colors[teamColor].getFichas()[3].getLlegada())
                {
                    MessageBox.Show("LA PARTIDA HA TERMINADO");
                }
            }
        }
        public void updateStatus(int ficha, int equipo, int posicion, bool casa)
        {
            if (casa)
            {
                if (posicion > 68)
                {
                    posicion -= 68;
                    colors[equipo].getFichas()[ficha].setContadorCasa(posicion);
                    MoverCasa(equipo, ficha);
                }
                else
                {
                    colors[equipo].getFichas()[ficha].setContadorCasa(posicion - colors[equipo].getCasillaLlegada());
                    MoverCasa(equipo, ficha);
                } 
            }
            else
            {
                colors[equipo].getFichas()[ficha].setCounter(posicion);
                moverFichaGrafica(equipo, ficha);
            }
        }
        public void Servidor(int ficha, int equipo, int posicion, bool casa,  int res)
        {
            if (ficha != 16)
            {
                if (colors[equipo].getFichas()[ficha].getCounter() > 68)
                {
                    casillas[colors[equipo].getFichas()[ficha].getCounter() - colors[equipo].getFichas()[ficha].getContadorCasa()].removeFicha(colors[equipo].getFichas()[ficha]);
                }
                else
                {
                    casillas[colors[equipo].getFichas()[ficha].getCounter()].removeFicha(colors[equipo].getFichas()[ficha]);
                }
                updateStatus(ficha, equipo, posicion, casa);
                if(!casa)
                {
                    casillas[colors[equipo].getFichas()[ficha].getCounter()].addFicha(colors[equipo].getFichas()[ficha]);
                }
                this.num_dado = res;
            }

            if (f1.res == 1)
            {
                
                DadoBox.Image = Image.FromFile("dado" + f1.res + ".jpg");
            }
            if (f1.res == 2)
            {

                DadoBox.Image = Image.FromFile("dado" + f1.res + ".jpg");
            }
            if (f1.res == 3)
            {

                DadoBox.Image = Image.FromFile("dado" + f1.res + ".jpg");
            }
            if (f1.res == 4)
            {

                DadoBox.Image = Image.FromFile("dado" + f1.res + ".jpg");
            }
            if (f1.res == 5)
            {

                DadoBox.Image = Image.FromFile("dado" + f1.res + ".jpg");
            }
            if (f1.res == 6)
            {

                DadoBox.Image = Image.FromFile("dado" + f1.res + ".jpg");
            }

            

            if (f1._n == 0)
            {
                turno.BackColor = Color.Red;
            }
            else if (f1._n == 1)
            {
                turno.BackColor = Color.Green;
            }
            else if (f1._n == 2)
            {
                turno.BackColor = Color.Yellow;
            }
            else if (f1._n == 3)
            {
                turno.BackColor = Color.Blue;
            }
        }

        public void setToken(bool _token)
        {
            this._token = _token;
        }

        private void SKIP_Click(object sender, EventArgs e)
        {
            enviar = 99+"/";
           
            f1.jugada();
            SKIP.Visible = false;
        }
        public void DadoVisible(bool token)
        {
            DadoButton.Visible = token;
        }

       


        private void button1_Click(object sender, EventArgs e)
        {

            FIXA1.Visible = true;
            FIXA2.Visible = true;
            FIXA3.Visible = true;
            FIXA4.Visible = true;

            FIXA1.Enabled = true;
            FIXA2.Enabled = true;
            FIXA3.Enabled = true;
            FIXA4.Enabled = true;

            this.num_dado = 20;

            DadoButton.Visible = false;



        }

        private void button2_Click(object sender, EventArgs e)
            {


            FIXA1.Visible = true;
            FIXA2.Visible = true;
            FIXA3.Visible = true;
            FIXA4.Visible = true;

            FIXA1.Enabled = true;
            FIXA2.Enabled = true;
            FIXA3.Enabled = true;
            FIXA4.Enabled = true;

            this.num_dado = 5;

            DadoButton.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FIXA1.Visible = true;
            FIXA2.Visible = true;
            FIXA3.Visible = true;
            FIXA4.Visible = true;

            FIXA1.Enabled = true;
            FIXA2.Enabled = true;
            FIXA3.Enabled = true;
            FIXA4.Enabled = true;

            this.num_dado = Convert.ToInt32(textBox1.Text);
        }
    }
}

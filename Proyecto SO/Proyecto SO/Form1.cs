using Microsoft.VisualBasic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//los colores son 100% provisionales y se pueden cambiar en cualquier momento, no prestarle demasiada atencion.
namespace Proyecto_SO
{
    public partial class Form1 : Form
    {
        Thread atender;
        bool atendiendo = false;

        Socket server;
        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }
        private void atenderServidor()
        {
            while (true)
            {
                int i;
                byte[] msg2 = new byte[80];
                try
                {
                    server.Receive(msg2);
                }
                catch
                {
                    ObjectDisposedException ex;
                }

                string mensaje;
                string[] trozos;
                int codigo = 1;
                try
                {
                    mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                    trozos = Encoding.ASCII.GetString(msg2).Split('/');
                    codigo = Convert.ToInt32(trozos[0]);
                }
                catch
                {
                    mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                    trozos = Encoding.ASCII.GetString(msg2).Split('/');
                }
                switch (codigo)
                {
                    case 1:
                        if (mensaje == "1/0")
                        {
                            MessageBox.Show("Bienvenido" + NOMBRE.Text);
                            label3.Visible = true;
                            PARAMETRO.Visible = true;
                            //botones
                            QUERY1.Visible = true;
                            QUERY2.Visible = true;
                            QUERY3.Visible = true;
                            //CONECTADOS.Visible = true;
                            dataGridView1.Visible = true;
                        }
                        if (mensaje == "1/1")
                        {
                            MessageBox.Show("Nombre o contraseña incorrectos");
                        }
                        break;
                    case 2:
                        {
                            if (mensaje == "2/0")
                            {
                                MessageBox.Show("Cuenta creada correctamente");
                                label3.Visible = true;
                                PARAMETRO.Visible = true;
                                //botones
                                QUERY1.Visible = true;
                                QUERY2.Visible = true;
                                QUERY3.Visible = true;
                                //CONECTADOS.Visible = true;
                                dataGridView1.Visible = true;
                            }
                            if (mensaje == "2/1")
                            {
                                MessageBox.Show("Esta cuenta ya existe");
                            }
                            break;
                        }
                    case 3:
                        {
                            if (mensaje != "3/1")
                            {
                                i = 2;
                                string resultado = "Los jugadores que han jugado con " + PARAMETRO.Text + " son " + mensaje.Split("/")[i];
                                i++;
                                while (i < resultado.Split("/").Length)
                                {
                                    resultado = resultado + " , " + mensaje.Split('/')[i];
                                    i++;
                                }
                                MessageBox.Show(resultado);
                            }
                            if (mensaje == "3/1")
                            {
                                MessageBox.Show("No matching data");
                            }
                            break;
                        }
                    case 4:
                        {
                            if (mensaje != "4/1")
                            {
                                i = 2;
                                string resultado = "los jugadores que han ganado con fichas de color " + PARAMETRO.Text + " son: " + mensaje.Split('/')[i];
                                i++;
                                while (i < resultado.Split("/").Length)
                                {
                                    resultado = resultado + " , " + mensaje.Split("/")[i];
                                    i++;
                                }
                                MessageBox.Show(resultado);
                            }
                            if (mensaje == "4/1")
                            {
                                MessageBox.Show("No matching data");
                            }
                            break;
                        }
                    case 5:
                        {
                            if (mensaje != "5/1")
                            {
                                i = 2;
                                string resultado = "Los jugadores que mas han ganado el dia " + PARAMETRO.Text + " son " + mensaje.Split("/")[i];
                                i++;
                                while (i < mensaje.Split('/').Length)
                                {
                                    resultado = resultado + " , " + mensaje.Split('/')[i];
                                    i++;
                                }
                                MessageBox.Show(resultado);

                            }
                            if (mensaje == "5/1")
                            {
                                MessageBox.Show("No matching data");
                            }
                            break;
                        }
                    case 6:
                        {
                            if (mensaje != "6/0/0")
                            {
                                dataGridView1.Rows.Clear();
                                int NumeroConectados = Convert.ToInt32(mensaje.Split("/")[2]);
                                if (NumeroConectados == 0)
                                {
                                    //XD

                                }
                                if (NumeroConectados != 0)
                                {
                                    i = 3;
                                    string nombre;
                                    try
                                    {
                                        nombre = mensaje.Split('/')[i];
                                    }
                                    catch
                                    {
                                        IndexOutOfRangeException ex;
                                        nombre = mensaje.Split('/')[i];//juan
                                    }


                                    while (i < NumeroConectados + 2)
                                    {
                                        dataGridView1.Rows.Add();
                                        i++;
                                    }
                                    i = 3;
                                    while (i < NumeroConectados + 3)
                                    {
                                        try
                                        {
                                            //señor profesor, por algun motivo desconocido a veces (no siempre)
                                            //aparece un 6 despues del nombre de uno de los conectados, no tenemos ni la mas minima sospecha de
                                            //porque puede pasar algo tan terrible para esta, nuestra (aunque en declive) sociedad.
                                            dataGridView1.Rows[i - 3].Cells[0].Value = nombre;
                                            i++;
                                            nombre = mensaje.Split('/')[i];


                                        }
                                        catch
                                        {
                                            IndexOutOfRangeException ex;//hace falta?
                                            break;
                                        }
                                    }
                                }
                            }

                            if (mensaje == "6/1")//esta situacion no se puede dar nunca ya que almenos siempre estara conectado el que hace la peticion.
                            {
                                MessageBox.Show("No hay nadie conectado");
                            }
                            break;
                        }
                }


            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            label4.Visible = false;
            label3.Visible = false;
            PARAMETRO.Visible = false;
            //botones
            QUERY1.Visible = false;
            QUERY2.Visible = false;
            QUERY3.Visible = false;
            //CONECTADOS.Visible = false;
            DISSCONECT.Visible = false;
            dataGridView1.ReadOnly = true;
            //dataGridView1.Visible = false;//si pongo esto luego no puedo hacerlo visible
            CONECTADOS.Visible = false;
            dataGridView1.Columns.Add("NombreConectados", "Conectados");

        }

        private void LOGIN_Click(object sender, EventArgs e)
        {


            string mensaje = "1/" + NOMBRE.Text + "/" + CONTRASEÑA.Text + "/";//no estoy seguro de si hay que poner una barra al final o no (da igual si esta o no)
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            if (atendiendo == false)
            {
                ThreadStart ts = delegate { atenderServidor(); };
                atender = new Thread(ts);
                atender.Start();
                atendiendo = true;
            }

        }

        private void SIGNIN_Click(object sender, EventArgs e)
        {
            string mensaje = "2/" + NOMBRE.Text + "/" + CONTRASEÑA.Text;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            if (atendiendo == false)
            {
                ThreadStart ts = delegate { atenderServidor(); };
                atender = new Thread(ts);
                atender.Start();
                atendiendo = true;
            }

        }

        private void QUERY1_Click(object sender, EventArgs e)
        {

            string mensaje = "3/" + PARAMETRO.Text;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);


        }

        private void QUERY2_Click(object sender, EventArgs e)//el parametro sera un valor del 1 al 5
        {

            string mensaje = "4/" + PARAMETRO.Text;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);


        }

        private void QUERY3_Click(object sender, EventArgs e)
        {

            string mensaje = "5/" + PARAMETRO.Text;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

        }

        private void DISSCONECT_Click(object sender, EventArgs e)
        {
            string mensaje = "0/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            try
            {
                atender.Abort();
            }
            catch
            {
                ThreadAbortException ex;
            }
            this.BackColor = Color.Gray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();

            this.Close();
        }

        private void CONNECT_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress direc = IPAddress.Parse("192.168.56.102");
                IPEndPoint ipep = new IPEndPoint(direc, 9050);

                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Connect(ipep);

                MessageBox.Show("Conectado");
                DISSCONECT.Visible = true;
            }
            catch (SocketException ex)
            {
                MessageBox.Show("no ha sido posible conectarse al servidor" + ex);
                return;
            }
        }

        private void CONECTADOS_Click(object sender, EventArgs e)//bro no existes ngl XD
        {
            string mensaje = "6/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //esto aun no lo usamos, quiza mas tarde tendra alguna utilidad
        }
    }
}
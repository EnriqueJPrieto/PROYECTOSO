using Microsoft.VisualBasic;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/* paleta de colores: 
 * 51, 245, 234//turquesa
 * 33, 166, 255//azul 
 * 0, 34, 230//azul oscuro
 * 87, 13, 252//morado
 * 191, 12, 242//rosa
 */
namespace Proyecto_SO
{
    public partial class Form1 : Form
    {
        Thread atender;
        bool atendiendo = false;
        public static string invitado;
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
                    case 1://log in
                        if (mensaje == "1/0")
                        {
                            label1.Visible = false;
                            label2.Visible = false;
                            label6.Text = "Bienvenido " + NOMBRE.Text;
                            label6.Visible = true;
                            label3.Visible = true;
                            PARAMETRO.Visible = true;
                            //botones
                            QUERY1.Visible = true;
                            QUERY2.Visible = true;
                            QUERY3.Visible = true;
                            //CONECTADOS.Visible = true;
                            dataGridView1.Visible = true;
                            label5.Text = NOMBRE.Text;
                            label5.Visible = true;
                            INVITAR.Visible = true;
                            NOMBRE.Visible = false;
                            CONTRASEÑA.Visible = false;
                            LOGIN.Visible = false;
                            SIGNIN.Visible = false;
                        }
                        if (mensaje == "1/1")
                        {
                            label6.Text = "Nombre o contraseña incorrectos";
                            label6.Visible= true;
                        }
                        break;
                    case 2://sign in 
                        {
                            if (mensaje == "2/0")
                            {
                                
                                label6.Text = "Cuenta creada correctamente";
                                label6.Visible = true;
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
                                label6.Text = "Este usuario ya existe";
                                label6.Visible = true;
                            }
                            break;
                        }
                    case 3://query1
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
                                label6.Text = resultado;
                                label6.Visible = true; 
                            }
                            if (mensaje == "3/1")
                            {
                                label6.Text = "No matching data";
                                label6.Visible = true;
                            }
                            break;
                        }
                    case 4://query2
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
                                label6.Text = resultado;
                                label6.Visible = true;
                            }
                            if (mensaje == "4/1")
                            {
                                label6.Text = "No matching data";
                                label6.Visible= true;
                            }
                            break;
                        }
                    case 5://query3
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
                                label6.Text = resultado;
                                label6.Visible = true;

                            }
                            if (mensaje == "5/1")
                            {
                                label6.Text = "No matching data";
                                label6.Visible = true;
                            }
                            break;
                        }
                    case 6://lista de conectados
                        {
                            if (mensaje != "6/0/0")
                            {
                                dataGridView1.Rows.Clear();
                                int NumeroConectados = Convert.ToInt32(mensaje.Split("/")[2]);
                                if (NumeroConectados == 0)
                                {
                                    //XD//el cliente no puede recibir esto nunca, asi que no pongo ningun codigo aqui :)

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
                                label6.Text = "No hay nadie conectado";
                                label6.Visible = true;
                            }
                            break;
                        }
                    case 7://crear partida
                        {
                            if (mensaje == "7/1")//esto no va a pasar nunca pero es posible so .. 
                            {
                                label6.Text = "No se ha podido crear la partida";
                                label6.Visible= true;
                            }
                            else if (mensaje.Split('/')[1] == "0")//7/0
                            {
                                string nombre;
                                nombre = mensaje.Split('/')[2];
                                    
                                
                                label4.Text = "El jugador " + nombre + " te ha invitado a una partida";
                                label4.Visible = true;
                                ACEPTAR.Visible = true;
                                RECHAZAR.Visible = true;
                            }

                            break;
                        }
                        case 8:
                        {
                            string decision = mensaje.Split("/")[1];
                            if (decision == "0")
                            {
                                label6.Text = "El jugador " + invitado + " ha aceptado";
                                label6.Visible = true;
                                label7.Visible = true;
                                label8.Visible = true;
                                label9.Visible = true;
                                label10.Visible = true;
                                //cuando el jugador acepta
                                label1.Visible = false;
                                label2.Visible = false;
                                label3.Visible = false;
                                label4.Visible = false;
                                label5.Visible = false;
                                label6.Visible = false;
                                //botones
                                LOGIN.Visible = false;
                                SIGNIN.Visible = false;
                                QUERY1.Visible = false;
                                QUERY2.Visible = false;
                                QUERY3.Visible = false;
                                DISSCONECT.Visible = false;
                                CONECTADOS.Visible = false;
                                ACEPTAR.Visible = false;
                                RECHAZAR.Visible = false;
                                NOMBRE.Visible = false;
                                CONTRASEÑA.Visible = false;
                                PARAMETRO.Visible = false;
                             
                                
                                

                                label7.Text = mensaje.Split("/")[2];
                                label8.Text = mensaje.Split("/")[3];
                                label9.Text = mensaje.Split("/")[4];
                                label10.Text = mensaje.Split("/")[5];


                            }
                            if (mensaje == "8/1")
                            {
                                label6.Text = "El jugador " + invitado + " no ha aceptado";
                                label6.Visible = true;
                            }
                            break;
                        }
                    case 9:
                        {
                            if (mensaje == "9/0")
                            {
                                label6.Text = "El jugador " + invitado + " ha recibido su invitacion";
                            }
                            if (mensaje == "9/1")
                            {
                                label6.Text = "El jugador " + invitado + " no ha recibido su invitacion";
                            }
                            break;
                        }
                }


            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Visible= false;
            label2.Visible= false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible= false;
            label6.Visible= false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            //botones
            LOGIN.Visible = false;
            SIGNIN.Visible = false;
            QUERY1.Visible = false;
            QUERY2.Visible = false;
            QUERY3.Visible = false;
            DISSCONECT.Visible = false;
            CONECTADOS.Visible = false;
            ACEPTAR.Visible = false;
            ACEPTAR.Text = "ACEPTAR";
            RECHAZAR.Visible = false;
            RECHAZAR.Text = "RECHAZAR";
            this.BackColor = Color.FromArgb(33, 166, 255);
            dataGridView1.BackgroundColor = Color.FromArgb(87, 13, 252);
            //textbox
            NOMBRE.Visible = false;
            CONTRASEÑA.Visible = false;
            PARAMETRO.Visible = false;
            //datagriedview
            dataGridView1.ReadOnly= true;
  
            dataGridView1.Visible = false;//si pongo esto luego no puedo hacerlo visible
            //CHECKBOX 
            INVITAR.Text = "Invitar";
            INVITAR.Visible = false;
            
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

        private void CONNECT_Click(object sender, EventArgs e)//----------------------------------------------------------------------------//
        {
            try
            {
                IPAddress direc = IPAddress.Parse("192.168.56.102");
                IPEndPoint ipep = new IPEndPoint(direc, 9050);

                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Connect(ipep);
                
                //MessageBox.Show("Conectado");
                DISSCONECT.Visible = true;
                CONNECT.Visible = false;
                label1.Visible = true;
                label2.Visible = true;
                NOMBRE.Visible = true;
                CONTRASEÑA.Visible = true;
                LOGIN.Visible = true;
                SIGNIN.Visible = true;
                dataGridView1.Visible = true;
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
            var dgv = sender as DataGridView;
            var check = dgv[e.ColumnIndex, e.RowIndex].Value;//detecta que celda he clickado

            invitado = check.ToString();

            if (INVITAR.Checked)
            {
                if (check != null)
                {
                    string mensaje = "7/" + NOMBRE.Text + "/" + check.ToString();
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    DataGridViewCellStyle style = new DataGridViewCellStyle();//cambia de color cuando invitas a alguien
                    style.BackColor = Color.FromArgb(191, 12, 242);
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.FromArgb(51, 245, 234);
                }              
            }

        }

        private void ACEPTAR_Click(object sender, EventArgs e)//en el servidor falta la funcion de saber si ha aceptado la inv o no.
        {
            string mensaje = "8/0";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            //ACEPTAR.Visible = false;
            RECHAZAR.Visible = false;
            label4.Visible = false;
        }

        private void RECHAZAR_Click(object sender, EventArgs e)
        {
            string mensaje = "8/1";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            ACEPTAR.Visible = false;
            RECHAZAR.Visible = false;
            label4.Visible = false;
        }
    }
}
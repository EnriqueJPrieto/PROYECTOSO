using System.Net;
using System.Net.Sockets;
using System.Text;


//los colores son 100% provisionales y se pueden cambiar en cualquier momento, no prestarle demasiada atencion.
namespace Proyecto_SO
{
    public partial class Form1 : Form
    {
        Socket server;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //
            label3.Visible = false;
            PARAMETRO.Visible = false;
            //botones
            QUERY1.Visible = false;
            QUERY2.Visible = false;
            QUERY3.Visible = false;
            CONECTADOS.Visible = false;
            DISSCONECT.Visible = false;


        }

        private void LOGIN_Click(object sender, EventArgs e)
        {


            string mensaje = "1/" + NOMBRE.Text + "/" + CONTRASEÑA.Text + "/";//no estoy seguro de si hay que poner una barra al final o no (da igual si esta o no)
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            byte[] msg2 = new byte[80];
            server.Receive(msg2);
            mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
            MessageBox.Show(mensaje);
            if (mensaje[2] == '1')
            {
                MessageBox.Show("Nombre o contraseña incorrectos");
            }
            if (mensaje[2] == '0')
            {
                MessageBox.Show("Bienvenido" + NOMBRE.Text);
            }
            label3.Visible = true;
            PARAMETRO.Visible = true;
            //botones
            QUERY1.Visible = true;
            QUERY2.Visible = true;
            QUERY3.Visible = true;
            CONECTADOS.Visible = true;
        }

        private void SIGNIN_Click(object sender, EventArgs e)
        {
            string mensaje = "2/" + NOMBRE.Text + "/" + CONTRASEÑA.Text;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            byte[] msg2 = new byte[80];
            server.Receive(msg2);
            mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
            if (mensaje[2] == '1')
            {
                MessageBox.Show("Este nombre de usuario ya existe");
            }
            if (mensaje[2] == '0')
            {
                MessageBox.Show("Cuenta creada correctamente");
            }
            label3.Visible = true;
            PARAMETRO.Visible = true;
            //botones
            QUERY1.Visible = true;
            QUERY2.Visible = true;
            QUERY3.Visible = true;
            CONECTADOS.Visible = true;
        }

        private void QUERY1_Click(object sender, EventArgs e)
        {//aqui hay que arreglar esto para que los nombres salgan de manera correcta

            string mensaje = "3/" + PARAMETRO.Text;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            byte[] msg2 = new byte[80];
            server.Receive(msg2);
            mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
            if (mensaje[0] == '1')
            {
                MessageBox.Show("No hay datos para esta busqueda");
            }
            if (mensaje[2] == '0')
            {
                int n = 3;
                string text = mensaje.Split('/')[2];//esto esta mal hecho, habria que hacer un while...//creo que asi ya esta bien

                while (text != null)
                {
                    try
                    {
                        MessageBox.Show("Los jugadores que han jugado con " + PARAMETRO.Text + " son: " + text);
                        text = mensaje.Split('/')[n];
                        n++;
                    }
                    catch
                    {
                        IndexOutOfRangeException ex;
                        break;
                    }
                }
            }
        }

        private void QUERY2_Click(object sender, EventArgs e)//el parametro sera un valor del 1 al 5
        {

            string mensaje = "4/" + PARAMETRO.Text;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            byte[] msg2 = new byte[80];
            server.Receive(msg2);
            mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
            if (mensaje[2] == '1')
            {
                MessageBox.Show("No hay datos para esta busqueda");
            }
            if (mensaje[2] == '0')
            {
                string text = mensaje.Split('/')[0];
                MessageBox.Show("Los jugadores que han ganado con fichas de color " + PARAMETRO.Text + "son: " + text);
            }

        }

        private void QUERY3_Click(object sender, EventArgs e)
        {

            string mensaje = "5/" + PARAMETRO.Text;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            byte[] msg2 = new byte[80];
            server.Receive(msg2);
            mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
            if (mensaje[2] == '1')
            {
                MessageBox.Show("No hay datos para esta busqueda");
            }
            if (mensaje[2] == '0')
            {
                string text = mensaje.Split('/')[2];
                MessageBox.Show("El jugador que mas partidas ha ganado el dia " + PARAMETRO.Text + " Es: " + text);
            }
        }


        private void DISSCONECT_Click(object sender, EventArgs e)
        {
            string mensaje = "0/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            this.BackColor = Color.Gray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }

        private void CONNECT_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress direc = IPAddress.Parse("192.168.56.102");
                IPEndPoint ipep = new IPEndPoint(direc, 9053);

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

        private void CONECTADOS_Click(object sender, EventArgs e)
        {
            string mensaje = "6/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            byte[] msg2 = new byte[80];
            server.Receive(msg2);
            mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];


            if (mensaje[2] == '1')//este resultado en realidad no es posible pero bueno
            {
                MessageBox.Show("No hemos encontrado jugadores conectados");
            }
            if (mensaje[2] == '0')//hay algun error en el display del datagreidview, hay que fixear eso
            {                       
                
                int n = 4;
                string nombre = mensaje.Split('/')[3];
                while (nombre != null)
                {
                    try
                    {
                        label4.Text = nombre;
                        dataGridView1.Rows[n-4].Cells[0].Value = nombre;
                        nombre = mensaje.Split('/')[n];
                        n++;
                        
                        
                    }
                    catch
                    {
                        IndexOutOfRangeException ex;
                        break;
                    }
                }

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //esto aun no lo usamos, quiza mas tarde tendra alguna utilidad
        }
    }
}
using Microsoft.VisualBasic;

using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;


namespace Proyecto_SO
{
    public partial class Form1 : Form
    {
        public string juegoServidor;
        public int _n;
        public int res;

        Thread atender;
        Thread atender2;

        bool atendiendo = false;
        public static string invitado;
        public int nInvitados = 1;
        public int partida;
        public string jugadores;

        Socket server;

        Form2 f2;

        public Form1()
        {
            
            InitializeComponent();
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
            label11.Visible = false;

            label11.Text = null;

            CHAT1.Visible= false;
            CHAT2.Visible= false;
            CHAT3.Visible= false;
            CHAT4.Visible= false;
            CHAT5.Visible= false;
            CHAT6.Visible= false;

            CHAT1.Text = null;
            CHAT2.Text = null;
            CHAT3.Text = null;
            CHAT4.Text = null;
            CHAT5.Text = null;
            CHAT6.Text = null;
            //botones
            BTNinvitar.Visible= false;
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
            BTNChat.Visible = false;
            //
            this.BackColor = Color.FromArgb(33, 166, 255);
            dataGridView1.BackgroundColor = Color.FromArgb(87, 13, 252);
            //textbox
            NOMBRE.Visible = false;
            CONTRASEÑA.Visible = false;
            PARAMETRO.Visible = false;
            TXTCHAT.Visible = false;
            //datagriedview
            dataGridView1.ReadOnly= true;
  
            dataGridView1.Visible = false;//si pongo esto luego no puedo hacerlo visible
            //CHECKBOX 
            INVITAR.Text = "Invitar";
            INVITAR.Visible = false;
            
            dataGridView1.Columns.Add("NombreConectados", "Conectados");

            CONNECT_Click();
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
                catch (ObjectDisposedException ex)
                {
                   string err = ex.ToString();
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
                        if (mensaje == "1/0")//esto siempre lo hace
                        {
                            this.Invoke(new Action(() =>
                            {
                                label1.Visible = false;
                                label2.Visible = false;
                                label6.Text = "Bienvenido " + NOMBRE.Text;
                                label6.Visible = true;
                                label3.Visible = true;
                                label11.Visible = true;
                                PARAMETRO.Visible = true;
                                //botones
                                QUERY1.Visible = true;
                                QUERY2.Visible = true;
                                QUERY3.Visible = true;
                                BTNinvitar.Visible = true;
                                //CONECTADOS.Visible = true;
                                dataGridView1.Visible = true;
                                label5.Text = NOMBRE.Text;
                                label5.Visible = true;
                                INVITAR.Visible = true;
                                NOMBRE.Visible = false;
                                CONTRASEÑA.Visible = false;
                                LOGIN.Visible = false;
                                SIGNIN.Visible = false;
                                label11.Visible = true;
                            }));
                        }
                        else
                        {
                            this.Invoke(new Action(() =>
                            {
                                label6.Text = "Nombre o contraseña incorrectos";
                                label6.Visible = true;
                            }));
                        }
                        break;
                    case 2://sign in 
                        {
                            if (mensaje == "2/0")
                            {
                                this.Invoke(new Action(() =>
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
                                }));
                            }
                            if (mensaje == "2/1")
                            {
                                this.Invoke(new Action(() =>
                                {
                                    label6.Text = "Este usuario ya existe";
                                    label6.Visible = true;
                                }));
                            }
                            break;
                        }
                    case 3://query1
                        {
                            if (mensaje != "3/1")
                            {
                                i = 2;
                                string resultado = "Los jugadores que han jugado con " + PARAMETRO.Text + " son " + trozos[i];
                                i++;
                                while (i < resultado.Split("/").Length)
                                {
                                    resultado = resultado + " , " + trozos[i];
                                    i++;
                                }
                                this.Invoke(new Action(() =>
                                {
                                    label6.Text = resultado;
                                    label6.Visible = true;
                                }));
                            }
                            if (mensaje == "3/1")
                            {
                                this.Invoke(new Action(() =>
                                {
                                    label6.Text = "No matching data";
                                    label6.Visible = true;
                                }));
                            }
                            break;
                        }
                    case 4://query2
                        {
                            if (mensaje != "4/1")
                            {
                                i = 2;
                                string resultado = "los jugadores que han ganado con fichas de color " + PARAMETRO.Text + " son: " + trozos[i];
                                i++;
                                while (i < resultado.Split("/").Length)
                                {
                                    resultado = resultado + " , " + trozos[i];
                                    i++;
                                }
                                this.Invoke(new Action(() =>
                                {
                                    label6.Text = resultado;
                                    label6.Visible = true;
                                }));
                            }
                            if (mensaje == "4/1")
                            {
                                this.Invoke(new Action(() =>
                                {
                                    label6.Text = "No matching data";
                                    label6.Visible = true;
                                }));   
                            }
                            break;
                        }
                    case 5://query3
                        {
                            if (mensaje != "5/1")
                            {
                                i = 2;
                                string resultado = "Los jugadores que mas han ganado el dia " + PARAMETRO.Text + " son " + trozos[i];
                                i++;
                                while (i < mensaje.Split('/').Length)
                                {
                                    resultado = resultado + " , " + trozos[i];
                                    i++;
                                }
                                this.Invoke(new Action(() =>
                                {
                                    label6.Text = resultado;
                                    label6.Visible = true;
                                }));
                            }
                            if (mensaje == "5/1")
                            {
                                this.Invoke(new Action(() =>
                                {   
                                    label6.Text = "No matching data";
                                    label6.Visible = true;

                                }));     
                            }
                            break;
                        }
                    case 6://lista de conectados
                        {
                            if (mensaje != "6/0/0")
                            {
                                this.Invoke(new Action(() =>
                                {
                                    dataGridView1.Rows.Clear();

                                }));
                                int NumeroConectados = Convert.ToInt32(trozos[2]);
                                if (NumeroConectados == 0)//esto solo es posible si no hay ningun cliente conectado, lo cual es posible pero como no hay nadie conectado nos da igual, ya que nunca se usara
                                {
                                    //el cliente no puede recibir esto nunca, asi que no pongo ningun codigo aqui :)                                  
                                }
                                if (NumeroConectados != 0)
                                {
                                    i = 3;
                                    string nombre;
                                    try
                                    {
                                        nombre = trozos[i];
                                    }
                                    catch (IndexOutOfRangeException ex)
                                    {
                                        string err = ex.Message;
                                        nombre = trozos[i];
                                    }
                                    while (i < NumeroConectados + 2)
                                    {
                                        this.Invoke(new Action(() =>
                                        {
                                            dataGridView1.Rows.Add();
                                        }));
                                        
                                        i++;
                                    }
                                    i = 3;
                                    while (i < NumeroConectados + 3)
                                    {
                                        try
                                        {
                                            this.Invoke(new Action(() =>
                                            {
                                                dataGridView1.Rows[i - 3].Cells[0].Value = nombre;
                                            }));
                                            i++;
                                            nombre = trozos[i];
                                        }
                                        catch (IndexOutOfRangeException ex)
                                        {
                                           string err = ex.Message;
                                           break;
                                        }
                                    }
                                }
                            }
                            if (mensaje == "6/1")//esta situacion no se puede dar nunca ya que almenos siempre estara conectado el que hace la peticion.
                            {
                                this.Invoke(new Action(() =>
                                {
                                    label6.Text = "No hay nadie conectado";
                                    label6.Visible = true;
                                }));
                            }
                            break;
                        }
                    case 7://crear partida
                        {
                            if (mensaje == "7/1")//esto no va a pasar nunca pero es posible so .. 
                            {
                                this.Invoke(new Action(() =>
                                {
                                    label6.Text = "No se ha podido crear la partida";
                                    label6.Visible= true;
                                }));
                            }
                            else if (trozos[1] == "0")//7/0
                            {
                                string peticion;
                                int partida;
                                string nombre;
                                partida = Convert.ToInt32(trozos[2]);//7/0/nombre
                                nombre = trozos[3];
                                if (MessageBox.Show(NOMBRE.Text + " Has sido invitado a jugar por: " + nombre, "Invitacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    peticion = "8/" + partida + "/0";
                                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(peticion);
                                    server.Send(msg);
                                }
                                else
                                {
                                    peticion = "8/1";
                                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(peticion);
                                    server.Send(msg);
                                }
                            }
                            break;
                        }
                    case 8://aceptar/rechazar
                        {
                            string decision = trozos[1];
                            partida = Convert.ToInt32(trozos[2]);
                            if (decision == "0")
                            {
                                this.Invoke(new Action(() =>
                                {
                                    label6.Text = "El jugador " + invitado + " ha aceptado";
                                    label6.Visible = true;
                                    label7.Visible = true;
                                    label8.Visible = true;
                                    label9.Visible = true;
                                    label10.Visible = true;
                                    label7.Text = null;
                                    label8.Text = null;
                                    label9.Text = null;
                                    label10.Text = null;

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
                                    try
                                    {
                                        label7.Text = "J1 " + trozos[3];
                                        label8.Text = "J2 " + trozos[4];
                                        label9.Text = "J3 " + trozos[5];
                                        label10.Text = "J4 " + trozos[6];
                                        jugadores = "J1/" + trozos[3] + "/J2/" + trozos[4] + "/J3/" + trozos[5] +"/J4/" + trozos[6] + "/";

                                    }
                                    catch (IndexOutOfRangeException ex)
                                    {
                                        string err = ex.Message;
                                    }
                                        TXTCHAT.Visible = true;
                                        BTNChat.Visible = true;
                                }));
                            }
                            if (mensaje == "8/1")
                            {
                                this.Invoke(new Action(() =>
                                {
                                    label6.Text = "El jugador " + invitado + " no ha aceptado";
                                    label6.Visible = true;
                                }));
                               
                            }
                            break;
                        }
                    case 9://chat
                        {
                            string chat = trozos[1];//9/mensaje
                            this.Invoke(new Action(() =>
                            {
                                CHAT6.Text = CHAT5.Text;
                                CHAT5.Text = CHAT4.Text;
                                CHAT4.Text = CHAT3.Text;
                                CHAT3.Text = CHAT2.Text;
                                CHAT2.Text = CHAT1.Text;
                                CHAT1.Text = chat;

                                if (CHAT1.Text != null)
                                {
                                    CHAT1.Visible = true;
                                }
                                if (CHAT2.Text != null)
                                {
                                    CHAT2.Visible = true;
                                }
                                if (CHAT3.Text != null)
                                {
                                    CHAT3.Visible = true;
                                }
                                if (CHAT4.Text != null)
                                {
                                    CHAT4.Visible = true;
                                }
                                if (CHAT5.Text != null)
                                {
                                    CHAT5.Visible = true;
                                }
                                if (CHAT6.Text != null)
                                {
                                    CHAT6.Visible = true;
                                }
                            }));
                            break;
                        }
                    case 10:
                        {//10/0

                            f2 = new Form2(this);
                            string[] jugador = jugadores.Split("/");
                            
                            if (NOMBRE.Text == jugador[1]) 
                            {
                                f2.teamColor = 0;
                            }
                            if (NOMBRE.Text == jugador[3]) 
                            {
                                f2.teamColor = 1;
                            }
                            if (NOMBRE.Text == jugador[5]) 
                            {
                                f2.teamColor = 2;
                            }
                            if (NOMBRE.Text == "Maria")
                            {
                                f2.teamColor = 3;
                            }

                            ThreadStart ts2 = delegate { f2.ShowDialog(); };
                            atender2 = new Thread(ts2);
                            atender2.Start();

                            break;
                        }
                    case 11:
                        {//11/nºfixa/nºposicion/bool casa/n/dado/comido

                            if (trozos[1] == "99")
                            {
                                if (_n == 3)
                                {
                                    _n = 0;
                                }
                                else
                                {
                                    _n++;
                                }
                                
                                f2.Servidor(16, 0, 0, false,  res);

                                if (_n == f2.teamColor)
                                {
                                    f2.setToken(true);
                                    f2.DadoVisible(true);
                                }
                                else
                                {
                                    f2.setToken(false);
                                    f2.DadoVisible(false);
                                }
                            }
                            else
                            {
                                int equipo = Convert.ToInt32(trozos[1]);
                                int ficha = Convert.ToInt32(trozos[2]);
                                int posicion = Convert.ToInt32(trozos[3]);
                                bool casa = Convert.ToBoolean(trozos[4]);
                                _n = Convert.ToInt32(trozos[5]);
                                res = Convert.ToInt32(trozos[6]);
                                
                                if (_n == f2.teamColor)
                                {
                                    f2.setToken(true);
                                    f2.DadoVisible(true);
                                }
                                else
                                {
                                    f2.setToken(false);
                                    f2.DadoVisible(false);
                                }
                                f2.Servidor(ficha, equipo, posicion, casa, res);
                            }
                            break;
                        }
                    case 12:
                        {
                            MessageBox.Show("Cuenta Eliminada correctamente");
                            this.Invoke(new Action(()=>
                            {
                                this.Close();
                            }
                            ));
                            break;
                        }
                }
            }
        }
        private void LOGIN_Click(object sender, EventArgs e)
        {


            string mensaje = "1/" + NOMBRE.Text + "/" + CONTRASEÑA.Text + "/";
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
        private void QUERY2_Click(object sender, EventArgs e)
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
            catch (ThreadAbortException ex)
            {
               string err = ex.Message;
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
                IPAddress direc = IPAddress.Parse("147.82.117.22");//192.168.56.102
                IPEndPoint ipep = new IPEndPoint(direc, 50080);

                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Connect(ipep);
                
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

        private void CONNECT_Click()//----------------------------------------------------------------------------//
        {
            try
            {
                IPAddress direc = IPAddress.Parse("147.83.117.22");
                IPEndPoint ipep = new IPEndPoint(direc, 50080);

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
            string[] invitados = new string[4];
             if (INVITAR.Checked)
            {
                if (check != null)
                {
                    if (nInvitados > 3)
                    {
                        MessageBox.Show("SOLO SE PUEDE INVITAR A 3 PERSONAS");
                    }
                    if (nInvitados == 1)
                    {
                        invitados[0] = label5.Text;
                        invitados[1] = check.ToString();
                        invitado = invitados[0] + "-" + invitados[1];
                        nInvitados++;
                    }
                    else if (nInvitados == 2)
                    {
                        invitados[2] = check.ToString();
                        invitado = invitado + "-" + invitados[2];
                        nInvitados++;
                    }
                    else if (nInvitados == 3)
                    {
                        invitados[3] = check.ToString();
                        invitado = invitado + "-" + invitados[3];
                        nInvitados++;
                    }
                    label11.Text = invitado + "-" + nInvitados;//esto se construye correctamente

                    DataGridViewCellStyle style = new DataGridViewCellStyle();//cambia de color cuando invitas a alguien

                    style.BackColor = Color.FromArgb(191, 12, 242);
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.FromArgb(51, 245, 234);

                    
                }              
            }

        }
    
        private void BTNChat_Click(object sender, EventArgs e)
        {
            string mensaje = "9/" + partida + "/" + TXTCHAT.Text;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            TXTCHAT.Text = null;
        }

        private void BTNinvitar_Click(object sender, EventArgs e)
        {
            string mensaje = "7/" + invitado;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        private void BTNSTART_Click(object sender, EventArgs e)
        {
            string mensaje = "10/" + partida;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        public void jugada()
        {
            juegoServidor = f2.enviar;
            int ND = f2.num_dado;
            string mensaje = "11/" + partida + "/" + juegoServidor.ToString() + "/" + _n + "/" + ND;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.NOMBRE.Text = "Juan";
            this.CONTRASEÑA.Text = "password1";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.NOMBRE.Text = "Ana";
            this.CONTRASEÑA.Text = "password2";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.NOMBRE.Text = "Pedro";
            this.CONTRASEÑA.Text = "password3";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.NOMBRE.Text = "Maria";
            this.CONTRASEÑA.Text = "password4";
        }

        private void DARSEDEBAJA_Click(object sender, EventArgs e)
        {
            string mensaje = "12/" + NOMBRE.Text;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }
    }
}
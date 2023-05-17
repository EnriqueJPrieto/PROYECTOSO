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
/*NOMBRES/PASS
 * Juan/password1
 * Ana/password2
 * Pedro/password3
 * Maria/password4
 */
namespace Proyecto_SO
{
    public partial class Form1 : Form
    {
        Thread atender;
        bool atendiendo = false;
        public static string invitado;
        public int nInvitados = 1;
        public int partida;
        Socket server;
        public Form1()
        {
            
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
                MessageBox.Show(mensaje);
                switch (codigo)
                {
                    case 1://log in
                        if (mensaje == "1/0")
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

                                }));
                                label6.Text = resultado;
                                label6.Visible = true; 
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
                                    catch
                                    {
                                        IndexOutOfRangeException ex;
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
                                        catch
                                        {
                                            IndexOutOfRangeException ex;
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

                                if (MessageBox.Show(label5.Text + " Has sido invitado a jugar por: " + nombre, "Invitacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                                        label7.Text = "J1 " +trozos[3];
                                        label8.Text = "J2 " + trozos[4];
                                        label9.Text = "J3 " + trozos[5];
                                        label10.Text = "J4 " + trozos[6];
                                    }
                                    catch (IndexOutOfRangeException ex)
                                    {
                                        //XDD I DO NOT GIVE HALF A FLYING FUCK
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
                            //AQUI CUANDO RECIVE UN MENSAJE, ESCRIBE EL TEXTO EN "CHAT1", EL RESTO LO VA SUBIENDO HACIA ARRIBA AND FUCK IT CUANDO LLEGA AL CHAT6
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
                IPEndPoint ipep = new IPEndPoint(direc, 9059);

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
                    //invitado = invitados[0] + "-" + invitados[1] + "-" + invitados[2] + "-" + invitados[3];

                    label11.Text = invitado + "-" + nInvitados;//esto se construye correctamente

                    DataGridViewCellStyle style = new DataGridViewCellStyle();//cambia de color cuando invitas a alguien
                    style.BackColor = Color.FromArgb(191, 12, 242);
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.FromArgb(51, 245, 234);

                    
                }              
            }

        }

        private void ACEPTAR_Click(object sender, EventArgs e)//este codigo (de momento) es inaccesible(porque los botones no aparecen)
        {
            string mensaje = "8/0";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            
            RECHAZAR.Visible = false;
            label4.Visible = false;
        }

        private void RECHAZAR_Click(object sender, EventArgs e)//este codigo (de momento) es inaccesible(porque los botones no aparecen)
        {
            string mensaje = "8/1";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            ACEPTAR.Visible = false;
            RECHAZAR.Visible = false;
            label4.Visible = false;
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
    }
}
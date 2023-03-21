using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

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
            //hola
        }

        private void LOGIN_Click(object sender, EventArgs e)
        {
            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9050);

            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);
                this.BackColor = Color.Green;
                MessageBox.Show("Conectado");
                
                    string mensaje = "1/" + NOMBRE.Text + "/" + CONTRASEÑA.Text;//no estoy seguro de si hay que poner una barra al final o no
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    byte[] msg2 = new byte[80];
                    server.Receive(msg2);
                    mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                    if (mensaje[0] == '1')
                {
                    MessageBox.Show("Nombre o contraseña incorrectos");
                }
                    if (mensaje[0] == '0')
                {
                    MessageBox.Show("Bienvenido" + NOMBRE.Text);
                }
                this.BackColor = Color.Gray;
                server.Shutdown(SocketShutdown.Both);
                server.Close();
            }
            catch (SocketException ex)
            {
                MessageBox.Show("no ha sido posible conectarse al servidor");
                return;
            }
        }

        private void SIGNIN_Click(object sender, EventArgs e)
        {
            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9050);

            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);
                this.BackColor = Color.Green;
                MessageBox.Show("Conectado");

                string mensaje = "2/" + NOMBRE.Text + "/" + CONTRASEÑA.Text ;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                if (mensaje[0] == '1')
                {
                    MessageBox.Show("Este nombre de usuario ya existe");
                }
                if (mensaje[0] == '0')
                {
                    MessageBox.Show("Cuenta creada correctamente");
                }
                this.BackColor = Color.Gray;
                server.Shutdown(SocketShutdown.Both);
                server.Close();
            }
            catch (SocketException ex)
            {
                MessageBox.Show("no ha sido posible conectarse al servidor");
                return;
            }
        }

        private void QUERY1_Click(object sender, EventArgs e)
        {
            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9050);

            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);
                this.BackColor = Color.Green;
                MessageBox.Show("Conectado");

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
                if (mensaje[0] == '0')
                {
                    string text = mensaje.Split('/')[0];
                    MessageBox.Show("Los jugadores que han jugado con " + PARAMETRO.Text + "son: "+ text);
                }
                this.BackColor = Color.Gray;
                server.Shutdown(SocketShutdown.Both);
                server.Close();
            }
            catch (SocketException ex)
            {
                MessageBox.Show("no ha sido posible conectarse al servidor");
                return;
            }
        }

        private void QUERY2_Click(object sender, EventArgs e)//el parametro sera un valor del 1 al 5
        {
            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9050);

            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);
                this.BackColor = Color.Green;
                MessageBox.Show("Conectado");

                string mensaje = "4/" + PARAMETRO.Text;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                if (mensaje[0] == '1')
                {
                    MessageBox.Show("No hay datos para esta busqueda");
                }
                if (mensaje[0] == '0')
                {
                    string text = mensaje.Split('/')[0];
                    MessageBox.Show("Los jugadores que han ganado con fichas de color " + PARAMETRO.Text + "son: " + text);
                }
                this.BackColor = Color.Gray;
                server.Shutdown(SocketShutdown.Both);
                server.Close();
            }
            catch (SocketException ex)
            {
                MessageBox.Show("no ha sido posible conectarse al servidor");
                return;
            }
        }

        private void QUERY3_Click(object sender, EventArgs e)
        {
            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9050);

            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);
                this.BackColor = Color.Green;
                MessageBox.Show("Conectado");

                string mensaje = "4/" + PARAMETRO.Text;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                if (mensaje[0] == '1')
                {
                    MessageBox.Show("No hay datos para esta busqueda");
                }
                if (mensaje[0] == '0')
                {
                    string text = mensaje.Split('/')[0];
                    MessageBox.Show("El jugador que mas partidas ha ganado el dia " + PARAMETRO.Text + "Es: " + text);
                }
                this.BackColor = Color.Gray;
                server.Shutdown(SocketShutdown.Both);
                server.Close();
            }
            catch (SocketException ex)
            {
                MessageBox.Show("no ha sido posible conectarse al servidor");
                return;
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
    }
}
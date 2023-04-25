namespace Proyecto_SO
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LOGIN = new System.Windows.Forms.Button();
            this.SIGNIN = new System.Windows.Forms.Button();
            this.QUERY1 = new System.Windows.Forms.Button();
            this.QUERY2 = new System.Windows.Forms.Button();
            this.QUERY3 = new System.Windows.Forms.Button();
            this.DISSCONECT = new System.Windows.Forms.Button();
            this.NOMBRE = new System.Windows.Forms.TextBox();
            this.CONTRASEÑA = new System.Windows.Forms.TextBox();
            this.PARAMETRO = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
<<<<<<< HEAD
            this.CONNECT = new System.Windows.Forms.Button();
            this.CONECTADOS = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
=======
>>>>>>> 7e8db6dcc7bbda568b05b0ae6c446ac57b06734d
            this.SuspendLayout();
            // 
            // LOGIN
            // 
            this.LOGIN.Location = new System.Drawing.Point(369, 63);
            this.LOGIN.Name = "LOGIN";
            this.LOGIN.Size = new System.Drawing.Size(77, 23);
            this.LOGIN.TabIndex = 0;
            this.LOGIN.Text = "LOG IN";
            this.LOGIN.UseVisualStyleBackColor = true;
            this.LOGIN.Click += new System.EventHandler(this.LOGIN_Click);
            // 
            // SIGNIN
            // 
            this.SIGNIN.Location = new System.Drawing.Point(369, 92);
            this.SIGNIN.Name = "SIGNIN";
            this.SIGNIN.Size = new System.Drawing.Size(77, 23);
            this.SIGNIN.TabIndex = 1;
            this.SIGNIN.Text = "SIGN IN ";
            this.SIGNIN.UseVisualStyleBackColor = true;
            this.SIGNIN.Click += new System.EventHandler(this.SIGNIN_Click);
            // 
            // QUERY1
            // 
            this.QUERY1.Location = new System.Drawing.Point(369, 121);
            this.QUERY1.Name = "QUERY1";
            this.QUERY1.Size = new System.Drawing.Size(77, 23);
            this.QUERY1.TabIndex = 2;
            this.QUERY1.Text = "QUERY 1";
            this.QUERY1.UseVisualStyleBackColor = true;
            this.QUERY1.Click += new System.EventHandler(this.QUERY1_Click);
            // 
            // QUERY2
            // 
            this.QUERY2.Location = new System.Drawing.Point(369, 150);
            this.QUERY2.Name = "QUERY2";
            this.QUERY2.Size = new System.Drawing.Size(77, 23);
            this.QUERY2.TabIndex = 3;
            this.QUERY2.Text = "QUERY 2";
            this.QUERY2.UseVisualStyleBackColor = true;
            this.QUERY2.Click += new System.EventHandler(this.QUERY2_Click);
            // 
            // QUERY3
            // 
            this.QUERY3.Location = new System.Drawing.Point(369, 179);
            this.QUERY3.Name = "QUERY3";
            this.QUERY3.Size = new System.Drawing.Size(77, 23);
            this.QUERY3.TabIndex = 4;
            this.QUERY3.Text = "QUERY 3";
            this.QUERY3.UseVisualStyleBackColor = true;
            this.QUERY3.Click += new System.EventHandler(this.QUERY3_Click);
            // 
            // DISSCONECT
            // 
            this.DISSCONECT.Location = new System.Drawing.Point(369, 208);
            this.DISSCONECT.Name = "DISSCONECT";
            this.DISSCONECT.Size = new System.Drawing.Size(77, 23);
            this.DISSCONECT.TabIndex = 5;
            this.DISSCONECT.Text = "DESCONECTAR";
            this.DISSCONECT.UseVisualStyleBackColor = true;
            this.DISSCONECT.Click += new System.EventHandler(this.DISSCONECT_Click);
            // 
            // NOMBRE
            // 
            this.NOMBRE.Location = new System.Drawing.Point(144, 63);
            this.NOMBRE.Name = "NOMBRE";
            this.NOMBRE.Size = new System.Drawing.Size(100, 23);
            this.NOMBRE.TabIndex = 6;
            // 
            // CONTRASEÑA
            // 
            this.CONTRASEÑA.Location = new System.Drawing.Point(144, 92);
            this.CONTRASEÑA.Name = "CONTRASEÑA";
            this.CONTRASEÑA.Size = new System.Drawing.Size(100, 23);
            this.CONTRASEÑA.TabIndex = 7;
            // 
            // PARAMETRO
            // 
            this.PARAMETRO.Location = new System.Drawing.Point(144, 122);
            this.PARAMETRO.Name = "PARAMETRO";
            this.PARAMETRO.Size = new System.Drawing.Size(100, 23);
            this.PARAMETRO.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "NOMBRE";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 15);
            this.label2.TabIndex = 10;
            this.label2.Text = "CONTRASEÑA";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 15);
            this.label3.TabIndex = 11;
            this.label3.Text = "PARAMETRO";
            // 
<<<<<<< HEAD
            // CONNECT
            // 
            this.CONNECT.Location = new System.Drawing.Point(369, 34);
            this.CONNECT.Name = "CONNECT";
            this.CONNECT.Size = new System.Drawing.Size(75, 23);
            this.CONNECT.TabIndex = 12;
            this.CONNECT.Text = "CONECTAR";
            this.CONNECT.UseVisualStyleBackColor = true;
            this.CONNECT.Click += new System.EventHandler(this.CONNECT_Click);
            // 
            // CONECTADOS
            // 
            this.CONECTADOS.Location = new System.Drawing.Point(369, 237);
            this.CONECTADOS.Name = "CONECTADOS";
            this.CONECTADOS.Size = new System.Drawing.Size(75, 23);
            this.CONECTADOS.TabIndex = 13;
            this.CONECTADOS.Text = "CONECTADOS";
            this.CONECTADOS.UseVisualStyleBackColor = true;
            this.CONECTADOS.Click += new System.EventHandler(this.CONECTADOS_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Highlight;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(644, -1);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.Size = new System.Drawing.Size(154, 449);
            this.dataGridView1.TabIndex = 14;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 187);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 15);
            this.label4.TabIndex = 15;
            this.label4.Text = "label4";
            // 
=======
>>>>>>> 7e8db6dcc7bbda568b05b0ae6c446ac57b06734d
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
<<<<<<< HEAD
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.CONECTADOS);
            this.Controls.Add(this.CONNECT);
=======
            this.ClientSize = new System.Drawing.Size(800, 450);
>>>>>>> 7e8db6dcc7bbda568b05b0ae6c446ac57b06734d
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PARAMETRO);
            this.Controls.Add(this.CONTRASEÑA);
            this.Controls.Add(this.NOMBRE);
            this.Controls.Add(this.DISSCONECT);
            this.Controls.Add(this.QUERY3);
            this.Controls.Add(this.QUERY2);
            this.Controls.Add(this.QUERY1);
            this.Controls.Add(this.SIGNIN);
            this.Controls.Add(this.LOGIN);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
<<<<<<< HEAD
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
=======
>>>>>>> 7e8db6dcc7bbda568b05b0ae6c446ac57b06734d
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button LOGIN;
        private Button SIGNIN;
        private Button QUERY1;
        private Button QUERY2;
        private Button QUERY3;
        private Button DISSCONECT;
        private TextBox NOMBRE;
        private TextBox CONTRASEÑA;
        private TextBox PARAMETRO;
        private Label label1;
        private Label label2;
        private Label label3;
<<<<<<< HEAD
        private Button CONNECT;
        private Button CONECTADOS;
        private DataGridView dataGridView1;
        private Label label4;
=======
>>>>>>> 7e8db6dcc7bbda568b05b0ae6c446ac57b06734d
    }
}
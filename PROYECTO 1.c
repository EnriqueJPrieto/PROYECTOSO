#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <mysql.h>

//esta es la version 1 del proyecto de SO.
int main(int argc, char *argv[])
{
	int sock_conn, sock_listen, ret;
	struct sockaddr_in serv_adr;
	char peticion[512];
	char respuesta[512];
	
	//abrimos el sockets
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
		printf("Error al crear el socket");
	
	memset(&serv_adr, 0, sizeof(serv_adr));
	serv_adr.sin_family = AF_INET;
	
	serv_adr.sin_port = htons(9050);
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf("Error al bind");
	
	if (listen(sock_listen, 3) < 0)
		printf("Error en el Listen");
	for (;;)//bucle infinito
	{
		printf("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf("He recibido conexi￳n\n");
		
		int terminar = 0;
		while (terminar == 0)
		{
			ret=read(sock_conn, peticion, sizeof(peticion));
			printf("Recibido\n");
			
			peticion[ret] = '\0';
			
			printf("Peticion: %s\n" , peticion);
			
			char *p = strtok(peticion, "/");
			int codigo = atoi(p);
			
			p = strtok(NULL, "/");
			char dato[20];
			strcpy(dato, p);
			if (codigo == 0)
			{
				terminar = 1;
			}
			else if (codigo == 1) //log in 
			{//el cliente enviara el mensaje de forma: 1/username/password
				char nombre[20];
				char contrasena[20];
				p=strtok(peticion, "/");//no estoy seguro
				strcpy(nombre, p);
				p=strtok(NULL, "/");
				strcpy(contrasena, p);
				LogIn(nombre, contrasena, respuesta);
				printf("%s", respuesta);
				//enviar respuesta
				write(sock_conn,respuesta,strlen(respuesta));
				close(sock_conn);
			}
			else if (codigo == 2) //sign in
			{//2/user/pass
				char nombre[20];
				char contrasena[20];
				p=strtok(peticion, "/");//no estoy seguro
				strcpy(nombre, p);
				p=strtok(NULL, "/");
				strcpy(contrasena, p);
				SignIn(nombre, contrasena, respuesta);
				
				//enviar respuesta
				write(sock_conn,respuesta,strlen(respuesta));
				close(sock_conn);
			}
			else if (codigo == 3) //query 1
			{//3/nombre
				char nombre[20];
				p = strtok(peticion, "/");
				strcpy(nombre, p);
				JugadoresQueJueganCon(nombre, respuesta);
				
				//enviar respuesta
				write(sock_conn,respuesta,strlen(respuesta));
				close(sock_conn);
			}
			else if (codigo == 4) //query 2
			{//4/color
				char color[20];
				p = strtok(peticion, "/");
				strcpy(color, p);
				GanadoresConFichasDeColor(color, respuesta);
				
				//enviar respuesta
				write(sock_conn,respuesta,strlen(respuesta));
				close(sock_conn);
			}
			else if (codigo == 5) //query 3
			{//5/fecha
				char fecha[11];
				p = strtok(peticion, "/");
				strcpy(fecha, p);
				JugadoresMasVictorias(fecha, respuesta);
				
				//enviar respuesta
				write(sock_conn,respuesta,strlen(respuesta));
				close(sock_conn);
			}
			close (sock_conn);
		}
	}
	
}

void LogIn(char nombre[20], char contrasena[20], char respuesta[512])
{
	MYSQL *conn;
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char UserName[20];
	char Consulta[512];
	conn = mysql_init(NULL);
	if (conn==NULL) 
	{
		printf ("Error al crear la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	conn = mysql_real_connect (conn, "localhost","root", "mysql", NULL, 0, NULL, 0);
	if (conn==NULL)
	{
		printf ("Error al inicializar la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	err=mysql_query(conn, "use Juego;");//"use database"
	if (err!=0)
	{
		printf ("Error al acceder a la base de datos %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	strcpy(Consulta, "SELECT Jugador.ID FROM Jugador Where Jugador.Nombre = '");
	strcat(Consulta, nombre);
	strcat(Consulta, "'AND contrase￱a = ');");
	strcat(Consulta, contrasena);
	strcat(Consulta, "');");
	err=mysql_query (conn, Consulta); 
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL)
	{
		printf ("No se han obtenido datos en la consulta\n");
		sprintf(respuesta, "1/1");// 1-Error puede cambiarse por cualquier otra cosa, pero que el cliente lo entienda
	}
	else
	{
		printf ("Acceso garantizado al usuario con id: %s\n", row[0]);
		sprintf(respuesta, "1/0");
	}
}
void SignIn(char Nombre[20], char contrasena[20], char respuesta[512])
{
	MYSQL *conn;
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char UserName[20];
	char consulta[512];
	char NNombre[20];//New Name
	int NID;		//New ID
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	conn = mysql_real_connect (conn, "localhost","root", "mysql", NULL, 0, NULL, 0);
	if (conn==NULL)
	{
		printf ("Error al inicializar la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	err=mysql_query(conn, "use Juego;");//"use database"
	if (err!=0)
	{
		printf ("Error al acceder a la base de datos %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//que el nombre no exista
	//ponerle una id mayor a la maxima
	//contraseￃﾃￂﾱa = any
	strcpy(consulta, "SELECT DISTINCT Jugador.Nombre FROM Jugador WHERE Jugador.Nombre = '");
	strcat(consulta, Nombre);
	strcat(consulta, "');");
	err=mysql_query (conn, consulta);
	err=mysql_query(conn, "use Juego;");//"use database"
	if (err!=0)
	{
		printf ("Error al acceder a la base de datos %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}

	
	if (err = 0)
		printf("2/1");//nombre ocupado
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL)
	{
		printf ("No se han obtenido datos en la consulta\n");
		sprintf(respuesta, "2/1");// envia 3/1 si hay error //no se ha podido crear la cuenta por algun motivo 
	}
	else
	{
		printf("Cuenta creada correctamente");
		sprintf(respuesta, "2/0");
	}
	strcpy(consulta, "");
	strcpy(consulta, "SELECT MAX(Jugador.id) FROM Jugador");
	err=mysql_query (conn, consulta);
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	NID = row[0] + 1;
	strcpy(consulta, "");
	strcpy(consulta, "INSERT INTO Jugador VALUES ( ");
	strcat(consulta, NID);
	strcat(consulta, ", '");
	strcat(consulta, NNombre);
	strcat(consulta, "','");
	strcat(consulta, contrasena);
	strcat(consulta, "');");
	
}

void JugadoresQueJueganCon(char nombre[20], char respuesta[512])//con el numbre de un jugador te devuelve los que han jugado con el.
{
	MYSQL *conn;
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	
	char nombres[200];
	char consulta[500];
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	conn = mysql_real_connect (conn, "localhost","root", "mysql", NULL, 0, NULL, 0);
	if (conn==NULL)
	{
		printf ("Error al inicializar la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	err=mysql_query(conn, "use Juego;");//"use database"
	if (err!=0)
	{
		printf ("Error al acceder a la base de datos %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}//esta query estaba mal en la entrega de la semana pasada(sesion 3), ahora deberia estar bien
	strcpy (consulta, "SELECT DISTINCT Jugador.Nombre FROM (Jugador, Partida, Participaci￳n) WHERE Partida.id in (SELECT Partida.id From (Jugador, Partida, participaci￳n) WHERE Jugador.Nombre = '");
	strcat (consulta, nombre);
	strcat (consulta, "AND Jugador.ID = Participaci￳n.ID_J AND Participaci￳n.ID_P = Partida.ID) AND Partida.ID = Participaci￳n.ID_P AND Participaci￳n.ID_J = Jugador.ID AND Jugador.Nombre NOT IN('");
	strcat (consulta, nombre);
	strcat (consulta, "');");
	err=mysql_query (conn, consulta); 
	if (err!=0)
	{
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL)
	{
		printf ("No se han obtenido datos en la consulta\n");
		sprintf(respuesta, "3/1");// envia 3/1 si hay error 
	}
	else
	{
		strcpy(nombres, row[0]);
		row = mysql_fetch_row (resultado);
		while(row!=NULL)
		{
			strcat (nombres, "/");
			strcat (nombres, row[0]);
			row = mysql_fetch_row (resultado);
		}
		strcat (nombres, "\n");
		printf("%s", nombres);
		sprintf(respuesta, "3/0/%s", nombres);// le envia al cliente los nombres, el cliente tiene que saber que hacer con ellos.
	}
}

void GanadoresConFichasDeColor(char color[20], char respuesta[512])//con el color de una ficha te dice que jugadores han ganado usandola
{
	MYSQL *conn;
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	
	char nombres[200];
	char consulta[500];
	
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	conn = mysql_real_connect (conn, "localhost","root", "mysql", NULL, 0, NULL, 0);
	if (conn==NULL)
	{
		printf ("Error al inicializar la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	err=mysql_query(conn, "use Juego;");//"use database"
	if (err!=0)
	{
		printf ("Error al acceder a la base de datos %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	strcpy (consulta,"SELECT Partida.ganador FROM Partida,Participaci￳n WHERE Participaci￳n.Color = '");
	strcat (consulta, color);
	strcat (consulta,"'AND Partida.ID_P = Participaci￳n.Partida");
	err=mysql_query (conn, consulta);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL)
	{
		printf ("No se han obtenido datos en la consulta\n");
		sprintf(respuesta, "4/1");// 1-Error puede cambiarse por cualquier otra cosa, pero que el cliente lo entienda
	}
	else
	{
		strcpy(nombres, row[0]);
		row = mysql_fetch_row (resultado);
		while(row!=NULL)
		{
			strcat (nombres, "/");
			strcat (nombres, row[0]);
			row = mysql_fetch_row (resultado);
		}
		strcat (nombres, "\n");
		printf("%s", nombres);
		sprintf(respuesta, "4/0/%s", nombres);// le envia al cliente los nombres, el cliente tiene que saber que hacer con ellos.
	}
}
void JugadoresMasVictorias(char fecha[20],char respuesta[512])//con una fecha, te dice que jugador ha ganado mas partidas
{//no estoy seguro de que esto funcione 100%, habria que modificar la base de datos para ver
	MYSQL *conn;
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	
	char nombres[200];
	
	char consulta[500];
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	conn = mysql_real_connect (conn, "localhost","root", "mysql", NULL, 0, NULL, 0);
	if (conn==NULL)
	{
		printf ("Error al inicializar la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	err=mysql_query(conn, "use Juego;");//"use database"
	if (err!=0)
	{
		printf ("Error al acceder a la base de datos %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	strcpy(consulta, "SELECT Partida.Ganador FROM Partida, Jugador WHERE Partida.Fecha = '");
	strcat(consulta, fecha);
	strcat(consulta, "'AND Partida.Ganador = Jugador.Nombre;");
		err=mysql_query (conn, consulta); 
	if (err!=0)
	{
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if (row == NULL)
	{
		printf ("No se han obtenido datos en la consulta\n");
		sprintf(respuesta, "5/1");// 2-Error puede cambiarse por cualquier otra cosa, pero que el cliente lo entienda
	}
	else
	{
		strcpy(nombres, row[0]);
		row = mysql_fetch_row (resultado);
		while(row!=NULL)
		{
			strcat (nombres, "/");
			strcat (nombres, row[0]);
			row = mysql_fetch_row (resultado);
		}
		strcat (nombres, "\n");
		printf("%s", nombres);
		sprintf(respuesta, "5/0/%s", nombres);// le envia al cliente el nombre, el cliente tiene que saber que hacer con ellos.
	}
	
}


//botones para login, signin, query 1,2,3 y desconectar (6 botones)
//textbox para nombre, contrase￱a y para el parametro (3)
//-std=c99 `mysql_config --cflags --libs`


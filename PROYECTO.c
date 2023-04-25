#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <mysql.h>
#include <pthread.h>

typedef struct {
	char nombre[20];
	int socket;
} Conectado;

typedef struct {
	Conectado conectados [100];
	int num;
} lConectados;

int sockets[100];

int i = 0;
void *atenderCliente(void *socket);

pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;
//esta es la version 1 del proyecto de SO.
int main(int argc, char *argv[])
{
	int sock_conn, sock_listen, ret;
	struct sockaddr_in serv_adr;
	char peticion[512];
	char respuesta[512];
	int conexion = 0;
	pthread_t thread;
	
	//abrimos el sockets
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
		printf("Error al crear el socket");
	
	memset(&serv_adr, 0, sizeof(serv_adr));
	serv_adr.sin_family = AF_INET;
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	serv_adr.sin_port = htons(9059);//----------------------------------------------------------------------//
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
	{
		printf("Error al bind");
	}
	
	if (listen(sock_listen, 3) < 0)
	{
		printf("Error en el Listen");
	}
	int rc;
	while (conexion == 0)
	{
		printf("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf("Conexion Recibida\n");
		
		sockets[i] = sock_conn;
		
		rc = pthread_create (&thread, NULL, atenderCliente , &sockets[i]);
		printf("Code %d = %s\n", rc, strerror(rc));
		
		i++;
	}
}

void *atenderCliente (void *socket)
{
	int sock_conn, ret;
	int *s;
	s = (int *) socket;
	sock_conn = *s;
	
	int conexion = 0;
	
	char peticion[512];
	char respuesta[512];
	
	while (conexion == 0)
	{
		ret=read(sock_conn, peticion, sizeof(peticion));
		printf("Recibido\n");
		peticion[ret] = '\0';
		
		printf("Peticion: %s\n" , peticion);
		int error = 1;
		char *p = strtok(peticion, "/");
		int codigo = atoi(p);
		
		
		if (codigo == 0)
		{
			conexion = 1;
		}
		else if (codigo == 1) //log in 
		{//el cliente enviara el mensaje de forma: 1/username/password
			pthread_mutex_lock(&mutex);
			char nombre[20];
			char contrasena[20];
			p=strtok(NULL, "/");//no estoy seguro
			strcpy(nombre, p);
			p=strtok(NULL, "/");
			strcpy(contrasena, p);
			LogIn(nombre, contrasena, respuesta);
			printf("%s\n", respuesta);
			//enviar respuesta
			write(sock_conn,respuesta,strlen(respuesta));
			
			pthread_mutex_unlock(&mutex);
		}
		else if (codigo == 2) //sign in
		{//2/user/pass
			pthread_mutex_lock(&mutex);
			char nombre[20];
			char contrasena[20];
			p=strtok(NULL, "/");//no estoy seguro
			strcpy(nombre, p);
			
			p=strtok(NULL, "/");
			strcpy(contrasena, p);
			SignIn(nombre, contrasena, respuesta);
			
			printf("%s\n", respuesta);
			//enviar respuesta
			write(sock_conn,respuesta,strlen(respuesta));
			
			pthread_mutex_unlock(&mutex);
		}
		else if (codigo == 3) //query 1
		{//3/nombre
			pthread_mutex_lock(&mutex);
			char nombre[20];
			p = strtok(NULL, "/");
			strcpy(nombre, p);
			JugadoresQueJueganCon(nombre, respuesta);
			
			printf("%s\n", respuesta);
			
			//enviar respuesta
			write(sock_conn,respuesta,strlen(respuesta));
			
			pthread_mutex_unlock(&mutex);
		}
		else if (codigo == 4) //query 2
		{//4/color
			pthread_mutex_lock(&mutex);
			char color[20];
			p = strtok(NULL, "/");
			strcpy(color, p);
			GanadoresConFichasDeColor(color, respuesta);
			
			//enviar respuesta
			write(sock_conn,respuesta,strlen(respuesta));
			
			pthread_mutex_unlock(&mutex);
		}
		else if (codigo == 5) //query 3
		{//5/fecha
			pthread_mutex_lock(&mutex);
			char fecha[11];
			p = strtok(NULL, "/");
			strcpy(fecha, p);
			printf("%s\n", fecha);
			JugadoresMasVictorias(fecha, respuesta);
			
			printf("%s\n", respuesta);
			//enviar respuesta
			write(sock_conn,respuesta,strlen(respuesta));
			
			pthread_mutex_unlock(&mutex);
		}
		
		close (sock_conn);
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
	strcat(Consulta, "'AND contrase￱a = '");
	strcat(Consulta, contrasena);
	strcat(Consulta, "';");
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
		sprintf(respuesta, "1/1/");// 1-Error puede cambiarse por cualquier otra cosa, pero que el cliente lo entienda
	}
	else
	{
		printf ("Acceso garantizado al usuario con id: %s\n", row[0]);
		sprintf(respuesta, "1/0");
	}
}
void SignIn(char Nombre[20], char contrasena[20], char respuesta[512])//esto no funciona at all xd
{
	MYSQL *conn;
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char UserName[20];
	char consulta[512];
	char CCuenta[512];//CrearCuenta
	
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
	//contrase\uffc3\uff83\uffc2\uffb1a = any
	strcpy(consulta, "SELECT Count(Jugador.Nombre) FROM Jugador WHERE Jugador.Nombre = '");
	strcat(consulta, Nombre);
	strcat(consulta, "';");
	
	err=mysql_query (conn, consulta);
	//EN EL CASO OPTIMO ME DA UN EMPTY SET, POR LO TANTO
	
	
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	
	//if (err == 0)//esto es util?
	//	printf("2/1");//nombre ocupado
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);

	int codigo = atoi(row[0]);
	
	if (codigo == 0)//si row == null entonces no hay nadie que tenga ese usuario, por lo tanto el nombre esta disponible para una cuenta nueva.
	{
		
		strcpy(consulta, "");
		strcpy(consulta, "SELECT MAX(Jugador.id) FROM Jugador;");
		err=mysql_query (conn, consulta);
		strcpy(resultado , "");
		resultado = mysql_store_result (conn);
		row = mysql_fetch_row (resultado);
		int idmax = atoi(row[0]);
		
		int NID = idmax + 1;
		
		char id[20];
		strcpy(id, NID);
		printf("%s\n", id);

		strcpy(CCuenta, "INSERT INTO Jugador VALUES ( ");
		//hasta aqui funciona
		strcat(CCuenta, id);//esto lo crashea, por algun motivo que desconcemos//-----------------------------------------------------------------------//
		printf("consulta2: %s\n", CCuenta);
		strcat(CCuenta, ", '");
		printf("consulta3: %s\n", CCuenta);
		strcat(CCuenta, Nombre);//esto no lo inserta
		printf("consulta4: %s\n", CCuenta);
		strcat(CCuenta, "','");
		printf("consulta5: %s\n", CCuenta);
		strcat(CCuenta, contrasena);//esto si lo inserta
		printf("consulta6: %s\n", CCuenta);
		strcat(CCuenta, "');");
		printf("consulta7: %s\n", CCuenta);
		
		err=mysql_query (conn, CCuenta);
		
		
		if (err!=0) {
			printf ("Error al consultar datos de la base %u %s\n",
					mysql_errno(conn), mysql_error(conn));
			exit (1);
			
			sprintf(respuesta, "2/1");
		}
		else 
		{
			printf ("Cuenta creada correctamente\n");
			sprintf(respuesta, "2/0");// envia 2/1 si la cuenta no existe, y por lo tanto se puede crear 
		}
		
	}
	else 
	{
		printf("Nombre ya ocupado\n");
		sprintf(respuesta, "2/1");
	}
	
}

void JugadoresQueJueganCon(char nombre[20], char respuesta[512])//con el numbre de un jugador te devuelve los que han jugado con el.
{//server side esto funciona perfectamente, hay que arreglar en el cliente
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
	strcpy (consulta, "SELECT DISTINCT Jugador.Nombre FROM (Jugador, Partida, Participaci￳n) WHERE Partida.id in (SELECT Partida.id From (Jugador, Partida, Participaci￳n) WHERE Jugador.Nombre = '");
	strcat (consulta, nombre);
	strcat (consulta, "'AND Jugador.ID = Participaci￳n.ID_J AND Participaci￳n.ID_P = Partida.ID) AND Partida.ID = Participaci￳n.ID_P AND Participaci￳n.ID_J = Jugador.ID AND Jugador.Nombre NOT IN('");
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
		printf("%s\n", nombres);
		sprintf(respuesta, "3/0/%s", nombres);// le envia al cliente los nombres, el cliente tiene que saber que hacer con ellos.
		
	}
}

void GanadoresConFichasDeColor(char color[20], char respuesta[512])//con el color de una ficha te dice que jugadores han ganado usandola
{//server side funciona perfectamente, arreglarlo en el cliente
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
	strcat (consulta,"'AND Partida.ID = Participaci￳n.ID_P");
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
	strcpy(consulta, "SELECT MAX(Partida.Ganador) FROM Partida WHERE Partida.Fecha = '");
	strcat(consulta, fecha);
	strcat(consulta, "';");
	
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
//textbox para nombre, contrase\ufff1a y para el parametro (3)
//-std=c99 `mysql_config --cflags --libs`
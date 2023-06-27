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


typedef struct {
	int num;
	int max;
	int ocupado;
	int aceptado[4];
	Conectado jugadores[4];
} Partida;

typedef struct {
	Partida partidas[500];
	int num;
} lPartidas;



int sockets[100];

lPartidas listaP;

lConectados listaC;

int i = 0;
void *atenderCliente(void *socket);void ConectarSQL();void LogIn(char nombre[20], char contrasena[20], char respuesta[512]);void SignIn(char Nombre[20], char contrasena[20], char respuesta[512]);
void JugadoresQueJueganCon(char nombre[20], char respuesta[512]);void GanadoresConFichasDeColor(char color[20], char respuesta[512]);void JugadoresMasVictorias(char fecha[20],char respuesta[512]);
int Conectar (lConectados *lista, char Nombre[20], int socket);int Desconectar (lConectados *lista, char Nombre[20]);int PosicionCliente (lConectados *lista, char nombre[20]);
void ListaConectados(lConectados *lista, char conectados[512]);int AgregarJugador(lPartidas *lp,lConectados *lc ,char invitados[80]);int BuscarPartidas(lPartidas *l);void IniciarPartida(lPartidas *l);
int partidaSocket(lPartidas *l, int socket);

pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;
//esta es la version 1 del proyecto de SO.
int main(int argc, char *argv[])
{

	ConectarSQL;
	IniciarPartida(&listaP);
	int sock_conn, sock_listen, ret;
	struct sockaddr_in serv_adr;
	char peticion[512];
	char respuesta[512];
	int conexion = 0;
	pthread_t thread;
	//abrimos el sockets
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
		printf("Error al crear el socket\n");
	
	memset(&serv_adr, 0, sizeof(serv_adr));
	serv_adr.sin_family = AF_INET;
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	serv_adr.sin_port = htons(9054);//----------------------------------------------------------------------//
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
	{
		printf("Error al bind\n");
	}
	
	if (listen(sock_listen, 3) < 0)
	{
		printf("Error en el Listen\n");
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
		
		printf("SOCKET: %d\n", sockets[i]);
		i++;

		
	}
}

void *atenderCliente (void *socket)
{
	
	int sock_conn, ret;
	int *s;
	s = (int *) socket;
	sock_conn = *s;
	
	int j, n;
	int r;
	int conexion = 0;
	
	char peticion[512];
	char respuesta[512];//hace lo mismo que notificacion pero se llama respuesta
	char notificacion[1024];//hace lo mismo que respuesta pero se llama notificacion
	char nombre[20];
	char contrasena[20];
	char fecha[11];
	char color[20];
	char conectados[512];
	int partida;
	char decision[1];
	char Zinvitados[80];
	char J1[20], J2[20], J3[20], J4[20];//seguramente esto no hace falta
	int socketJ;
	char chat[200];
	int CT = 3;
	
	

	while (conexion == 0)
	{
		ret=read(sock_conn, peticion, sizeof(peticion));
		printf("Recibido\n");
		peticion[ret] = '\0';
		
		printf("Peticion: %s\n" , peticion);
		int error = 1;
		char *p = strtok(peticion, "/");
		int codigo = atoi(p);
		printf("Codigo: %d\n", codigo);
		if (codigo == 0)
		{//esto es para deconectarse.
			pthread_mutex_lock(&mutex);
			
			conexion = 1;
			r = Desconectar(&listaC, nombre);
			pthread_mutex_unlock(&mutex);
		}
		else if (codigo == 1) //log in 
		{//1/username/password
			
			
		
			p=strtok(NULL, "/");//no estoy seguro
			strcpy(nombre, p);
			p=strtok(NULL, "/");
			strcpy(contrasena, p);
			
			LogIn(nombre, contrasena, respuesta);
			
			pthread_mutex_lock(&mutex);
			if (strcmp (respuesta, "1/0") == 0)
			{
				r = Conectar(&listaC, nombre, sock_conn);
				
			}
			printf("Respuesta: %s\n", respuesta);
			pthread_mutex_unlock(&mutex);
			//enviar respuesta
			write(sock_conn,respuesta,strlen(respuesta));
			
			
		}
		else if (codigo == 2) //sign in
		{//2/user/pass
			
			p=strtok(NULL, "/");//no estoy seguro
			strcpy(nombre, p);
			
			p=strtok(NULL, "/");
			strcpy(contrasena, p);
			SignIn(nombre, contrasena, respuesta);
			pthread_mutex_lock(&mutex);
			if (strcmp(respuesta, "2/0") == 0)
			{
				r = Conectar(&listaC, nombre, sock_conn);
			}
			pthread_mutex_unlock(&mutex);
			printf("Respuesta: %s\n", respuesta);
			//enviar respuesta
			write(sock_conn,respuesta,strlen(respuesta));
			
			
		}
		else if (codigo == 3) //query 1
		{//3/nombre
			
			p = strtok(NULL, "/");
			strcpy(nombre, p);
			JugadoresQueJueganCon(nombre, respuesta);
			
			printf("Respuesta: %s\n", respuesta);
			
			//enviar respuesta
			write(sock_conn,respuesta,strlen(respuesta));
			
			
		}
		else if (codigo == 4) //query 2
		{//4/color
			p = strtok(NULL, "/");
			strcpy(color, p);
			GanadoresConFichasDeColor(color, respuesta);
			
			printf("Respuesta: %s\n", respuesta);
			//enviar respuesta
			write(sock_conn,respuesta,strlen(respuesta));
			
			
		}
		else if (codigo == 5) //query 3
		{//5/fecha
			
			
			p = strtok(NULL, "/");
			strcpy(fecha, p);
			
			JugadoresMasVictorias(fecha, respuesta);
			
			printf("Respuesta: %s\n", respuesta);
			//enviar respuesta
			write(sock_conn,respuesta,strlen(respuesta));
		}
		else if (codigo == 7)//esto es el protocolo de invitacion
		{//7/numeropartida/nombreInvitador/nombreInvitados

			
			n = 1;
			p = strtok(NULL, "/");
			strcpy(Zinvitados, p);
			
			partida = AgregarJugador(&listaP,&listaC, Zinvitados);
			
			listaP.partidas[partida].aceptado[0] = 1;
			
			
			char invitador[20];
			p = strtok(Zinvitados, "-");
			strcat(invitador, p);
			
			sprintf(notificacion, "7/0/%d/%s", partida, invitador);
			
			printf("Notificacion: %s\n", notificacion);
			
			
			while (n < 5)
			{
				write(listaP.partidas[partida].jugadores[n].socket,notificacion,strlen(notificacion));
				
				n++;
			}
			
			
			
			
		}
		else if (codigo == 8)//8/0/NP es acpetado, 8/1/NP		NP = numero partida
		{//8/0/numeropartida
			n = 0;
			p = strtok(NULL, "/");
			partida = atoi(p);
			p = strtok(NULL, "/");
			
			sprintf(notificacion, "8/0/%d", partida);
			
		
			n = atoi(p);
			if (n == 0)//n = 0 signifca que el jugador ha aceptado
			{
			
				
				n = 0;
				while(strcmp(listaP.partidas[partida].jugadores[n].nombre, nombre) != 0)//busco la posicion del jugador en la tabla 
				{
					n++;
					
				}
				listaP.partidas[partida].aceptado[n] = 1;//el cliente ha aceptado, por lo tanto aceptado = 1;
				
				n = 0;
				while (n<4)
				{
					if(listaP.partidas[partida].aceptado[n] == 1)//se añaden los jugadores que han aceptado la partida
					{
						sprintf(notificacion, "%s/%s", notificacion, listaP.partidas[partida].jugadores[n].nombre);
						
					}
					n++;
				}
				printf("respuesta: %s\n", notificacion);
				n = 0;
				while (n<4)
				{
					if (listaP.partidas[partida].aceptado[n] == 1)//se les envia los jugadoes que han acpetado a todos los jugadores del lobby
					{
						write(listaP.partidas[partida].jugadores[n].socket, notificacion, strlen(notificacion));
					}
					
					n++;
					
				}
			}

		}
		else if (codigo == 9)
		{
			n = 0;
			p = strtok(NULL, "/");
			partida = atoi(p);
			p = strtok(NULL, "/");
			
			sprintf(notificacion, "9/%s", p);
			printf("Respuesta: %s\n", notificacion);
			while (n<4)
			{
				if (listaP.partidas[partida].aceptado[n] == 1)
				{
					write(listaP.partidas[partida].jugadores[n].socket, notificacion, strlen(notificacion));//se les envia a los usuarios de la lista una string con el mensaje 
				}
				n++;
			}
		}
		else if (codigo == 10)
		{//10/npartida
			
			p = strtok(NULL, "/");
			partida = atoi(p);
			
			sprintf(notificacion, "10/0");
			
			for (j = 0; j < 4; j++)
			{
				write(listaP.partidas[partida].jugadores[j].socket, notificacion, strlen(notificacion));
			}
		}
		else if (codigo == 11)
		{//11/partida/Nºficha/Nºposicion/casa/token
			p = strtok(NULL, "/");
			partida = atoi(p);
			
			
			p = strtok(NULL, "/");
			int equipo = atoi(p);
	
			if (equipo == 99)
			{
				sprintf(notificacion, "11/99/\n");
				printf("Notificacion: %s\n", notificacion);
				for (j = 0; j<4; j++)
				{
					write(listaP.partidas[partida].jugadores[j].socket, notificacion, strlen(notificacion));
				}
			}
			else{
				p = strtok(NULL, "/");
				int ficha = atoi(p);
				
				p = strtok(NULL, "/");
				int posicion = atoi(p);
				
				
				p = strtok(NULL, "/");
				char casa[6];
				strcpy(casa, p);
				
				p = strtok(NULL, "/");
				CT = atoi(p);
				
				p = strtok(NULL, "/");
				int numD = atoi(p);
				
				
				if (CT < 3)
				{
					CT++;
				}
				else 
				{
					CT = 0;
				}
				sprintf(notificacion, "11/%d/%d/%d/%s/%d/%d",equipo, ficha, posicion, casa, CT, numD);
				printf("Notificacion: %s\n", notificacion);
				for (j = 0; j<4; j++)
				{
					write(listaP.partidas[partida].jugadores[j].socket, notificacion, strlen(notificacion));
				}
			}
			
		}
		else if (codigo == 12)
		{
			p = strtok(NULL, "/");
			
			strcpy(nombre, p);
			
			DarseDeBaja(nombre, respuesta);
			
			
			write(sock_conn,respuesta,strlen(respuesta));
			
		}
		if ((codigo == 0||codigo == 1 || codigo ==2))//0 es Desconectar, 1 es conectar, 2 es registrarse
		{
			
			pthread_mutex_lock(&mutex);//no interrumpas
			ListaConectados(&listaC, conectados);//haz update de la lista
			pthread_mutex_unlock(&mutex);//molesta de nuevo
			sprintf(notificacion, "6/0/%s", conectados);

			printf("Notificacion: %s\n", notificacion);
			for (j=0;j<i;j++)//este bucle le enviara la tabla actualizada a todos los conectados.
			{
				write(sockets[j],notificacion,strlen(notificacion));
			}
			
		}
		
		
	}
	close (sock_conn);
	
}

void LogIn(char nombre[20], char contrasena[20], char respuesta[512])
{//recibe usuario y contraseña, hace una query en una base de datos, en caso de encontrar respuesta, permite que el usuario se conecte.
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
	conn = mysql_real_connect (conn, "147.83.117.53","root", "mysql", NULL, 0, NULL, 0);
	if (conn==NULL)
	{
		printf ("Error al inicializar la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	err=mysql_query(conn, "use T4_JUEGO;");//"use database"
	if (err!=0)
	{
		printf ("Error al acceder a la base de datos %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}	

	
		
	strcpy(Consulta, "SELECT Jugador.ID FROM Jugador Where Jugador.Nombre = '");
	strcat(Consulta, nombre);
	strcat(Consulta, "'AND contrasena = '");
	strcat(Consulta, contrasena);
	strcat(Consulta, "';");
	//hasta aqui funciona
	
	
	err=mysql_query (conn, Consulta);
	
	
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	
	if (row == NULL)//contraseña
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
void SignIn(char Nombre[20], char contrasena[20], char respuesta[512])
{//recibe un usuario y contrasña, crea una cuenta nueva en la base de datos y permite que el usuario se conecte
	MYSQL *conn;
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	
	
	conn = mysql_init(NULL);
	if (conn==NULL)
	{
		printf ("Error al crear la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	conn = mysql_real_connect (conn, "147.83.117.53","root", "mysql", T4_JUEGO, 0, NULL, 0);
	if (conn==NULL)
	{
		printf ("Error al inicializar la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	err=mysql_query(conn, "use T4_JUEGO;");//"use database"
	if (err!=0)
	{
		printf ("Error al acceder a la base de datos %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}	
		
	char UserName[20];
	char consulta[512];
	char CCuenta[512];//CrearCuenta
	
	
	//que el nombre no exista
	//ponerle una id mayor a la maxima
	//contrase\uffc3\uff83\uffc2\uffb1a = any
	strcpy(consulta, "SELECT Count(Jugador.Nombre) FROM Jugador WHERE Jugador.Nombre = '");
	strcat(consulta, Nombre);
	strcat(consulta, "';");
	
	err=mysql_query (conn, consulta);
	//EN EL CASO OPTIMO devuelve un 0, POR LO TANTO
	
	
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	
	//if (err == 0)//esto es util?
	//	printf("2/1");//nombre ocupado
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	
	int code = atoi(row[0]);//por algun motivo no nos deja igualar row[0] == '0'
	
	if (code == 0)//si code == 0 entonces no hay nadie que tenga ese usuario, por lo tanto el nombre esta disponible para una cuenta nueva.
	{
		//hasta aqui funciona
		strcpy(consulta, "");
		strcpy(consulta, "SELECT MAX(Jugador.id) FROM Jugador;");
		err=mysql_query (conn, consulta);
		//strcpy(resultado, "");//esto da una advertencia, no se si pasa algo si lo quito, creo que no
		resultado = mysql_store_result (conn);
		row = mysql_fetch_row (resultado);
		int idmax = atoi(row[0]);
		idmax++;
		//hasta aqui funciona
		
		
		printf("%d\n",idmax);
		//hasta aqui funciona
		sprintf(consulta, "insert into Jugador values (%d, '%s', '%s');", idmax, Nombre, contrasena);
		printf("Consulta %s\n", consulta);
		
		err=mysql_query (conn, consulta);
		
		
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

void JugadoresQueJueganCon(char nombre[20], char respuesta[512])
{//recibe un usuario y hace una query para ver quien ha jugado con el y guarda esta informacion en "respuesta"
	MYSQL *conn;
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	
	
	conn = mysql_init(NULL);
	if (conn==NULL)
	{
		printf ("Error al crear la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	conn = mysql_real_connect (conn, "147.83.117.53","root", "mysql", T4_JUEGO, 0, NULL, 0);
	if (conn==NULL)
	{
		printf ("Error al inicializar la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	err=mysql_query(conn, "use T4_JUEGO;");//"use database"
	if (err!=0)
	{
		printf ("Error al acceder a la base de datos %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}	
	
	char nombres[200];
	char consulta[500];
	
	strcpy (consulta, "SELECT DISTINCT Jugador.Nombre FROM (Jugador, Partida, Participacion) WHERE Partida.id in (SELECT Partida.id From (Jugador, Partida, Participacion) WHERE Jugador.Nombre = '");
	strcat (consulta, nombre);
	strcat (consulta, "'AND Jugador.ID = Participacion.ID_J AND Participacion.ID_P = Partida.ID) AND Partida.ID = Participacion.ID_P AND Participacion.ID_J = Jugador.ID AND Jugador.Nombre NOT IN('");
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

void GanadoresConFichasDeColor(char color[20], char respuesta[512])
{//recibe un color (numero) y hace una query para ver quien ha ganado con ese color, guarda esta informacion en "respuesta"
	MYSQL *conn;
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	
	
	conn = mysql_init(NULL);
	if (conn==NULL)
	{
		printf ("Error al crear la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	conn = mysql_real_connect (conn, "147.83.117.53","root", "mysql", T4_JUEGO, 0, NULL, 0);
	if (conn==NULL)
	{
		printf ("Error al inicializar la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	err=mysql_query(conn, "use T4_JUEGO;");//"use database"
	if (err!=0)
	{
		printf ("Error al acceder a la base de datos %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}	
	
	char nombres[200];
	char consulta[500];
	
	
	strcpy (consulta,"SELECT Partida.ganador FROM Partida,Participacion WHERE Participacion.Color = '");
	strcat (consulta, color);
	strcat (consulta,"'AND Partida.ID = Participacion.ID_P");
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
void DarseDeBaja(char nombre[20], char respuesta[512])
{//recibe un nombre y da de baja(elimina) la cuenta de la base de datos
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
	conn = mysql_real_connect (conn, "147.83.117.53","root", "mysql", T4_JUEGO, 0, NULL, 0);
	if (conn==NULL)
	{
		printf ("Error al inicializar la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	err=mysql_query(conn, "use T4_JUEGO;");//"use database"
	if (err!=0)
	{
		printf ("Error al acceder a la base de datos %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}	
	
	strcpy(Consulta, "DELETE FROM Jugador WHERE Nombre = '");
	strcat(Consulta, nombre);
	strcat(Consulta, "'");
	
	err=mysql_query (conn, Consulta);
	
	
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}

	printf ("Cuenta %s Eliminada", nombre);
	sprintf(respuesta, "12/0");	

}
void JugadoresMasVictorias(char fecha[20],char respuesta[512])
{//recibe una fecha y hace una query para ver quien(es) han ganado mas partidas este dia, guarda esta informacion en "respuesta"
	MYSQL *conn;
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	
	
	conn = mysql_init(NULL);
	if (conn==NULL)
	{
		printf ("Error al crear la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	conn = mysql_real_connect (conn, "147.83.117.53","root", "mysql", T4_JUEGO, 0, NULL, 0);
	if (conn==NULL)
	{
		printf ("Error al inicializar la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	err=mysql_query(conn, "use T4_JUEGO;");//"use database"
	if (err!=0)
	{
		printf ("Error al acceder a la base de datos %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}	
	
	
	char nombres[200];
	
	char consulta[500];
	
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


int Conectar (lConectados *lista, char Nombre[20], int socket)
{//recibe nombre y socket, añade el nuevo cliente a la lista, devuelve 0 si todo okay, -1 si la lista de clientes esta llena
	if (lista->num == 100)
	{
		return -1;
	}
	else
	{
		strcpy(lista->conectados[lista->num].nombre, Nombre);
		lista->conectados[lista->num].socket = socket;
		lista->num++;
		return 0;
	}
}

int Desconectar (lConectados *lista, char Nombre[20])
{//recibe un nombre y lo elimina de la lista de conectados
	int posicion = PosicionCliente(lista, Nombre);
	if (posicion == -1)
	{
		return -1;
	}
	else 
	{
		int i;
		for (i = posicion; i < lista->num-1; i++)
		{
			lista->conectados[i] = lista->conectados[i+1];
		}
		lista->num--;
		return 0;
	}
}

int PosicionCliente (lConectados *lista, char nombre[20])
{//recibe un nombre, lo busca en la lista de conectados y devuelve su posicion en la lista
	int i=0;
	int encontrado = 0;
	while (i<lista->num && !encontrado)
	{
		if (strcmp(lista->conectados[i].nombre, nombre) == 0)
		{
			encontrado = 1;
		}
		if (!encontrado)
		{
			i++;
		}
	}
	if (encontrado)
		return i;
	else 
		return -1;
}
void ListaConectados(lConectados *lista, char conectados[512])
{//guarda en "conectados" la lista de personas que hay conectadas
	int i;
	sprintf(conectados, "%d", lista->num);
	
	for (i=0;i<lista->num;i++)
	{
		sprintf(conectados, "%s/%s", conectados, lista->conectados[i].nombre);
	}
	
}



int AgregarJugador(lPartidas *lp,lConectados *lc, char invitados[80])
{//recibe 4 jugadores, y los agrega a una partida, devuelve el numero de la partida
	
	int n = 0;
	int j = 0;
	int encontrado = 0;
	char *p;
	
	p = strtok(invitados, "-");
	
	while (n < 99 && !encontrado)
	{
		if (lp->partidas[n].ocupado == 0)
		{
			encontrado =1;
			lp->partidas[n].max = 4;
		}
		else 
			n++;
		
	}
	
	
	if (encontrado == 1)
	{
		
		listaP.partidas[n].aceptado[0] == 1;
		while (j < lp->partidas[n].max)
		{
			strcpy(lp->partidas[n].jugadores[j].nombre, p);
			int z = PosicionCliente(lc, p);
			
			lp->partidas[n].jugadores[j].socket = lc->conectados[z].socket;
			printf("socket de %s es %d\n", p, lc->conectados[z].socket);
			p = strtok(NULL, "-");
			
			j++;
		}
		 
		return n;
	}
	else 
	{
		return -1;
	}
	
}
 
int BuscarPartidas(lPartidas *l) 
{//encuentra y devuelve el numero de la primera partida vacia que encuentre
	for (int i = 0; i < 500; i++) 
	{
		if (l->partidas[i].ocupado == 0) 
		{
			return i;//numero de partida
		}
	}
	return -1; // no hay partidas disponibles
}

void IniciarPartida(lPartidas *l)
{//inicializa la lista de partidas
	int i;
	for (i = 0; i< 99; i++)
	{
		l->partidas[i].ocupado = 0;
	}
}

int partidaSocket(lPartidas *l, int socket)
{//encuentra y devuelve el numero de partida de un jugador determinado
	int encontrado = 0;
	int j;
	int i;
	for (i = 0; i<99 && !encontrado;i++)
	{
		for (j = 0; j<4;j++);
		{
			if (l->partidas[i].jugadores[j].socket == socket)
			encontrado = 1;	
		}
	}
	return i;
}



//-std=c99 `mysql_config --cflags --libs`

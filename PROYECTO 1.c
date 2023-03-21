#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/sockets.h>
#include <netinet/in.h>
#include <studio.h>

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
	
	int i;
	
	for (i=0;i<5;i++)
	{
		printf("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf("He recibido conexión\n");
		
		ret=read(sock_conn, peticion, sizeof(peticion));
		printf("Recibido\n");
		
		peticion[ret] = '\0';
		
		printf("Peticion: %s\n" , peticion);
		
		char *p = strtok(peticion, "/");
		int codigo = atoi(p);
		p = strtok(NULL, "/");
		char dato[20];
		strcpy(dato, p);
		
		if (codigo == 1) //log in 
		
		if (codigo == 2) //sign in
		
		if (codigo == 3) //query 1
		
		if (codigo == 4) //query 2
		
		if (codigo == 5) //query 3
		
	}
}

void JugadoresQueJueganCon(char nombre[20], char respuesta[512])
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
	strcpy (consulta, "SELECT Nombre FROM (Jugador, Participacion) where Jugador.nombre = '");
	strcpy (consulta, nombre);
	strcpy (consulta, "' AND Jugador.id = participacion.id_j AND relacion.id_p = relacion.id_j;\n");
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
		sprintf(respuesta, "1-Error");// 1-Error puede cambiarse por cualquier otra cosa, pero que el cliente lo entienda
	}
	else
	{
		strcpy(nombres, row[0]);
		row = mysql_fetch_row (resultado);
		while(row!=NULL)
		{
			strcat (nombres, "-");
			strcat (nombres, row[0]);
			row = mysql_fetch_row (resultado);
		}
		strcat (nombres, "\n");
		printf(nombres);
		sprintf(respuesta, "1-%s", nombres);// le envia al cliente los nombres, el cliente tiene que saber que hacer con ellos.
	}
}
	
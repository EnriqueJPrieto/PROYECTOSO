DROP DATABASE IF EXISTS Juego;
CREATE DATABASE Juego;

SET FOREIGN_KEY_CHECKS=0;/*pongo esto porque sino me da un error con las foreign keys en la tabla Participación */

USE Juego;

create table Jugador(
	ID int,
	nombre varchar(20),
	contraseña varchar(20),
	PRIMARY KEY(ID)
);
	
create table Partida(
	ID int,
	ganador varchar(20),
	fecha varchar(11),
	jugadores int,/*numero de jugadores*/
	PRIMARY KEY(ID)
);

create table Participación(
	ID_P int,
	ID_J int,
	color int,
	CONSTRAINT FOREIGN KEY (ID_P) REFERENCES Partida(ID),
	CONSTRAINT FOREIGN KEY (ID_J) REFERENCES Jugador(ID)

);



 

INSERT INTO Jugador VALUES (0, 'Juan', 'password1');
INSERT INTO Jugador VALUES (1, 'Ana', 'password2');
INSERT INTO Jugador VALUES (2, 'Pedro', 'password3');
INSERT INTO Jugador VALUES (3, 'Maria', 'password4');
INSERT INTO Jugador VALUES (4, 'Luis', 'password5');
INSERT INTO Jugador VALUES (5, 'Sofia', 'password6');
INSERT INTO Jugador VALUES (6, 'David', 'password7');
INSERT INTO Jugador VALUES (7, 'Laura', 'password8');
INSERT INTO Jugador VALUES (8, 'Diego', 'password9');
INSERT INTO Jugador VALUES (9, 'Lucia', 'password10');



INSERT INTO Partida VALUES (1, 'Juan', '2022-03-01', 2);
INSERT INTO Partida VALUES (2, 'Pedro', '2022-03-02', 4);
INSERT INTO Partida VALUES (3, 'Sofia', '2022-03-03', 2);
INSERT INTO Partida VALUES (4, 'David', '2022-03-04', 3);
INSERT INTO Partida VALUES (5, 'Maria', '2022-03-05', 4);
INSERT INTO Partida VALUES (6, 'Lucia', '2022-03-06', 2);
INSERT INTO Partida VALUES (7, 'Diego', '2022-03-07', 3);
INSERT INTO Partida VALUES (8, 'Laura', '2022-03-08', 2);
INSERT INTO Partida VALUES (9, 'Luis', '2022-03-09', 4);
INSERT INTO Partida VALUES (10, 'Ana', '2022-03-10', 3);
INSERT INTO Partida VALUES (11, 'Juan', '2022-03-11', 2);
INSERT INTO Partida VALUES (12, 'Pedro', '2022-03-12', 4);
INSERT INTO Partida VALUES (13, 'Sofia', '2022-03-13', 2);
INSERT INTO Partida VALUES (14, 'David', '2022-03-14', 3);
INSERT INTO Partida VALUES (15, 'Maria', '2022-03-15', 4);
INSERT INTO Partida VALUES (16, 'Lucia', '2022-03-16', 2);
INSERT INTO Partida VALUES (17, 'Diego', '2022-03-17', 3);
INSERT INTO Partida VALUES (18, 'Laura', '2022-03-18', 2);
INSERT INTO Partida VALUES (19, 'Luis', '2022-03-19', 4);
INSERT INTO Partida VALUES (20, 'Ana', '2022-03-20', 3);

/* los colores estan puestos de forma numerica, tenemos intencion de cambiar esto en el futuro */
INSERT INTO Participación VALUES (1, 2, 1);
INSERT INTO Participación VALUES (1, 3, 2);
INSERT INTO Participación VALUES (2, 1, 2);
INSERT INTO Participación VALUES (2, 3, 3);
INSERT INTO Participación VALUES (3, 2, 4);
INSERT INTO Participación VALUES (3, 4, 5);
INSERT INTO Participación VALUES (4, 1, 3);
INSERT INTO Participación VALUES (4, 2, 1);
INSERT INTO Participación VALUES (5, 1, 4);
INSERT INTO Participación VALUES (5, 2, 2);
INSERT INTO Participación VALUES (6, 2, 5);
INSERT INTO Participación VALUES (6, 3, 1);
INSERT INTO Participación VALUES (7, 1, 2);
INSERT INTO Participación VALUES (7, 3, 3);
INSERT INTO Participación VALUES (8, 2, 5);
INSERT INTO Participación VALUES (8, 4, 2);
INSERT INTO Participación VALUES (9, 1, 1);
INSERT INTO Participación VALUES (9, 3, 5);
INSERT INTO Participación VALUES (10, 2, 2);
INSERT INTO Participación VALUES (10, 4, 1);
INSERT INTO Participación VALUES (11, 3, 4);
INSERT INTO Participación VALUES (11, 4, 2);
INSERT INTO Participación VALUES (12, 1, 1);
INSERT INTO Participación VALUES (12, 3, 3);
INSERT INTO Participación VALUES (13, 2, 4);
INSERT INTO Participación VALUES (13, 4, 1);
INSERT INTO Participación VALUES (14, 1, 3);
INSERT INTO Participación VALUES (14, 2, 5);
INSERT INTO Participación VALUES (15, 2, 2);
INSERT INTO Participación VALUES (15, 4, 1);
INSERT INTO Participación VALUES (16, 1, 5);
INSERT INTO Participación VALUES (16, 3, 4);
INSERT INTO Participación VALUES (17, 3, 3);
INSERT INTO Participación VALUES (17, 4, 2);
INSERT INTO Participación VALUES (18, 1, 1);
INSERT INTO Participación VALUES (18, 4, 5);
INSERT INTO Participación VALUES (19, 1, 4);
INSERT INTO Participación VALUES (19, 2, 3);
INSERT INTO Participación VALUES (20, 2, 1);
INSERT INTO Participación VALUES (20, 4, 2);
INSERT INTO Participación VALUES (21, 2, 5);
INSERT INTO Participación VALUES (21, 3, 3);
INSERT INTO Participación VALUES (22, 1, 2);
INSERT INTO Participación VALUES (22, 4, 4);
INSERT INTO Participación VALUES (23, 2, 1);
INSERT INTO Participación VALUES (23, 3, 2);
INSERT INTO Participación VALUES (24, 1, 5);
INSERT INTO Participación VALUES (24, 4, 3);
INSERT INTO Participación VALUES (25, 1, 1);
INSERT INTO Participación VALUES (25, 3, 4);
INSERT INTO Participación VALUES (26, 2, 2);
INSERT INTO Participación VALUES (26, 4, 5);
INSERT INTO Participación VALUES (27, 1, 3);
INSERT INTO Participación VALUES (27, 3, 1);
INSERT INTO Participación VALUES (28, 2, 4);
INSERT INTO Participación VALUES (28, 4, 2);
INSERT INTO Participación VALUES (29, 2, 5);
INSERT INTO Participación VALUES (29, 3, 1);
INSERT INTO Participación VALUES (30, 1, 2);
INSERT INTO Participación VALUES (30, 4, 3);
INSERT INTO Participación VALUES (31, 2, 1);
INSERT INTO Participación VALUES (31, 3, 5);
INSERT INTO Participación VALUES (32, 1, 4);
INSERT INTO Participación VALUES (32, 3, 1);
INSERT INTO Participación VALUES (33, 1, 5);
INSERT INTO Participación VALUES (33, 4, 2);
INSERT INTO Participación VALUES (34, 2, 3);
INSERT INTO Participación VALUES (34, 3, 4);
INSERT INTO Participación VALUES (35, 2, 1);
INSERT INTO Participación VALUES (35, 4, 2);






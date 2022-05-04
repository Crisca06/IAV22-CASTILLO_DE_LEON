# IAV22 Cristian Castillo de León - Proyecto Final
### Esta es la documentación para el proyecto final de la asignatura de Inteligencia Artificial para Videojuegos del curso 2021/2022, por Cristian Castillo.

## Propuesta
El proyecto final se centra principalmente en un pequeño juego del género de terror y mazmorras, con generación aleatoria de las mismas.
La historia se basa en un demonio aterrador que se encuentra en una mazmorra a punto de obtener un poder prohibido, el protagonista se adentra en dicha mazmorra y debe encontrar las 7 cabras del sacrificio para poder detener al demonio. Las cabras se encontrarán repartidas de forma aleatoria por la mazmorra y solo podrán seguir al jugador en caso de que éste le dé de comer, una vez ganada su confianza, la cabra seguirá al jugador mientras lo vea, pero huirá en caso de encontrarse cerca el demonio durante demasiado tiempo, para lograr todo este comportamiento se hará uso de una máquina de estados. Las cabras seguirán al jugador hasta llegar a la zona de sacrificio dónde se quedarán esperando a estar todas para poder derrotar al demonio, esta zona será inaccesible para el demonio.

El demonio contará con un árbol de comportamientos que funcionará en función del número de cabras que se encuentren tanto en la zona segura como siguiendo al jugador. En un principio el demonio únicamente divagará por la mazmorra ignorando al jugador pero si éste lleva un número de cabras protegidas o siguiéndole entonces se volverá agresivo y lo perseguirá, el jugador podrá ahuyentarlo utilizando una antorcha, que lo mantendrá alejado un tiempo pero en caso de apagarse la antorcha éste podrá acercarse y las cabras que sigan al jugador huirán y perderán la confianza.

Se tiene pensado incluir un desplazamiento en bandada a la hora de juntarse las cabras persiguiendo al jugador.
En cuanto a la generación aleatoria de las mazmorras, se tiene pensado crear un generador que permita recrear una mazmorra con un camino accesible desde el principio hasta la zona segura y con caminos adicionales hacia distintas salas donde estarán las cabras y comida. 

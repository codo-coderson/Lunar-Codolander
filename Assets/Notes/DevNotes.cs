/*
 
* GAME LOOP *

El jugador tendr� que poner banderas en varias zonas planas del mapa, teniendo que aterrizar en cada una de ellas,
pero para conseguir ponerlas todas necesitar� los fuel cans, estrat�gicamente/aleatoriamente colocados en otras zonas planas.

En el HUD habr� una fila de banderas, las que faltan por poner, y una barra que simbolice el fuel (componente de ShipController) que quede.

Cada fuel can dar� 100 de fuel, y habr� un m�ximo de 1000. Si el jugador no llega a poner todas las banderas, o se estrella,
tendr� que volver a empezar. Si el jugador pone todas las banderas, pasa a la siguiente fase. Si se tiene ahorrado m�s de X fuel
en cada una de las fases se desbloquear� un nivel secreto. Se har� saber que hay un nivel secreto de alguna forma; por ejemplo, premiando
que haya m�s fuel del necesario al final de cada fase. �C�mo se puede premiar?  D�ndole al jugador un power up. El �ltimo powerup es un
mega propulsor que lleva al jugador a la �ltima fase, m�s lejana que ninguna. Asteroides, fuerte gravedad, lava, viento... todo lo que
se pueda imaginar. El jugador tendr� que aterrizar en una zona plana, pero con la gravedad y el viento en contra, y los asteroides
cayendo del cielo. Si el jugador consigue aterrizar, habr� ganado el juego.

En la fase 2 se recrudece.

En la fase 3 las zonas planas est�n en ca�ones profundos.

En la fase 4 hay asteroides.

El nivel secreto es hiper dif�cil.




* FEATURES *

La nave levanta polvo al acercarse a la superficie.
Fase de paseo de un planeta a otro.



* IDEAS DE POWER UPS *
Im�n de fuel cans
Escudo
Velocidad
Inmunidad a los asteroides
Inmunidad a la gravedad



* PASOS *

1. Crear el HUD:
fuel, banderas, puntuaci�n

2. Crear el sistema de banderas:
- Colocar banderas en zonas planas
- Crear el HUD de banderas
- Crear el sistema de puntuaci�n

3. Crear el sistema de fuel cans:
- Colocar fuel cans en zonas planas
- Crear el HUD de fuel
- Crear el sistema de fuel

4. Crear el sistema de puntuaci�n:
- Crear el HUD de puntuaci�n
- Crear el sistema de puntuaci�n


*/

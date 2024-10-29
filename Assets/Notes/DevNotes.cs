/*
 
* GAME LOOP *

El jugador tendrá que poner banderas en varias zonas planas del mapa, teniendo que aterrizar en cada una de ellas,
pero para conseguir ponerlas todas necesitará los fuel cans, estratégicamente/aleatoriamente colocados en otras zonas planas.

En el HUD habrá una fila de banderas, las que faltan por poner, y una barra que simbolice el fuel (componente de ShipController) que quede.

Cada fuel can dará 100 de fuel, y habrá un máximo de 1000. Si el jugador no llega a poner todas las banderas, o se estrella,
tendrá que volver a empezar. Si el jugador pone todas las banderas, pasa a la siguiente fase. Si se tiene ahorrado más de X fuel
en cada una de las fases se desbloqueará un nivel secreto. Se hará saber que hay un nivel secreto de alguna forma; por ejemplo, premiando
que haya más fuel del necesario al final de cada fase. ¿Cómo se puede premiar?  Dándole al jugador un power up. El último powerup es un
mega propulsor que lleva al jugador a la última fase, más lejana que ninguna. Asteroides, fuerte gravedad, lava, viento... todo lo que
se pueda imaginar. El jugador tendrá que aterrizar en una zona plana, pero con la gravedad y el viento en contra, y los asteroides
cayendo del cielo. Si el jugador consigue aterrizar, habrá ganado el juego.

En la fase 2 se recrudece.

En la fase 3 las zonas planas están en cañones profundos.

En la fase 4 hay asteroides.

El nivel secreto es hiper difícil.




* FEATURES *

La nave levanta polvo al acercarse a la superficie.
Fase de paseo de un planeta a otro.



* IDEAS DE POWER UPS *
Imán de fuel cans
Escudo
Velocidad
Inmunidad a los asteroides
Inmunidad a la gravedad



* PASOS *

1. Crear el HUD:
fuel, banderas, puntuación

2. Crear el sistema de banderas:
- Colocar banderas en zonas planas
- Crear el HUD de banderas
- Crear el sistema de puntuación

3. Crear el sistema de fuel cans:
- Colocar fuel cans en zonas planas
- Crear el HUD de fuel
- Crear el sistema de fuel

4. Crear el sistema de puntuación:
- Crear el HUD de puntuación
- Crear el sistema de puntuación


*/

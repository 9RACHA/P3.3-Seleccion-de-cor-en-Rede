# P3.2-NGO-Move
Realiza o titorial Get started with NGO. 
Logo elimina o Network transform que introduciches no último paso. 
Fai que as capsulas respondan aos movementos esquerda, dereita, arriba e abaixo; 
Asegúrate de ese momento o reproduzan en rede (que o player se mova en todos os equipos). 
Non podes usar o Network transform para isto, só Networkvariables e chamadas RPC. Eses movementos terán como input as teclas que ti decidas.

Entregables:

Pequeno informe explicativo con algunhas capturas
Link ao repositorio do xogo

## Informe
Almacene la Variable de red NetworkVariable llamada Position cuyo objetivo es el de guardar la posicion del jugador.

Esta Position es fundamental para generar una posicion aleatoria mediante un [ServerRpc] o posteriormente un [ClientRpc] que generará al principio una posicion aleatoria y después podremos generar nosotros otra posicion pulsando las flechas de dirección.

Es importante recalcar que todo lo relacionado con Inputs en este caso las flechas tendran que ir integradas dentro del metodo Update ya que se va actualizando constantemente.

Posicion de inicio
![image](https://github.com/9RACHA/P3.2-NGO-Move/assets/66274956/c31177fb-3cfa-42b8-9891-9ecedb364ccd)

Me muevo a la izquierda
![image](https://github.com/9RACHA/P3.2-NGO-Move/assets/66274956/3a485aa1-0987-4dd8-b031-35b4268c2aa5)

El player solo se movera una vez por tecla pulsada ya que utilizo la funcion GetKeyDown 
![KeyDown](https://github.com/9RACHA/P3.2-NGO-Move/assets/66274956/9e7b51e9-c49d-4988-9a56-3d3d3ccbf83b)

El resto del codigo esta en el Script PlayerMovement.






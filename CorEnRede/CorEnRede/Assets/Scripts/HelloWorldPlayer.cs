using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour {

        // Variable de red para almacenar la posición del jugador
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        // Lista estática de colores disponibles
        public static List<Color> coloresDisponibles; 

        // Variable de red para almacenar el color actual del jugador
        public NetworkVariable<Color> colorActual = new NetworkVariable<Color>();

        // Referencia al componente Renderer del jugador
        private Renderer r;

        // Método llamado cuando el jugador se crea en red
        public override void OnNetworkSpawn()
        {
            // Si es el propietario del objeto (jugador local)
            if (IsOwner)
            {
                Move(); // Mover el jugador a una posición aleatoria 
            }
        }

        public void Move()
        {
            //Podria eliminarse
            /*if (NetworkManager.Singleton.IsServer) {
                var randomPosition = GetRandomPositionOnPlane(); // Generar una posición aleatoria en un plano
                transform.position = randomPosition; // Actualizar la posición del jugador localmente
                Position.Value = randomPosition; // Actualizar la posición del jugador en la red
            }
            else
            {*/
                SubmitPositionRequestServerRpc(); // Enviar una solicitud al servidor para mover el jugador
           // }
        }

        // Procesa la solicitud de posición del jugador
        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionOnPlane(); // Generar una nueva posición aleatoria en el servidor
            //transform.position = Position.Value; // Actualizar la posición del jugador localmente
        }

        // Obtiene una posición aleatoria en un plano para que sea equivalente en todos los players
        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        // Método para seleccionar los colores disponibles
        void SeleccionColoresDisponibles()
        {
            // Agregar colores a la lista de colores disponibles
            coloresDisponibles.Add(Color.yellow);
            coloresDisponibles.Add(Color.white);
            coloresDisponibles.Add(Color.black);
            coloresDisponibles.Add(Color.blue);
            coloresDisponibles.Add(Color.grey);
            coloresDisponibles.Add(Color.cyan);
            coloresDisponibles.Add(Color.magenta);
            coloresDisponibles.Add(Color.red);
            coloresDisponibles.Add(Color.green);
            coloresDisponibles.Add(Color.clear); //Transparente
            //Añadir 1 mas
        }
        
        public Color ColorAleatorio(bool first = false) {
            Color colorViejo = r.material.color; // Guardar el color actual del jugador

            Color newColor = coloresDisponibles[Random.Range(0, coloresDisponibles.Count)]; // Obtener un color aleatorio de la lista

            coloresDisponibles.Remove(newColor); // Eliminar el color seleccionado de la lista de colores disponibles

            if (!first)
                coloresDisponibles.Add(colorViejo); // Agregar el color anterior, nuevamente a la lista de colores disponibles

            return newColor; // Devolver
        }

        public void CambiaColor()
        {
            SubmitColorRequestServerRpc(); // Enviar una solicitud al servidor para cambiar el color del jugador
        }

        // Método de servidor remoto para procesar la solicitud de cambio de color del jugador
        [ServerRpc]
        void SubmitColorRequestServerRpc(bool primero = false, ServerRpcParams rpcParams = default)
        {
            Debug.Log(coloresDisponibles.Count); // Muestra por pantalla los colores disponibles

            Color colorViejo = colorActual.Value; // Guardar el color actual del jugador

            Color colorNuevo = coloresDisponibles[Random.Range(0, coloresDisponibles.Count)]; // Obtener un nuevo color aleatorio de la lista

            coloresDisponibles.Remove(colorNuevo); // Eliminar el nuevo color seleccionado de la lista de colores disponibles

            if (!primero)
            {
                coloresDisponibles.Add(colorViejo); // Agregar el color anterior nuevamente a la lista de colores disponibles
            }

            colorActual.Value = colorNuevo; // Actualizar el color actual del jugador en la red
        }

        void Awake()
        {
            coloresDisponibles = new List<Color>(); // Inicializar la lista de colores disponibles

            if (coloresDisponibles.Count == 0)
            {
                SeleccionColoresDisponibles(); // Seleccionar los colores disponibles si la lista está vacía
            }
        }

        void Start()
        {
            r = GetComponent<Renderer>(); // Obtener el componente Renderer del jugador

            if (IsOwner)
            {
                SubmitColorRequestServerRpc(true); // Enviar una solicitud al servidor para obtener el primer color del jugador
            }
        }

        void Update()
        {
            transform.position = Position.Value; // Actualizar la posición del jugador localmente

            r.material.color = colorActual.Value; // Actualizar el color del jugador localmente
        }
    }
}


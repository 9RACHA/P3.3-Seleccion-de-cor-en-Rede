using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        public NetworkVariable<Color> colorActual = new NetworkVariable<Color>();

        public static List<Color> coloresDisponibles;

        private Renderer r;

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Move();
            }
        }

        public void Move()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var randomPosition = GetRandomPositionOnPlane();
                transform.position = randomPosition;
                Position.Value = randomPosition;
            }
            else
            {
                SubmitPositionRequestServerRpc();
            }
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionOnPlane();
            transform.position = Position.Value;
        }

        void SeleccionarColoresDisponibles() {
            coloresDisponibles = new List<Color>();
            coloresDisponibles.Add(Color.blue);
            coloresDisponibles.Add(Color.black);
            coloresDisponibles.Add(Color.white);
        }

        public Color ColorAleatorio(bool primero = false)
        {
            Color colorViejo = r.material.color;
            Color colorNuevo = coloresDisponibles[Random.Range(0, coloresDisponibles.Count)];
            coloresDisponibles.Remove(colorNuevo);
            if (!primero) coloresDisponibles.Add(colorViejo);
            return colorNuevo;
        }

        public void CambiarColor()
        {
            SubmitColorRequestServerRpc();
        }

        [ServerRpc]
        void SubmitColorRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Debug.Log(coloresDisponibles.Count);
            Color colorViejo = colorActual.Value;
            Color colorNuevo = coloresDisponibles[Random.Range(0, coloresDisponibles.Count)];
            coloresDisponibles.Remove(colorNuevo);
            if (!primero)
            {
                coloresDisponibles.Add(colorViejo);
            }
            colorActual.Value = colorNuevo;
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        void Awake()
        {
            coloresDisponibles = new List<Color>();
            if (coloresDisponibles.Count == 0)
            {
                SeleccionarColoresDisponibles();
            }
        }

        void Start()
        {
            r = GetComponent<Renderer>();
            if (IsOwner)
            {
                SubmitColorRequestServerRpc(true);
            }
        }

        void Update()
        {
            transform.position = Position.Value;
            r.material.color = colorActual.Value;
        }
    }
}

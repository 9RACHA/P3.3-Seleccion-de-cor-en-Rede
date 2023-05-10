using Unity.Netcode;
using UnityEngine;

//SOLO NETWORKVARIABLES Y LLAMADAS AL RPC

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

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
           /*  else 
             {
                SubmitPositionRequestClientRpc();
             } */
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionOnPlane();
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        /*
        [ClientRpc]
        void SubmitPositionRequestClientRpc(ClientRpcParms rpcParams = default){
            Position.Value = GetRandomPositionOnPlane();
        }

        static Vector3 GetRandomPositionOnPlaneC(){
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        } */

        void Update()
        {
            transform.position = Position.Value;
        }
    }
}
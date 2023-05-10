using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkCommandLine : MonoBehaviour
{
   private NetworkManager netManager;

   void Start()
   {
       netManager = GetComponentInParent<NetworkManager>();

       if (Application.isEditor) return;

       var args = GetCommandlineArgs();

       if (args.TryGetValue("-mode", out string mode))
       {
           switch (mode)
           {
               case "server":
                   netManager.StartServer();
                   break;
                //   Debug.Log("Servidor");
               case "host":
                   netManager.StartHost();
                   break;
                //   Debug.Log("Host");
               case "client":
                   netManager.StartClient();
                   break;
                //   Debug.Log("Cliente");
           }
       }
   }

   private Dictionary<string, string> GetCommandlineArgs()
   {
       Dictionary<string, string> argDictionary = new Dictionary<string, string>();

       var args = System.Environment.GetCommandLineArgs();

       for (int i = 0; i < args.Length; ++i)
       {
           var arg = args[i].ToLower();
           if (arg.StartsWith("-"))
           {
               var value = i < args.Length - 1 ? args[i + 1].ToLower() : null;
               value = (value?.StartsWith("-") ?? false) ? null : value;

               argDictionary.Add(arg, value);
           }
       }
       return argDictionary;
   }
}
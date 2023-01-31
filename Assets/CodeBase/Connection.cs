using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase
{
    public class Connection : MonoBehaviour
    {
        [SerializeField] private NetworkManager networkManager;
        [SerializeField] private Button hostButton;
        [SerializeField] private Button clientButton;
        
        private void Start()
        {
            StartClientIfBatch();
            AddListeners();
        }

        private void JoinClient()
        {
            networkManager.networkAddress = "localhost";
            networkManager.StartClient();
        }

        private void StartClientIfBatch()
        {
            if (!Application.isBatchMode)
            {
                networkManager.StartClient();
            }
        }

        private void AddListeners()
        {
            hostButton.onClick.AddListener(networkManager.StartHost);
            clientButton.onClick.AddListener(JoinClient);
        }
    }
}

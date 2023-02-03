using Mirror;
using TMPro;
using UnityEngine;

namespace CodeBase.Player
{
    public class Scoreboard : NetworkBehaviour
    {
        public TextMeshProUGUI currentScoreText;
        public GameObject winUI;
        
        private GameObject _scoreKills;
        private GameObject _winUi;

        private void Start()
        {
            UpdateScoreText(0);
        }

        [ClientRpc]
        public void ActivateWinUi()
        {
            _winUi = Instantiate(winUI);
            NetworkServer.Spawn(_winUi);
        }
        
        public void UpdateScoreText(int score) => currentScoreText.text = "Your Kills: " + score;
    }
}
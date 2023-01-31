using System;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Player
{
    public class PlayerCamera : NetworkBehaviour
    {
        private Camera _mainCam;
        
        private void Awake()
        {
            _mainCam = Camera.main;
        }

        public override void OnStartLocalPlayer()
        {
            if (_mainCam != null)
            {
                _mainCam.orthographic = false;
                _mainCam.transform.SetParent(transform);
                _mainCam.transform.localPosition = new Vector3(0f, 3f, -8f);
                _mainCam.transform.localEulerAngles = new Vector3(10f, 0f, 0f);
            }
        }

        public override void OnStopLocalPlayer()
        {
            if (_mainCam != null)
            {
                _mainCam.transform.SetParent(null);
                SceneManager.MoveGameObjectToScene(_mainCam.gameObject, SceneManager.GetActiveScene());
                _mainCam.orthographic = true;
                _mainCam.transform.localPosition = new Vector3(0f, 70f, 0f);
                _mainCam.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
            }
        }
    }
}

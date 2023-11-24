using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class StartCtrl : MonoBehaviour
{
    [SerializeField]
    Transform _canvas;

    private TMP_InputField _ip;
    
    void Start()
    {
        Button createBtn= _canvas.Find("CreateBtn").GetComponent<Button>();
        Button joinBtn = _canvas.Find("JoinBtn").GetComponent<Button>();
        createBtn.onClick.AddListener(OnCreateClick);
        joinBtn.onClick.AddListener(OnJoinClick);
        
        _ip = _canvas.Find("IP").GetComponent<TMP_InputField>();
    }

    private void OnJoinClick()
    {
        var transport = NetworkManager. Singleton. NetworkConfig.NetworkTransport as UnityTransport;
        transport.SetConnectionData(_ip.text, 7777);
        NetworkManager.Singleton.StartClient();
    }

    private void OnCreateClick()
    {
        var transport = NetworkManager. Singleton. NetworkConfig.NetworkTransport as UnityTransport;
        transport.SetConnectionData(_ip.text, 7777);
        
        NetworkManager.Singleton.StartHost();
        GameManager.Instance.LoadSence("Lobby");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

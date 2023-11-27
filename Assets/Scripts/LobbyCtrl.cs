using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCtrl : NetworkBehaviour
{
    [SerializeField] private Transform _canvas;

    private Transform _content;
    private GameObject _originCell;
    private Button _startBtn;
    private Toggle _ready;
    private List<PlayerListCell> _cellList;


    public override void OnNetworkSpawn()
    {
        _content = _canvas.Find("List/Viewport/Content");
        _originCell = _content.Find("Cell").gameObject;
        _startBtn = _canvas.Find("StartBtn").GetComponent<Button>();
        _ready = _canvas.Find("Ready").GetComponent<Toggle>();
        _startBtn.onClick.AddListener(OnStartClick);
        _startBtn.onClick.AddListener(OnReadyToggle);
        _cellList=new List<PlayerListCell>();
        AddPlayer(NetworkManager.LocalClientId,false);
        base.OnNetworkSpawn();
    }

    public void AddPlayer(ulong PlayID,bool IsReady)
    {
        GameObject clone = Instantiate(_originCell);
        clone.transform.SetParent(_content,false);
        PlayerListCell Cell = clone.GetComponent<PlayerListCell>();
        Cell.Initial(PlayID,IsReady);
        _cellList.Add(Cell);
        clone.SetActive(true);
    }

   

    private void OnReadyToggle()
    {
       
    }

    private void OnStartClick()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
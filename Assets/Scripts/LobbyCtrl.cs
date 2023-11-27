using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public struct PlayerInfo : INetworkSerializable
{
    public ulong ID;
    public bool IsReady;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ID);
        serializer.SerializeValue(ref IsReady);
    }
}

public class LobbyCtrl : NetworkBehaviour
{
    [SerializeField] private Transform _canvas;

    private Transform _content;
    private GameObject _originCell;
    private Button _startBtn;
    private Toggle _ready;
    private List<PlayerListCell> _cellList;
    private Dictionary<ulong, PlayerInfo> _allPlayerInfos;

    public override void OnNetworkSpawn()
    {
        //如果是服务端（主机）监听其他玩家加入
        if (IsServer)
        {
            NetworkManager.OnClientConnectedCallback += OnClientConn;
        }

        _content = _canvas.Find("List/Viewport/Content");
        _originCell = _content.Find("Cell").gameObject;
        _startBtn = _canvas.Find("StartBtn").GetComponent<Button>();
        _ready = _canvas.Find("Ready").GetComponent<Toggle>();
        _startBtn.onClick.AddListener(OnStartClick);
        _startBtn.onClick.AddListener(OnReadyToggle);
        _cellList = new List<PlayerListCell>();
        _allPlayerInfos = new Dictionary<ulong, PlayerInfo>();

        PlayerInfo playInfo = new PlayerInfo();
        playInfo.ID = NetworkManager.LocalClientId;
        playInfo.IsReady = false;

        AddPlayer(playInfo);
        base.OnNetworkSpawn();
    }

    private void OnClientConn(ulong obj)
    {
        PlayerInfo playerInfo = new PlayerInfo();
        playerInfo.ID = obj;
        playerInfo.IsReady = false;
        AddPlayer(playerInfo);
        UpdateAllPlayerInfos();
    }

    void UpdateAllPlayerInfos()
    {
        foreach (var item in _allPlayerInfos)
        {
            //通知其他客户端把没有的player加进去。
            UpdatePlayerInfoClientRpc(item.Value);
        }
    }

    [ClientRpc]
    void UpdatePlayerInfoClientRpc(PlayerInfo playerInfo)
    {
        if (!IsServer)
        {
            if (_allPlayerInfos.ContainsKey(playerInfo.ID))
            {
                _allPlayerInfos[playerInfo.ID] = playerInfo;
            }
            else
            {
                AddPlayer(playerInfo);
            }
        }
    }


    public void AddPlayer(PlayerInfo playerInfo)
    {
        _allPlayerInfos.Add(playerInfo.ID, playerInfo);
        GameObject clone = Instantiate(_originCell);
        clone.transform.SetParent(_content, false);
        PlayerListCell Cell = clone.GetComponent<PlayerListCell>();
        Cell.Initial(playerInfo);
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
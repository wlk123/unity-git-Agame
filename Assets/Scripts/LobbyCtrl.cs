using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public struct PlayerInfo : INetworkSerializable
{
    public ulong ID;
    public string name;
    public bool IsReady;
    public int gender;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ID);
        serializer.SerializeValue(ref IsReady);
        serializer.SerializeValue(ref gender);
        serializer.SerializeValue(ref name);
      
    }
}

public class LobbyCtrl : NetworkBehaviour
{
    [SerializeField] private Transform _canvas;

    private Transform _content;
    private GameObject _originCell;
    private Button _startBtn;

    private Toggle _ready;

    //private List<PlayerListCell> _cellList;
    private Dictionary<ulong, PlayerListCell> _cellDictionary;

    private Dictionary<ulong, PlayerInfo> _allPlayerInfos;

    private TMP_InputField _name;

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
        _ready.onValueChanged.AddListener(OnReadyToggle);
        
        _name = _canvas.Find("Name").GetComponent<TMP_InputField>();
        _name.onEndEdit.AddListener(OnEndEdit);
        
        _cellDictionary = new Dictionary<ulong, PlayerListCell>();
        _allPlayerInfos = new Dictionary<ulong, PlayerInfo>();

        PlayerInfo playInfo = new PlayerInfo();
        playInfo.ID = NetworkManager.LocalClientId;
        playInfo.name = "玩家" + playInfo.ID;
        _name.text = playInfo.name;
        playInfo.IsReady = false;
        playInfo.gender = 0;

        AddPlayer(playInfo);

        Toggle male = _canvas.Find("Gender/Male").GetComponent<Toggle>();
        Toggle female = _canvas.Find("Gender/Female").GetComponent<Toggle>();
        male.onValueChanged.AddListener(OnMaleToggle);
        female.onValueChanged.AddListener(OnFemaleToggle);


        base.OnNetworkSpawn();
    }

    private void OnEndEdit(string arg0)
    {
        if (string.IsNullOrEmpty(arg0))
        {
            return;
        }

        PlayerInfo playerInfo = _allPlayerInfos[NetworkManager.LocalClientId];
        playerInfo.name = arg0;
        _allPlayerInfos[NetworkManager.LocalClientId] = playerInfo;
        _cellDictionary[NetworkManager.LocalClientId].UpdateInfo(playerInfo);
        if (IsServer)
        {
            UpdateAllPlayerInfos();
        }
        else
        {
            // UpdatePlayerInfoClientRpc(playerInfo);
            UpdateAllPlayerInfosServerRpc(playerInfo);
        }
    }

    private void OnFemaleToggle(bool arg0)
    {
        if (arg0)
        {
            PlayerInfo playerInfo = _allPlayerInfos[NetworkManager.LocalClientId];
            playerInfo.gender = 1;
            _allPlayerInfos[NetworkManager.LocalClientId] = playerInfo;
            _cellDictionary[NetworkManager.LocalClientId].UpdateInfo(playerInfo);
            //更新
            if (IsServer)
            {
                UpdateAllPlayerInfos();
            }
            else
            {
               // UpdatePlayerInfoClientRpc(playerInfo);
                UpdateAllPlayerInfosServerRpc(playerInfo);
            }
            BodyCtrl.Instance.SwitchGender(1);
        }
            
    }

    private void OnMaleToggle(bool arg0)
    {
        if (arg0)
        {
            PlayerInfo playerInfo = _allPlayerInfos[NetworkManager.LocalClientId];
            playerInfo.gender = 0;
            _allPlayerInfos[NetworkManager.LocalClientId] = playerInfo;
            _cellDictionary[NetworkManager.LocalClientId].UpdateInfo(playerInfo);
            //更新
            if (IsServer)
            {
                UpdateAllPlayerInfos();
            }
            else
            {
                //UpdatePlayerInfoClientRpc(playerInfo);
                UpdateAllPlayerInfosServerRpc(playerInfo);
            }
            BodyCtrl.Instance.SwitchGender(0);
        }
           
    }

    private void OnClientConn(ulong obj)
    {
        PlayerInfo playerInfo = new PlayerInfo();
        playerInfo.ID = obj;
        playerInfo.name = "玩家" + obj;
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

            //更改显示层
            UpdatePlayerCells();
        }
    }

    private void UpdatePlayerCells()
    {
        foreach (var item in _allPlayerInfos)
        {
            _cellDictionary[item.Key].UpdateInfo(item.Value);
        }
    }


    public void AddPlayer(PlayerInfo playerInfo)
    {
        _allPlayerInfos.Add(playerInfo.ID, playerInfo);
        GameObject clone = Instantiate(_originCell);
        clone.transform.SetParent(_content, false);
        PlayerListCell Cell = clone.GetComponent<PlayerListCell>();
        Cell.Initial(playerInfo);

        _cellDictionary.Add(playerInfo.ID, Cell);
        //_cellList.Add(Cell);
        clone.SetActive(true);
    }


    private void OnReadyToggle(bool arg0)
    {
        _cellDictionary[NetworkManager.LocalClientId].SetReady(arg0);
        UpdatePlayerInfo(NetworkManager.LocalClientId, arg0);

        if (IsServer)
        {
            UpdateAllPlayerInfos();
        }
        else
        {
            //让服务器去通知其他客户端更新
            UpdateAllPlayerInfosServerRpc(_allPlayerInfos[NetworkManager.LocalClientId]);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void UpdateAllPlayerInfosServerRpc(PlayerInfo player)
    {
        _allPlayerInfos[player.ID] = player;
        _cellDictionary[player.ID].UpdateInfo(player);
        UpdateAllPlayerInfos();
    }


    void UpdatePlayerInfo(ulong id, bool isReady)
    {
        PlayerInfo info = _allPlayerInfos[id];
        info.IsReady = isReady;
        _allPlayerInfos[id] = info;
    }

    private void OnStartClick()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
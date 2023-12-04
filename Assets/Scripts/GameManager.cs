using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager :  NetworkBehaviour
{
    // Start is called before the first frame update
    public static GameManager Instance;

    public Dictionary<ulong, PlayerInfo> AllplayerInfos { get; private set; }
    
    
    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene(1);
        AllplayerInfos=new Dictionary<ulong, PlayerInfo>();
    }

    public void StartGame(Dictionary<ulong, PlayerInfo> playerInfos)
    {
        AllplayerInfos = playerInfos;
        UpdateAllPlayerInfos();
    }

    void UpdateAllPlayerInfos()
    {
        foreach (var item in AllplayerInfos)
        {
            
        }
    }
    

    public void LoadSence(string SenceName)
    {
        NetworkManager.SceneManager.LoadScene(SenceName,LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerListCell : MonoBehaviour
{
    private TMP_Text _name;
    private TMP_Text _ready;
    private TMP_Text _gender;
    public PlayerInfo PlayerInfo { get; private set; }
    

    public void Initial(PlayerInfo playerInfo)
    {
        PlayerInfo = playerInfo;
        _name = transform.Find("Name").GetComponent<TMP_Text>();
        _ready = transform. Find("Ready").GetComponent<TMP_Text>();
        _name.text = playerInfo.name;
        _ready.text = playerInfo.IsReady ? "准备" : "未准备";
        _gender = transform.Find("Gender").GetComponent<TMP_Text>();
        _gender.text = playerInfo.gender == 0 ?"男":"女";
    }

    public void UpdateInfo(PlayerInfo playerInfo)
    {
        PlayerInfo = playerInfo;
        _gender.text = PlayerInfo.gender == 0 ?"男":"女";
        _ready.text = PlayerInfo.IsReady ? "准备" : "未准备";
        _name.text =  playerInfo.name;
    }

    public void SetReady(bool isready)
    {
        _ready.text = isready ? "准备" : "未准备";
    }
    
    public void SetGender(int gender)
    {
        _gender.text = gender == 0 ?"男":"女";
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

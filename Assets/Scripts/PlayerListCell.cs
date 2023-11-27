using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerListCell : MonoBehaviour
{
    private TMP_Text _name;
    private TMP_Text _ready;
    

    public void Initial(ulong PlayerID,bool isReady)
    {
        _name = transform.Find("Name").GetComponent<TMP_Text>();
        _ready = transform. Find("Ready").GetComponent<TMP_Text>();
        _name.text = "玩家" + PlayerID;
        _ready.text = isReady ? "准备" : "未准备";
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

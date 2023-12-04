using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameCtrl : MonoBehaviour
{
    
    [SerializeField]Transform _canvas;
    TMP_InputField _input;
    RectTransform _content;
    GameObject _dialogCell;
    // Start is called before the first frame update
    void Start()
    {
        _input = _canvas.Find("Dialog/Input").GetComponent<TMP_InputField>();
        _content = _canvas.Find("Dialog/DialogPanel/Viewport/Content") as RectTransform;
        _dialogCell = _content.Find("Cell").gameObject;
        Button sendBtn = _canvas.Find("Dialog/SendBtn").GetComponent<Button>();
        sendBtn.onClick.AddListener(OnSendClick);
    }

    private void OnSendClick()
    {
        if (string.IsNullOrEmpty(_input.text))
        {
            return;
        }

        PlayerInfo playerInfo = GameManager.Instance.AllplayerInfos[NetworkManager.Singleton.LocalClientId];
        AddDialogCell(playerInfo.name,_input.text);
        
    }

    void AddDialogCell(string playerName,string content)
    {
        GameObject clone = Instantiate(_dialogCell);
        clone.transform.SetParent(_content, false);
        clone.AddComponent<DialogCell>().Initial(playerName, content);
        clone.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

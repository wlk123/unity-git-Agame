using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogCell : MonoBehaviour
{

    public void Initial(string playerName,string content)
    {
        TMP_Text nameText = transform.Find("Name").GetComponent<TMP_Text>() ;
        nameText.text = playerName;
        TMP_Text contentText = transform.Find("Text").GetComponent<TMP_Text>();
        contentText.text = content;
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

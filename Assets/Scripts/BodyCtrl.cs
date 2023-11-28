using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCtrl : MonoBehaviour
{
    // Start is called before the first frame update

    public static BodyCtrl Instance;
    
    void Start()
    {
        Instance = this;
    }

    public void SwitchGender(int Gender)
    {
        transform.GetChild(Gender).gameObject.SetActive(true);
        transform.GetChild(1-Gender).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

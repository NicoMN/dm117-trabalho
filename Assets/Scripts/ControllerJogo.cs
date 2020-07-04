using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerJogo : MonoBehaviour
{

    public static int gems = 0;

    public static void UpdateGems()
    {
        gems++;
        Text gemCount = GameObject.Find("GemTxt").gameObject.GetComponent<Text>();
        var gemCurrent = string.Format("Gems: {0:D1}", gems);
        gemCount.text = gemCurrent;
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

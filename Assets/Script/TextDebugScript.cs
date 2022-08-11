using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class TextDebugScript : MonoBehaviour
{
    public Text text1;
    public double _WalkPower;
    public double _data_walkPower;
    // Start is called before the first frame update
    void Start()
    {
        text1.text = "walk power =";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void textup()
    {
        text1.text = "walk power =" ;
    }

    [DllImport("WalkerBase", CallingConvention = CallingConvention.Cdecl)]
    static extern bool GetWalkerData(int index, ref int bodyyaw, ref double walkpower, ref int movedirection, ref int ismoving, ref float distancer);
}

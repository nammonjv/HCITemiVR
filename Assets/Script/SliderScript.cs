using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider mainSlider;
    public Text text1;

    //Invoked when a submit button is clicked.
    public void SubmitSliderSetting()
    {
        text1.text = mainSlider.value.ToString();

    }
}

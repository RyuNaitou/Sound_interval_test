using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    public SoundGenerator soundGenerator;
    public TextMeshProUGUI numberText;

    public void wrapperChangeSoundNum()
    {
        int sliderValue = Mathf.RoundToInt(this.GetComponent<Slider>().value);
        soundGenerator.changeSoundNum(sliderValue);
        numberText.text = (sliderValue.ToString());
    }
    public void wrapperChangePresentInterval()
    {
        int sliderValue = Mathf.RoundToInt(this.GetComponent<Slider>().value);
        soundGenerator.changePresentInterval(sliderValue);
        numberText.text = (sliderValue * 0.01f).ToString("f2");
    }
}

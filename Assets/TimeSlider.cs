using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSlider : MonoBehaviour {

    [SerializeField] Toggle toggle; 
    [SerializeField] Slider slider;

    [SerializeField] CreateFlightRoot flightRoot;

    void Awake() {
        flightRoot.OnTimeChange += (double value) => {
            slider.normalizedValue = (float)value;
        };
        toggle.onValueChanged.AddListener(OnToggle);
        slider.onValueChanged.AddListener(OnValueChange);
    }

	// Use this for initialization
    public void OnToggle (bool toggleState) {
        flightRoot.playing = toggleState;
	}
	
	// Update is called once per frame
    public void OnValueChange (float value) {
        flightRoot.SetTime((value-slider.minValue)/(slider.maxValue - slider.minValue));
	}
}

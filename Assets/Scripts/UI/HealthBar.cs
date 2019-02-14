using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public float barDisplay; 

    // Start is called before the first frame update
    void Start()
    {
        healthSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        Mathf.Lerp(0f, 100f, barDisplay);
        barDisplay += 0.1f * Time.deltaTime;
    }
    private void OnGUI()
    {

        healthSlider.value = barDisplay;
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider healthSlider;
    public float amount; 

    // Start is called before the first frame update
    void Start()
    {
        healthSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Displaying amount: " + amount);
        healthSlider.value = amount;
    }
    private void OnGUI()
    {


    }



}

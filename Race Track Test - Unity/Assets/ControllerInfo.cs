using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerInfo : MonoBehaviour
{
    public Slider pedal;
    public Slider volante;
    public CarController car;

    void Update()
    {
        volante.value = car.horizontalInput;
        pedal.value = car.verticalInput;
    }
}

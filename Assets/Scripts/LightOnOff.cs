using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOnOff : MonoBehaviour
{
    Light microwavelight;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            microwavelight.enabled = !microwavelight.enabled;
        }
    }
}


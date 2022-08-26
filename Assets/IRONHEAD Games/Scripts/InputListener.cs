using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputListener : MonoBehaviour
{
    private List<InputDevice> inputDevices;

    private InputDeviceCharacteristics _inputDeviceCharacteristics;

    public XRNode controllerNode;

    private void Awake()
    {
        inputDevices = new List<InputDevice>();
    }

    void Update()
    {
        _inputDeviceCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand |
                                      InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left;

        InputDevices.GetDevicesAtXRNode(controllerNode, inputDevices);
        
        foreach (InputDevice i in inputDevices)
        {
            bool inputValue;
            if (i.TryGetFeatureValue(CommonUsages.primaryButton, out inputValue) && inputValue)
            {
                Debug.Log("You pressed the primary button");
            }
           
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginUIManager : MonoBehaviour
{
    [SerializeField] private GameObject ConnectOptionPanel;

    [SerializeField] private GameObject ConnectWithNamePanel;
    // Start is called before the first frame update
    void Start()
    {
        ConnectOptionPanel.SetActive(true);
        ConnectWithNamePanel.SetActive(false);
    }

}

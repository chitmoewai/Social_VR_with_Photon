using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public GameObject VRMenu_GameObject;
    public Button GoHome_Button;
    
    // Start is called before the first frame update
    void Start()
    {
        VRMenu_GameObject.SetActive(false);
        GoHome_Button.onClick.AddListener(VirtualWorldManager.Instance.LeaveRoomAndLoadHomeScene);
    }
    
}

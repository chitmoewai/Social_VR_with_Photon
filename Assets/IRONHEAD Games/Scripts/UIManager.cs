using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public GameObject UI_VRMenuGameObject;

    public GameObject UI_OpenWorldsGameObject;
    // Start is called before the first frame update
    void Start()
    {
        UI_VRMenuGameObject.SetActive(false);
        UI_OpenWorldsGameObject.SetActive(false);
    }

    public void OnWorldButtonClicked()
    {
        Debug.Log("World button is clicked");
        if (UI_OpenWorldsGameObject != null)
        {
            UI_OpenWorldsGameObject.SetActive(true);
        }
    }

    public void OnGoHomeButtonClicked()
    {
        Debug.Log("Go Home button is clicked");
    }

    public void OnChangeAvatarButtonClicked()
    {
        Debug.Log("Change Avatar button is clicked");
        AvatarSelectionManager.Instance.ActivateAvatarSelectionPlatform();
    }
}

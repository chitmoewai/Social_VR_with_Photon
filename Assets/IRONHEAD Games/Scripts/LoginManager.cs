using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using  TMPro;

public class LoginManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField PlayerName_InputField;
    void Start()
    {
        //PhotonNetwork.ConnectUsingSettings();
    }

    public void ConnectToPhotonServer()
    {
        if (PlayerName_InputField != null)
        {
            PhotonNetwork.NickName = PlayerName_InputField.text;
            PhotonNetwork.ConnectUsingSettings(); // btn anonymously connect linked
        }
      
    }
    public override void OnConnected()
    {
        Debug.Log("On connected is called. The Server is available.");
        
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to the master server with player name "+ PhotonNetwork.NickName);
        PhotonNetwork.LoadLevel("HomeSceneCM");
    }
}

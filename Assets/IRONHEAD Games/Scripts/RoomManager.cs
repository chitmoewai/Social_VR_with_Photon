using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Random = UnityEngine.Random;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI OccupancyRateText_ForSchool;
    [SerializeField] private TextMeshProUGUI OccupancyRateText_ForOutdoor;
    
    private string mapType;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        //if (PhotonNetwork.IsConnectedAndReady)
        //{
          //  PhotonNetwork.JoinLobby();
        //}
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PhotonNetwork.JoinLobby();
        }
    }

    #region UI CallBack Methods

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnEnterRoomButtonClicked_Outdoor()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_OUTDOOR;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable(){{MultiplayerVRConstants.MAP_TYPE_KEY, mapType}};
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    public void OnEnterRoomButtonClicked_School()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_SCHOOL;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable(){{MultiplayerVRConstants.MAP_TYPE_KEY, mapType}};
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }
    #endregion

    #region Photon CallBack Methods

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        CreateAndJoinRoom();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("A room is created with the name : " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server again");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("The local Player" + PhotonNetwork.NickName + "joined to" + PhotonNetwork.CurrentRoom.Name + "Player Count:"+ PhotonNetwork.CurrentRoom.PlayerCount);
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(MultiplayerVRConstants.MAP_TYPE_KEY))
        {
            object maptype;
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(MultiplayerVRConstants.MAP_TYPE_KEY, out maptype))
            {
                Debug.Log("Joined room with the map:"+ (string)maptype);
                if ((string) maptype == MultiplayerVRConstants.MAP_TYPE_VALUE_SCHOOL)
                {
                    //Load School Scene
                    PhotonNetwork.LoadLevel("World_SchoolCM");
                }
                else if((string) maptype == MultiplayerVRConstants.MAP_TYPE_VALUE_OUTDOOR)
                {
                    //Load Outdoor Scene
                    PhotonNetwork.LoadLevel("World_OutdoorCM");
                }
            }

        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + "joined to:"+"Player Count:" + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (roomList.Count == 0)
        {
            OccupancyRateText_ForSchool.text = 0 + " / " + 20;
            OccupancyRateText_ForOutdoor.text = 0 + " / " + 20;
        }
        foreach (RoomInfo room in roomList)
        {
            Debug.Log(room.Name);
            if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_OUTDOOR))
            {
                //Update outdoor room map
                Debug.Log("Room is a OUTDOOR map. Player Count is:"+ room.PlayerCount);
                OccupancyRateText_ForOutdoor.text = room.PlayerCount + " / " + 20;
            }
            else if(room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_SCHOOL))
            {
                // Update school room map
                Debug.Log("Room is a SCHOOL map. Player Count is:"+ room.PlayerCount);
                OccupancyRateText_ForSchool.text = room.PlayerCount + " / " + 20;
            }
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined to lobby.");
    }

    #endregion

    
    private void CreateAndJoinRoom()
    {
        string randomRoomName = "Room_"+ mapType + Random.Range(0, 10000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;

        string[] roomPropsInLobby = {MultiplayerVRConstants.MAP_TYPE_KEY};
        //There are two different maps : Outdoor and school;
        //1. Outdoor = "outdoor"
        //2. School = "school"

        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable(){{MultiplayerVRConstants.MAP_TYPE_KEY, mapType}};
        roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
        roomOptions.CustomRoomProperties = customRoomProperties;
        
        PhotonNetwork.CreateRoom(randomRoomName , roomOptions);
    }
}

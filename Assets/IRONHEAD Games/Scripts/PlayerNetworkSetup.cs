using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class PlayerNetworkSetup : MonoBehaviourPunCallbacks
{
   public GameObject LocalXRRigGameObject;
   public GameObject MainAvatarGameObject;

   public GameObject AvatarHeadGameObject;
   public GameObject AvatarBodyGameObject;

   public GameObject[] AvatarModelPrefabs;

    public TextMeshProUGUI PlayerName_Text;
   private void Start()
   {
      if (photonView.IsMine)
      {
         // The player is local
         LocalXRRigGameObject.SetActive(true);
         gameObject.GetComponent<MovementController>().enabled = true;
         gameObject.GetComponent<AvatarInputConverter>().enabled = true;
         
         // Getting the correct avatar selection data so that the correct avatar models can be instantiated.
         object avatarSelectionNumber;
         if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerVRConstants.AVATAR_SELECTION_NUMBER,
            out avatarSelectionNumber))
         {
            Debug.Log("Avatar selection number : " + (int) avatarSelectionNumber);
            photonView.RPC("InitializeSelectedAvatarModel", RpcTarget.AllBuffered , (int) avatarSelectionNumber);
         }
         
         SetLayerRecursively(AvatarHeadGameObject, 11);
         SetLayerRecursively(AvatarBodyGameObject, 12);

         TeleportationArea[] teleportationAreas = FindObjectsOfType<TeleportationArea>();
         if (teleportationAreas.Length > 0)
         {
            Debug.Log("Found" + teleportationAreas.Length + " teleportation area.");
            foreach (var item in teleportationAreas)
            {
               item.teleportationProvider = LocalXRRigGameObject.GetComponent<TeleportationProvider>();
            }
         }
         
         MainAvatarGameObject.AddComponent<AudioListener>();
      }
      else
      {
         // The player is remote
         LocalXRRigGameObject.SetActive(false);
         gameObject.GetComponent<MovementController>().enabled = false;
         gameObject.GetComponent<AvatarInputConverter>().enabled = false;
         
         // Remote player can be seen by local player
         // So, we set avatar head and body to Default Layer
         SetLayerRecursively(AvatarHeadGameObject, 0);
         SetLayerRecursively(AvatarBodyGameObject, 0);
         
      }

      if(PlayerName_Text != null){

            PlayerName_Text.text = photonView.Owner.NickName;
      }
   }
   void SetLayerRecursively(GameObject go, int layerNumber)
   {
      if (go == null) return;
      foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
      {
         trans.gameObject.layer = layerNumber;
      }
   }
   [PunRPC]
   public void InitializeSelectedAvatarModel(int avatarSelectionNumber)
   {
      GameObject selectedAvatarGameobject = Instantiate(AvatarModelPrefabs[avatarSelectionNumber], LocalXRRigGameObject.transform);

      AvatarInputConverter avatarInputConverter = transform.GetComponent<AvatarInputConverter>();
      AvatarHolder avatarHolder = selectedAvatarGameobject.GetComponent<AvatarHolder>();
      SetUpAvatarGameobject(avatarHolder.HeadTransform,avatarInputConverter.AvatarHead);
      SetUpAvatarGameobject(avatarHolder.BodyTransform,avatarInputConverter.AvatarBody);
      SetUpAvatarGameobject(avatarHolder.HandLeftTransform, avatarInputConverter.AvatarHand_Left);
      SetUpAvatarGameobject(avatarHolder.HandRightTransform, avatarInputConverter.AvatarHand_Right);
   }

   void SetUpAvatarGameobject(Transform avatarModelTransform, Transform mainAvatarTransform)
   {
      avatarModelTransform.SetParent(mainAvatarTransform);
      avatarModelTransform.localPosition = Vector3.zero;
      avatarModelTransform.localRotation = Quaternion.identity;
   }
   
}

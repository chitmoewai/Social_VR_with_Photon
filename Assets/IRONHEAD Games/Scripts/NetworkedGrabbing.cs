using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkedGrabbing : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    private PhotonView m_photonView;
    private Rigidbody rb;

    public bool isBeingHeld = false; // we can keep track if object is being held or not
    private void Awake()
    {
        m_photonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (isBeingHeld) // Then the Nerf Gun is begin grabbed.
        {
            rb.isKinematic = true;
            gameObject.layer = 13; // Change the layer to InHand
        }
        else
        {
            rb.isKinematic = false;
            gameObject.layer = 8; // Change the layer back to Interactable
        }
    }

    public void TransferOwnership()
    {
        m_photonView.RequestOwnership();
    }
    public void OnSelectEnter()
    {
        Debug.Log("Grabbed");
        m_photonView.RPC("StartNetworkedGrabbing", RpcTarget.AllBuffered);

        if (m_photonView.Owner == PhotonNetwork.LocalPlayer)
        {
            Debug.Log("we don't request the ownership. Already mine.");
        }
        else
        {
            TransferOwnership();
        }
       
    }

    public void OnSelectExit()
    {
        Debug.Log("Released");
        m_photonView.RPC("StopNetworkedGrabbing", RpcTarget.AllBuffered);
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView != m_photonView)
        {
            return;
        }
        Debug.Log("OnOwnerShip Requested for: " + targetView.name + "from" + requestingPlayer.NickName);
        m_photonView.TransferOwnership(requestingPlayer);
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
       Debug.Log("Transfer is complete. New owner: "+ targetView.Owner.NickName);
    }

    [PunRPC]
    public void StartNetworkedGrabbing()
    {
        isBeingHeld = true;
    }
    [PunRPC]
    public void StopNetworkedGrabbing()
    {
        isBeingHeld = false;
    }
}

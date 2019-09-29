﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

namespace Photon.Pun.Demo.PunBasics
{
    public class ScrollController : MonoBehaviourPunCallbacks
    {
        public string spellName;
        public string spellSchool;
        private string scrollID;
        private SpellListController spellList;

        PhotonView photonView;

        void Start()
        {
            //Change image based on spellSchool
            scrollID = System.Guid.NewGuid().ToString("N");
            photonView = gameObject.GetComponent<PhotonView>();
            spellList = GameObject.Find("Spell List").GetComponent<SpellListController>();
        }

        void Update()
        {

        }

        public string GetScrollID()
        {
            return scrollID;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.tag == "Player")
            {
                WizardPlayer player = other.gameObject.GetComponent<WizardPlayer>();
                if(player.GetPhotonView().IsMine)
                {
                    spellName = spellList.RandomScroll();
                    if(player.spellList.AddSpell(spellName))
                    {
                        player.ScrollPickup();
                        // PhotonNetwork.Destroy(this.gameObject);
                        photonView.RPC("DestroySelfRPC", RpcTarget.All);
                    }
                }
            }
        }

        // public void SetSpellName(string newSpellName)
        // {
        //     photonView.RPC("SetSpellNameRPC", RpcTarget.All, newSpellName);
        // }

        // [PunRPC]
        // public void SetSpellNameRPC(string newSpellName)
        // {
        //     Debug.Log("Setting spell name " + newSpellName);
        //     spellName = newSpellName; 
        // }

        [PunRPC]
        private void DestroySelfRPC()
        {
            Destroy(this.gameObject);
        } 
                            
    }

}

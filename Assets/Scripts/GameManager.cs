/*
 * Copyright (c) 2019 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

using Photon.Realtime;

namespace Photon.Pun.Demo.PunBasics
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public GameObject player1SpawnPosition;

        public GameObject player2SpawnPosition;
        public GameObject scrollPrefab;

        private GameObject player;
        private BoardManager boardScript;
        private Dictionary<string, bool> alivePlayers;
        private int playerNumber;

        private int layerMask;
        private SpellListController spellList;


        private Vector3[] spawnPositions;
        private int numberOfPlayers;
        public Text numberOfPlayersText;
        public GameObject winUI;
        public Text winText;

        // Start Method
        void Start()
        {
            layerMask = LayerMask.GetMask("Players", "Scrolls");
            spellList = GameObject.Find("Spell List").GetComponent<SpellListController>();
            if (!PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene("Launcher");
                return;
            }

            if (PlayerManager.LocalPlayerInstance == null)
            {

                numberOfPlayers = PhotonNetwork.PlayerList.Length;
                numberOfPlayersText.text = "Players Left: "  + numberOfPlayers;
                spawnPositions = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 9, 0), new Vector3(9, 0, 0), new Vector3(9, 9, 0)};
                alivePlayers = new Dictionary<string, bool>();
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    if(PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.NickName)
                    {
                        playerNumber = i;
                    }

                    alivePlayers.Add(PhotonNetwork.PlayerList[i].NickName, true);
                }

                player = PhotonNetwork.Instantiate("Player", spawnPositions[playerNumber], player1SpawnPosition.transform.rotation, 0);
                addScroll();
            }
        }

        // Update Method
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        void Awake()
        {
            boardScript = GetComponent<BoardManager>();
            InitGame();
        }

        void InitGame()
        {
            boardScript.SetupScene();
        }


        // Photon Methods
        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.Log("OnPlayerLeftRoom() " + other.NickName); // seen when other disconnects
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("Launcher");
            }
            ReducePlayers(other.NickName);
        }

        // Helper Methods
        public void QuitRoom()
        {
            Application.Quit();
        }

        public void KillPlayer()
        {
            Debug.Log("I am Dead");
            PhotonNetwork.Destroy(player);
        }

        public void ReducePlayers(string playerName)
        {
            numberOfPlayers--;
            numberOfPlayersText.text = "Players Left: "  + numberOfPlayers;
            
            alivePlayers[playerName] = false;

            if (numberOfPlayers <= 1)
            {
                winUI.SetActive(true);
                foreach (string alivePlayer in alivePlayers.Keys)
                {
                    if(alivePlayers[alivePlayer])
                    {
                        winText.text = alivePlayer + " wins!";
                    }
                }

            }
        }

        public void addScroll()
        {
            int x = Random.Range(0, boardScript.columns);
            int y = Random.Range(0, boardScript.rows);
            Vector3 position = new Vector3(x, y, 0);
            while (Physics2D.OverlapPoint(position, layerMask))
            {
                x = Random.Range(0, boardScript.columns);
                y = Random.Range(0, boardScript.rows);
                position = new Vector3(x, y, 0);
            }
            GameObject scroll = Instantiate(scrollPrefab, position, Quaternion.identity);
            scroll.GetComponent<ScrollController>().spellName = spellList.RandomScroll();
        }
       
    }
}

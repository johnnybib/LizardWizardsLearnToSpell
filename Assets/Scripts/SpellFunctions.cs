using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Photon.Pun.Demo.PunBasics
{
    public class SpellFunctions : MonoBehaviour
    {
        private Vector2 playerPosition;
        private Rigidbody2D rigidBody; 
        // public ProjectileController[] projectiles;
        public string[] projectileNames = new string[] {"Fireball", "firebolt", "righteous-flare", "bigsnow", "lightningcloud", "righteous-pellet", "demon-fire", "air-blast"};

        public AudioClip[] sfx;
        private AudioSource audioSource;

        // Start is called before the first frame update
        void Start()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            audioSource = gameObject.GetComponent<AudioSource>();
        }

        public void FireSpell (string spellName)
        {
            
            playerPosition = transform.position;
            switch(spellName)
            {
            case "cone of frost":
                ConeSpell(3, 0); //ice cone
                break;
            case "save the amazon":
                CircleSpell(0, 3);
                break;
            case "fire blast":
                LineSpell(1, 0);
                break;
            case "righteous flare":
                CircleSpell(2, 1);
                break;
            case "cone of flame":
                ConeSpell(0, 3); //fire cone
                break;
            case "it's going to rain":
                LingeringAoeSpell(4, 1);
                break;
            case "sear":
                SmallSelfAoeSpell(6, 0); //change to 6
                break;
            case "let there be light":
                LargeSelfAoeSpell(2, 4);
                break;
            case "fear":
                XSelfAoeSpell(6, 4);
                break;
            case "sonic boom":
                LineSpell(7, 2);
                break;
            }

        }

        void ConeSpell(int prefabId, int sfxId)
        {
            int damage = 1;
            float duration = 0.75f;
            string spellType = "projectile";
            Quaternion playerDirection = transform.rotation; //CHANGE THIS LATER
            int direction = this.GetComponentInParent<WizardPlayer>().direction;
            Dictionary<int, int[]> projectileSettings = new Dictionary<int, int[]>()
            {
                {0, new int[] {0, 4, 0, 1, damage} },
                {1, new int[] {1, 4, 0, 2, damage} },
                {2, new int[] {1, 4, 1, 2, damage} },
                {3, new int[] {1, 4, -1, 2, damage} },
                {4, new int[] {2, 4, 0, 3, damage} },
                {5, new int[] {2, 4, 1, 3, damage} },
                {6, new int[] {2, 4, -1, 3, damage} },
                {7, new int[] {2, 4, 2, 3, damage} },
                {8, new int[] {2, 4, -2, 3, damage} },
            };
            //Key Value: [startTime, endTime, xdisplacement, ydisplacement, damage, duration]

            int lastSetting = projectileSettings[0].Length - 2;
            
            if (spellType == "projectile") //check if spell is projectile
            {
                int[] endTimes = new int[projectileSettings.Count];
                int[] startTimes = new int[projectileSettings.Count];
                int j = 0;
                foreach(int item in projectileSettings.Keys) {
                    endTimes[0] = projectileSettings[item][1];
                    startTimes[0] = projectileSettings[item][0];
                    j++;
                }
                int maxEnd = endTimes.Max();
                int maxStart = startTimes.Max();
                audioSource.clip = sfx[sfxId];
                audioSource.Play();
                StartCoroutine(ProjectileTiming(maxEnd, maxStart));
            }
            
            IEnumerator ProjectileTiming(int maxEnd, int maxStart)
            {
                for (int k = 0; k < maxEnd + 1; k++)
                {
                foreach(int item in projectileSettings.Keys)
                {
                    if (k == projectileSettings[item][0]) {
                        int[] settings = projectileSettings[item];
                        GameObject fireball = PhotonNetwork.Instantiate(projectileNames[prefabId],
                            playerPosition,
                            playerDirection
                            );
                        Vector2 displacement = new Vector3(settings[2], settings[3], 0f);
                        if (direction == 1)
                        {
                                displacement = -1*Vector2.Perpendicular(displacement);
                        }
                        if (direction == 2)
                        {
                                displacement = -1*displacement;
                        }
                        if (direction == 3)
                        {
                                displacement = Vector2.Perpendicular(displacement);
                        }

                        fireball.GetComponent<ProjectileController>().Shoot(settings[0], settings[1], displacement, settings[4], duration, maxStart, maxEnd);
                    }
                }
                yield return new WaitForSeconds(duration/(maxStart + 1));
                }  
            }
        }

        void CircleSpell(int prefabId, int sfxId)
        {
            int damage = 1;
            float duration = 1.5f;
            //int prefabId = 0;
            string spellType = "projectile";
            Quaternion playerDirection = transform.rotation; //CHANGE THIS LATER
            int direction = this.GetComponentInParent<WizardPlayer>().direction;
            Dictionary<int, int[]> projectileSettings = new Dictionary<int, int[]>()
            {
                //Initial Ring
                {0, new int[] {0, 1, 0, 1, damage} },
                {1, new int[] {0, 1, 1, 1, damage} },
                {2, new int[] {0, 1, 1, 0, damage} },
                {3, new int[] {0, 1, 1, -1, damage} },
                {4, new int[] {0, 1, 0, -1, damage} },
                {5, new int[] {0, 1, -1, -1, damage} },
                {6, new int[] {0, 1, -1, 0, damage} },
                {7, new int[] {0, 1, -1, 1, damage} },
                //Outer Ring
                {8, new int[] {1, 2, 0, 2, damage} },
                {9, new int[] {1, 2, 2, 2, damage} },
                {10, new int[] {1, 2, 2, 0, damage} },
                {11, new int[] {1, 2, 2, -2, damage} },
                {12, new int[] {1, 2, 0, -2, damage} },
                {13, new int[] {1, 2, -2, -2, damage} },
                {14, new int[] {1, 2, -2, 0, damage} },
                {15, new int[] {1, 2, -2, 2, damage} },

            };
            //Key Value: [startTime, endTime, xdisplacement, ydisplacement, damage, duration]

            int lastSetting = projectileSettings[0].Length - 2;
            
            if (spellType == "projectile") //check if spell is projectile
            {
                int[] endTimes = new int[projectileSettings.Count];
                int[] startTimes = new int[projectileSettings.Count];
                int j = 0;
                foreach(int item in projectileSettings.Keys) {
                    endTimes[0] = projectileSettings[item][1];
                    startTimes[0] = projectileSettings[item][0];
                    j++;
                }
                int maxEnd = endTimes.Max();
                int maxStart = startTimes.Max();
                audioSource.clip = sfx[sfxId];
                audioSource.Play();
                StartCoroutine(ProjectileTiming(maxEnd, maxStart));
            }
            
            IEnumerator ProjectileTiming(int maxEnd, int maxStart)
            {
                for (int k = 0; k < maxEnd + 1; k++)
                {
                foreach(int item in projectileSettings.Keys)
                {
                    if (k == projectileSettings[item][0]) {
                        int[] settings = projectileSettings[item];
                        GameObject fireball = PhotonNetwork.Instantiate(projectileNames[prefabId],
                            playerPosition,
                            playerDirection
                            );
                        Vector2 displacement = new Vector3(settings[2], settings[3], 0f);
                        if (direction == 1)
                        {
                                displacement = -1*Vector2.Perpendicular(displacement);
                        }
                        if (direction == 2)
                        {
                                displacement = -1*displacement;
                        }
                        if (direction == 3)
                        {
                                displacement = Vector2.Perpendicular(displacement);
                        }

                        fireball.GetComponent<ProjectileController>().Shoot(settings[0], settings[1], displacement, settings[4], duration, maxStart, maxEnd);
                    }
                }
                yield return new WaitForSeconds(duration/(maxStart + 1));
                }  
            }
        }

        void LineSpell(int prefabId, int sfxId)
        {
            int damage = 1;
            float duration = 1f;
            //int prefabId = 0;
            string spellType = "projectile";
            Quaternion playerDirection = transform.rotation; //CHANGE THIS LATER
            int direction = this.GetComponentInParent<WizardPlayer>().direction;
            Dictionary<int, int[]> projectileSettings = new Dictionary<int, int[]>()
            {
                {0, new int[] {1, 1, 0, 1, damage} },
                {1, new int[] {2, 2, 0, 2, damage} },
                {2, new int[] {3, 3, 0, 3, damage} },
                {3, new int[] {4, 4, 0, 4, damage} },
                {4, new int[] {5, 5, 0, 5, damage} },
                {5, new int[] {6, 6, 0, 6, damage} },
            };
            //Key Value: [startTime, endTime, xdisplacement, ydisplacement, damage, duration]

            int lastSetting = projectileSettings[0].Length - 2;
            
            if (spellType == "projectile") //check if spell is projectile
            {
                int[] endTimes = new int[projectileSettings.Count];
                int[] startTimes = new int[projectileSettings.Count];
                int j = 0;
                foreach(int item in projectileSettings.Keys) {
                    endTimes[0] = projectileSettings[item][1];
                    startTimes[0] = projectileSettings[item][0];
                    j++;
                }
                int maxEnd = endTimes.Max();
                int maxStart = startTimes.Max();
                audioSource.clip = sfx[sfxId];
                audioSource.Play();
                StartCoroutine(ProjectileTiming(maxEnd, maxStart));
            }
            
            IEnumerator ProjectileTiming(int maxEnd, int maxStart)
            {
                for (int k = 0; k < maxEnd + 1; k++)
                {
                foreach(int item in projectileSettings.Keys)
                {
                    if (k == projectileSettings[item][0]) {
                        int[] settings = projectileSettings[item];
                        Vector2 displacement = new Vector3(settings[2], settings[3], 0f);
                        Quaternion spriteRotation;
                        if (direction == 1)
                        {
                                displacement = -1*Vector2.Perpendicular(displacement);
                                spriteRotation = playerDirection;
                        }
                        else if (direction == 2)
                        {
                                displacement = -1*displacement;
                                spriteRotation = Quaternion.Euler(0f, 0f, -90f);
                        }
                        else if (direction == 3)
                        {
                                displacement = Vector2.Perpendicular(displacement);
                                spriteRotation = Quaternion.Euler(0f, 0f, 180f);
                        }
                        else
                        {
                            spriteRotation = Quaternion.Euler(0f, 0f, 90f);
                        }
                        GameObject fireball = PhotonNetwork.Instantiate(projectileNames[prefabId],
                            playerPosition,
                            spriteRotation
                            );
                        fireball.GetComponent<ProjectileController>().Shoot(settings[0], settings[1], displacement, settings[4], duration, maxStart, maxEnd);
                    }
                }
                yield return new WaitForSeconds(duration/(maxStart + 1));
                }  
            }
        }
        void LingeringAoeSpell(int prefabId, int sfxId)
        {
            int damage = 1;
            float duration = 5f;
            string spellType = "projectile";
            Quaternion playerDirection = transform.rotation; //CHANGE THIS LATER
            int direction = this.GetComponentInParent<WizardPlayer>().direction;
            Dictionary<int, int[]> projectileSettings = new Dictionary<int, int[]>()
            {
                {0, new int[] {0, 1, 0, 3, damage} },
                {1, new int[] {0, 1, 1, 3, damage} },
                {2, new int[] {0, 1, -1, 3, damage} },
                {3, new int[] {0, 1, 0, 4, damage} },
                {4, new int[] {0, 1, 1, 4, damage} },
                {5, new int[] {0, 1, -1, 4, damage} },
                {6, new int[] {0, 1, 0, 5, damage} },
                {7, new int[] {0, 1, 1, 5, damage} },
                {8, new int[] {0, 1, -1, 5, damage} },
            };
            //Key Value: [startTime, endTime, xdisplacement, ydisplacement, damage, duration]

            int lastSetting = projectileSettings[0].Length - 2;
            
            if (spellType == "projectile") //check if spell is projectile
            {
                int[] endTimes = new int[projectileSettings.Count];
                int[] startTimes = new int[projectileSettings.Count];
                int j = 0;
                foreach(int item in projectileSettings.Keys) {
                    endTimes[0] = projectileSettings[item][1];
                    startTimes[0] = projectileSettings[item][0];
                    j++;
                }
                int maxEnd = endTimes.Max();
                int maxStart = startTimes.Max();
                audioSource.clip = sfx[sfxId];
                audioSource.Play();
                StartCoroutine(ProjectileTiming(maxEnd, maxStart));
            }
            
            IEnumerator ProjectileTiming(int maxEnd, int maxStart)
            {
                for (int k = 0; k < maxEnd + 1; k++)
                {
                foreach(int item in projectileSettings.Keys)
                {
                    if (k == projectileSettings[item][0]) {
                        int[] settings = projectileSettings[item];
                        GameObject fireball = PhotonNetwork.Instantiate(projectileNames[prefabId],
                            playerPosition,
                            playerDirection
                            );
                        Vector2 displacement = new Vector3(settings[2], settings[3], 0f);
                        if (direction == 1)
                        {
                                displacement = -1*Vector2.Perpendicular(displacement);
                        }
                        if (direction == 2)
                        {
                                displacement = -1*displacement;
                        }
                        if (direction == 3)
                        {
                                displacement = Vector2.Perpendicular(displacement);
                        }

                        fireball.GetComponent<ProjectileController>().Shoot(settings[0], settings[1], displacement, settings[4], duration, maxStart, maxEnd);
                    }
                }
                yield return new WaitForSeconds(duration/(maxStart + 1));
                }  
            }
        }

        void SmallSelfAoeSpell(int prefabId, int sfxId)
        {
            int damage = 1;
            float duration = 0.5f;
            //int prefabId = 0;
            string spellType = "projectile";
            Quaternion playerDirection = transform.rotation; //CHANGE THIS LATER
            int direction = this.GetComponentInParent<WizardPlayer>().direction;
            Dictionary<int, int[]> projectileSettings = new Dictionary<int, int[]>()
            {
                //Initial Ring
                {0, new int[] {0, 1, 0, 1, damage} },
                {2, new int[] {0, 1, 1, 0, damage} },
                {3, new int[] {0, 1, -1, 0, damage} },
                {4, new int[] {0, 1, 0, -1, damage} },
            };
            //Key Value: [startTime, endTime, xdisplacement, ydisplacement, damage, duration]

            int lastSetting = projectileSettings[0].Length - 2;
            
            if (spellType == "projectile") //check if spell is projectile
            {
                int[] endTimes = new int[projectileSettings.Count];
                int[] startTimes = new int[projectileSettings.Count];
                int j = 0;
                foreach(int item in projectileSettings.Keys) {
                    endTimes[0] = projectileSettings[item][1];
                    startTimes[0] = projectileSettings[item][0];
                    j++;
                }
                int maxEnd = endTimes.Max();
                int maxStart = startTimes.Max();
                audioSource.clip = sfx[sfxId];
                audioSource.Play();
                StartCoroutine(ProjectileTiming(maxEnd, maxStart));
            }
            
            IEnumerator ProjectileTiming(int maxEnd, int maxStart)
            {
                for (int k = 0; k < maxEnd + 1; k++)
                {
                foreach(int item in projectileSettings.Keys)
                {
                    if (k == projectileSettings[item][0]) {
                        int[] settings = projectileSettings[item];
                        GameObject fireball = PhotonNetwork.Instantiate(projectileNames[prefabId],
                            playerPosition,
                            playerDirection
                            );
                        Vector2 displacement = new Vector3(settings[2], settings[3], 0f);
                        if (direction == 1)
                        {
                                displacement = -1*Vector2.Perpendicular(displacement);
                        }
                        if (direction == 2)
                        {
                                displacement = -1*displacement;
                        }
                        if (direction == 3)
                        {
                                displacement = Vector2.Perpendicular(displacement);
                        }

                        fireball.GetComponent<ProjectileController>().Shoot(settings[0], settings[1], displacement, settings[4], duration, maxStart, maxEnd);
                    }
                }
                yield return new WaitForSeconds(duration/(maxStart + 1));
                }  
            }
        }

        void LargeSelfAoeSpell(int prefabId, int sfxId)
        {
            int damage = 1;
            float duration = 0.75f;
            //int prefabId = 0;
            string spellType = "projectile";
            Quaternion playerDirection = transform.rotation; //CHANGE THIS LATER
            int direction = this.GetComponentInParent<WizardPlayer>().direction;
            Dictionary<int, int[]> projectileSettings = new Dictionary<int, int[]>()
            {
                //Initial Ring
                {0, new int[] {0, 1, 0, 1, damage} },
                {1, new int[] {0, 1, 1, 1, damage} },
                {2, new int[] {0, 1, 1, 0, damage} },
                {3, new int[] {0, 1, 1, -1, damage} },
                {4, new int[] {0, 1, 0, -1, damage} },
                {5, new int[] {0, 1, -1, -1, damage} },
                {6, new int[] {0, 1, -1, 0, damage} },
                {7, new int[] {0, 1, -1, 1, damage} },
                //Second Ring
                {8, new int[] {0, 1, 0, 2, damage} },
                {9, new int[] {0, 1, 1, 2, damage} },
                {10, new int[] {0, 1, -1, 2, damage} },
                {11, new int[] {0, 1, 0, -2, damage} },
                {12, new int[] {0, 1, 1, -2, damage} },
                {13, new int[] {0, 1, -1, -2, damage} },
                {14, new int[] {0, 1, 2, 0, damage} },
                {15, new int[] {0, 1, 2, 1, damage} },
                {16, new int[] {0, 1, 2, -1, damage} },
                {17, new int[] {0, 1, -2, 0, damage} },
                {18, new int[] {0, 1, -2, 1, damage} },
                {19, new int[] {0, 1, -2, -1, damage} },
                //Third Ring
                {20, new int[] {0, 1, -3, 0, damage} },
                {21, new int[] {0, 1, 3, 0, damage} },
                {22, new int[] {0, 1, 0, -3, damage} },
                {23, new int[] {0, 1, 0, 3, damage} },
            };
            //Key Value: [startTime, endTime, xdisplacement, ydisplacement, damage, duration]

            int lastSetting = projectileSettings[0].Length - 2;
            
            if (spellType == "projectile") //check if spell is projectile
            {
                int[] endTimes = new int[projectileSettings.Count];
                int[] startTimes = new int[projectileSettings.Count];
                int j = 0;
                foreach(int item in projectileSettings.Keys) {
                    endTimes[0] = projectileSettings[item][1];
                    startTimes[0] = projectileSettings[item][0];
                    j++;
                }
                int maxEnd = endTimes.Max();
                int maxStart = startTimes.Max();
                audioSource.clip = sfx[sfxId];
                audioSource.Play();
                StartCoroutine(ProjectileTiming(maxEnd, maxStart));
            }
            
            IEnumerator ProjectileTiming(int maxEnd, int maxStart)
            {
                for (int k = 0; k < maxEnd + 1; k++)
                {
                foreach(int item in projectileSettings.Keys)
                {
                    if (k == projectileSettings[item][0]) {
                        int[] settings = projectileSettings[item];
                        GameObject fireball = PhotonNetwork.Instantiate(projectileNames[prefabId],
                            playerPosition,
                            playerDirection
                            );
                        Vector2 displacement = new Vector3(settings[2], settings[3], 0f);
                        if (direction == 1)
                        {
                                displacement = -1*Vector2.Perpendicular(displacement);
                        }
                        if (direction == 2)
                        {
                                displacement = -1*displacement;
                        }
                        if (direction == 3)
                        {
                                displacement = Vector2.Perpendicular(displacement);
                        }

                        fireball.GetComponent<ProjectileController>().Shoot(settings[0], settings[1], displacement, settings[4], duration, maxStart, maxEnd);
                    }
                }
                yield return new WaitForSeconds(duration/(maxStart + 1));
                }
            }
        }
        void XSelfAoeSpell(int prefabId, int sfxId)
        {
            int damage = 1;
            float duration = 0.5f;
            //int prefabId = 0;
            string spellType = "projectile";
            Quaternion playerDirection = transform.rotation; //CHANGE THIS LATER
            int direction = this.GetComponentInParent<WizardPlayer>().direction;
            Dictionary<int, int[]> projectileSettings = new Dictionary<int, int[]>()
            {
                //Initial Ring
                {0, new int[] {0, 1, 1, 1, damage} },
                {2, new int[] {0, 1, 1, -1, damage} },
                {3, new int[] {0, 1, -1, -1, damage} },
                {4, new int[] {0, 1, -1, 1, damage} },
            };
            //Key Value: [startTime, endTime, xdisplacement, ydisplacement, damage, duration]

            int lastSetting = projectileSettings[0].Length - 2;
            
            if (spellType == "projectile") //check if spell is projectile
            {
                int[] endTimes = new int[projectileSettings.Count];
                int[] startTimes = new int[projectileSettings.Count];
                int j = 0;
                foreach(int item in projectileSettings.Keys) {
                    endTimes[0] = projectileSettings[item][1];
                    startTimes[0] = projectileSettings[item][0];
                    j++;
                }
                int maxEnd = endTimes.Max();
                int maxStart = startTimes.Max();
                audioSource.clip = sfx[sfxId];
                audioSource.Play();
                StartCoroutine(ProjectileTiming(maxEnd, maxStart));
            }
            
            IEnumerator ProjectileTiming(int maxEnd, int maxStart)
            {
                for (int k = 0; k < maxEnd + 1; k++)
                {
                foreach(int item in projectileSettings.Keys)
                {
                    if (k == projectileSettings[item][0]) {
                        int[] settings = projectileSettings[item];
                        GameObject fireball = PhotonNetwork.Instantiate(projectileNames[prefabId],
                            playerPosition,
                            playerDirection
                            );
                        Vector2 displacement = new Vector3(settings[2], settings[3], 0f);
                        if (direction == 1)
                        {
                                displacement = -1*Vector2.Perpendicular(displacement);
                        }
                        if (direction == 2)
                        {
                                displacement = -1*displacement;
                        }
                        if (direction == 3)
                        {
                                displacement = Vector2.Perpendicular(displacement);
                        }

                        fireball.GetComponent<ProjectileController>().Shoot(settings[0], settings[1], displacement, settings[4], duration, maxStart, maxEnd);
                    }
                }
                yield return new WaitForSeconds(duration/(maxStart + 1));
                }  
            }
        }

    }

}

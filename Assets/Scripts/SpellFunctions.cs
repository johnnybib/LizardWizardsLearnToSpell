using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpellFunctions : MonoBehaviour
{
    private Vector2 playerPosition;
    private Rigidbody2D rigidBody; 
    public ProjectileController[] projectiles;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            playerPosition = transform.position;
            Spell1();
        }
    }

    void Spell1()
    {
        int damage = 1;
        int duration = 1;
        int prefabId = 0;
        string spellType = "projectile";
        Quaternion playerDirection = transform.rotation; //CHANGE THIS LATER
        Dictionary<int, int[]> projectileSettings = new Dictionary<int, int[]>()
        {
            {0, new int[] {1, 4, 0, 1, damage, duration} },
            {1, new int[] {2, 4, 0, 2, damage, duration} },
            {2, new int[] {2, 4, 1, 2, damage, duration} },
            {3, new int[] {2, 4, -1, 2, damage, duration} },
            {4, new int[] {3, 4, 0, 3, damage, duration} },
            {5, new int[] {3, 4, 1, 3, damage, duration} },
            {6, new int[] {3, 4, -1, 3, damage, duration} },
            {7, new int[] {3, 4, 2, 3, damage, duration} },
            {8, new int[] {3, 4, -2, 3, damage, duration} },
        };
        //Key Value: [startTime, endTime, xdisplacement, ydisplacement, damage, duration]

        int lastSetting = projectileSettings[0].Length - 2;
        
        if (spellType == "projectile") //check if spell is projectile
        {
            int[] endTimes = new int[projectileSettings.Count];
            int j = 0;
            foreach(int item in projectileSettings.Keys) {
                endTimes[0] = projectileSettings[item][1];
                j++;
            }
            // for (int i = 0; i < projectileSettings.Count; i++) {
            //     endTimes[i] = projectileSettings[i][1];
            //     print(i);
            // }
            for (int k = 0; k < endTimes.Max() + 1; k++)
            {
                foreach(int item in projectileSettings.Keys)
                {
                    if (k == projectileSettings[item][0]) {
                        int[] settings = projectileSettings[item];
                        //int[] fullsettings = projectileSettings[item];
                        // int[] settings = new int[lastSetting];
                        // for (int i = 1; i < lastSetting; i++)
                        // {
                        //     settings[i-1] = fullsettings[i];
                        // }
                        ProjectileController fireball = Instantiate(
                            projectiles[prefabId],
                            playerPosition,
                            playerDirection
                            );
                        Vector2 displacement = new Vector3(settings[2], settings[3], 0f);
                        fireball.Shoot(settings[0], settings[1], displacement, settings[4], settings[5]);
                    }
                }
                //yield WaitForSeconds(5f);
            }                   
        }
    }

    IEnumerator Waiting(int duration) {
        yield return new WaitForSeconds(duration);
    }
    
}

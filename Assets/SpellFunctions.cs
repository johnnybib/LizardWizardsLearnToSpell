using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpellFunctions : MonoBehaviour
{
    public Vector2 playerPosition;
    private Rigidbody2D rigidBody; 
    public ProjectileController[] projectiles;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerPosition = rigidBody.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            Spell1();
        }
    }

    void Spell1()
    {
        int damage = 1;
        int speed = 4;
        int prefabId = 0;
        string spellType = "projectile";

        Dictionary<int, int[]> projectileSettings = new Dictionary<int, int[]>()
        {
            {1, new int[] {1, 4, 1, 1, damage, speed} },
        };
        //Key Value: [startTime, endTime, xdisplacement, ydisplacement, damage, speed]

        int lastSetting = projectileSettings[1].Length - 2;

        if (spellType == "projectile") //check if spell is projectile
        {
            int[] endTimes = new int[projectileSettings.Count];
            for (int i = 0; i < projectileSettings.Count; i++) {
                endTimes[i] = projectileSettings[i][1];
            }
            for (int k = 0; k < endTimes.Max(); k++)
            {
                foreach(int item in projectileSettings.Keys)
                {
                    if (k == projectileSettings[item][0]) {
                        int[] fullsettings = projectileSettings[item];
                        int[] settings = new int[lastSetting];
                        for (int i = 2; i < lastSetting; i++)
                        {
                            settings[i-2] = fullsettings[i];
                        }
                        projectiles[prefabId].Shoot(settings[0], settings[1], settings[2], settings[3]);
                    }
                    else if (k == projectileSettings[item][1]) {
                        Destroy(projectiles[prefabId]);
                    }
                }
            }                    
        }
    }
}

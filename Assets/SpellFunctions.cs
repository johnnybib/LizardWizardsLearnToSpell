using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        int speed = 40;
        int prefabId = 0;
        string spellType = "projectile";

        Dictionary<int, int[]> projectileSettings = new Dictionary<int, int[]>()
        {
            {1, new int[] {1, 2, 1, 1, damage, speed} },
        };
        //Key Value: [startTime, endTime, xdisplacement, ydisplacement, damage, speed]

        int lastSetting = projectileSettings[1].Length - 2;

        if (spellType == "projectile") //check if spell is projectile
        {
            foreach(int item in projectileSettings.Keys)
            {
                int[] fullsettings = projectileSettings[item];
                int[] settings = new int[lastSetting];
                for (int i = 2; i < lastSetting; i++)
                {
                    settings[i-2] = fullsettings[i];
                }
                projectiles[prefabId].Shoot(settings[0], settings[1], settings[2], settings[3]);
            }
        }
    }

}

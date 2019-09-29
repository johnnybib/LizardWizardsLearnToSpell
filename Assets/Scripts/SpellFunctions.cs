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
        // if (Input.GetKeyDown("k"))
        // {
        //     playerPosition = transform.position;
        //     Spell1();
        // }
    }

    public void FireSpell (string spellName)
    {
        
        playerPosition = transform.position;
        switch(spellName)
        {
        case "potato":
            FireCone();
            break;
        }
    }

    void FireCone()
    {
        int damage = 1;
        float duration = 0.75f;
        int prefabId = 0;
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
                    ProjectileController fireball = Instantiate(
                        projectiles[prefabId],
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

                    fireball.Shoot(settings[0], settings[1], displacement, settings[4], duration, maxStart, maxEnd);
                }
            }
            yield return new WaitForSeconds(duration/(maxStart + 1));
            }  
        }
    }    
}

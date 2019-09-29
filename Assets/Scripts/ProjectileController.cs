using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon;
using Photon.Pun;

public class ProjectileController : MonoBehaviour
{
    public float moveTime = 1f;
    private int spellDamage;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;
  
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
    }

    bool Move (int xDir, int yDir, out RaycastHit2D hit)
    {
       
            
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;
            
        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
        }
        return false;
    }

    void AttemptMove (int xDir, int yDir)
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        if (hit.transform == null)
        {
            return;
        }
    }
    IEnumerator SmoothMovement(Vector3 end)
    {
        float elapsedTime = 0;
        Vector3 startingPos = transform.position;
        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = end;

        // float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        // while (sqrRemainingDistance > float.Epsilon)
        // {
        //     Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
        //     rb2D.MovePosition(newPosition);
        //     sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        //     yield return null;
        // }
    }

    public void Shoot(int startTime, int endTime, Vector3 displacement, int damage, int duration)
    {
        spellDamage = damage;
        transform.position = transform.position + displacement;
        int deletionTime = endTime - startTime;
        StartCoroutine(Waiting(duration));

    }
    IEnumerator Waiting(int duration) {
        print("Waiting...");
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
    
}
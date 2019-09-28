﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon;
using Photon.Pun;

public abstract class MovingObject : MonoBehaviour
{
    public float moveTime = 0.01f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;

    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;


    }

    protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
    {
        
    
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);
        Debug.Log(start);
        Debug.Log(end);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;
        Debug.Log("----");
        if (hit.transform == null)
        {
            transform.position = end;
            return true;
            
            //StartCoroutine(SmoothMovement(end));
            //return true;
        }

        
        return false;

    }

    protected virtual void AttemptMove (int xDir, int yDir)
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        if (hit.transform == null)
        {
            return;
        }

        //T hitComponent = hit.transform.GetComponent<T>();

        //if (!canMove && hitComponent != null)
        //    OnCantMove(hitComponent);
    }
    protected IEnumerator SmoothMovement(Vector3 end)
    {
        //float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        //while (sqrRemainingDistance > float.Epsilon)
        //{
        //    Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
        //    rb2D.MovePosition(newPosition);
        //    sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        //    yield return null;
        //}
        yield return null;

    }

    protected abstract void OnCantMove<T>(T component)
        where T : Component; 

    
}
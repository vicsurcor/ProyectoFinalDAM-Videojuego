using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private List<Transform> encounterPoints;
    private Rigidbody2D rb;

    public float moveSpeed = 5f;
    private Transform currentTarget;
    public bool IsBranching = false;
    private bool IsJumping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Find all encounter points in the scene
        encounterPoints = FindObjectsOfType<Transform>().Where(t => t.name == "EncounterPoint").ToList();
        if (encounterPoints.Where(t => t.GetComponent<EncounterScript>().IsActive).Count() > 0)
        {
            FindNextTarget();
        }
        
    }

    void FixedUpdate()
    {
        if (currentTarget != null && IsBranching == false && IsJumping == false)
        {
            MoveTowardsTarget();
        }
        else if (IsJumping == true)
        {
            if (rb.velocity.y == 0)
            {
                IsJumping = false;
            }
        }
    }

    private void FindNextTarget()
    {
        currentTarget = encounterPoints
            .Where(t => t.GetComponent<EncounterScript>().IsActive)
            .OrderBy(t => Vector2.Distance(transform.position, t.position))
            .FirstOrDefault();
            Debug.Log("Moving towards: " + currentTarget.name + currentTarget.position.x);
    }

    private void MoveTowardsTarget()
    {
        float dir = currentTarget.position.x - transform.position.x;
        Vector2 direction = new(dir/Math.Abs(dir), 0);
        rb.velocity = direction * moveSpeed;

        // Check if the target has become inactive
        if (!currentTarget.GetComponent<EncounterScript>().IsActive && !(encounterPoints.Where(t => t.GetComponent<EncounterScript>().IsActive).Count() == 0))
        {
            FindNextTarget();
        }
    }
    public void BranchMovement(int input)
    {
        //TODO: Changing IsActive from posible Points on Branch.
        if (input == 1)
        {
            rb.AddForce(new Vector2(550,1700), ForceMode2D.Impulse);
            Debug.Log("Going Top");
        }
        else if (input == 2)
        {
            rb.AddForce(new Vector2(550,1000), ForceMode2D.Impulse);
            Debug.Log("Going Middle");
        }
        else if (input == 3)
        {
            rb.AddForce(new Vector2(550,500), ForceMode2D.Impulse);
            Debug.Log("Going Bottom");
        }
        else 
        {
            rb.AddForce(new Vector2(550,1000), ForceMode2D.Impulse);
            Debug.Log("Going Default");
        }
        IsBranching = false;
        IsJumping = true;
    }
    public void MergeMovement(int input)
    {

        if (input == 1)
        {
            rb.AddForce(new Vector2(500,700), ForceMode2D.Impulse);
            Debug.Log("Merging Top");
        }
        else if (input == 2)
        {
            rb.AddForce(new Vector2(550,1000), ForceMode2D.Impulse);
            Debug.Log("Merging Middle");
        }
        else if (input == 3)
        {
            rb.AddForce(new Vector2(500,1500), ForceMode2D.Impulse);
            Debug.Log("Merging Bottom");
        }
        else 
        {
            rb.AddForce(new Vector2(550,1000), ForceMode2D.Impulse);
            Debug.Log("Merging Default");
        }
        IsBranching = false;
        IsJumping = true;
    }
}

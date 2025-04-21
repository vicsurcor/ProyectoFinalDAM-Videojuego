using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterScript : MonoBehaviour
{
    private PlayerMovement pm;
    public bool IsActive = true;

    private void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && this.gameObject.name == "EncounterPoint")
        {
            IsActive = false;
            Debug.Log("Player entered the collider. IsActive set to false.");
        }
        else if ( other.CompareTag("Player") && this.gameObject.name == "BranchPoint")
        {
            pm.IsBranching = true;
            // Choose direction
            Debug.Log("Branch reached. Select path to branch to : arrow keys / wasd");
            //TODO: InputBased Branching
            pm.BranchMovement(3);
            IsActive = false;
            
            
        }
        else if (other.CompareTag("Player") && this.gameObject.name == "MergePoint")
        {
            pm.IsBranching = true;
            // Choose direction
            Debug.Log("Merge reached.");
            //TODO: Save Previous Branch Number for convenience.
            pm.MergeMovement(3);
            IsActive = false;
        }
    }

}

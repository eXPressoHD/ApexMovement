using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageDestroyedParts : MonoBehaviour
{

    public Dictionary<int, GameObject> DestroyedParts { get; set; }

    private void Start()
    {
        DestroyedParts = new Dictionary<int, GameObject>();
    }

    public void AddToParts(GameObject go)
    {
        int id = DestroyedParts.Count + 1;
        DestroyedParts.Add(id, go);
        StartDestructionTimer(id);
    }

    private void StartDestructionTimer(int id)
    {
        StartCoroutine(DestructionTimer(id));
    }

    private IEnumerator DestructionTimer(int id)
    {
        yield return new WaitForSeconds(5f);
        Destroy(DestroyedParts[id]);
    }
}

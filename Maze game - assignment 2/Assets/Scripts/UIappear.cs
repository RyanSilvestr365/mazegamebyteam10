using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIappear : MonoBehaviour
{
    public GameObject instructions;
    void Start()
    {
        instructions.SetActive(false);    
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            instructions.SetActive(true);
        }
    }

}

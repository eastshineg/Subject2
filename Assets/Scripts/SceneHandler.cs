using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameSupervisor.WarmUp();
        if (GameSupervisor.TryGet(out var gs) == false)
        {
            return;
        }
        
        gs.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _DebugMode : MonoBehaviour
{
    [SerializeField] private GameObject debugMenu;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && Input.GetKey(KeyCode.LeftControl))
        {
            // Toggle the debug menu on and off
            debugMenu.SetActive(!debugMenu.activeSelf);
        }
    }
}

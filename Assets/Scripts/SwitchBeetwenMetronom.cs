using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBeetwenMetronom : MonoBehaviour
{
    public GameObject panel;
    public void Switcher() {
        if(panel != null) {
            bool IsActive = panel.activeSelf;
            panel.SetActive(!IsActive);
        }
    }
}

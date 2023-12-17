using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
public class PanelOpener : MonoBehaviour
{
    public GameObject panel; // ваша панель

    private List<GameObject> panels;

    public void Start() {
        panels = new List<GameObject>() {
        GameObject.Find("Panel"),
        GameObject.Find("Panel1"),
        GameObject.Find("Panel2"),
        GameObject.Find("Panel3"),
        GameObject.Find("Panel4"),
        GameObject.Find("Panel5"),
        GameObject.Find("Panel6"),
        };
        panels.Remove(panel);
    }

    public void Switcher() {
        panels.ForEach(x => x.SetActive(false));
        if(panel != null) {
            bool IsActive = panel.activeSelf;
            panel.SetActive(!IsActive);
        }
    }
}
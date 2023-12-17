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
        GameObject.Find("/Canvas/RawImage/Panel"),
        GameObject.Find("/Canvas/RawImage/Panel1"),
        GameObject.Find("/Canvas/RawImage/Panel2"),
        GameObject.Find("/Canvas/RawImage/Panel3"),
        GameObject.Find("/Canvas/RawImage/Panel4"),
        GameObject.Find("/Canvas/RawImage/Panel5"),
        GameObject.Find("/Canvas/RawImage/Panel6"),
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
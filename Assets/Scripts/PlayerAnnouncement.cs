using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAnnouncement : MonoBehaviour
{
    public UnityEngine.UI.Text textPrefab;

    public void Show(string message)
    {
        var canvas = GameObject.Find("Canvas");
        var text = Instantiate<Text>(textPrefab, canvas.transform);
        text.text = message;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAnnouncement : MonoBehaviour
{
    public UnityEngine.UI.Text bad_text_prefab;
    public UnityEngine.UI.Text info_text_prefab;
    public UnityEngine.UI.Text success_text_prefab;

    IEnumerator show_coroutine;
    Text current_text = null;

    public void Show(string message, string type)
    {
        if (current_text != null && current_text.gameObject != null)
            Destroy(current_text.gameObject);

        var prefab = bad_text_prefab;
        if (string.Equals(type, "info"))
            prefab = info_text_prefab;
        else if (string.Equals(type, "success"))
            prefab = success_text_prefab;

        var canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            current_text = Instantiate<Text>(prefab, canvas.transform);
            current_text.text = message;
        }
    }

    public void ShowFor(string message, string type, float sec)
    {
        Show(message, type);
        if (current_text != null)
        {
            Debug.Log(current_text.text);
            show_coroutine = ShowCoroutine(current_text, sec);
            StartCoroutine(show_coroutine);
        }
    }

    private IEnumerator ShowCoroutine(Text text, float sec)
    {
        yield return new WaitForSeconds(sec);
        if(text != null && text.gameObject != null)
            Destroy(text.gameObject);
    }
}

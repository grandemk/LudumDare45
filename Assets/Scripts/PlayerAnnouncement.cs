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

    public void Show(string message, string type)
    {
        Show_(message, type);
    }

    private Text Show_(string message, string type)
    {
        var prefab = bad_text_prefab;
        if (string.Equals(type, "info"))
            prefab = info_text_prefab;
        else if (string.Equals(type, "success"))
            prefab = success_text_prefab;

        var canvas = GameObject.Find("Canvas");
        var text = Instantiate<Text>(prefab, canvas.transform);
        text.text = message;
        return text;
    }

    public void ShowFor(string message, string type, float sec)
    {
        var text = Show_(message, type);
        Debug.Log(text);
        show_coroutine = ShowCoroutine(text, sec);
        StartCoroutine(show_coroutine);
    }

    private IEnumerator ShowCoroutine(Text text, float sec)
    {
        yield return new WaitForSeconds(sec);
        Destroy(text);
    }
}

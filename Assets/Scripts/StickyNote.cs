using UnityEngine;
using TMPro;

public class StickyNote : MonoBehaviour
{
    public TMP_Text noteText;

    public static StickyNote activeNote = null;
    private bool isEditing = false;

    private TouchScreenKeyboard vrKeyboard;

    public void EditNote()
    {
        if (activeNote != null && activeNote != this)
        {
            activeNote.isEditing = false;
            Debug.Log("Stopped editing previous sticky note");
        }

        activeNote = this;
        isEditing = true;

        Debug.Log("Started editing sticky note: " + noteText.text);

#if UNITY_ANDROID && !UNITY_EDITOR
        Debug.Log("Opening VR OS keyboard");
        vrKeyboard = TouchScreenKeyboard.Open(
            noteText.text,
            TouchScreenKeyboardType.Default,
            false,
            true,
            false,
            false,
            "Edit Sticky Note"
        );
#endif
    }

    void Update()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (vrKeyboard != null)
        {
            if (vrKeyboard.active)
            {
                noteText.text = vrKeyboard.text;
            }
            else if (vrKeyboard.status == TouchScreenKeyboard.Status.Done)
            {
                Debug.Log("VR keyboard submitted text: " + vrKeyboard.text);
                noteText.text = vrKeyboard.text;
                vrKeyboard = null;
                isEditing = false;
                activeNote = null;
            }
            return;
        }
#endif

        if (!isEditing || activeNote != this)
            return;

        foreach (char c in Input.inputString)
        {
            Debug.Log("Input char: " + (int)c + " (" + c + ")");

            if (c == '\b')
            {
                if (noteText.text.Length > 0)
                {
                    noteText.text = noteText.text.Substring(0, noteText.text.Length - 1);
                    Debug.Log("Backspace pressed");
                }
            }
            else if (c != '\n' && c != '\r')
            {
                noteText.text += c;
                Debug.Log("Typed character: " + c);
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Finished editing (Enter pressed)");
            isEditing = false;
            activeNote = null;
        }
    }
}

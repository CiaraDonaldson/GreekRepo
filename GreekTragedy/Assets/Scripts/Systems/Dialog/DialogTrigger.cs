using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Dialog struct to hold a speaker name to display, a line that speaker says, and an image to display if there is one
/// </summary>
[System.Serializable]
public sealed class DialogLine
{
    public string speaker;
    public string line;
    public Sprite image;
}

/// <summary>
/// Triggers Dialog at start of scene, or when something from a layer enters a trigger with this script attached
/// </summary>
public sealed class DialogTrigger : MonoBehaviour
{
    [SerializeField] Canvas dialogCanvas;
    [SerializeField] bool startOnLoad;
    [SerializeField, Tooltip("Delay before dialog begins, in Seconds")]
    float preDialogDelay = 1;
    [SerializeField] DialogLine[] dialog;
    [SerializeField] TMP_Text talkerNameTMPText;
    [SerializeField] TMP_Text talkerDialogTMPText;
    [SerializeField] TMP_Text continueDialogText;
    [SerializeField] Image targetImage;
    [SerializeField] float textSpeed;
    [SerializeField] float timeBetweenDialogLines;
    [SerializeField] KeyCode continueDialogKey = KeyCode.Space;
    [SerializeField] UnityEvent OnDialogStarted;
    [SerializeField] UnityEvent OnDialogFinished;
    readonly Queue<DialogLine> _currentLines = new();
    bool _continueKeyPressed;
    bool _hasStarted = false;

    private void Start()
    {
        continueDialogText.text = $"Press '{continueDialogKey}' to continue...";
        if (dialogCanvas != null)
            dialogCanvas.gameObject.SetActive(false);
        foreach (var l in dialog)
            _currentLines.Enqueue(l);
        if (startOnLoad)
            Trigger();
    }

    private void Update()
    {
        if (!Input.GetKeyDown(continueDialogKey)) return;
        _continueKeyPressed = true;
    }

    [ContextMenu(nameof(Trigger))]
    /// <summary>
    /// Public call to trigger dialog line if called from another source
    /// </summary>
    public void Trigger() => StartCoroutine(TriggerDialog());

    IEnumerator TriggerDialog()
    {
        if (talkerDialogTMPText == null || talkerNameTMPText == null) yield return null;
        if (_hasStarted) yield return null;
        continueDialogText.gameObject.SetActive(false);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(preDialogDelay);
        _hasStarted = true;
        if (dialogCanvas != null)
            dialogCanvas.gameObject.SetActive(true);
        OnDialogStarted.Invoke();
        if (_currentLines.TryPeek(out DialogLine dialog))
        {
            if (dialog == null) yield return null;
            if (dialog.speaker == string.Empty) yield return null;
            talkerNameTMPText.text = dialog.speaker;
            talkerDialogTMPText.text = string.Empty;

            if (targetImage != null && dialog.image != null)
            {
                targetImage.sprite = dialog.image;
                targetImage.color = new Color(1, 1, 1, 1);
            }
            else
                targetImage.color = new Color(1, 1, 1, 0);

            foreach (var character in dialog.line)
            {
                yield return new WaitForSecondsRealtime(textSpeed);
                talkerDialogTMPText.text += character;
            }

            yield return new WaitForSecondsRealtime(timeBetweenDialogLines);
            _currentLines.Dequeue();
        }
        continueDialogText.gameObject.SetActive(true);
        yield return new WaitUntil(() => _continueKeyPressed);
        _continueKeyPressed = false;
        if (_currentLines.Count != 0)
        {
            _hasStarted = false;
            Trigger();
        }
        else
        {
            _hasStarted = false;
            OnDialogFinished.Invoke();
            Time.timeScale = 1;
        }
    }
}

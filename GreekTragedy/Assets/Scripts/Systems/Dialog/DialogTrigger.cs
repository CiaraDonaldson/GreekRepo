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
    public bool waitForContinueButton = true;
    public float textSpeed = .2f;
    public int delayAfterLine = 1;
}

/// <summary>
/// Triggers Dialog at start of scene, or when something from a layer enters a trigger with this script attached
/// </summary>
public sealed class DialogTrigger : MonoBehaviour
{
    [SerializeField] GlobalBool HasSeenAlready;
    [SerializeField] Canvas dialogCanvas;
    [SerializeField] bool startOnLoad;
    [SerializeField, Tooltip("Delay before dialog begins, in Seconds")]
    float preDialogDelay = 1;
    [SerializeField] DialogLine[] dialog;
    [SerializeField] TMP_Text talkerNameTMPText;
    [SerializeField] TMP_Text talkerDialogTMPText;
    [SerializeField] TMP_Text continueDialogText;
    [SerializeField] Image targetImage;
    [SerializeField] KeyCode continueDialogKey = KeyCode.Space;
    [SerializeField] UnityEvent OnDialogStarted;
    [SerializeField] UnityEvent OnDialogFinished;
    readonly Queue<DialogLine> _currentLines = new();
    bool _continueKeyPressed;
    bool _hasStarted = false;

    private void OnEnable()
    {
        if (dialogCanvas != null)
            dialogCanvas.gameObject.SetActive(false);
        InitializeDialogTrigger();
    }

    void InitializeDialogTrigger()
    {
        continueDialogText.text = $"Press '{continueDialogKey}' to continue...";
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

    private void OnDisable() => StopAllCoroutines();


    [ContextMenu(nameof(Trigger))]
    /// <summary>
    /// Public call to trigger dialog line if called from another source
    /// </summary>
    public void Trigger()
    {
        if (gameObject.activeSelf)
            StartCoroutine(TriggerDialog());
    }

    IEnumerator TriggerDialog()
    {
        if (!HasSeenAlready.Value)
        {
            if (talkerDialogTMPText == null || talkerNameTMPText == null) yield break;
            if (_hasStarted) yield break;
            continueDialogText.gameObject.SetActive(false);
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(preDialogDelay);
            _hasStarted = true;
            if (dialogCanvas != null)
                dialogCanvas.gameObject.SetActive(true);
            OnDialogStarted.Invoke();
            bool continueDialog = false;
            if (_currentLines.TryPeek(out DialogLine dialog))
            {
                if (dialog == null) yield return null;
                if (dialog.speaker == string.Empty) yield return null;
                talkerNameTMPText.text = dialog.speaker;
                talkerDialogTMPText.text = string.Empty;
                continueDialog = dialog.waitForContinueButton;
                if (targetImage != null && dialog.image != null)
                {
                    targetImage.sprite = dialog.image;
                    targetImage.color = new Color(1, 1, 1, 1);
                }
                else
                    targetImage.color = new Color(1, 1, 1, 0);
                string line = dialog.line;
                foreach (var character in line)
                {
                    yield return new WaitForSecondsRealtime(dialog.textSpeed);
                    talkerDialogTMPText.text += character;
                }

                yield return new WaitForSecondsRealtime(dialog.delayAfterLine);
                _currentLines.Dequeue();
            }
            continueDialogText.gameObject.SetActive(true);
            if (continueDialog)
            {
                yield return new WaitUntil(() => _continueKeyPressed);
                _continueKeyPressed = false;
            }
            if (_currentLines.Count != 0)
            {
                _hasStarted = false;
                Trigger();
                yield return null;
            }
            else
                FinishDialog();
        }
        else
            FinishDialog(); 
    }

    void FinishDialog()
    {
        _hasStarted = false;
        HasSeenAlready.Value = true;
        OnDialogFinished.Invoke();
        Time.timeScale = 1;
    }
}

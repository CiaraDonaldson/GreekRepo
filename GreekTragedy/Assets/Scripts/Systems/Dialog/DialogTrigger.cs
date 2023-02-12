using darcproducts;
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
    public Image image;
}

/// <summary>
/// Triggers Dialog at start of scene, or when something from a layer enters a trigger with this script attached
/// </summary>
public sealed class DialogTrigger : MonoBehaviour
{
    [Tooltip("For speeding up or slowing base text speed.")]
    public float textSpeedMultiplier = 1;
    [SerializeField] bool startOnLoad;
    [SerializeField, Tooltip("Delay before dialog begins, in Seconds")]
    float preDialogDelay = 1;
    [SerializeField] DialogLine[] dialog;
    [SerializeField] TMP_Text talkerNameText;
    [SerializeField] TMP_Text talkerDialogText;
    [SerializeField] Image targetImage;
    [SerializeField] float textSpeed;
    [SerializeField] float timeBetweenDialogLines;
    [SerializeField] bool useTrigger;
    [SerializeField] LayerMask triggerLayers;
    [SerializeField] UnityEvent OnDialogStarted;
    [SerializeField] UnityEvent OnDialogFinished;
    Queue<DialogLine> _currentLines = new();
    bool _hasStarted = false;

    private void Start()
    {
        foreach (var l in dialog)
            _currentLines.Enqueue(l);
        if (!useTrigger && startOnLoad)
            Trigger();
    }

    /// <summary>
    /// Public call to trigger dialog line if called from another source
    /// </summary>
    public void Trigger() => StartCoroutine(TriggerDialog());

    IEnumerator TriggerDialog()
    {
        if (talkerDialogText == null || talkerNameText == null) yield return null;
        if (_hasStarted) yield return null;
        OnDialogStarted.Invoke();
        _hasStarted = true;
        yield return new WaitForSecondsRealtime(preDialogDelay);
        if (_currentLines.TryPeek(out DialogLine dialog))
        {
            if (dialog == null) yield return null;
            if (dialog.speaker == string.Empty) yield return null;
            talkerNameText.text = dialog.speaker;
            talkerDialogText.text = string.Empty;

            if (targetImage != null && dialog.image != null)
                targetImage.sprite = dialog.image.sprite;

            foreach (var character in dialog.line)
            {
                yield return new WaitForSecondsRealtime(textSpeed * textSpeedMultiplier);
                talkerDialogText.text += character;
            }

            yield return new WaitForSecondsRealtime(timeBetweenDialogLines);
        }
        if (_currentLines.Count != 0)
        {
            _hasStarted = false;
            StartCoroutine(TriggerDialog());
        }
        else
        {
            _hasStarted = false;
            OnDialogFinished.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!Utilities.IsInLayerMask(other.gameObject, triggerLayers) | !useTrigger | startOnLoad) return;
        Trigger();
    }
}

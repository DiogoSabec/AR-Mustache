using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Console : MonoBehaviour
{
    public TextMeshProUGUI consoleText;
    private Queue<string> logQueue = new Queue<string>();
    public int maxLines = 30;

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Add the log message to the queue
        logQueue.Enqueue(logString);

        // Remove the oldest log if the maxLines is exceeded
        while (logQueue.Count > maxLines)
        {
            logQueue.Dequeue();
        }

        // Update the console text
        consoleText.text = string.Join("\n", logQueue.ToArray());
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using DG.Tweening;
using TMPro;
using crass;

public class XRConsole : MonoBehaviour
{
    [SerializeField] float m_ScaleTransitionDuration;
    [SerializeField] EnumMap<LogType, Color> m_LogTypeColors;
    [SerializeField] string m_TimestampFormat;

    [SerializeField] TextMeshProUGUI m_TextContainer;

    [SerializeField] Transform m_ConsoleParent, m_ShrinkSphereParent;
    [SerializeField] Collider m_ConsoleCollider, m_ShrinkSphereCollider;

    [SerializeField] SmoothFollow m_SmoothFollow;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    bool maximized;

    void Awake ()
    {
        Application.logMessageReceived += log;
    }

    void Start ()
    {
        setMaximizedState(false);
        m_SmoothFollow.Target = CameraCache.Main.transform;
    }

    void OnDestroy ()
    {
        Application.logMessageReceived -= log;
    }

    public void OnActivated ()
    {
        setMaximizedState(!maximized);
    }

    public void OnSelected ()
    {
        m_SmoothFollow.enabled = false;
    }

    public void OnDeselected ()
    {
        m_SmoothFollow.enabled = true;
    }

    void log (string condition, string stackTrace, LogType type)
    {
        string prefix = $"<b>{type.ToString().ToUpper()}</b> @ {DateTime.Now.ToString(m_TimestampFormat)}:";
        string text = $"{prefix.WrapInTMProColorTag(m_LogTypeColors[type])} {condition}";

        m_TextContainer.text += "\n\n" + text;
    }

    void setMaximizedState (bool state)
    {
        maximized = state;

        m_ConsoleParent.DOScale(maximized ? 1 : 0, m_ScaleTransitionDuration);
        m_ConsoleCollider.enabled = maximized;

        m_ShrinkSphereParent.DOScale(maximized ? 0 : 1, m_ScaleTransitionDuration);
        m_ShrinkSphereCollider.enabled = !maximized;
    }
#else // UNITY_EDITOR || DEVELOPMENT_BUILD
    void Awake ()
    {
        Destroy(gameObject);
    }

    public void OnActivated () { }
    public void OnSelected () { }
    public void OnDeselected () { }
#endif // UNITY_EDITOR || DEVELOPMENT_BUILD
}

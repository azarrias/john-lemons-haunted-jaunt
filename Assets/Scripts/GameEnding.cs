using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public GameObject player;
    public CanvasGroup exitBackgroundImageCanvasGroup;

    bool m_IsPlayerAtExit;
    // ensure that the game doesn't end before the fade has finished
    float m_Timer;

    private void OnTriggerEnter(Collider other)
    {
        // make sure that ending is only triggered when the PC hits the box collider
        if (other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
        }
    }

    private void Update()
    {
        if (m_IsPlayerAtExit)
        {
            m_Timer += Time.deltaTime;
            exitBackgroundImageCanvasGroup.alpha = m_Timer / fadeDuration;

            if (m_Timer > fadeDuration + displayImageDuration)
            {
                Application.Quit();
            }
        }
    }
}

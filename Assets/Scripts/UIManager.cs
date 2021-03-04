using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Cached References")]

    [SerializeField] private StateManager stateManagerSC;

    [SerializeField] private GameObject gameSceneCanvas;

    [SerializeField] private GameObject playButton;

    [SerializeField] private TextMeshProUGUI startPlayTimerTMP;

    // Private Variables.

    // private GameObject _gameSceneCanvas;

    public void LoadGameSceneCanvas()
    {
        gameSceneCanvas.SetActive(true);

    }

    public void TurnOffButtonAndTurnOnTimer()
    {
        playButton.SetActive(false);

        startPlayTimerTMP.gameObject.SetActive(true);

        StartCoroutine(StartPlayTimer());

    }

    public IEnumerator StartPlayTimer()
    {
        for (int x = 3; x >= 0; x--)
        {
            if (x == 0)
            {
                startPlayTimerTMP.text = "GO";

            }
            else
            {
                startPlayTimerTMP.text = "" + x;

            }

            yield return new WaitForSeconds(1f);

        }

        startPlayTimerTMP.gameObject.SetActive(false);

        stateManagerSC.PlayStartedEvent.Invoke();

        yield break;

    }

}

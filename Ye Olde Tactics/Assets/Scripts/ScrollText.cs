using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// put this code on the scrolling text view
public class ScrollText : MonoBehaviour {

    public Text phaseText; // ex: 'player phase'  'enemy phase'
    private float duration;
    private RectTransform rectTransform;
    private Vector2 startPos, endPos;
    private Coroutine PhaseCoroutine;
    private int phase; // 0 for player 1 for enemy
    public bool textOn; 

	// Use this for initialization
	void Start () {
        textOn = true;

        rectTransform = phaseText.GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition; // same for both
        endPos = new Vector2(startPos.x, Screen.height);
        duration = 4f;	
	}

    // Update is called once per frame
    void Update() {

        if(textOn)
        {
            phaseText.enabled = true;
        }
        else
        {
            phaseText.enabled = false;
        }

    }

    public void StartText(int phase)
    {
        this.phase = phase;
        PhaseCoroutine = StartCoroutine(ShowText());
        ShowText();

    }

     IEnumerator ShowText()
    {
        textOn = true;
        if (phase == 1)
        {
            // enemy phase
            phaseText.text = "Enemy Phase";
        }
        else
        {
            phaseText.text = "Player Phase";
        }

        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        textOn = false;
        StopCoroutine(PhaseCoroutine);

    }
}

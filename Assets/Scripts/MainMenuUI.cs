using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("------ Lucky Number ------")]
    [SerializeField] private int m_minLuckyNumber = 1;
    [SerializeField] private int m_maxLuckyNumber = 13;
    [Space(15)]
    [SerializeField] private TMP_InputField m_inputField;
    [SerializeField] private Image m_backgroundImage;
    private bool m_bLuckyNumberIsSet = false;
    public void OnNewGameButtonPressed()
    {
        if (m_bLuckyNumberIsSet)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.LoadScene((int)Scenes.Level1);
        }
        else
        {
            Debug.Log("Favourity Number Hasn't beem set yet");
            StartCoroutine(BlinkError());
        }
    }
    public void OnTutorialButtonPressed()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.LoadScene((int)Scenes.Tutorial);
    }
    public void OnQuitGameButtonPressed()
    {
        if (GameManager.Instance != null) 
            GameManager.Instance.QuitGame();
    }
    public void ReadNumber()
    {
        string inputNumber = m_inputField.text;

        if(int.TryParse(inputNumber, out int luckyNumber))
        {
            if (luckyNumber <= m_maxLuckyNumber && luckyNumber >= m_minLuckyNumber)
            {
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.SetLuckyNumber(luckyNumber);
                    m_bLuckyNumberIsSet = true;
                }
            }
            else
            {
                Debug.Log("Zahl entspricht nicht den vorgaben!");
                StartCoroutine(BlinkError());
            }
        }
        else
        {
            Debug.Log("Zahl entspricht nicht den vorgaben!");
            StartCoroutine(BlinkError());
        }
    }
    private IEnumerator BlinkError()
    {
        Color originalColor = m_backgroundImage.color;

        for (int i = 0; i < 2; i++)
        {
            m_backgroundImage.color = new Color(1f, 0.5f, 0.5f);
            yield return new WaitForSeconds(0.1f);

            m_backgroundImage.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
    }
}

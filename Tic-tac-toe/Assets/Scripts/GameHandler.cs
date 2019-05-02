using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour {

    private static int humanscore;
    private static int aiscore;
    private static int tiesscore;
    public static GameHandler instance;

	public int HumanScore
    {
        get
        {
            return humanscore;
        }
        set
        {
            if(value >= 0)
            {
                humanscore = value;
                humanScoreText.text = humanscore.ToString();
                
            }
            else
            {
                Debug.Log("HumanScore must be more than 0!");
            }
        }
    }
    public int AIScore
    {
        get
        {
            return aiscore;
        }
        set
        {
            if (value >= 0)
            {
                aiscore = value;
                aiScoreText.text = aiscore.ToString();
            }
            else
            {
                Debug.Log("AIScore must be more than 0!");
            }
        }
    }
    public int TiesScore
    {
        get
        {
            return tiesscore;
        }
        set
        {
            if (value >= 0)
            {
               tiesscore = value;
                tiesScoreText.text = tiesscore.ToString();
            }
            else
            {
                Debug.Log("TiesScore must be more than 0!");
            }
        }
    }
    [SerializeField] private UnityEngine.UI.Text humanScoreText;
    [SerializeField] private UnityEngine.UI.Text aiScoreText;
    [SerializeField] private UnityEngine.UI.Text tiesScoreText;
    [SerializeField] private UnityEngine.UI.Button[] buttons;
    [SerializeField] private UnityEngine.UI.Image resultTable;

    private void Start()
    {
        humanScoreText.text = humanscore.ToString();
        aiScoreText.text = aiscore.ToString();
        tiesScoreText.text = tiesscore.ToString();
    }

    private void Awake()
    {
        instance = this;
    }

    public void OkResultTable()
    {
        LevelsController.instance.LoadGameField(LevelsController.mode);

    }

    public void ShowResultTable(Sprite resultSprite)
    {
        TurnOffAllButtons();
        resultTable.sprite = resultSprite;
        resultTable.gameObject.SetActive(true);

    }

    private void TurnOffAllButtons()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].enabled = false;
        }
    }

}

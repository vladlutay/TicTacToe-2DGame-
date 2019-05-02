using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsController : MonoBehaviour {


    public static LevelsController instance;
    public static int mode = -1; // 0 - easy, 1 - middle, 2 - hard

    private void Awake()
    {
        instance = this;
    }


    public void LoadGameField(int _mode)
    {
        mode = _mode;
        SceneManager.LoadScene("GameField");
    }

    

    public void LoadMenu()
    {
        ResetScore();
        SceneManager.LoadScene("Menu");
    }


    private void ResetScore()
    {
        GameHandler.instance.HumanScore = 0;
        GameHandler.instance.AIScore = 0;
        GameHandler.instance.TiesScore = 0;
        MiniMaxLogic.currentPlayer = -100;
        MiniMaxLogic.currentPlayerPreviousRound = -100;

    }

    public void Exit()
    {
        Application.Quit();
    }

}

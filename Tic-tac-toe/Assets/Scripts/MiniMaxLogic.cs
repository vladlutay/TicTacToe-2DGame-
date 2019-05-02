using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class MiniMaxLogic : MonoBehaviour {

    private List<int> origBoard; // Game board status
    private int aiPlayer = 100; // Artifical Intelligence
    private int huPlayer = -100; // It's me
    private int bestMoveIndex;
    public static int currentPlayer = -100;
    public static int currentPlayerPreviousRound = -100; 
    private Sprite humanSprite;
    private Sprite aiSprite;
    private bool theFirstMoveAI = true;
    private bool buttonAccess = true;


    [SerializeField] private UnityEngine.UI.Image[] images;
    [SerializeField] private UnityEngine.UI.Button[] buttons;
    [SerializeField] private Sprite spriteNull;
    [SerializeField] private Sprite spriteCross;
    [SerializeField] private Sprite winSprite;
    [SerializeField] private Sprite loseSprite;
    [SerializeField] private Sprite tieSprite;

    private void Start()
    {
        origBoard = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

        if(currentPlayer == aiPlayer)
        {
            aiSprite = spriteCross;
            humanSprite = spriteNull;
            MakeMove(new System.Random().Next(0, 8), aiSprite, aiPlayer);
            theFirstMoveAI = false;


        }
        if (currentPlayer == huPlayer)
        {
            humanSprite = spriteCross;
            aiSprite = spriteNull;
        }

    }

    private List<int> EmptyIndices(List<int> _board) // Check empty indices
    {

        return new List<int>(from i in _board
                                            where i != aiPlayer && i != huPlayer
                                            select i);
    }

    public void ButtonWasPressed(int _id) // To process user input and make AI move
    {
        if (buttonAccess)
        {
            buttonAccess = false;
            MoveHuman(_id, humanSprite);

            if (IsWinning(origBoard, huPlayer))
            {
                GameHandler.instance.HumanScore++;
                GameHandler.instance.ShowResultTable(winSprite);
                currentPlayerPreviousRound = currentPlayer;
                currentPlayer = huPlayer;

                return;
            }
            if (IsTie())
            {
                return;
            }


            MoveAI(LevelsController.mode);


            if (IsWinning(origBoard, aiPlayer))
            {
                GameHandler.instance.AIScore++;
                GameHandler.instance.ShowResultTable(loseSprite);
                currentPlayerPreviousRound = currentPlayer;
                currentPlayer = aiPlayer;
                return;
            }

            if (IsTie())
            {
                return;
            }
            buttonAccess = true;
        }
    }

    private bool IsWinning(List<int> _board, int _player) // Check on win
    {
        if (
            (_board[0] == _player && _board[1] == _player && _board[2] == _player) ||
            (_board[3] == _player && _board[4] == _player && _board[5] == _player) ||
            (_board[6] == _player && _board[7] == _player && _board[8] == _player) ||
            (_board[0] == _player && _board[3] == _player && _board[6] == _player) ||
            (_board[1] == _player && _board[4] == _player && _board[7] == _player) ||
            (_board[2] == _player && _board[5] == _player && _board[8] == _player) ||
            (_board[0] == _player && _board[4] == _player && _board[8] == _player) ||
            (_board[2] == _player && _board[4] == _player && _board[6] == _player)
           )
        {
            return true;

        }
        else
            return false; 
    }

    private int Minimax(List<int> newboard, int _player)
    {
        List<int> availSpots = EmptyIndices(newboard);

        if(IsWinning(newboard, aiPlayer))
        {
            return 10;
        }else
            if(IsWinning(newboard, huPlayer))
        {
            return -10;
        }else
            if(availSpots.Count == 0)
        {
            return 0;
        }

        List<Move> moves = new List<Move>();

        for(int i = 0; i < availSpots.Count; i++)
        {
            Move move = new Move();
            move.index = newboard[availSpots[i]];
            newboard[availSpots[i]] = _player;
            if(_player == aiPlayer)
            {
                move.score = Minimax(newboard, huPlayer);

            }
            else
            {
                move.score = Minimax(newboard, aiPlayer);
            }
            newboard[availSpots[i]] = move.index;
            moves.Add(move);

        }

        int bestmove = 0;
        if(_player == aiPlayer)
        {
            int bestscore = -10000;
            for(int i = 0; i < moves.Count; i++)
            {
                if(moves[i].score > bestscore)
                {
                    bestscore = moves[i].score;
                    bestmove = i;
                }
            }
        }
        else
        {
            int bestscore = 10000;
            for (int i = 0; i < moves.Count; i++)
            {
                if (moves[i].score < bestscore)
                {
                    bestscore = moves[i].score;
                    bestmove = i;
                }
            }
        }

        bestMoveIndex = moves[bestmove].index;


        return 0;
    }

    private struct Move
    {
        public int index;
        public int score;
    }

    private void MakeMove(int _id, Sprite _sprite, int _player)
    {
        images[_id].sprite = _sprite;
        images[_id].color = new Color(images[_id].color.r, images[_id].color.g, images[_id].color.b, 255);
        origBoard[_id] = _player;
        buttons[_id].gameObject.SetActive(false);
    }

    private bool IsTie()
    {
        if(!IsWinning(origBoard, huPlayer) && !IsWinning(origBoard, aiPlayer) && EmptyIndices(origBoard).Count == 0)
        {
            GameHandler.instance.ShowResultTable(tieSprite);
            GameHandler.instance.TiesScore++;
            currentPlayerPreviousRound = currentPlayer;
            if (currentPlayerPreviousRound == huPlayer)
                currentPlayer = aiPlayer;
            if (currentPlayerPreviousRound == aiPlayer)
                currentPlayer = huPlayer;

            return true;
        }
        return false;
    }

    private void MoveAI(int _mode)
    {
        if (theFirstMoveAI)
        {
            _mode = 3;
            theFirstMoveAI = false;
        }
        
        switch (_mode)
        {

            case 0: // Easy
                Minimax(origBoard, aiPlayer);
                break;

            case 1: // Middle
                Minimax(origBoard, huPlayer);
                break;

            case 2: // Hard
                List<int> newBoard = new List<int>(origBoard);
                Minimax(origBoard, aiPlayer);
                newBoard[bestMoveIndex] = aiPlayer;
                if (IsWinning(newBoard, aiPlayer))
                {
                    break;
                }
                else
                {
                    Minimax(origBoard, huPlayer);
                }

                break;

            case 3: // The first move will be random
                int temp = new System.Random().Next(0, 8);
                while (origBoard[temp] == huPlayer)
                    temp = new System.Random().Next(0, 8);
                bestMoveIndex = temp;

                break;
        }

        MakeMove(bestMoveIndex, aiSprite, aiPlayer);
    }

    private void MoveHuman(int _id, Sprite _sprite)
    {
        MakeMove(_id, _sprite, huPlayer);
    }




}


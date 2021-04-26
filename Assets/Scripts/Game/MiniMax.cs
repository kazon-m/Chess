using Game.Controller;
using Game.Logic;
using Game.Pieces;
using UnityEngine;

namespace Game
{
    public class MiniMax
    {
        private readonly int _maxDepth = 3;
        private Move _bestMove;

        private Piece _pieceToMove;
        private int _vueltas;

        public void RunMiniMax(Player maxPlayer, Player minPlayer, Square[,] board)
        {
            _vueltas = 0;
            var maxValue = MiniMaxFunctionRoot(maxPlayer, minPlayer, board);

            Debug.Log("Total Boards Evaluated: " + _vueltas);
            _pieceToMove.Move(_bestMove.Square.Behaviour);
            _bestMove.RunCallback();

            GameController.Instance.ChangeTurns();
            GameController.Instance.EvaluatePlayers();
        }

        public int MiniMaxFunctionRoot(Player maxPlayer, Player minPlayer, Square[,] board)
        {
            var bestValue = int.MinValue;
            for(var i = 0; i < maxPlayer.Pieces.Count; i++)
            {
                for(var j = 0; j < maxPlayer.Pieces[i].PossibleMoves.Count; j++)
                {
                    //Generate a copy of the game state (players and board)
                    var boardCopy = Board.Instance.GenerateBoardCopy(board);
                    var maxCopy = new Player(maxPlayer, boardCopy);
                    var minCopy = new Player(minPlayer, boardCopy);

                    maxCopy.Evaluate(boardCopy);
                    minCopy.Evaluate(boardCopy);

                    maxCopy.EvaluateCastling(boardCopy, minCopy);
                    minCopy.EvaluateCastling(boardCopy, maxCopy);

                    maxCopy.EvaluateCheckOffMoves(minCopy, boardCopy);
                    minCopy.ResetPawnsState();

                    maxCopy.Pieces[i].Move(maxCopy.Pieces[i].PossibleMoves[j].Square);
                    maxCopy.Pieces[i].PossibleMoves[j].RunCallback();

                    var value = MiniMaxFunction(maxCopy, minCopy, boardCopy, false, _maxDepth - 1,
                                                int.MinValue, int.MaxValue);

                    if(value >= bestValue)
                    {
                        bestValue = value;
                        _bestMove = maxPlayer.Pieces[i].PossibleMoves[j];
                        _pieceToMove = maxPlayer.Pieces[i];
                    }
                }
            }

            return bestValue;
        }

        public int MiniMaxFunction(Player maxPlayer, Player minPlayer, Square[,] board, bool maxTurn,
                                   int depth, int alpha, int beta)
        {
            if(depth == 0) return EvaluateBoard(maxPlayer, minPlayer);

            //Generate all the moves
            var actualPlayer = maxTurn ? maxPlayer : minPlayer;
            var otherPlayer = maxTurn ? minPlayer : maxPlayer;

            actualPlayer.Evaluate(board);
            otherPlayer.Evaluate(board);
            actualPlayer.EvaluateCastling(board, otherPlayer);
            otherPlayer.EvaluateCastling(board, actualPlayer);
            actualPlayer.EvaluateCheckOffMoves(otherPlayer, board);
            otherPlayer.ResetPawnsState();

            var bestValue = maxTurn ? int.MinValue : int.MaxValue;
            for(var i = 0; i < actualPlayer.Pieces.Count; i++)
            {
                var piece = actualPlayer.Pieces[i];
                var moves = actualPlayer.Pieces[i].PossibleMoves;
                for(var j = 0; j < actualPlayer.Pieces[i].PossibleMoves.Count; j++)
                {
                    _vueltas++;

                    var previousPiece = moves[j].Square.Piece;
                    var previousSquare = piece.ActualSquare;
                    //Temporal move with the board copy. After this we will restore the board to its previous state.  
                    piece.Move(moves[j].Square);
                    piece.PossibleMoves[j].RunCallback();

                    var copyBoard = Board.Instance.GenerateBoardCopy(board);

                    if(maxTurn) //Max
                    {
                        bestValue =
                            Mathf.Max(bestValue,
                                      MiniMaxFunction(new Player(actualPlayer, copyBoard),
                                                      new Player(otherPlayer, copyBoard), copyBoard, !maxTurn,
                                                      depth - 1, alpha, beta));
                        alpha = Mathf.Max(bestValue, alpha);
                    }
                    else //Min
                    {
                        bestValue =
                            Mathf.Min(bestValue,
                                      MiniMaxFunction(new Player(otherPlayer, copyBoard),
                                                      new Player(actualPlayer, copyBoard), copyBoard,
                                                      !maxTurn, depth - 1, alpha, beta));
                        beta = Mathf.Min(beta, bestValue);
                    }

                    //Restore the previous state of the board to continue the loop
                    piece.Move(previousSquare);
                    otherPlayer.AddPiece(previousPiece);
                    if(previousPiece != null) previousPiece.Move(board[moves[j].Square.Y, moves[j].Square.X]);
                    piece.PossibleMoves[j].RunCallbackReset();

                    //Prune
                    if(beta <= alpha) return bestValue;
                }
            }


            return bestValue;
        }

        public int EvaluateBoard(Player max, Player min) =>
            max.EvaluateBoardValue() - min.EvaluateBoardValue();
    }
}
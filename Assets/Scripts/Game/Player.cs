using System.Collections.Generic;
using System.Linq;
using Game.Pieces;

namespace Game
{
    public class Player
    {
        public enum PlayerType
        {
            AI,
            Human
        }

        private static int _playerID;
        private int _boardValue;
        private King _king;

        public Player(PlayerType type = PlayerType.Human, int modSquareValue = 0)
        {
            ID = _playerID;
            _playerID++;
            Type = type;
            ModSquareTableValue = modSquareValue;
            Pieces = new List<Piece>();
        }

        public Player(Player other, Square[,] newBoard)
        {
            ID = other.ID;
            ModSquareTableValue = other.ModSquareTableValue;
            Type = other.Type;
            Pieces = new List<Piece>();

            foreach(var newPiece in other.Pieces.Select(
                piece => piece.Copy(newBoard[piece.ActualSquare.Y, piece.ActualSquare.X])))
                newPiece.SetPropietary(this);
        }

        public PlayerType Type { get; }

        public int ID { get; }

        public int ModSquareTableValue { get; }

        public List<Piece> Pieces { get; }

        public void Evaluate(Square[,] board = null)
        {
            foreach(var piece in Pieces)
            {
                if(piece is King king) _king = king;
                piece.Evaluate(board);
            }
        }

        public int EvaluateBoardValue()
        {
            _boardValue = 0;

            foreach(var piece in Pieces) _boardValue += piece.EvaluateBoardScore();

            return _boardValue;
        }

        public void EvaluateCastling(Square[,] board = null, Player otherPlayer = null) =>
            _king.CastlingRound(board, otherPlayer);

        public void AddPiece(Piece piece)
        {
            if(piece != null) Pieces.Add(piece);
        }

        public void AddPieceInNumber(Piece piece, int number) => Pieces.Insert(number, piece);

        public int ReturnNumberPiece(Piece piece) => Pieces.FindIndex(p => p == piece);

        public void DestroyPiece(Piece piece) => Pieces.Remove(piece);

        public bool Equal(object other) => other is Player otherPlayer && otherPlayer.ID == ID;

        public void EvaluateCheckOffMoves(Player otherPlayer, Square[,] b = null)
        {
            var kingPosition = (from piece in Pieces where piece is King select piece).ToArray()[0]
               .ActualSquare;

            var actualBoard =
                b == null ? Board.Instance.GenerateBoardCopy() : Board.Instance.GenerateBoardCopy(b);

            var copyMe = new Player(this, actualBoard);
            var copyOther = new Player(otherPlayer, actualBoard);
            copyMe.Evaluate(actualBoard);
            copyOther.Evaluate(actualBoard);
            copyMe.EvaluateCastling(actualBoard, copyOther);
            copyOther.EvaluateCastling(actualBoard, copyMe);
            var movesToErase = new List<Square>();

            for(var i = 0; i < Pieces.Count; i++)
            {
                var moves = Pieces[i].PossibleMoves;
                var piece = copyMe.Pieces[i];
                movesToErase.Clear();


                for(var j = 0; j < moves.Count; j++)
                {
                    var previousPiece = piece.PossibleMoves[j].Square.Piece;
                    var previousSquare = piece.ActualSquare;

                    piece.Move(actualBoard[moves[j].Square.Y, moves[j].Square.X]);
                    piece.PossibleMoves[j].RunCallback();
                    copyOther.Evaluate(actualBoard);

                    if(piece is King)
                    {
                        if(copyOther.CheckIfSquareIsInMoves(piece.ActualSquare))
                            movesToErase.Add(moves[j].Square);
                    }
                    else
                    {
                        if(copyOther.CheckIfSquareIsInMoves(kingPosition)) movesToErase.Add(moves[j].Square);
                    }

                    piece.Move(previousSquare);
                    copyOther.AddPiece(previousPiece);
                    previousPiece?.Move(actualBoard[moves[j].Square.Y, moves[j].Square.X]);
                    piece.PossibleMoves[j].RunCallbackReset();
                }

                foreach(var square in movesToErase) Pieces[i].RemoveMove(square);
            }
        }

        public bool CheckIfCheckMate() => Pieces.All(piece => piece.PossibleMoves.Count <= 0);

        public void DestroyLastPiece() => Pieces[Pieces.Count - 1].Destroy();

        public bool CheckIfSquareIsInMoves(Square square) => Pieces.Any(
            piece => piece.PossibleMoves.Any(move => move.IsHarmMove && move.Square.Equal(square)));

        public void ResetPawnsState()
        {
            foreach(var piece in Pieces)
                if(piece is Pawn pawn)
                    pawn.ResetJustMovedTwo();
        }
    }
}
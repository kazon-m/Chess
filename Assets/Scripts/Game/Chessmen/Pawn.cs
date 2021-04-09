namespace Game.Chessmen
{
    public class Pawn : AbstractChess
    {
        public override bool[,] PossibleMove()
        {
            var cells = new bool[8, 8];
            AbstractChess targetChess;

            if(IsWhite)
            {
                if(Position.x != 0 && Position.y != 7)
                {
                    targetChess = BoardManager.Instance.ChessMans[Position.x - 1, Position.y + 1];
                    if(targetChess != null && !targetChess.IsWhite)
                        cells[Position.x - 1, Position.y + 1] = true;
                }

                if(Position.x != 7 && Position.y != 7)
                {
                    targetChess = BoardManager.Instance.ChessMans[Position.x + 1, Position.y + 1];
                    if(targetChess != null && !targetChess.IsWhite)
                        cells[Position.x + 1, Position.y + 1] = true;
                }

                if(Position.y != 7)
                {
                    targetChess = BoardManager.Instance.ChessMans[Position.x, Position.y + 1];
                    if(targetChess == null) cells[Position.x, Position.y + 1] = true;
                }

                if(Position.y == 1)
                {
                    targetChess = BoardManager.Instance.ChessMans[Position.x, Position.y + 1];
                    var targetChess2 = BoardManager.Instance.ChessMans[Position.x, Position.y + 2];
                    if(targetChess == null && targetChess2 == null) cells[Position.x, Position.y + 2] = true;
                }
            }
            else
            {
                if(Position.x != 0 && Position.y != 0)
                {
                    targetChess = BoardManager.Instance.ChessMans[Position.x - 1, Position.y - 1];
                    if(targetChess != null && targetChess.IsWhite)
                        cells[Position.x - 1, Position.y - 1] = true;
                }

                if(Position.x != 7 && Position.y != 0)
                {
                    targetChess = BoardManager.Instance.ChessMans[Position.x + 1, Position.y - 1];
                    if(targetChess != null && targetChess.IsWhite)
                        cells[Position.x + 1, Position.y - 1] = true;
                }

                if(Position.y != 0)
                {
                    targetChess = BoardManager.Instance.ChessMans[Position.x, Position.y - 1];
                    if(targetChess == null) cells[Position.x, Position.y - 1] = true;
                }

                if(Position.y == 6)
                {
                    targetChess = BoardManager.Instance.ChessMans[Position.x, Position.y - 1];
                    var targetChess2 = BoardManager.Instance.ChessMans[Position.x, Position.y - 2];
                    if(targetChess == null && targetChess2 == null) cells[Position.x, Position.y - 2] = true;
                }
            }

            return cells;
        }
    }
}
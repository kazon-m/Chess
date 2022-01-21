using Data.Enums;

namespace Components.Chess
{
    public struct PlayerComponent
    {
        public TeamType team;
        public PlayerType type;
        public SquareComponent square;
    }
}
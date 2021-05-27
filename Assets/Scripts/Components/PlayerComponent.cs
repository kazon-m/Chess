namespace Components
{
    public struct PlayerComponent
    {
        public enum PlayerType
        {
            AI,
            Player
        }
        
        public PlayerType Type;
    }
}
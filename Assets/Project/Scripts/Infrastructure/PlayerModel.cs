namespace Project.Scripts.Infrastructure
{
    public class PlayerModel
    {
        public int PlayerLevel { get; private set; }

        public void CompleteLevel()
        {
            PlayerLevel++;
        }
    }
}
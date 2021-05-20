
using Player.Scripts;

namespace GameScripts.Saving_Game_Data
{
    [System.Serializable]
    public class MovementPlayerData
    {
        //TODO set variable to be savable
        private bool x; public bool X
        {
            get => x;
            set => x = value;
        }
        public MovementPlayerData(MovementPlayer movementPlayer)
        {
            x = movementPlayer.CanDash;
        }
    }
}
namespace Saving_Game_Data
{
    [System.Serializable]
    public class SettingsData
    {

        public bool isButtonControls;

        public SettingsData (CanvasMenu canvasMenu)
        {
            isButtonControls = canvasMenu.IsButtonControl;
        }


    }
}

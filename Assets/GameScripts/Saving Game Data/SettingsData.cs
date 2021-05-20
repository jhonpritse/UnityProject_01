namespace GameScripts.Saving_Game_Data
{
    [System.Serializable]
    public class SettingsData
    {
        private bool isButtonControls; public bool IsButtonControls
        {
            get => isButtonControls;
            set => isButtonControls = value;
        }

        public SettingsData (CanvasMenu canvasMenu)
        {
            isButtonControls = canvasMenu.IsButtonControl;
        }
        

    }
}

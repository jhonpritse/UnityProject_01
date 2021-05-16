using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


namespace Saving_Game_Data
{
    public static class SavingSystem 
    {
        
         static readonly string SettingsDataPath = Application.persistentDataPath + "/SettingsData.jpt";

         
         
         public static void SaveSettings(CanvasMenu canvasMenu)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(SettingsDataPath, FileMode.Create);
            SettingsData data = new SettingsData(canvasMenu);
            formatter.Serialize(stream,data);
            stream.Close();
        }
         public static SettingsData LoadSettings()
        {
            if (File.Exists(SettingsDataPath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(SettingsDataPath, FileMode.Open);
                SettingsData data = formatter.Deserialize(stream) as SettingsData;
                stream.Close();
                return data;
            }
            else
            {
                Debug.LogError("no Path in " + SettingsDataPath);

                return null;
            }
            
        }
         
         
         
        
    }
}

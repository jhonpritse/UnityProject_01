using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Player.Scripts;
using UnityEngine;

namespace GameScripts.Saving_Game_Data
{
    public static class SavingSystem 
    {
        
         static readonly string SettingsDataPath = Application.persistentDataPath + "/1.jpt";
         static readonly string MovementDataPath = Application.persistentDataPath + "/2.jpt";

         public static bool CreatePathIfNull()
         {
             if (!File.Exists(SettingsDataPath)) return true;
             if (!File.Exists(MovementDataPath)) return true;
             
             return false;
             
         }
         

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
         
         
         public static void SaveMovementData(MovementPlayer movementPlayer)
         {
             BinaryFormatter formatter = new BinaryFormatter();
             FileStream stream = new FileStream(MovementDataPath, FileMode.Create);
             MovementPlayerData data = new MovementPlayerData(movementPlayer);
             formatter.Serialize(stream,data);
             stream.Close();
         }
         public static MovementPlayerData LoadMovementData()
         {
             if (File.Exists(MovementDataPath))
             {
                 BinaryFormatter formatter = new BinaryFormatter();
                 FileStream stream = new FileStream(MovementDataPath, FileMode.Open);
                 MovementPlayerData data = formatter.Deserialize(stream) as MovementPlayerData;
                 stream.Close();
                 return data;
             }
             else
             {
                 Debug.LogError("no Path in " + MovementDataPath);
                 return null;
             }
            
         }



         
        
    }
}

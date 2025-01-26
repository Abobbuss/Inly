using System;
using System.IO;
using UnityEngine;

namespace Game.Data
{
    public class SaveManager
    {
        private static readonly string SavePath = Path.Combine(Application.persistentDataPath, "GameData.json");

        public void Save(GameData data)
        {
            try
            {
                string json = JsonUtility.ToJson(data);
                string encrypted = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json));
                File.WriteAllText(SavePath, encrypted);
            }
            catch (IOException ex)
            {
                Debug.LogError($"Failed to save game data: {ex.Message}");
            }
        }

        public GameData Load()
        {
            try
            {
                if (!File.Exists(SavePath)) 
                    return new GameData();
                
                string encrypted = File.ReadAllText(SavePath);
                string json = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encrypted));
                return JsonUtility.FromJson<GameData>(json);
            }
            catch (IOException ex)
            {
                Debug.LogError($"Failed to load game data: {ex.Message}");
                return new GameData();
            }
        }
    }
}
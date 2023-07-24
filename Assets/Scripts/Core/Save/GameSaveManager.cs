using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Save
{
    public static class GameSaveManager
    {
        public const string SaveFolder = "Save";
        private static Dictionary<string,ISaveHandler> saveHandlers = new();


        static GameSaveManager()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings {
                    Formatting = Formatting.Indented,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            
        }

        /// <summary>
        /// Adds a save handler
        /// </summary>
        /// <param name="saveId">The id to differentiate this save by. Id capitalisation does not matter.</param>
        /// <param name="saveHandler">The save id.</param>
        /// <exception cref="Exception"></exception>
        public static ISaveHandler AddSaveHandler(string saveId, ISaveHandler saveHandler)
        {
            saveId = saveId.ToLower();
            if (saveHandlers.ContainsKey(saveId))
            {
                throw new Exception($"Duplicate save id {saveId}");
            }
            saveHandlers.Add(saveId,saveHandler);
            return saveHandler;
        }

        public static string GetSavePath(string saveName = "DefaultSave") =>Path.Combine(Application.persistentDataPath,SaveFolder, saveName);
        public static void LoadSave(string saveName = "DefaultSave")
        {
            string savePath = GetSavePath(saveName);
            foreach ((string saveId, ISaveHandler saveHandler) in saveHandlers)
            {
                string currentPath = Path.Combine(savePath, $"{saveId}.json");
                try
                {
                    string contents = File.ReadAllText(currentPath);
                    saveHandler.OnLoadSave(contents);
                }
                catch (FileNotFoundException _)
                {
                    Debug.LogWarning($"Could not find save file at path {currentPath}. This is normal if level is not saved!");
                }
                catch (DirectoryNotFoundException _)
                {
                    Debug.LogWarning($"Could not find save file at path {currentPath}. This is normal if level is not saved!");
                }
                catch (Exception e)
                {
                    Debug.LogError($"{e} : Error while trying to read save file at path {currentPath}");
                }
                
            }
        }
        
        public static void WriteSave(string saveName = "DefaultSave")
        {
            string savePath = GetSavePath(saveName);
            Directory.CreateDirectory(savePath);
            
            foreach ((string saveId, ISaveHandler saveHandler) in saveHandlers)
            {
                string currentPath = Path.Combine(savePath, $"{saveId}.json");
                using (StreamWriter writer = new StreamWriter(currentPath))
                {
                    try
                    {
                        writer.Write(saveHandler.OnWriteSave());
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Unexpected error while writing save file to ${currentPath}, Error: {e}");
                    }
                }
            }
        }
        
        public static void DeleteSave(string saveName = "DefaultSave")
        {
            string savePath = GetSavePath(saveName);
            try
            {
                Directory.Delete(savePath,true);
            }
            catch (Exception e)
            {
                Debug.LogError($"Unexpected error when deleting save at ${savePath}, Error: {e}");
            }
            
        }

        /// <summary>
        /// Removes all save handlers.
        /// </summary>
        public static void ClearSaveHandlers()
        {
            saveHandlers.Clear();
        }
    }
}
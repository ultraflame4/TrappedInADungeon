using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Save
{
    public static class GameSaveManager
    {
        public const string SaveFolder = "Save";
        public const string SaveFolderExt = ".save";
        private static Dictionary<string,ISaveHandler> saveHandlers = new();

        static GameSaveManager()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings {
                    Formatting = Formatting.Indented,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new CustomContractResolver()
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

        public static string GetSavePath(string saveName = "DefaultSave") => Path.Combine(Application.persistentDataPath, SaveFolder, $"{saveName.Clean()}{SaveFolderExt}").FullPath();
        public static void LoadSave(string saveName = "DefaultSave")
        {
            string savePath = GetSavePath(saveName);
            foreach ((string saveId, ISaveHandler saveHandler) in saveHandlers)
            {
                string currentPath = Path.Combine(savePath, $"{saveId}.json").FullPath();
                Debug.Log($"Loading save: {saveName} - saveId: {saveId} at: {currentPath}");
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
                    Debug.LogError($"Error while trying to read save file at path {currentPath} : {e}");
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

        public static string[] GetSaves()
        {
            string savesLocation = Path.Combine(Application.persistentDataPath,SaveFolder);
            if (!Directory.Exists(savesLocation))
            {
                return Array.Empty<string>();
            }
            return Directory.GetDirectories(savesLocation)
                    .Select(x=>Path.GetFileName(x))
                    .Where(x=>x.EndsWith(SaveFolderExt))
                    .Select(x=>x.Substring(0,x.Length - SaveFolderExt.Length))
                    .ToArray();
        }
        
        public static bool SaveExists(string saveName = "DefaultSave")
        {
            string savePath = GetSavePath(saveName);
            Debug.Log(savePath);
            return Directory.Exists(savePath);
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
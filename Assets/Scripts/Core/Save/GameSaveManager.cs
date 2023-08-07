using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Save
{
    /// <summary>
    /// Helper class for saving and loading game data.
    /// </summary>
    public static class GameSaveManager
    {
        // Name of the folder to write saves to. (relative to Application.persistentDataPath)
        public const string SaveFolder = "Save";

        // suffix to  identify save folders with. (Also avoids the whole problem on windows with special file names like COM1 COM2 etc.)
        public const string SaveFolderExt = ".save";
        private static Dictionary<string, ISaveHandler> saveHandlers = new();

        static GameSaveManager()
        {
            // Set default json.net settings for serialising and deserialising json using JsonConvert
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings {
                    Formatting = Formatting.Indented, // Indent json for readability
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore, // Ignore circular references, even though they shouldn't exist
                    ContractResolver = new CustomContractResolver() // Use custom contract resolver
            };
        }

        /// <summary>
        /// Adds a save handler
        /// </summary>
        /// <param name="saveId">The id to differentiate this save data by. Id capitalisation does not matter.</param>
        /// <param name="saveHandler">The save id.</param>
        /// <exception cref="Exception"></exception>
        public static ISaveHandler AddSaveHandler(string saveId, ISaveHandler saveHandler)
        {
            saveId = saveId.ToLower(); // Ignore capitalisation
            if (saveHandlers.ContainsKey(saveId)) // Check if save id in use already exists
            {
                throw new Exception($"Duplicate save id {saveId}");
            }

            // Add to saveHandler list
            saveHandlers.Add(saveId, saveHandler);
            return saveHandler;
        }

        /// <summary>
        /// Returns the save path to store save data at.
        /// </summary>
        /// <param name="saveName">Name of current save</param>
        /// <returns></returns>
        public static string GetSavePath(string saveName = "DefaultSave") => Path.Combine(Application.persistentDataPath, SaveFolder, $"{saveName.Clean()}{SaveFolderExt}").FullPath();

        /// <summary>
        /// Loads a save.
        /// </summary>
        /// <param name="saveName">Name of save to laod</param>
        public static void LoadSave(string saveName = "DefaultSave")
        {
            string savePath = GetSavePath(saveName);
            // Loop through all save handlers and get them to load their save data
            foreach ((string saveId, ISaveHandler saveHandler) in saveHandlers)
            {
                // Get path to save data file for the current save handler using the save id
                string currentPath = Path.Combine(savePath, $"{saveId}.json").FullPath();
                Debug.Log($"Loading save: {saveName} - saveId: {saveId} at: {currentPath}");
                try
                {
                    // Try to read the save data file and pass the contents to the save handler
                    string contents = File.ReadAllText(currentPath);
                    saveHandler.OnLoadSave(contents);
                }
                catch (FileNotFoundException _)
                {
                    // If the file does not exist, this is normal if the level has not been saved yet.
                    Debug.LogWarning($"Could not find save file at path {currentPath}. This is normal if level is not saved!");
                }
                catch (DirectoryNotFoundException _)
                {
                    // Same as above
                    Debug.LogWarning($"Could not find save file at path {currentPath}. This is normal if level is not saved!");
                }
                catch (Exception e)
                {
                    // If any other error occurs, log it
                    Debug.LogError($"Error while trying to read save file at path {currentPath} : {e}");
                }
            }
        }

        /// <summary>
        /// Saves the current level data (or at least the data of the save handlers).
        /// </summary>
        /// <param name="saveName">Name of save to write to</param>
        public static void WriteSave(string saveName = "DefaultSave")
        {
            // Get path to save folder
            string savePath = GetSavePath(saveName);
            // Create save folder if it does not exist
            Directory.CreateDirectory(savePath);

            // Loop through all save handlers and get them to write their save data
            foreach ((string saveId, ISaveHandler saveHandler) in saveHandlers)
            {
                // Path to save data file for the current save handler
                string currentPath = Path.Combine(savePath, $"{saveId}.json");
                // Write save data to file
                using (StreamWriter writer = new StreamWriter(currentPath))
                {
                    try
                    {
                        // Try and get save data from save handler and write it to file
                        writer.Write(saveHandler.OnWriteSave());
                    }
                    catch (Exception e)
                    {
                        // If any error occurs, log it
                        Debug.LogError($"Unexpected error while writing save file to ${currentPath}, Error: {e}");
                    }
                }
            }
        }

        /// <summary>
        /// Deletes a save.
        /// </summary>
        /// <param name="saveName"></param>
        public static void DeleteSave(string saveName = "DefaultSave")
        {
            string savePath = GetSavePath(saveName);
            try
            {
                // Delete save folder and all files in it
                Directory.Delete(savePath, true);
            }
            catch (Exception e)
            {
                Debug.LogError($"Unexpected error when deleting save at ${savePath}, Error: {e}");
            }
        }

        /// <summary>
        /// Returns a list of all saves.
        /// </summary>
        /// <returns></returns>
        public static string[] GetSaves()
        {
            // Get to the save location
            string savesLocation = Path.Combine(Application.persistentDataPath, SaveFolder);
            // If the save location does not exist, return an empty array
            if (!Directory.Exists(savesLocation))
            {
                return Array.Empty<string>();
            }
            
            return Directory.GetDirectories(savesLocation) // Get all directories in the save location
                    .Select(x => Path.GetFileName(x)) // Get the name of each directory from the full path (GetDirectories returns full path)
                    .Where(x => x.EndsWith(SaveFolderExt)) // Only get directories that has the correct suffix
                    .Select(x => x.Substring(0, x.Length - SaveFolderExt.Length)) // Remove the suffix from the directory name
                    .ToArray();
        }

        /// <summary>
        /// Checks if a save exists.
        /// </summary>
        /// <param name="saveName"></param>
        /// <returns></returns>
        public static bool SaveExists(string saveName = "DefaultSave")
        {
            string savePath = GetSavePath(saveName);
            Debug.Log(savePath);
            return Directory.Exists(savePath); // Check if the save path exists
        }


        /// <summary>
        /// Removes all save handlers.
        /// </summary>
        public static void ClearSaveHandlers()
        {
            // clear save handlers list
            saveHandlers.Clear();
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Save
{
    public static class GameSaveManager
    {
        private static Dictionary<string,ISaveHandler> saveHandlers = new();

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

        public static void LoadSave()
        {
            
        }
        
        public static void WriteSave()
        {
            
            foreach ((string saveId, ISaveHandler saveHandler) in saveHandlers)
            {
                Debug.Log(saveHandler.OnWriteSave());
            }
        }
        
        public static void ClearSave()
        {
            
        }
    }
}
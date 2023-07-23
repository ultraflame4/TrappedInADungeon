using UnityEngine;
        
namespace Core.Save
{
    
    public interface ISaveHandler
    {
        public void OnLoadSave(string json)
        {
            JsonUtility.FromJsonOverwrite(json,this);
        }

        /// <summary>
        /// Called when game saves. Expected to return a json string.
        /// </summary>
        /// <returns></returns>
        public string OnWriteSave()
        {
            return JsonUtility.ToJson(this,true);
        }
        
    }
}
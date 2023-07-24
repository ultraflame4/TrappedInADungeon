using Newtonsoft.Json;

namespace Core.Save
{
    [JsonObject(MemberSerialization.OptIn)]
    public interface ISaveHandler
    {
        public void OnLoadSave(string json)
        {
            JsonConvert.PopulateObject(json,this);
        }

        /// <summary>
        /// Called when game saves. Expected to return a json string.
        /// </summary>
        /// <returns></returns>
        public string OnWriteSave()
        {
            return JsonConvert.SerializeObject(this);
        }
        
    }
}
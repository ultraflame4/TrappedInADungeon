using Newtonsoft.Json;

namespace Core.Save
{
    [JsonObject(MemberSerialization.OptIn)] // Only serialise members (in child) with JsonProperty attribute
    public interface ISaveHandler
    {
        public void OnLoadSave(string json)
        {
            // Override all data (fields with JsonProperty attribute) in this instance with data from json
            JsonConvert.PopulateObject(json,this);
        }

        /// <summary>
        /// Called when game saves. Expected to return a json string.
        /// </summary>
        /// <returns></returns>
        public string OnWriteSave()
        {
            // Serialise all data (fields with JsonProperty attribute) in this instance to json
            return JsonConvert.SerializeObject(this);
        }
        
    }
}
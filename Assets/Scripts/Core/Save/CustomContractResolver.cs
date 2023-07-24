using System;
using System.Collections;
using Newtonsoft.Json.Serialization;

namespace Core.Save
{
    /// <summary>
    /// CustomContractResolver for some custom configs regarding how json.net serialises and deserializes stuff.
    /// </summary>
    public class CustomContractResolver : DefaultContractResolver
    {
        // Copied from https://stackoverflow.com/a/35483868
        protected override JsonArrayContract CreateArrayContract(Type objectType)
        {
            // Clear collection types before adding to them.
            var c = base.CreateArrayContract(objectType);
            c.OnDeserializingCallbacks.Add((obj, streamingContext) =>
            {
                
                var list = obj as IList;
                if (list != null && !list.IsReadOnly)
                    list.Clear();
            });
            return c;
        }
    }
}
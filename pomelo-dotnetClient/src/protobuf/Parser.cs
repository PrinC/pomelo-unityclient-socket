using System;
using SimpleJson;
using System.IO;

namespace Pomelo.Protobuf
{
  public class Parser
  {
    public static JsonObject adaptToPomelo(JsonObject message) {
      return Parser.ParseJsonMessages((JsonArray)message["messages"]);
    }

    public static JsonObject readOriginJsonFile (string protoPath) {
      string protoString;
      using (StreamReader sr = new StreamReader(protoPath)
      {
        protoString = sr.ReadToEnd();
      }
      return (JsonObject)SimpleJson.DeserializeObject(protoString);
    }

    public static JsonObject parse (string protoPath) {
      
    }

    public static JsonObject ParseJsonMessages(JsonArray messages)
    {
      JsonObject prot = new JsonObject();
      if (messages == null)
        return prot;
      foreach (dynamic message in messages)
      {
        JsonObject msg = new JsonObject();
        JsonObject tags = new JsonObject();
        foreach (dynamic field in message["fields"])
        {
          JsonObject fld = new JsonObject();
          fld["option"] = field["rule"];
          fld["type"] = field["type"];
          fld["tag"] = field["id"];
          msg[field["name"]] = fld;
          tags[((long)field["id"]).ToString()] = field["name"];
        }
        msg["__tags"] = tags;
        JsonObject m = message;
        if (m.ContainsKey("messages"))
        {
          msg["__messages"] = ParseJsonMessages((JsonArray)message["messages"]);
        }
        prot[message["name"]] = msg;
      }
      return prot;
    }
  }
}

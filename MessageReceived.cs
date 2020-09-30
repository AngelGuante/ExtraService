using Dapper;
using System;
using Newtonsoft.Json;
using static ExtraService.wsConnection;
using static ExtraService.Utils.GlobalVariables;

namespace ExtraService
{
    public class MessageReceived
    {
        public static void Message(string data)
        {
            try
            {
                var resultset = Conn.Query(data);
                SendMessageAsync(JsonConvert.SerializeObject(resultset));
            }
            catch (Exception e)
            {
                SendMessageAsync($"Error: {e.Message}");
            }
        }
    }
}
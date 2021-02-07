using Dapper;
using System;
using Newtonsoft.Json;
using static ExtraService.wsConnection;
using System.Data.SqlClient;

namespace ExtraService
{
    public class MessageReceived
    {
        public static void Message(string data)
        {
            try
            {
                Console.WriteLine($"{data}\n");
                var server = (data.Split("-->>"))[2];
                var database = (data.Split("-->>"))[3];
                var user = (data.Split("-->>"))[4];
                var pass = (data.Split("-->>"))[5];

                var resultset = (new SqlConnection($@"Server={server}; Database={database}; UID={user}; PWD={pass};")).Query(data);
                var json = JsonConvert.SerializeObject(resultset);
                json += "-->>" + (data.Split("-->>"))[1];

                SendMessageAsync(json);
            }
            catch (Exception e)
            {
                SendMessageAsync($"Error: {e.Message}");
            }
        }
    }
}
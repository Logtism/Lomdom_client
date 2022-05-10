using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Net.Http;
using Newtonsoft.Json.Linq;

public class Request
{
    public static (int status_code, JObject json_content) Post(string url, string JsonBody)
    {
        using (var client = new HttpClient())
        {
            StringContent string_content = new StringContent("{" + JsonBody.Replace("'", "\"") + "}", Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, string_content).Result;
            return ((int)response.StatusCode, JObject.Parse(response.Content.ReadAsStringAsync().Result));
        }
    }
}
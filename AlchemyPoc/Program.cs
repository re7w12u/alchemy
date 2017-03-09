using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AlchemyPoc
{
    class Program
    {
        static void Main(string[] args)
        {

            var url = "https://gateway-a.watsonplatform.net/calls/text/TextGetCombinedData";

            string token = (Task.Run(async () => await PostFormUrlEncoded<string>(url))).Result;

            Console.WriteLine(token);
            Console.ReadLine();
        }

     
        private static void WriteResult(string message)
        {
            File.WriteAllText(String.Format("output_{0}.txt", DateTime.Now.Ticks), message);
        }


        public static async Task<string> PostFormUrlEncoded<TResult>(string url)
        {
            string data = File.ReadAllText("./../../recette.txt", Encoding.UTF8);
            List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();

            postData.Add(new KeyValuePair<string, string>("apikey", "xxx"));
            postData.Add(new KeyValuePair<string, string>("outputMode", "json"));
            postData.Add(new KeyValuePair<string, string>("extract", "entities,keywords"));
            postData.Add(new KeyValuePair<string, string>("model", "rxxx"));
            postData.Add(new KeyValuePair<string, string>("sentiment", "1"));
            postData.Add(new KeyValuePair<string, string>("url", "https://www.ibm.com/us-en/"));
            postData.Add(new KeyValuePair<string, string>("text", data));


            using (var httpClient = new HttpClient())
            {
                using (var content = new FormUrlEncodedContent(postData))
                {
                    content.Headers.Clear();
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                    HttpResponseMessage response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string json;
                        using(Stream responseStream = await response.Content.ReadAsStreamAsync())
                        {
                            json = new StreamReader(responseStream).ReadToEnd();                            
                        }
                        return json;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

    }
}

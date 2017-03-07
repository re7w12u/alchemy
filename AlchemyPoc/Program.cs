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

        private static void Method1(string url)
        {
            using (WebClient client = new WebClient())
            {
                string data = File.ReadAllText("./../../recette.txt", Encoding.UTF8);

                client.Encoding = Encoding.UTF8;
                client.Headers["apikey"] = "3ea96d21f40035354488d86b20387df5fdb6a6d1";
                client.Headers["outputMode"] = "json";
                client.Headers["extract"] = "entities,keywords";
                client.Headers["model"] = "c5737a8e-14c0-44e4-ab78-11886b0129dd";
                client.Headers["sentiment"] = "1";
                client.Headers["maxRetrieve"] = "1";
                client.Headers["url"] = "https://www.ibm.com/us-en/";
                client.Headers["text"] = data;

                var reqparm = new System.Collections.Specialized.NameValueCollection();
                client.UploadStringCompleted += (object sender, UploadStringCompletedEventArgs e) =>
                {
                    Console.WriteLine("response received...");
                    if (!String.IsNullOrWhiteSpace(e.Error.Message)) WriteResult(e.Error.Message);
                    else WriteResult(e.Result);

                    Environment.Exit(0);
                };

                client.UploadStringAsync(new Uri(url), data);
                Console.WriteLine("request sent...");
            }
        }

        private static void WriteResult(string message)
        {
            File.WriteAllText(String.Format("output_{0}.txt", DateTime.Now.Ticks), message);
        }


        public static async Task<string> PostFormUrlEncoded<TResult>(string url)
        {
            string data = File.ReadAllText("./../../recette.txt", Encoding.UTF8);
            List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();

            postData.Add(new KeyValuePair<string, string>("apikey", "3ea96d21f40035354488d86b20387df5fdb6a6d1"));
            postData.Add(new KeyValuePair<string, string>("outputMode", "json"));
            postData.Add(new KeyValuePair<string, string>("extract", "entities,keywords"));
            postData.Add(new KeyValuePair<string, string>("model", "c5737a8e-14c0-44e4-ab78-11886b0129dd"));
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
                    //return await response.Content.ReadAsAsync<TResult>();
                }
            }
        }

    }
}

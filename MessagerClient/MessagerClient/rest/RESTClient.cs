using MessagerClient.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MessagerClient.REST
{
    public class RESTClient
    {
        private string? url;

        private HttpClient? Client;

        private int? sessionId;


        public RESTClient Initilize()
        {
            if (url is null)
                throw new MessagerHardClientException("url is null.");
            if(Client is not null)
                Client.Dispose();
            
            Client = new HttpClient();

            return this;
        }

        public RESTClient SetURL(string url)
        {
            this.url = url;
            return this;
        }

        public RESTClient SetSessionId(int sessionId)
        {
            this.sessionId = sessionId;
            return this;
        }

        private HttpRequestMessage buildMessage(HttpMethod method, string subUrl, string? content)
        {
            HttpRequestMessage message = new HttpRequestMessage()
            {
                Method = method,
                RequestUri = new Uri(this.url + subUrl)
            };

            if (this.sessionId is not null)
                message.Headers.Add("sessionId", this.sessionId.ToString());

            if (content is not null)
                message.Content = new StringContent(content, Encoding.UTF8, "application/json");

            return message;
        }

        public string SendAndResiveRequest(HttpMethod method, string subUrl, string? content = null)
        {
            if(Client is null)
                throw new MessagerHardClientException("HTTP client initilized.");
            Exception? ex = null;
            var responce = Client.SendAsync(buildMessage(method, subUrl, content)).ContinueWith((responseTask) =>
            {
                string result = "";
                try { 
                    result = responseTask.Result.Content.ReadAsStringAsync().Result;
                }
                catch(Exception _ex) { ex = _ex; }
                return result;
            }).Result;
            if (ex is not null)
                throw ex;
            return responce;
        }
    }
}

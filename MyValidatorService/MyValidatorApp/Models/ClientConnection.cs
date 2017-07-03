using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Net;
using System.Text;
using MyValidatorApp.Interfaces;
using Newtonsoft.Json.Linq;
using NLog;

namespace MyValidatorApp.Models
{

    public class ClientConnection : IConnectionClient
    {
        private const string _method = "POST";
        private const string _ContentType = "application/json";
        private const string _ServiceURL = "http://localhost:61067/";
        private StreamReader _reader;
        private WebResponse _response;
        private Stream _dataStream;
        // private HttpClient _httpClient = new HttpClient();
        private WebRequest _request;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        public object GetServiceResponse(string id)
        {
            try
            {
                _request = WebRequest.Create(_ServiceURL);
                _request.Method = _method;
                string postData = "{'id':'" + id + "'}";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                _request.ContentType = _ContentType;
                _request.ContentLength = byteArray.Length;
                _dataStream = _request.GetRequestStream();
                _dataStream.Write(byteArray, 0, byteArray.Length);
                _dataStream.Close();
                _response = _request.GetResponse();
                _dataStream = _response.GetResponseStream();
                _reader = new StreamReader(_dataStream);
                string responseFromServer = _reader.ReadToEnd();
                dynamic Response = JObject.Parse(responseFromServer);
                return Response;
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message);
                throw;
            }

        }

        public void CloseConnection()
        {
            _reader.Close();
            _dataStream.Close();
            _response.Close();
        }

        public object GetSavedUserDetails()
        {
            try
            {
                _request = WebRequest.Create(_ServiceURL);
                _response = _request.GetResponse();
                _dataStream = _response.GetResponseStream();
                _reader = new StreamReader(_dataStream);

                string responseFromServer = _reader.ReadToEnd();
                //TOdo: return null from web service..
                if (responseFromServer == "[]") //Empty 
                {
                    return null;
                }
                else
                {
                    dynamic Response = JObject.Parse(responseFromServer);
                    return Response;
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message);
                throw;
            }


        }

    }
}
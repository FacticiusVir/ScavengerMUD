using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Keeper.Prolog.SwishApi
{
    public static class WebApi
    {
        public static Guid Create(string program)
        {
            var request = WebRequest.CreateHttp("http://swish.swi-prolog.org/pengine/create");
            request.Method = "POST";
            request.ContentType = "application/json";
            var requestData = Create(new CreateRequest()
            {
                src_text = program,
                format = "json",
                application = "swish",
                destroy = false
            });

            request.ContentLength = requestData.Length;
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(requestData, 0, requestData.Length);
            }

            var response = request.GetResponse();

            using (var responseStream = response.GetResponseStream())
            {
                var responseData = Read<CreateResponse>(responseStream);
                return responseData.id;
            }
        }

        public static XDocument Send(Guid programGuid, string query)
        {
            var request = WebRequest.CreateHttp(string.Format("http://swish.swi-prolog.org/pengine/send?id={0}&event={1}&format=json",
                                                                programGuid,
                                                                Uri.EscapeUriString("ask(('$swish wrapper'((" + query + "))), [breakpoints([])])")));
            request.ContentType = "application/json";

            var response = request.GetResponse();

            var buffer = new MemoryStream();

            using (var responseStream = response.GetResponseStream())
            {
                responseStream.CopyTo(buffer);
            }

            buffer.Position = 0;

            using (var responseStringReader = new StreamReader(buffer, Encoding.UTF8, false, 128, true))
            {
                var responseString = responseStringReader.ReadToEnd();

                //Console.WriteLine(responseString);
            }

            buffer.Position = 0;

            using (var responseReader = JsonReaderWriterFactory.CreateJsonReader(buffer, new System.Xml.XmlDictionaryReaderQuotas()))
            {
                while (responseReader.Read() && responseReader.Name != "data") ;

                return XDocument.Parse(responseReader.ReadInnerXml());
            }
        }

        private static byte[] Create<T>(T value)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, value);
                return stream.ToArray();
            }
        }

        private static T Read<T>(Stream stream)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            return (T)serializer.ReadObject(stream);
        }

        [DataContract]
        private struct CreateRequest
        {
            [DataMember]
            public string src_text;
            [DataMember]
            public string format;
            [DataMember]
            public string application;
            [DataMember]
            public bool destroy;
        }

        [DataContract]
        private struct CreateResponse
        {
            [DataMember]
            public string @event;
            [DataMember]
            public Guid id;
            [DataMember]
            public int slave_limit;
        }
    }
}

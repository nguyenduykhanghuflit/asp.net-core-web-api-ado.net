using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ApiDemo.Services
{
    public class SendNotify
    {

        public string HandleSendNotify(string title = "hihi", string body = "hahaa", int badge = 1)
        {
            try
            {
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                // ServerKey - Key from Firebase cloud messaging server  
                tRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAAS3DZ9Ro:APA91bEQDFwlIFWyIcjdHKWBSJxLsVkw1UmRnpdr9v4HatogpDFroQWGpu-awkKVz-846slzL8YvZ0f7eKjvrx0xPilC76h7MqI8iayvuenm42h3T-FWRLNbXdETmFU0l1sh2Pm3wnV5"));
                // SenderId - From firebase project setting  
                tRequest.Headers.Add(string.Format("Sender: id={0}", "324015879450"));
                tRequest.ContentType = "application/json";
                // DeviceId - obtained when the device is registered
                var deviceId = "e9WwouAwtOI4gVq7nMbOzh:APA91bFKQrqeSplnSiVXMTQAAioRDf0H2f8JeBVLvPaV8sVmhCLKsH8cPNIsYHmNb6yZb6isD_UMi_vNBldWDLkIUatCBhDucFr33r8CIaOzwbnhRmnFK2vM0i04WgiHS8SSrggCUywg";
                var payload = new
                {
                    to = deviceId,
                    priority = "high",
                    content_available = true,
                    notification = new
                    {
                        body,
                        title,
                        badge
                    },
                };

                string postbody = JsonConvert.SerializeObject(payload).ToString();
                Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    String sResponseFromServer = tReader.ReadToEnd();
                                    return (sResponseFromServer);
                                }
                        }
                    }
                }

                return ("No res");
            }
            catch (Exception exp)
            {
                return (exp.Message);
            }
        }
    }
}

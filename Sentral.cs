using System.Net;
using System.IO;
using System.Text;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace KHC_Athletics_and_House_Points
{
    class Sentral
    {
        static CookieContainer cookies;
        public static List<Data> houseData = new();
        public static bool connected;


        public class Data
        {
            public string house_name;
            public string house_colour;
            public int house_points;
            public int house_sentralid;
        }


        public static void Login()
        {
            string formUrl = "https://kelsohs.sentral.com.au/check_login";
            string formParams = string.Format("sentral-username={0}&sentral-password={1}", Program.sentral_username, Program.sentral_password);
            byte[] postBytes = Encoding.ASCII.GetBytes(formParams);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(formUrl);
            cookies = new CookieContainer();
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postBytes.Length;
            request.CookieContainer = cookies;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            using (var stream = request.GetRequestStream()) { stream.Write(postBytes, 0, postBytes.Length); }
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            if (responseString == "FAIL")
            {
                MessageBox.Show("Error logging in. Please Try again.");
                connected = false;
            }
            else { connected = true; }
        }


        public static void DownloadHouseData()
        {
            if (connected == true)
            {
                //Download the house points page
                var scoreReq = WebRequest.Create("https://kelsohs.sentral.com.au/housepoints/") as HttpWebRequest;
                scoreReq.CookieContainer = cookies;
                scoreReq.Method = "GET";
                scoreReq.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                HttpWebResponse resp = scoreReq.GetResponse() as HttpWebResponse;
                var responseString = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                // Read the html
                HtmlDocument doc = new();
                doc.LoadHtml(responseString);
                var nodes = doc.DocumentNode.SelectNodes("//div[@class='house-point-container center']");
                string name;
                foreach (var div in nodes)
                {
                    var newline = new Data();
                    name = div.SelectSingleNode(".//h3").InnerHtml;
                    newline.house_colour = div.GetAttributeValue("style", null).Split(": ")[1];
                    newline.house_name = name.Split("8517 ")[1];
                    try { newline.house_points = int.Parse(div.SelectSingleNode(".//span").InnerHtml.Split(',')[0] + div.SelectSingleNode(".//span").InnerHtml.Split(',')[1]); }
                    catch { newline.house_points = int.Parse(div.SelectSingleNode(".//span").InnerHtml); }
                    newline.house_sentralid = int.Parse(div.SelectSingleNode(".//a[@href]").GetAttributeValue("href", string.Empty).Split('/').Last());
                    houseData.Add(newline);
                }
            }
            else
            {
                MessageBox.Show("Error downloading House Points from Sentral, please retry by pressing Login again.\nIf the problem persists, check your internet connection, or restart the program.");
                Login();
            }
        }


        public static void AddHousePoints(int house_id, int points, string event_name)
        {
            if (connected == true)
            {
                string formUrl = "https://kelsohs.sentral.com.au/housepoints/house/add_points/";
                string formParams = string.Format("houses[]={0}&date&points={1}&description={2}&action={3}", houseData[house_id].house_sentralid, points, event_name, "add-points");
                byte[] postBytes = Encoding.ASCII.GetBytes(formParams);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(formUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postBytes.Length;
                request.CookieContainer = cookies;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                using (var stream = request.GetRequestStream()) { stream.Write(postBytes, 0, postBytes.Length); }
                var response = (HttpWebResponse)request.GetResponse();
            }
            else
            {
                MessageBox.Show("Error Adding House Points to Sentral, please retry by pressing submit again.\nIf the problem persists, check your internet connection, or restart the program.");
                Login();
            }
        }
    }
}
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;

namespace Tides
{
    class Program
    {
        static void Main(string[] args)
        {
            string beginDate = "";
            string endDate = "";
            string station = "";
            string location = "8654467 USCG Station Hatteras, NC";
            string fileName = "";

            for(int c=0;c<args.Length;c++){
                if(args[c].Length > 0){
                    if(args[c].Substring(0,1) == "-")
                    {
                        string testDateString = "";
                        DateTime testDate = new DateTime(1970, 1,1);
                        switch(args[c].ToUpper())
                        {
                            case "-BEGINDATE":
                                if(c+1 < args.Length)
                                {
                                    c++;
                                    testDateString = args[c];
                                    if(DateTime.TryParse(testDateString, out testDate))
                                    {
                                        beginDate = testDate.ToString("yyyyMMdd");
                                    }
                                }
                            break;
                            case "-ENDDATE":
                                if(c+1 < args.Length)
                                {
                                    c++;
                                    testDateString = args[c];
                                    if(DateTime.TryParse(testDateString, out testDate))
                                    {
                                        endDate = testDate.ToString("yyyyMMdd");
                                    }
                                }
                            break;
                            case "-STATION":
                                if(c+1 < args.Length)
                                {
                                    c++;
                                    station = args[c];
                                }
                            break;
                            case "-FILENAME":
                                if(c+1 < args.Length)
                                {
                                    c++;
                                    fileName = args[c];
                                }
                            break;
                        }
                    
                    }
                }
            }

            if(endDate == "" || beginDate == "" || station == ""){
                Console.WriteLine("Usage:");
                Console.WriteLine("\tTides -beginDate <beginDate> -endDate <endDate> -station <station> -fileName <fileName>");
                Console.WriteLine("\tFilename is optional.  If nothing is passed, the output will write to the console.");
                return;
            }
            string uri = "https://api.tidesandcurrents.noaa.gov/api/prod";
            RestClient client = new RestClient(uri);
            RestRequest request = new RestRequest("datagetter?product=predictions&application=NOS.COOPS.TAC.WL&begin_date=" + beginDate + "&end_date=" + endDate + "&datum=MLLW&station=" + station + "&time_zone=lst_ldt&units=english&interval=hilo&format=json", Method.GET);
            IRestResponse<List<string>> response = client.Execute<List<string>>(request);
            try
            {
                Newtonsoft.Json.Linq.JObject myReturnObject = Newtonsoft.Json.Linq.JObject.Parse(response.Content);
                if(myReturnObject["predictions"] == null){
                    throw new Exception("No predictions in data returned!");
                }
                if(myReturnObject["predictions"] is not Newtonsoft.Json.Linq.JArray){
                    throw new Exception("Predictions were not returned as an array!");
                }
                string outputString = "Subject,Start Date,Start Time,End Date,End Time,Description,Location,Private\n";
                foreach(var tideItem in myReturnObject["predictions"])
                {

                    string tide = (tideItem["type"].ToString()== "L")?"Low Tide":"High Tide";
                    DateTime startTime = DateTime.Parse(tideItem["t"].ToString());
                    DateTime endTime = startTime.AddMinutes(30);

                    outputString += tide + ": " 
                        + tideItem["v"].ToString() + " ft.," 
                        + startTime.ToString("MM/dd/yyyy") + "," 
                        + startTime.ToString("hh:mm tt") + "," 
                        + endTime.ToString("MM/dd/yyyy") + "," 
                        + endTime.ToString("hh:mm tt") + ","
                        + "\"Tides from: http://tidesandcurrents.noaa.gov/tide_predictions.shtml.\\nCalendar template created in dotNetCore by Brian Wharton 'https://github.com/dafoink/tidesToGoogleAPI'\","
                        + "\"Location: " + location + "\","
                        + "false"
                        + "\n"
                        ;
                }

                if(!string.IsNullOrEmpty(fileName))
                {
                    try
                    {
                        using(StreamWriter outputFile = new StreamWriter(fileName))
                        {
                            outputFile.WriteLine(outputString);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Could not write to " + fileName);
                    }
                }
                else
                {
                    Console.WriteLine(outputString);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Could not retrieve data.\n" + ex.Message);
            }

        }
    }
}

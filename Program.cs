using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Xml.Linq;
using System.IO;

namespace AddressesToCoords
{
    class Program
    {
        static void Main(string[] args)
        {
            string API_KEY = "";            //Your personal API_KEY goes here

            Console.WriteLine("Enter the path of the text document that contains the adresses:");       //Get the adress list from text file
            string path = Console.ReadLine();

            Console.WriteLine("Enter the path where the results will be stored in a csv file:");        //Get the destination folder
            string resPath = Console.ReadLine();

            string[] adresses = File.ReadAllLines(path);        //Get adresses from the file

            var csv = new StringBuilder();                      //Stringbuilder for making the csv file

            for (int i = 0; i < adresses.Length; i++)           //loop through all adresses
            {
                if (adresses[i] != "")                          //if the line is not empty
                {
                    string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?key={1}&address={0}&sensor=false", Uri.EscapeDataString(adresses[i]), API_KEY);
                                                                //Request url

                    WebRequest request = WebRequest.Create(requestUri);             //Make the request 
                    WebResponse response = request.GetResponse();                   //Get the response
                    XDocument xdoc = XDocument.Load(response.GetResponseStream());  //Set the response to a xdoc variable

                    XElement result = xdoc.Element("GeocodeResponse").Element("result");        //Get result part of the xml
                    XElement locationElement = result.Element("geometry").Element("location");  //Get the location element
                    XElement lat = locationElement.Element("lat");                              //Get latitude
                    XElement lng = locationElement.Element("lng");                              //Get longitude
                    Console.WriteLine((lat.Value + " " + lng.Value)+ " " + adresses[i]);        //Write to the console

                    var first = lat.Value;      //Store Coords and addresses to the stringbuilder
                    var second = lng.Value;
                    var third = adresses[i];
                    var newLine = string.Format("{0},{1},{2}", first, second, third);
                    csv.AppendLine(newLine);
                }
            }
            Console.WriteLine("Done.");
            File.WriteAllText(resPath+"Coordinates.csv", csv.ToString());   //Make the Csv file using the stringbuilder
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RESTConsumerTemplate
{
    class Program
    {
        private static string uriLocal = "https://localhost:44336/api/customer/"; // Skifte Uri til den Korrekte
        private static string uri = "https://restdatabasetemplate.azurewebsites.net/api/DBTemps/"; // Skifte Uri til den Korrekte

        public static async Task<IEnumerable<DBTemp>> GetLists()
        {
            using (HttpClient client = new HttpClient())
            {
                //Ved at skrive Await sikrer vi at den udfører GetStringAsync
                //Send et GET request til uri og returer reponse body som et string
                string content = await client.GetStringAsync(uri);
                //Laver JSONFormatet om til string så man kan printe den ud
                IEnumerable<DBTemp> list = JsonConvert.DeserializeObject<IEnumerable<DBTemp>>(content);
                return list;
            }
        }

        public static async Task<DBTemp> GetByiId(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                //Ved at skrive Await sikrer vi at den udfører GetStringAsync
                //Send et GET request til uri og returer reponse body som et string
                string content = await client.GetStringAsync(uri + id);
                //Laver JSONFormatet om til string så man kan printe den ud
                DBTemp oneID = JsonConvert.DeserializeObject<DBTemp>(content);
                return oneID;
            }
        }

        public static async Task<DBTemp> DeleteById(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                //Ved at skrive Await sikrer vi at den udfører DeleteAsync
                //DeleteAsync sender en request til
                HttpResponseMessage response = await client.DeleteAsync(uri + id);
                Console.WriteLine("Statuscode" + response.StatusCode);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new Exception("Customer not found or customer is persistent. Try another id");

                }
                response.EnsureSuccessStatusCode();
                string str = await response.Content.ReadAsStringAsync();
                DBTemp deletedCustomer = JsonConvert.DeserializeObject<DBTemp>(str);
                return deletedCustomer;
            }
        }

        public static async Task<DBTemp> AddAsync(DBTemp newDBTemp)
        {
            using (HttpClient client = new HttpClient())
            {
                //int lastId = GetLists().Result.Last().Id;

                var jsonString = JsonConvert.SerializeObject(newDBTemp); //, lastId + 1

                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(uri, content);
                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    throw new Exception("Customer already exists. Try another id");
                }
                response.EnsureSuccessStatusCode();
                string str = await response.Content.ReadAsStringAsync();
                DBTemp copyOfNewCustomer = JsonConvert.DeserializeObject<DBTemp>(str);
                return copyOfNewCustomer;
            }
        }

        public static async Task<DBTemp> UpdateCustomerAsync(DBTemp newCustomer, int id)
        {
            using (HttpClient client = new HttpClient())
            {
                string requestUri = uri + id;
                // Laver object om til en JSON string
                var jsonString = JsonConvert.SerializeObject(newCustomer);
                Console.WriteLine("JSON: " + jsonString);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                // await gøre at den async skal være færdig før den gå videre
                HttpResponseMessage response = await client.PutAsync(requestUri, content);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new Exception("Customer not found. Try another id");
                }
                //response.EnsureSuccessStatusCode();
                string str = await response.Content.ReadAsStringAsync();
                DBTemp updCustomer = JsonConvert.DeserializeObject<DBTemp>(str);
                return updCustomer;
            }
        }

        static void Main(string[] args)
        {
            //var resDelete = DeleteById(3).Result;


            //IEnumerable<DBTemp> result = GetLists().Result;
            //Console.WriteLine("Amount of Elements: " + result.Count());
            //foreach (DBTemp i in result)
            //{
            //    Console.WriteLine(i.ToString());
            //}
            //Console.WriteLine("");
            //DBTemp resById = GetByiId(1).Result;
            //Console.WriteLine(resById.ToString());
            //Console.WriteLine("");

            DBTemp newDbTemp = new DBTemp("Pig", "FattoX");
            DBTemp adding = AddAsync(newDbTemp).Result;

            //DBTemp newDbTemp1 = new DBTemp("Fat", "Godx");
            //DBTemp adding2 = UpdateCustomerAsync(newDbTemp, 4).Result;


            Console.ReadLine();
        }
    }
}

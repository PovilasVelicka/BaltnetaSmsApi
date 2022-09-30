using BaltnetaSmsApi.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BaltnetaSmsApi
{
    public class SmsService
    {
        static Dictionary<string, string> _errorCodes = new Dictionary<string, string>( );
        private static readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _login;

        static SmsService ( )
        {
            _httpClient = new HttpClient( );
            _httpClient.BaseAddress = new Uri("https://sms.baltneta.lt/");
            _httpClient.DefaultRequestHeaders.Accept.Clear( );
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _errorCodes.Add("000", "Service unavailable");
            _errorCodes.Add("1", "Signature not specified");
            _errorCodes.Add("2", "Login not specified");
            _errorCodes.Add("3", "Text not specified");
            _errorCodes.Add("4", "Phone number not specified");
            _errorCodes.Add("5", "Sender not specified");
            _errorCodes.Add("6", "Invaild signature");
            _errorCodes.Add("7", "Invalid login");
            _errorCodes.Add("8", "Invalid sender name");
            _errorCodes.Add("9", "Sender name not registered");
            _errorCodes.Add("10", "Sender name not approved");
            _errorCodes.Add("11", "There are forbidden words in the text");
            _errorCodes.Add("12", "Error in SMS sending");
            _errorCodes.Add("13", "Phone number is in the stop list. SMS sending to this number is forbidden.");
            _errorCodes.Add("14", "There are more than 50 numbers in the request");
            _errorCodes.Add("15", "List not specified");
            _errorCodes.Add("16", "Invalid phone number");
            _errorCodes.Add("17", "SMS ID not specified");
            _errorCodes.Add("18", "Status not obtained");
            _errorCodes.Add("19", "Empty response");
            _errorCodes.Add("20", "The number already exists");
            _errorCodes.Add("21", "No name");
            _errorCodes.Add("22", "Template already exists");
            _errorCodes.Add("23", "Month not specifies (Format: YYYY-MM)");
            _errorCodes.Add("24", "Timestamp not specified");
            _errorCodes.Add("25", "Error in access to the list");
            _errorCodes.Add("26", "There are no numbers in the list");
            _errorCodes.Add("27", "No valid numbers");
            _errorCodes.Add("28", "Date of start not specified (Format: YYYY-MM-DD)");
            _errorCodes.Add("29", "Date of end not specified (Format: YYYY-MM-DD)");
            _errorCodes.Add("30", "No date (format: YYYY-MM-DD)");
            _errorCodes.Add("31", "Closing direction to the user");
            _errorCodes.Add("32", "Not enough money");
            _errorCodes.Add("33", "Phone number is not set");
            _errorCodes.Add("34", "Phone is in stop list");
            _errorCodes.Add("35", "Not enough money");
            _errorCodes.Add("36", "Can not obtain information about phone");
            _errorCodes.Add("37", "Base Id is not set");
            _errorCodes.Add("38", "Phone number is already exist in this base");
            _errorCodes.Add("39", "Phone number is not exist in this base");
        }

        public SmsService (string apiKey, string login)
        {
            _apiKey = apiKey;
            _login = login;
        }

        public async Task<IServerResponse> SendSmsAsync (string comaSeparatedNumbers, string smsText, string sender)
        {
            var validNumbers = GetValidNumbers(comaSeparatedNumbers);
            var sms = new Sms(_login, validNumbers, sender, smsText);

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync(GetRequestString(sms));
            }
            catch (HttpRequestException e)
            {
                return new ServerResponse(false, e.InnerException.Message);
            }
            catch (Exception)
            {
                throw;
            }

            if (response.IsSuccessStatusCode)
            {

                var result = response.Content.ReadAsStringAsync( ).Result;
                if (JsonConvert.DeserializeObject<ResponseDto>(result).error == null)
                {
                    var msg = JsonConvert.DeserializeObject<Dictionary<string, ResponseDto>>(result);
                    return new ServerResponse(
                        isSucess: true,
                        smsReports: msg.Select(m =>
                        new ResponseDto
                        {
                            phone_nr = m.Key,
                            count_sms = m.Value.count_sms,
                            cost = m.Value.cost,
                            error = m.Value.error,
                            message = _errorCodes.FirstOrDefault(e => e.Key == m.Value.error).Value ?? "",
                            id_sms = m.Value.id_sms,

                        }).ToList( ));
                }
                else
                {
                    return new ServerResponse(
                        isSucess: false,
                        message: "");
                }
            }
            else
            {
                return new ServerResponse(
                       isSucess: false,
                       message: "Service error");
            }
        }

        private string GetRequestString (Sms sms)
        {
            return string.Format("external/get/send.php" +
                "?login={0}" +
                "&signature={1}" +
                "&phone={2}" +
                "&text={3}" +
                "&sender={4}" +
                "&timestamp={5}",
                sms.Login,
                CreateMD5(sms.ToString( ) + _apiKey).ToLower( ),
                sms.Phone,
                sms.Text,
                sms.Sender,
                sms.TimeStamp
                );
        }
        private static string GetValidNumbers (string separatedPhones)
        {
            string result = Regex.Replace(separatedPhones, @"[^\d\+\;\,]", "");
            return string.Join(",", result.Split(new char[ ] { ';', ',', '+' }, StringSplitOptions.RemoveEmptyEntries));
        }

        public static string CreateMD5 (string input)
        {
            return
                BitConverter.ToString(
                    MD5.Create( )
                    .ComputeHash(Encoding.UTF8.GetBytes(input)))
                .Replace("-", "");
        }

        private bool disposed = false;

        public void Dispose ( )
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose (bool disposing)
        {
            if (!this.disposed)
            {
                disposed = true;
            }
        }
    }
}

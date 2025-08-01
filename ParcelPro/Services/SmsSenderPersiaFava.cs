using System.Text.Json;

namespace ParcelPro.Services
{
    public class SmsSenderPersiaFava
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _baseUrl = "http://sms.persiafava.com/webservice/rest";
        private readonly string _apiKey;
        private readonly string _senderNumber;

        public SmsSenderPersiaFava(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = "195:65465daab29bd";
            _senderNumber = "30005381801038";
        }

        public async Task<(bool Success, string MessageId, string Error)> SendSmsAsync(string phoneNumber, string message)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var parameters = new Dictionary<string, string>
                {
                    { "api_key", _apiKey },
                    { "receiver_number", phoneNumber },
                    { "sender_number", _senderNumber },
                    { "note_arr[]", message },
                    { "date", "0" },
                    { "request_uniqueid", "0" },
                    { "flash", "no" },
                    { "onlysend", "no" },
                    { "show_faktor", "no" }
                };

                var content = new FormUrlEncodedContent(parameters);
                var response = await client.PostAsync($"{_baseUrl}/sms_send", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return (false, null, $"خطا در ارسال پیامک: {response.StatusCode}");
                }

                var result = JsonSerializer.Deserialize<PersiaFavaResponse>(responseContent);
                if (result?.Result == true && result.List?.Any() == true)
                {
                    return (true, result.List.First(), null);
                }

                return (false, null, "خطا در ارسال پیامک: پاسخ نامعتبر از سرور");
            }
            catch (Exception ex)
            {
                return (false, null, $"خطا در ارسال پیامک: {ex.Message}");
            }
        }

        public async Task<(bool Success, UserInfo UserInfo, string Error)> GetUserInfoAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{_baseUrl}/user_info?api_key={_apiKey}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return (false, null, $"خطا در دریافت اطلاعات کاربری: {response.StatusCode}");
                }

                var result = JsonSerializer.Deserialize<PersiaFavaUserInfoResponse>(responseContent);
                if (result?.Result == true && result.List != null)
                {
                    return (true, result.List, null);
                }

                return (false, null, "خطا در دریافت اطلاعات کاربری: پاسخ نامعتبر از سرور");
            }
            catch (Exception ex)
            {
                return (false, null, $"خطا در دریافت اطلاعات کاربری: {ex.Message}");
            }
        }
    }

    public class PersiaFavaResponse
    {
        public bool Result { get; set; }
        public List<string> List { get; set; }
    }

    public class PersiaFavaUserInfoResponse
    {
        public bool Result { get; set; }
        public UserInfo List { get; set; }
    }

    public class UserInfo
    {
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Cash { get; set; }
        public string Date { get; set; }
        public string DateExpire { get; set; }
        public NumberInfo Number { get; set; }
    }

    public class NumberInfo
    {
        public bool Result { get; set; }
        public List<UserNumber> ListUser { get; set; }
    }

    public class UserNumber
    {
        public string Number { get; set; }
        public string Order { get; set; }
    }

}


namespace BaltnetaSmsApiCore.Entities
{
    internal class ResponseDto : IResponseDto
    {
        public string phone_nr { get; set; }
        public string error { get; set; }
        public string message { get; set; }
        public string id_sms { get; set; }
        public string cost { get; set; }
        public string count_sms { get; set; }

    }
}

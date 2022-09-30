namespace BaltnetaSmsApiCore
{
    public interface IResponseDto
    {
        string phone_nr { get; }
        string error { get; }
        string message { get; }
        string id_sms { get; }
        string cost { get; }
        string count_sms { get; }
    }
}

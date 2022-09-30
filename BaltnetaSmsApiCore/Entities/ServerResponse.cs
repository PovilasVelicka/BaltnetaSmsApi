using System.Collections.Generic;

namespace BaltnetaSmsApiCore.Entities
{
    internal class ServerResponse : IServerResponse
    {
        public bool IsSucess { get; set; }
        public string Message { get; set; }
        public IEnumerable<IResponseDto> SmsReports { get; set; }

        public ServerResponse (bool isSucess, List<ResponseDto> smsReports)
        {
            IsSucess = isSucess;
            Message = "";
            SmsReports = smsReports;
        }

        public ServerResponse (bool isSucess, string message)
        {
            IsSucess = isSucess;
            Message = message;
        }

        public ServerResponse (bool isSucess, string message, List<ResponseDto> smsReports)
        {
            IsSucess = isSucess;
            Message = message;
            SmsReports = smsReports;
        }
    }
}

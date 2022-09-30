using BaltnetaSmsApi.Entities;
using System.Collections.Generic;

namespace BaltnetaSmsApi
{
    public interface IServerResponse
    {
        bool IsSucess { get; }
        string Message { get; }
        IEnumerable<IResponseDto> SmsReports { get; }
    }
}

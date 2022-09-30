
using System.Collections.Generic;

namespace BaltnetaSmsApiCore
{
    public interface IServerResponse
    {
        bool IsSucess { get; }
        string Message { get; }
        IEnumerable<IResponseDto> SmsReports { get; }
    }
}

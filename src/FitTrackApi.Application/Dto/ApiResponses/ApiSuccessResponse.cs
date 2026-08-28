namespace FitTrackApi.Application.Dto.ApiResponses;

/// <summary>
/// Represents a success response returned by API
/// </summary>
/// <param name="Message">
///     A human‑readable of success message
/// </param>
/// <param name="Data">
///     Data that return API in a response body
/// </param>
public record ApiSuccessResponse
(
    string Message, 
    object Data
);

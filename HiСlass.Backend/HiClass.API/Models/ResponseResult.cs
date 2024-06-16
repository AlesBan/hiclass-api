using HiClass.Application.Common.Exceptions;

namespace HiClass.API.Models;

public class ResponseResult
{
    public bool Result { get; set; }
    public List<ExceptionDto> Errors { get; set; } = new List<ExceptionDto>();
    public object? Value { get; set; }
}
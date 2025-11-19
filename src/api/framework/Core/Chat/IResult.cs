using System.Collections.Generic;

// namespace AkhbarBlazorBackend.Shared.Wrapper
namespace FSH.Framework.Core.Chat;
    public interface IResult
    {
        List<string> Messages { get; set; }

        bool Succeeded { get; set; }
    }

    public interface IResult<out T> : IResult
    {
        T Data { get; }
    }

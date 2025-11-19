using System.Collections.Generic;
using System.Threading.Tasks;
using FSH.Framework.Core.Chat.Models;

namespace FSH.Framework.Core.Chat;
    public interface IChatService
    {
        Task<Result<IEnumerable<ChatUserResponse>>> GetChatUsersAsync(string userId);

        Task<IResult> SaveMessageAsync(ChatHistory<IChatUser> message);

        Task<Result<IEnumerable<ChatHistoryResponse>>> GetChatHistoryAsync(string userId, string contactId);
    }

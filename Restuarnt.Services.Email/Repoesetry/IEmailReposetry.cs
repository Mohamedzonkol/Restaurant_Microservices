using Restuarnt.Services.Email.Messages;
namespace Restuarnt.Services.Email.Repoesetry
{
    public interface IEmailReposetry
    {
        Task SendAndEmailLog(UpdatePaymentResult message);
    }
}

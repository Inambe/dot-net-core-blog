using InambeBlog.Models;
using InambeBlog.Models.User;

namespace InambeBlog.Repositories
{
    public interface IUserRepo
    {
        void Create(UserModel user);
        bool ValidateCredentials(string email, string password, out UserModel user);
        UserModel GetById(int id, bool relatedData = false);

        void CreateUserVerification(UserModel userModel);
        void VerifyUser(string token, out SignInViewState state);
    }
}

using System;
using System.Linq;
using InambeBlog.Data;
using InambeBlog.Helpers;
using InambeBlog.Models;
using InambeBlog.Models.User;
using InambeBlog.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InambeBlog.Repositories
{
    public class UserRepo : IUserRepo
    {
        private const string MailTitle = "Verify your account | inambe";
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public UserRepo(
            AppDbContext context,
            IEmailService emailService,
            IConfiguration configuration)
        {
            _context = context;
            _emailService = emailService;
            _configuration = configuration;
        }

        public void Create(UserModel userModel)
        {
            _context.Users.Add(userModel);
            _context.SaveChanges();
        }

        public UserModel GetById(int id, bool relatedData = false)
        {
            if (relatedData)
            {
                var user = _context.Users
                    //.Include((UserModel u) => u.UserPosts)
                    .FirstOrDefault(u => u.Id == id);
                _context.Entry(user)
                .Collection(u => u.UserPosts)
                .Load();
            }
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public bool ValidateCredentials(string email, string password, out UserModel user)
        {
            var findUser = _context.Users.FirstOrDefault(u => u.Email == email);
            if (findUser == null)
            {
                user = null;
                return false;
            }
            
            if(!Hash.Validate(password, findUser.PasswordSalt, findUser.PasswordHash))
            {
                user = null;
                return false;
            }

            user = findUser;
            return true;
        }

        public void CreateUserVerification(UserModel userModel)
        {
            var hash = Hash.RandomHash();
            var activationUrl = _configuration["BaseURL"] + "/Account/Activate?token=" + hash;
            var mailBody = $"Activate your account using <a href=\"{ activationUrl }\">this</a>";

            var userVerification = new UserVerification
            {
                Hash = hash,
                ExpiresOn = DateTime.Now.AddMinutes(20),
                User = userModel
            };

            _context.UserVerifications.Add(userVerification);
            _context.SaveChanges();

            _emailService.Send(
                userModel.Email,
                MailTitle,
                mailBody
            );
        }

        public void VerifyUser(string token, out SignInViewState state)
        {
            var verificationModel = _context.UserVerifications
                .Include(v => v.User)
                .FirstOrDefault(v => v.Hash == token);

            if (verificationModel == null)
            {
                state = SignInViewState.InvalidToken;
                return;
            }
            
            if (verificationModel.ExpiresOn < DateTime.Now)
            {
                state = SignInViewState.ExpiredToken;
                return;
            }

            verificationModel.User.IsActivated = true;
            _context.UserVerifications.Remove(verificationModel);
            _context.SaveChanges();

            state = SignInViewState.ActivationSuccess;
            return;
        }
    }
}

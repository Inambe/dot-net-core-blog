using System;
using InambeBlog.Helpers;
using System.Security.Claims;
using System.Threading.Tasks;
using InambeBlog.Models.User;
using InambeBlog.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using InambeBlog.Models;
using InambeBlog.Models.User.ViewModels;

namespace InambeBlog.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepo _userRepo;

        public AccountController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public IActionResult SignIn(SignInViewState? state)
        {
            var viewModel = new SignInUserVM
            {
                State = state == null ? null : state
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInUserVM signInUserModel)
        {
            if (!ModelState.IsValid)
            {
                return View(signInUserModel);
            }

            if (!_userRepo.ValidateCredentials(signInUserModel.Email, signInUserModel.Password, out UserModel userModel))
                return RedirectToAction(nameof(SignIn), new { state = SignInViewState.Failed });

            if (!userModel.IsActivated)
                return RedirectToAction(nameof(SignIn), new { state =  SignInViewState.NotActive});

            var claims = new List<Claim>
            {
                new Claim(nameof(UserModel.Id), Convert.ToString(userModel.Id)),
                new Claim(ClaimTypes.Name, userModel.Name),
                new Claim(ClaimTypes.NameIdentifier, userModel.Email)
            };

            var userIdentity = new ClaimsIdentity(claims, Constants.ReaderAuth);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            var authProps = new AuthenticationProperties();
            if (signInUserModel.RememberMe)
            {
                // it should expire after 14 days
                authProps.IsPersistent = true;
            }
            else
            {
                authProps.IsPersistent = false;
            }

            // default authentication scheme shall be used here to signin
            await HttpContext.SignInAsync(userPrincipal, authProps);

            return RedirectToAction(
                nameof(HomeController.Index),
                nameof(HomeController).ControllerName()
            );
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(SignUpUserVM signUpUserModel)
        {
            if (!ModelState.IsValid)
            {
                return View(signUpUserModel);
            }

            var passwordSalt = Hash.CreateSalt();
            var passwordHash = Hash.Create(
                signUpUserModel.Password,
                passwordSalt
            );

            var userModel = new UserModel()
            {
                Name = signUpUserModel.Name,
                Email = signUpUserModel.Email,
                PasswordSalt = passwordSalt,
                PasswordHash = passwordHash,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            _userRepo.Create(userModel);
            _userRepo.CreateUserVerification(userModel);

            return RedirectToAction(nameof(SignIn), new {
                state = SignInViewState.AccountCreated
            });
        }

        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(
                nameof(PostController.Index),
                nameof(PostController).ControllerName()
            );
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Activate(string token)
        {
            _userRepo.VerifyUser(token, out SignInViewState state);
            return RedirectToAction(nameof(SignIn), new {
                state = state
            });
        }
    }
}

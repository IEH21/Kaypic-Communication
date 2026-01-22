// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using web3_kaypic.Models;
using Web3_kaypic.service;

namespace ExempleAuthentification.Areas.Identity.Pages.Account
{
    public class LoginWith2faModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginWith2faModel> _logger;
        private readonly ISMSSenderService _smsSenderService;

        public LoginWith2faModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<LoginWith2faModel> logger,
            ISMSSenderService smsSenderService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _smsSenderService = smsSenderService;

        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Authenticator code")]
            public string TwoFactorCode { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember this machine")]
            public bool RememberMachine { get; set; }
            public string TwoFactAuthProviderName { get; set; }
        }

        //public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl = null)
        //{
        //    // Ensure the user has gone through the username & password screen first
        //    var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

        //    if (user == null)
        //    {
        //        throw new InvalidOperationException($"Unable to load two-factor authentication user.");
        //    }

        //    ReturnUrl = returnUrl;
        //    RememberMe = rememberMe;

        //    return Page();
        //}
        //Ce méthode est utilisé pour envoyer des jetons à deux facteurs
        //à l'utilisateur en fonction
        //du fournisseur d'authentification disponible (téléphone ou e-mail)
        //et de la demande de l'utilisateur.
        public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl =
        null)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Impossible de charger l'utilisateur d'authentification à deux facteurs.");
            }
            var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);
            Input = new InputModel(); // Crée une instance de la classe InputModel
                                      // Vérifie s'il y a un fournisseur d'authentification à deux facteurs de
            if (providers.Any(_ => _ == "Phone"))
            {
                // Configure le fournisseur actuel comme "Phone"
                Input.TwoFactAuthProviderName = "Phone";
                // Génère un jeton à deux facteurs pour le téléphone
                var token = await _userManager.GenerateTwoFactorTokenAsync(user,
               "Phone");
                // Envoie le jeton par SMS à l'utilisateur
                await _smsSenderService.SendSmsAsync(user.PhoneNumber, $"OTP Code:{token}");
            }
            // Sinon, vérifie s'il y a un fournisseur d'authentification à deux
            else if (providers.Any(_ => _ == "Email"))
            {
                // Configure le fournisseur actuel comme "Email"
                Input.TwoFactAuthProviderName = "Email";
                // Génère un jeton à deux facteurs pour l'e-mail
                var token = await _userManager.GenerateTwoFactorTokenAsync(user,
               "Email");
                // Envoie le jeton par e-mail à l'utilisateur
                // await _emailSender.SendEmailAsync(user.Email, "2FA Code",$"<h3>{token}</h3>.");
            }
            else
            {
                throw new InvalidOperationException($"Impossible de charger l'utilisateur d'authentification à deux facteurs.");
            }
            // Configure les valeurs de retour pour la page
            ReturnUrl = returnUrl;
            RememberMe = rememberMe;
            // Renvoie la page actuelle
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            var authenticatorCode = Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            //var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, Input.RememberMachine);
            var result = await _signInManager.TwoFactorSignInAsync(Input.TwoFactAuthProviderName,
            authenticatorCode, rememberMe, Input.RememberMachine);

            var userId = await _userManager.GetUserIdAsync(user);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
                return LocalRedirect(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
                return RedirectToPage("./Lockout");
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return Page();
            }
        }
    }
}

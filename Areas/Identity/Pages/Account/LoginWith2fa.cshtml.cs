using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using web3_kaypic.Models;
using Microsoft.AspNetCore.Identity;
using Web3_kaypic.service;

namespace Web3_kaypic.Areas.Identity.Pages.Account
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

        [BindProperty]
        public InputModel Input { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }


        public class InputModel
        {
            [Required]
            [StringLength(6, ErrorMessage = "Le code doit contenir 6 chiffres.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Code d'authentification")]
            public string TwoFactorCode { get; set; }

            [Display(Name = "Se souvenir de cet appareil")]
            public bool RememberMachine { get; set; }
            public string TwoFactAuthProviderName { get; set; }
        }


        // -------------------
        //   GET = Envoi SMS
        // -------------------
        /*
         public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl = null)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            { 
            throw new InvalidOperationException("Impossible de charger l'utilisateur d'authentification à deux facteurs.");
            }
            // ReturnUrl = returnUrl;
            //RememberMe = rememberMe;

            // ✔ Générer un OTP MFA
            // var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Phone");

            // ✔ Envoyer code SMS via Twilio
            //await _smsSender.SendSmsAsync(user.PhoneNumber, $"Votre code MFA est : {token}");

            // Récupère la liste des fournisseurs d'authentification à deux facteurs valides pour cet utilisateur

            var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);
            Input = new InputModel(); // Crée une instance de la classe InputModel

            // Vérifie s'il y a un fournisseur d'authentification à deux facteurs de 
            //type "Phone" (téléphone)

            if (providers.Any(_ => _ == "Phone"))
            {
                // Configure le fournisseur actuel comme "Phone"
                Input.TwoFactAuthProviderName = "Phone";
                // Génère un jeton à deux facteurs pour le téléphone
                var token = await _userManager.GenerateTwoFactorTokenAsync(user,
               "Phone");

                // Envoie le jeton par SMS à l'utilisateur
                await _smsSenderService.SendSmsAsync(user.PhoneNumber, $"OTP Code: {token} ");
            }
            // Sinon, vérifie s'il y a un fournisseur d'authentification à deux 2f

            else if (providers.Any(_ => _ == "Email"))
            {
                // Configure le fournisseur actuel comme "Email"
                Input.TwoFactAuthProviderName = "Email";
                // Génère un jeton à deux facteurs pour l'e-mail
                var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            }
            // Envoie le jeton par e-mail à l'utilisateur
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
        */

        public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl = null)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                _logger.LogWarning("Tentative d'accès à LoginWith2fa sans utilisateur en cours.");
                return RedirectToPage("./Login"); // redirige vers login au lieu de throw
            }

            ReturnUrl = returnUrl;
            RememberMe = rememberMe;
            Input = new InputModel();

            // Récupère les providers disponibles
            var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);

            if (providers.Contains("Phone"))
            {
                Input.TwoFactAuthProviderName = "Phone";
                var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Phone");

                // Envoi SMS via Twilio
                await _smsSenderService.SendSmsAsync(user.PhoneNumber, $"Votre code MFA est : {token}");
            }
            else if (providers.Contains("Email"))
            {
                Input.TwoFactAuthProviderName = "Email";
                var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

                // Ici tu pourrais envoyer le token par email si nécessaire
                // await _emailSender.SendEmailAsync(user.Email, "Code MFA", $"Votre code est : {token}");
            }
            else
            {
                _logger.LogError("Aucun provider 2FA valide trouvé pour l'utilisateur {UserId}", user.Id);
                return RedirectToPage("./Login");
            }

            return Page();
        }


        // -------------------
        //   POST = Validation
        // -------------------
        public async Task<IActionResult> OnPostAsync(bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
                return Page();

            returnUrl ??= Url.Content("~/");

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            { 

            throw new InvalidOperationException("Impossible de charger l'utilisateur d'authentification à deux facteurs..");
            }

            // Nettoyage du code
            //var code = Input.TwoFactorCode.Replace(" ", "").Replace("-", "");

            // Récupère le code d'authentification à deux facteurs de l'entrée (Input) en supprimant  les espaces et les tirets

            var authenticatorCode = Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);


            //connexion 2fa
            /*var result = await _signInManager.TwoFactorSignInAsync(
                "Phone",
                code,
                rememberMe,
                Input.RememberMachine
            );
            */

            // Tente de valider le code d'authentification à deux facteurs en utilisant le gestionnaire  d'authentification
            var result = await _signInManager.TwoFactorSignInAsync(Input.TwoFactAuthProviderName, authenticatorCode, rememberMe, Input.RememberMachine);

            // Récupère l'ID de l'utilisateur
            var userId = await _userManager.GetUserIdAsync(user);


            if (result.Succeeded)
            {
                // L'authentification à deux facteurs a réussi, enregistre un message de journalisation
                _logger.LogInformation("Connexion 2FA réussie pour l'utilisateur '{UserId}'.", user.Id);
                // Redirige l'utilisateur vers l'URL de retour spécifiée
                return LocalRedirect(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                // Le compte de l'utilisateur est verrouillé, enregistre un message de journalisation
                _logger.LogWarning("Compte verrouillé pour l'utilisateur '{UserId}'.", user.Id);

                // Redirige l'utilisateur vers une page de verrouillage de compte
                return RedirectToPage("./Lockout");
            }
            else
            {
                // Le code d'authentification à deux facteurs est incorrect, enregistre un message de journalisation
                _logger.LogWarning("Code d'authentification incorrect pour l'utilisateur avec l'ID '{UserId}'.", user.Id);

            }

            // Ajoute une erreur de modèle pour afficher un message d'erreur à l'utilisateur
            ModelState.AddModelError(string.Empty, "Code invalide.");
           
            // Recharge la page actuelle
            return Page();
        }
    }
}

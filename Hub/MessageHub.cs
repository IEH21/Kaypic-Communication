using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using web3_kaypic.Models;
using Web3_kaypic.Data;

namespace Web3_kaypic.Hub
{
    public class MessageHub : Hub<IMessageClient>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessageHub(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // --- Messages directs ---
        public async Task SendDirectMessage(string receiverUserId, string content)
        {
            var sender = Context.User.Identity?.Name ?? "Visiteur";

            var msg = new TMessagingChatPersonaMessage
            {
                mcpm_message = content,
                created_at = DateTime.UtcNow,
                is_deleted = false
            };

            _context.TMessagingChatPersonaMessage.Add(msg);
            await _context.SaveChangesAsync();

            // Diffusion uniquement au destinataire
            await Clients.User(receiverUserId).ReceiveDirectMessage(sender, content);
        }

        // --- Messages personnels (self notes) ---
        public async Task SendSelfMessage(string content)
        {
            var sender = Context.User.Identity?.Name ?? "Moi";

            var msg = new TMessagingChatPersonaMessage
            {
                mcpm_message = content,
                created_at = DateTime.UtcNow,
                is_deleted = false
            };

            _context.TMessagingChatPersonaMessage.Add(msg);
            await _context.SaveChangesAsync();

            await Clients.Caller.ReceiveSelfMessage(sender, content);
        }

        // --- Messages broadcast (tous les utilisateurs) ---
        public async Task SendBroadcastMessage(string content)
        {
            var sender = Context.User.Identity?.Name ?? "Visiteur";

            var msg = new TMessagingChatPersonaMessage
            {
                mcpm_message = content,
                created_at = DateTime.UtcNow,
                is_deleted = false
            };

            _context.TMessagingChatPersonaMessage.Add(msg);
            await _context.SaveChangesAsync();

            await Clients.All.ReceiveBroadcastMessage(sender, content);
        }

        // --- Messages de groupe ---
        public async Task SendGroupMessage(string groupName, string content)
        {
            var sender = Context.User.Identity?.Name ?? "Visiteur";

            var msg = new TMessagingChatPersonaMessage
            {
                mcpm_message = content,
                created_at = DateTime.UtcNow,
                is_deleted = false
            };

            _context.TMessagingChatPersonaMessage.Add(msg);
            await _context.SaveChangesAsync();

            await Clients.Group(groupName).ReceiveGroupMessage(groupName, sender, content);
        }

        public async Task SendGroupAnnouncement(string groupName, string announcement)
        {
            var msg = new TMessagingChatPersonaMessage
            {
                mcpm_message = $"Annonce ({groupName}): {announcement}",
                created_at = DateTime.UtcNow,
                is_deleted = false
            };

            _context.TMessagingChatPersonaMessage.Add(msg);
            await _context.SaveChangesAsync();

            await Clients.Group(groupName).ReceiveGroupAnnouncement(groupName, announcement);
        }

        // --- Gestion des groupes ---
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.Notify($"Vous avez rejoint le groupe {groupName}");
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.Notify($"Vous avez quitté le groupe {groupName}");
        }

        // --- Réactions ---
        public async Task AddReaction(int messageId, string reaction)
        {
            var user = Context.User.Identity?.Name ?? "Visiteur";

            // Exemple de persistance si tu ajoutes une table TMessagingReaction
            // var reactionEntity = new TMessagingReaction { ... }
            // _context.MessagingReactions.Add(reactionEntity);
            // await _context.SaveChangesAsync();

            await Clients.All.ReceiveReaction(messageId, user, reaction);
        }

        // --- Connexion ---
        public override async Task OnConnectedAsync()
        {
            var userName = Context.User.Identity?.Name ?? "Visiteur";

            await Clients.Caller.Notify($"Bienvenue {userName}, vous êtes connecté au Hub!");
            await Clients.Others.Notify($"{userName} vient de rejoindre le chat.");

            await base.OnConnectedAsync();
        }

        // --- Médias ---
        public async Task SendMedia(string receiverUserId, string mediaUrl, string category)
        {
            var sender = Context.User.Identity?.Name ?? "Visiteur";

            var media = new TMessagingMedia
            {
                mc_id = 0, // à remplacer par l'ID de la conversation
                ts_id = 0, // idem
                mcm_media_category = category, // "i" image, "v" vidéo, "f" fichier
                mcm_url = mediaUrl,
                created_by_mp_id = 0, // à remplacer par l'ID du persona
                created_at = DateTime.UtcNow
            };

            _context.TMessagingMedia.Add(media);
            await _context.SaveChangesAsync();

            await Clients.User(receiverUserId).ReceiveMedia(sender, mediaUrl, category);
        }
        //Circle
        // Publier un post
        public async Task SendCirclePost(string userName, string content, string? imageUrl)
        {
            // Récupérer la photo de profil de l'utilisateur
            var user = await _userManager.FindByEmailAsync(userName);
            var profileImageUrl = user?.ProfileImageUrl;
            var message = new TMessage
            {
                msg_username = userName,
                msg_content = content,
                msg_image_url = imageUrl,
                msg_user_profile_image_url = profileImageUrl,
                msg_created_at = DateTime.Now
            };

            _context.TMessages.Add(message);
            await _context.SaveChangesAsync();

            await Clients.All.ReceiveCirclePost(
                message.msg_username,
                message.msg_content,
                message.msg_created_at.ToString("o"),
                message.msg_id,
                message.msg_image_url,
                profileImageUrl);
        }

        // Liker un post
        public async Task LikeCirclePost(int messageId)
        {
            var message = await _context.TMessages.FindAsync(messageId);
            if (message != null)
            {
                message.msg_likes_count++;
                await _context.SaveChangesAsync();

                await Clients.All.ReceiveLike(messageId, message.msg_likes_count);
            }
        }

        // Commenter un post
        public async Task CommentCirclePost(int messageId, string userName, string content)
        {
            var user = await _userManager.FindByEmailAsync(userName);
            var profileImageUrl = user?.ProfileImageUrl;

            var comment = new TMessageComment
            {
                msg_id = messageId,
                comment_username = userName,
                comment_content = content,
                comment_user_profile_image_url = profileImageUrl,
                comment_created_at = DateTime.Now
            };

            _context.TMessageComments.Add(comment);
            await _context.SaveChangesAsync();

            await Clients.All.ReceiveComment(
                messageId,
                comment.comment_id,
                userName,
                content,
                comment.comment_created_at.ToString("o"),
                profileImageUrl);
        }


        // Supprimer un post
        public async Task DeleteCirclePost(int messageId, string userName)
        {
            var message = await _context.TMessages
                .Include(m => m.Comments)
                .FirstOrDefaultAsync(m => m.msg_id == messageId);

            if (message == null || message.msg_username != userName)
                return;

            _context.TMessages.Remove(message);
            await _context.SaveChangesAsync();

            await Clients.All.ReceiveDeletePost(messageId);
        }

        // Supprimer un commentaire
        public async Task DeleteComment(int commentId, string userName)
        {
            var comment = await _context.TMessageComments.FindAsync(commentId);

            if (comment == null || comment.comment_username != userName)
                return;

            var messageId = comment.msg_id;

            _context.TMessageComments.Remove(comment);
            await _context.SaveChangesAsync();

            await Clients.All.ReceiveDeleteComment(messageId, commentId);
        }

    }
}

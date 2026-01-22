namespace Web3_kaypic.Hub
{
    public interface IMessageClient
    {
        // --- Messages directs (envoyés/reçus entre deux utilisateurs) ---
        Task ReceiveDirectMessage(string sender, string message);

        // --- Messages personnels (envoyés/reçus par soi-même) ---
        Task ReceiveSelfMessage(string sender, string message);

        // --- Messages généraux (broadcast à tous) ---
        Task ReceiveBroadcastMessage(string sender, string message);

        // --- Messages de groupe ---
        Task ReceiveGroupMessage(string groupName, string sender, string message);
        Task ReceiveGroupAnnouncement(string groupName, string announcement);
        Task ReceiveGroupUsers(string groupName, List<string> users);

        // --- Notifications système ---
        Task Notify(string notification);

        // --- Réactions aux messages ---
        Task ReceiveReaction(int messageId, string user, string reaction);

        //fichier
        Task ReceiveMedia(string sender, string mediaUrl, string category);


        //circle
        Task ReceiveCirclePost(string userName, string content, string createdAt, int messageId, string? imageUrl, string? profileImageUrl); Task ReceiveLike(int messageId, int newLikesCount);
        Task ReceiveComment(int messageId, int commentId, string userName, string content, string createdAt, string? profileImageUrl);
        Task ReceiveDeletePost(int messageId);
        Task ReceiveDeleteComment(int messageId, int commentId);
    }
}

/*// Crée une connexion SignalR en spécifiant l'URL du hub
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/learningHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

const notificationPanel = $('#notifications');

// Fonction pour afficher des notifications
function showNotification(message) {
    notificationPanel.text(message).fadeIn().delay(3000).fadeOut();
}

// Fonction appelée lorsqu'un message est reçu du hub
connection.on("ReceiveMessage", (message) => {
    $('#signalr-message-panel').prepend($('<div />').text(message));
});

// Fonction appelée lorsqu'une notification est reçue du hub
connection.on("Notify", (notification) => {
    showNotification(notification);
});

// Fonction pour recevoir et afficher les utilisateurs d'un groupe spécifique
connection.on("ReceiveGroupUsers", (groupName, users) => {
    const userList = users.length > 0 ? users.join(", ") : "Aucun utilisateur dans ce groupe.";
    $('#group-users-panel').text(`Utilisateurs dans le groupe ${groupName} : ${userList}`);
});

// Gestionnaire d'événements pour le bouton "Diffuser"
$('#btn-broadcast').click(function () {
    const message = $('#broadcast').val();
    connection.invoke("BroadcastMessage", message).catch(err => console.error(err.toString()));
});

// Gestionnaire d'événements pour le bouton "Envoyer aux autres"
$('#btn-others-message').click(function () {
    const message = $('#others-message').val();
    connection.invoke("SendToOthers", message).catch(err => console.error(err.toString()));
});

// Gestionnaire d'événements pour le bouton "Envoyer à soi-même"
$('#btn-self-message').click(function () {
    const message = $('#self-message').val();
    connection.invoke("SendToCaller", message).catch(err => console.error(err.toString()));
});

// Gestionnaire d'événements pour le bouton "Envoyer à un utilisateur spécifique"
$('#btn-individual-message').click(function () {
    const message = $('#individual-message').val();
    const connectionId = $('#connection-for-message').val();
    connection.invoke("SendToIndividual", connectionId, message).catch(err => console.error(err.toString()));
});

// Gestionnaire d'événements pour le bouton "Créer un groupe"
$('#btn-group-create').click(function () {
    const group = $('#group-to-create').val();
    connection.invoke("CreateGroup", group).catch(err => console.error(err.toString()));
});

// Gestionnaire d'événements pour le bouton "Envoyer au groupe"
$('#btn-group-message').click(function () {
    const message = $('#group-message').val();
    const group = $('#group-for-message').val();
    connection.invoke("SendToGroup", group, message).catch(err => console.error(err.toString()));
});

// Gestionnaire d'événements pour le bouton "Rejoindre le groupe"
$('#btn-group-add').click(function () {
    const group = $('#group-to-add').val();
    connection.invoke("AddUserToGroup", group).catch(err => console.error(err.toString()));
});

// Gestionnaire d'événements pour le bouton "Quitter le groupe"
$('#btn-group-remove').click(function () {
    const group = $('#group-to-remove').val();
    connection.invoke("RemoveUserFromGroup", group).catch(err => console.error(err.toString()));
});

// Gestionnaire d'événements pour obtenir la liste des utilisateurs d'un groupe
$('#btn-get-group-users').click(function () {
    const group = $('#group-for-message').val();
    connection.invoke("GetGroupUsers", group).catch(err => console.error(err.toString()));
});

// Fonction asynchrone pour démarrer la connexion SignalR
async function start() {
    try {
        await connection.start();
        console.log('connected');
        showNotification("Connexion établie avec succès !");
    } catch (err) {
        console.error(err.toString());
        showNotification("Échec de la connexion. Nouvelle tentative...");
        setTimeout(() => start(), 5000);
    }
}

// Redémarre la connexion si elle est fermée
connection.onclose(async () => {
    await start();
});

start();*/

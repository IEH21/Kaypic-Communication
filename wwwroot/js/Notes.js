// ============================
// Connexion au hub SignalR
// ============================
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/messageHub")
    .withAutomaticReconnect()
    .build();

// ⚠️ Ici tu mets ton propre identifiant utilisateur
// Par exemple : ton email ou ton ID tel que défini dans Context.UserIdentifier côté serveur
const selfId = "monIdentifiant";

connection.start()
    .then(() => appendMessage("msg-received", "✅ Connecté au hub SignalR"))
    .catch(err => console.error("❌ Erreur de connexion SignalR:", err));

// ============================
// Réception des messages privés
// ============================
connection.on("ReceivePrivateMessage", (sender, message) => {
    appendMessage("msg-received", `${sender}: ${message}`);
});

connection.on("Notify", (info) => {
    appendMessage("msg-received", `ℹ️ ${info}`);
});

// ============================
// Fonction utilitaire
// ============================
function appendMessage(cssClass, text) {
    const container = document.getElementById("messagesContainer");
    if (!container) return;
    const div = document.createElement("div");
    div.classList.add("msg", cssClass);
    div.textContent = text;
    container.appendChild(div);
    container.scrollTop = container.scrollHeight;
}

// ============================
// Envoi de messages à soi-même
// ============================
const sendBtn = document.getElementById("sendBtn");
if (sendBtn) {
    sendBtn.addEventListener("click", async () => {
        const input = document.getElementById("messageInput");
        const message = input.value.trim();
        if (message) {
            // Affiche le message côté expéditeur
            appendMessage("msg-sent", message);

            // Envoie au Hub (à soi-même)
            try {
                await connection.invoke("SendPrivateMessage", selfId, message);
            } catch (err) {
                console.error("Erreur envoi:", err);
            }

            // Vide le champ
            input.value = "";
        }
    });
}

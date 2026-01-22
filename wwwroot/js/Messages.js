// ============================
// Connexion au hub SignalR
// ============================
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/messageHub")
    .withAutomaticReconnect()
    .build();

connection.start()
    .then(() => console.log("✅ Connecté au hub SignalR"))
    .catch(err => console.error("❌ Erreur de connexion SignalR:", err));

// ============================
// Réception des messages
// ============================
connection.on("ReceiveBroadcastMessage", (sender, message) => {
    appendMessage("msg-received", `${sender}: ${message}`);
});

connection.on("ReceiveDirectMessage", (sender, message) => {
    appendMessage("msg-received", `💌 ${sender}: ${message}`);
});

connection.on("ReceiveReaction", (messageId, user, reaction) => {
    appendMessage("msg-received", `🔖 ${user} a réagi avec ${reaction} au message #${messageId}`);
});

connection.on("ReceiveMedia", (sender, mediaUrl, category) => {
    appendMessage("msg-received", `${sender} a envoyé un fichier: 
        <a href="${mediaUrl}" target="_blank">${category.toUpperCase()}</a>`);
});

connection.on("Notify", (info) => {
    appendMessage("msg-received", `ℹ️ ${info}`);
});

// ============================
// Fonctions utilitaires
// ============================
function appendMessage(cssClass, text) {
    const container = document.querySelector(".messages-area");
    if (!container) return;
    const div = document.createElement("div");
    div.classList.add("msg", cssClass);
    div.innerHTML = text; // innerHTML pour supporter les liens
    container.appendChild(div);
    container.scrollTop = container.scrollHeight;
}

// ============================
// Envoi de messages broadcast
// ============================
const sendBtn = document.querySelector(".send-btn");
if (sendBtn) {
    sendBtn.addEventListener("click", async () => {
        const input = document.getElementById("messageInput");
        const message = input.value.trim();
        if (message) {
            appendMessage("msg-sent", message);
            await connection.invoke("SendBroadcastMessage", message);
            input.value = "";
        }
    });
}

// ============================
// Envoi d’une réaction
// ============================
const reactionBtn = document.getElementById("reactionButton");
if (reactionBtn) {
    reactionBtn.addEventListener("click", async () => {
        const messageId = 123; // ⚠️ ID du message ciblé
        const reaction = "👍"; // exemple
        await connection.invoke("AddReaction", messageId, reaction);
    });
}



document.getElementById("fileInput").addEventListener("change", function () {
    if (this.files.length > 0) {
        const formData = new FormData();
        formData.append("file", this.files[0]);
        formData.append("mc_id", 123); // id du chat courant
        formData.append("ts_id", 456); // id de la saison
        formData.append("created_by_mp_id", 789); // id du persona

        fetch("/Message/Upload", {
            method: "POST",
            body: formData
        })
            .then(res => res.json())
            .then(data => {
                console.log("Fichier envoyé :", data.url);
            })
            .catch(err => console.error("Erreur :", err));
    }
});

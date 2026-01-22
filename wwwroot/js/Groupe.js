const connection = new signalR.HubConnectionBuilder()
    .withUrl("/messageHub")
    .withAutomaticReconnect()
    .build();

const activeGroup = "Kaypic Team"; // ⚠️ à rendre dynamique si besoin

connection.start()
    .then(async () => {
        appendGroupMessage("System", "✅ Connecté au hub SignalR", "msg-received");
        await connection.invoke("JoinChat", activeGroup);
    })
    .catch(err => console.error("❌ Erreur de connexion SignalR:", err));

// Réception des messages de groupe
connection.on("ReceiveGroupMessage", (groupName, sender, message) => {
    if (groupName === activeGroup) {
        appendGroupMessage(sender, message, "msg-received");
    }
});

connection.on("Notify", (info) => {
    appendGroupMessage("System", info, "msg-received");
});

// Fonction utilitaire
function appendGroupMessage(sender, text, cssClass) {
    const container = document.querySelector(".messages-area");
    if (!container) return;
    const block = document.createElement("div");
    block.classList.add("msg-block");
    const senderDiv = document.createElement("div");
    senderDiv.classList.add("sender-name");
    if (cssClass === "msg-sent") senderDiv.classList.add("self");
    senderDiv.textContent = sender;
    const msgDiv = document.createElement("div");
    msgDiv.classList.add("msg", cssClass);
    msgDiv.textContent = text;
    block.appendChild(senderDiv);
    block.appendChild(msgDiv);
    container.appendChild(block);
    container.scrollTop = container.scrollHeight;
}

// Envoi de messages au groupe
const sendBtn = document.querySelector(".send-btn");
if (sendBtn) {
    sendBtn.addEventListener("click", async () => {
        const input = document.querySelector(".input-field input");
        const message = input.value.trim();
        if (message) {
            appendGroupMessage("Moi", message, "msg-sent");
            await connection.invoke("SendGroupMessage", activeGroup, message);
            input.value = "";
        }
    });
}

const signalR = require("@microsoft/signalr");

async function main() {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5197/hubs/orders", {
            accessTokenFactory: () => "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjJkZDgwN2M5LTQ1MDctNDNhNy05YzE0LTQ0YmRlYjM3ZGMxMiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJuYXVmYWwiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVU0VSIiwiZXhwIjoxNzY5MzQ5MTc5LCJpc3MiOiJOaGpJc3N1ZXIiLCJhdWQiOiJOaGpBdWRpZW5jZSJ9.0BmYUSraqrrj_gCaLC9Cl_rMoFlhS-RK3J5lYe523Q4"
        })
        .withAutomaticReconnect()
        .build();

    connection.on("OrderStatusUpdated", (orderId, status) => {
        console.log("🔔 Order updated:", orderId, status);
    });

    try {
        await connection.start();
        console.log("✅ SignalR connected");
    } catch (err) {
        console.error("❌ SignalR connection error:", err);
    }
}

main();

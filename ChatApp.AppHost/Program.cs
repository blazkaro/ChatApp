var builder = DistributedApplication.CreateBuilder(args);

var conversations = builder.AddProject<Projects.ChatApp_Conversations>("chatapp-conversations");

var rtc = builder.AddProject<Projects.ChatApp_RealTimeCommunication>("chatapp-realtimecommunication")
    .WithReference(conversations)
    .WaitFor(conversations);

var messages = builder.AddProject<Projects.ChatApp_Messages>("chatapp-messages")
    .WithReference(conversations)
    .WaitFor(conversations);

var spaServer = builder.AddProject<Projects.ChatApp_Spa_Server>("chatapp-spa-server")
    .WithReference(rtc)
    .WithReference(conversations)
    .WithReference(messages)
    .WaitFor(rtc)
    .WaitFor(conversations)
    .WaitFor(messages);

builder.AddNpmApp("chatapp-spa-client", "../Spa/src/chatapp.spa.client")
    .WithReference(spaServer)
    .WaitFor(spaServer)
    .WithHttpsEndpoint(env: "DEV_ANGULAR_SPA_PORT");

builder.Build().Run();

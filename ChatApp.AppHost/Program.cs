var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ChatApp_RealTimeCommunication>("chatapp-realtimecommunication");

builder.AddProject<Projects.ChatApp_Conversations>("chatapp-conversations");

builder.Build().Run();

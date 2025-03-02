var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ChatApp_RealTimeCommunication>("chatapp-realtimecommunication");

builder.Build().Run();

# ChatApp

<i>This project doesn't follow best practices and makes a lot of compromises. This application was done to remind myself how things works (backend, frontend, frameworks, integrations) after a little break, caused by my involvement in Competetive Programming.</i>

<b>Overall concept</b>
Chat application that allows to create conversations and invite others to them (and of course, it allows messaging). Uses Auth0 (OIDC) for user authentication and authorization, both authorization_code and client_credentials grant (in case of machine-to-machine flow).

The messages and groups are handled by SignalR Redis backplane, to which (Redis) subscribe Messages service (saves sent messages) and RealTimeCommunication service (handles conversation creation and invitation acceptations in real time). Conversations service publishes events to Redis.

Services also communicate through HTTP, to check user's access to specific conversation, whether they are authorized to send messages in specified in request conversation etc. It caused that this project uses Polly to provide resiliency between services communication.

There is BFF pattern used, which allows to provide more secure authentication. The backend for frontend also acts as proxy to microservices, and achieves it via using YARP (Yet Another Reversed Proxy) .

The database which is used by both Conversations and Messages services is YugabyteDB. Conversations service uses EntityFramework, while Messages service uses Dapper.

<b>Solution architecture</b>
At the beginning, I thought it would be great to follow microservices architecture for this project. But due to really coupled nature of services, at the end of the day I wouldn't call it microservices. Rather just separeted APIs to provide smoother scalling.

The code architecture also isn't perfect, but probably enough taking into account size of every service. The Clean Architecture or Vertical Slices would be overkill, as every service is kind of 1-3 features at the moment.

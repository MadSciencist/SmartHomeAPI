Here are placed all the contracts for different device types

### Conventions:
* Important assembly metadata: 
  * Comment - this will be put as Control Strategy display name
  * Product - this will be name-friendly unique contract assembly identifier
* Name of assembly starts with `SmartHome.Contracts`
* All the commands are put in `SmartHome.Contracts.{name}.Commands` and implement **IControlCommand** interface
* Commands (classes) should be annotated with **[DisplayText("")]** attribute - this value will be presented on UI
* Assembly should contain exactly one class which implements **INodeDataMapper** preferably through NodeDataMapperBase, the name of the class is NOT important
* Assembly should contain exactly one class which implements **IMessageHandler** preferably through MessageHandlerBase, the name of the class is NOT important
Here are placed all the contracts for different device types

### Conventions:
* Important assembly metadata: 
  * Comment - this will be put as Control Strategy display name
  * Product - this will be name-friendly unique contract assembly identifier
* Name of assembly starts with 'SmartHome.Contracts'
* All the commands are put in Commands folder and implement IControlStrategy interface
* Commands (classes) should be annotated with [DisplayName("")] attribute - this value will be presented on UI
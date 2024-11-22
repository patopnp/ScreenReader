# Implementation details


## Design patterns

### Mediator pattern. 
CommandsMediator acts as a mediator between commands and the Controller, keeping commands from referring to VoiceInputHandler. 
Colleagues invokes Mediator methods to communicate with controller, as it is generally considered not a good practice to let the colleague interact directly with the controller. 

### Facade
VoiceUI hides the implementation details from its clients simplyifing access. 
Text to speech, voice recognition and syncing is performed within VoiceUI through modules communication (VoiceInputHandler and Speaker) oblivious to the client. 

### Simple Factory Pattern
While not strictly a Factory Method pattern, since there are no subclasses of Creator and consequently the class itself is responsible for object instantiation, object creation is still delegated to a Factory albeit of a single type. 
Widget factory is responsible for Widget creation of the appropriate subtype and it may be thought as an utility class. 

### Others
While command objects are the one responsible for operations executions it's not a command pattern implementation because the invoker (Form1.cs) and the commands (e.g ClickButtonCommand) are tightly coupled. Click button command invocation from Form1.cs always corresponds to ClickButtonCommand execution and, moreover, requests invocation parameters are supplied by commands themselves.
Which violates GOF Design pattern command pattern consequences: "Command decouples the object that invokes the operation from the one that knows how to perform it." 

## Data structures

Widgets implement a tree structures with depth first tree traversal on populateChildrenNodes and again a priority driven depth first traversal for sorting widgets.

Maps (or dictionary) are used for mapping the voice commands to actions.

## Thread sync

Voice recognition runs in a separate thread and syncs with the main thread which runs business logic and text to speech via AutoResetEvent.

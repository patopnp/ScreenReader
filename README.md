# Screen reader

This is a screen reader that reads a Java swing application and allows the user to interact through voice commands.

## Implementation details

### Software architecture:

#### Mediator pattern. 
CommandsMediator acts as a mediator between commands and the Controller, keeping commands from referring to VoiceInputHandler. 
Colleagues invokes Mediator methods to communicate with controller, as it is generally considered not a good practice to let the colleague interact directly with the controller. 

#### Facade
VoiceUI hides the implementation details from its clients simplyifing access. 
Text to speech, voice recognition and syncing is performed within VoiceUI through modules communication (VoiceInputHandler and Speaker) oblivious to the client. 

#### Simple Factory Pattern
While not strictly a Factory Method pattern, since there are no subclasses of Creator and consequently the class itself is responsible for object instantiation, object creation is still delegated to a Factory albeit of a single type. 
Widget factory is responsible for Widget creation of the appropriate subtype and it may be thought as an utility class. 

#### Others
While command objects are the one responsible for operations executions, it's not a command pattern implementation because the invoker (CommandMediators) and the commands (e.g ClickButtonCommand) are tightly coupled. Click button command invocation from CommandMediators always corresponds to ClickButtonCommand execution and, moreover, requests invocation parameters are supplied by commands themselves.
Which deviates from GOF Design pattern command pattern consequences: <i>"Command decouples the object that invokes the operation from the one that knows how to perform it." </i>

### Data structures:

Widgets implement a <b>tree</b> structure with depth first tree traversal on populateChildrenNodes and again a priority driven depth first traversal for sorting widgets.

<b>Maps</b> (or dictionary) are used for mapping the voice commands to actions.

### Libraries:

WindowInterOp makes use of WindowsAccessBridgeInterOp DLL from google access bridge explorer, which in turn makes use of Java Access Bridge technology to read into the JVM.

### Thread sync:

Voice recognition runs in a separate thread and syncs with the main thread which runs business logic and text to speech via AutoResetEvent.

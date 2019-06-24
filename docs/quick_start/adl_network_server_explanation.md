# ADL.Network.Server Class Explanations/Information:  
Contains a Standalone Commandline Server and the functionality to use it as a libary

## ServerConsole
The Standalone Console.

### Functions:
* Run(command)
	- runs the specified command.

## NetworkServerConfig

### Settings:
* Id2NameMap
	- Maps the IDS of the programs to readable filenames.
* Port
	- the port where the listener is listening.
* TimeFormatString
	- Specifies how the file should be named based on ID and time of creation.

### Functions:
* Load(path)
	- loads the config from file
* Save(path, config)
	- Saves the config to a file.

## ClientSession

### Functions:
* GetLogPath()
	- returns the filename of the client session.
* Initialize()
	- sets up the logstream with the right mask
	- opens the log file stream
* Authenticate()
	- Starts the authentication handshake
* CloseSession()
	- closes all streams and ends the current session
* GetPackage(out bool disconnect)
	- tries to get the package from the client session
	- returns empty log package if client got disconnected.

## NetworkListener

### Settings:
* RefreshMillis:int
	- The amount of time the threads will sleep when they finished the last update of all clients.
* TimeFormatString:string
	- Wrapper to the Config file. This is where it is defined.
* Port:int
	- Wrapper to the Config file. This is where it is defined.
* DebugNetworking:bool
	- Threadsave Flag to output all received logs to the console with mask 0
* IsStarted:bool
	- Readonly flag that can be accessed without lock.
	- It states the current state of the NetworkListener.

### Functions:
* Start()
	- Starts the listener and server thread.
* Stop()
	- Stops the listener and server thread

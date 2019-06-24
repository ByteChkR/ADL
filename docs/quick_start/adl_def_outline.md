# ADL Definition Outline
## Definition
ADL is a Debugging Framework that is specifically designed for developing Game Engines and other projects with time constraints on processing time.  
It enables you to take as many debug logs as you like without noticable slowdown since the whole logging process is threadsave and can be multithreaded.  
The framework features multiple outputs with mask based filters, so you can specify freely to what logs you want to listen.

## Extensions
There are multiple extensions available to (for example) have a custom console that supports colored debug logs based on the filter.

* ADL.Unity
	- Unity Wrapper for the ADL Framework
	- Although it is working, it is not recommended to use this(just use unitys debug capabilities)
* ADL.CustomCMD
	- Windows forms based Custom Command line output that supports colored logs and features more functionality like changing the mask on the fly to selectively ignore logs.
* ADL.Network
	- ADL.Network.Client
		+ Extension to be able to Connect your debug framework on your local machine to a server that runs somewhere in the network.
		+ It uses TCP connections to pass the logs. 
		+ The Client works the same as a normal log stream except you have to specify IP / Port / Application Specific ID to be able to correctly authenticate with the server.
	- ADL.Network.Server
		+ Extension that can be used as a Standalone Commandline Server or as a library. The server can be used to log any amount of clients concurrently.
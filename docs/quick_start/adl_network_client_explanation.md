# ADL.Network.Client Class Explanations/Information:  
ADL.Network.Client is the Client side libary for the Network Project.

## NetUtils

### Functions:
* CreateNetworkStream(id, version, ip, port, mask, matchtype, settimestamp)
	- ID: a unique program id for every project that is connecting to the same server
	- version: your assembly version(0.0.0.1 for example)
	- IP/Port: Server address
	- mask: the same as a normal LogStream
	- matchtype: the same as normal logstream
	- settimestamp: the same as normal logstream

* CreateNetworkStream(NetworkConfig, id, verison)
	- NetworkConfig: information packed away in an object
	- ID: a unique program id for every project that is connecting to the same server
	- version: your assembly version(0.0.0.1 for example)

* CreateNetworkStream(NetworkConfig, id, version, mask, matchtype, settiestamp)
	- NetworkConfig: informatiom packed away in an object
	- ID: a unique program id for every project that is connecting to the same server
	- version: your assembly version(0.0.0.1 for example)
	- mask: the same as a normal LogStream
	- matchtype: the same as normal logstream
	- settimestamp: the same as normal logstream

## NetLogStream

### Functions:
* Write(Log)
	- additional try catch in case the server goes down or sudden sunwinds. whatever.

## NetworkConfig

### Settings:
* IP:string
* Port:int

### Functions:
* Load(path)
	- Loads the configuration directly from a file
* Save(path, conf)
	- Saves the configuration to a file.
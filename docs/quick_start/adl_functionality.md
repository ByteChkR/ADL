# Functionality
* The logging outputs are custom stream classes that can be created out of any stream. The filters are based on bitwise operations which makes them really fast.  
* You can specify prefixes for masks that will be prepended to the log, so you can always see where exactly this log is coming from.
* Furthermore it features timestamps and thanks to a lot of settings that can be loaded and saved or just set directly it can also be used to send all kinds of data through the framework
* ADL.Network implements a Server and client to send logs over the network in realtime.
* ADL.CustomCMD implements a custom commandline output that supports colored output and is multithreaded.
* ADL.Unity is implementing Unity Components and wrappers to make the Framework usable in Unity without writing special code.
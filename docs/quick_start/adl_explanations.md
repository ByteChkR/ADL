# ADL Class Explanations/Information:  
ADL is the core libary used by all extensions. It implements the actual logging framework.  

## ADL.Debug  
ADL.Debug is the main Class that is used to communicate with the framework.  
### Settings:  
* AdlEnabled:bool is used to toggle if the whole framework should be active or not.  
* SendUpdateMessageOnFirstLog:bool is the flag that determines if 
	the Framework should search for updates in the github pages.  
	Since it is downloading the version info from github 
	it will (depending on your internet speed) block the app until the task completed(or failed)  
* SendWarnings:bool is used to determine if the Framework should send Warnings by itself when you overuse the logging capabilities or when you try to log things when AdlEnabled is not set.  
* TextEncoding:System.Text.Encoding is used by the framework to serialize a string to bytes and to get it back.  
* WarningMask:BitMask sets the mask where the framework should send warnings when warnings are activated.  
* UpdateMask:BitMask sets the mask where the framework should send update messages to when SendUpdateMessageOnFirstLog is set.  
* PrefixLookupMode:PrefixLookupSettings defines how the framework should look up the prefixes. And what optimizations to use.  

### Functions:  

* AddOutputStream(LogStream)
	- Adds a Log Stream to the Framework if not already added.
* RemoveOutputStream(LogStream, closeUnderlyingStream)  
	- Removes the specified log stream from the framework if it was added.  
	- closeUnderlyingStream specifies if the underlying stream should be closed.
* LoadConfig(path)
	- Loads the settings specified in the conets all the variables.  
	- Wrapper that loads the config from XML and then calls LoadConfig(AdlConfig)  
* SaveConfig(path)  
	- Saves the config as XML to the specified path  
* GetPrefixMask(prefix, out BitMask)  
	- searches the Prefix Register if there is a mask that is mapped to this specific prefix.  
* GetMaskPrefix(BitMask)  
	- returns the prefix registered with this mask  
	- returns "" when not having a prefix or PrefixLookupMode is set to     PrefixLookupSettings.Noprefix  

## ADL.Bitmask  
ADL.BitMask is used to create Masks fast and without writing the same binary operations over and over again. It has implicit conversions with int and enums when using the generic version(otherwise they behave the same)  

## Functions:  
* SetAllFlags(newMask)  
	- changes the whole internal mask without changing the object.  
* SetFlag(flag, isSet)  
	- changes the specified flag to the state of isSet  
* HasFlag(flags, MatchType)  
	- Wrapper that calls IsContainedInMask  
* Flip()  
	- inverts every flag in the mask (can be seen as "anti-mask")  
* static IsContainedInMask(mask, flag, matchType)  
	- returns true if the Mask contains the flag based on the match type.  
* static GetUniqueMasksSet(mask)  
	- Splits the specified mask into seperate flags  
* static IsUniqueMask(mask)  
	- returns true if the mask is a power of 2(aka. a mask with only one flag set to true)  
* static CombineMaskas(MaskCombineType, int[])  
	- Combines the specified masks with the specified method  
* static RemoveFlags(mask, flags)  
	- returns the mask with all flags specified turned off.  
* static FlipMask()  
	- flips all bits in the mask  
* supports autoconvert(implicit conversion to int and back to bitmask)  
	- So BitMask bm = 1 is correct  
	- as well as BitMask = new Bitmask(1)  
	- or int mask = new BitMask(1)  

## ADL.MaskCombineType  
ADL.MaskCombineType is an enum specifying how masks should be merged

### Values:  
* BitOr  
	- includes every set flag from every mask supplied  
	- (1000 + 0110 = 1110) in binary  
* BitAnd  
	- includes ever set flag that is set in EVERY mask supplied  
	- (1000 + 0110 = 0000) in binary  

## ADL.MatchType  
ADL.MatchType is an enum specifying how masks should be compared.  

### Values:  
* MatchAll  
	- when Mask A contains EVERY set flag from Mask B  
* MatchOne  
	- when Mask A contains at least ONE flag from Mask B  

## ADL.Configs.PrefixLookupSettings  
ADL.Configs.PrefixLookupSettings is the enum specifying how prefixes should be appended.  

### Values:  
* Noprefix: Do not add a prefix to the log.  
* Addprefixifabailable: If we happen to have a prefix for the mask, we can prepend it.  
* Deconstructmasktofind: Will completely deconstruct the mask into unique flags to find the prefixes and add them all.  
* Onlyoneprefix: Cancels the search for prefixes when one prefix was found.  
	- Has no effect when Addprefixifavailable it not set.  
* BakePrefixes will remember the prefixes that were constructed before so we can cache them to reuse instead of regenerating.  
	- Has no effect when Addprefixifavailable and Deconstructmasktofind are not set.  

## ADL.UpdateDataObject  
ADL.UpdateDataObject is implementing the Update Check for each Extension.  
It is not meant to be used by programs outside of ADL. 

## ADL.Utils  
ADL.Utils implements some useful functions and helpers that are used through the entire Solution.

## ADL.Configs.AdlConfig  
ADL.Configs.AdlConfig is holding all the settings that can be set in the ADL core libary  

## ADL.Configs.ConfigManager  
ADL.Configs.ConfigManager is the class capable of reading all config files in the solution

## ADL.Configs.IAdlConfig   
ADL.Configs.IAdlConfigsets the nessecary constraints to be able to load and save configs with the ConfigManager.  

## ADL.Configs.SerializableDictionary  
ADL.Configs.SerializableDictionary is a class that is mimicing a dictionary that is able to save itself to xml.  

## ADL.Streams.Log  
ADL.Streams.Log represents a single log, it contains mask and message.  
and is able to serialize and deserialize itself from or to bytes  

## ADL.Streams.LogPackage  
ADL.Streams.LogPackage is a container of logs that is used to be able to process more logs in one continuous stream read operation.  

## ADL.Streams.LogStream  
ADL.Streams.LogStream is the base class that is used to get the logs/logpackages to the output streams.  

### Settings:  
* OverrideChannelTag:bool if this is set to true, the framework will not add prefixes to this logstreams logs(other logstreams still get the prefixes)  
* Mask:int specifies on what logs can reach this stream.  
* AddTimeStamp:bool sets if the log should have a timestamp infront of the prefixes.  
* PBaseStream is a little hack to still make the Unity Extension work

### Functions:  
* The same functions as System.IO.Stream  
* Write(log) writes a log to the stream.  
	- will under the hood call Write(byte[], start, length)  
* IsContainedInMask(BitMask) wrapper of Bitmask.IsContainedInMask(mask0, mask1)  

## ADL.Streams.LogTextStream  
ADL.Streams.LogTextStream is a class derrived from LogStream that will serialize the logs as plain text instead of binary in combination with mask.  
This needs to be used with the standard console output.  
The Streams are usable in parallel.  

### Functions:  
* Write(log) override from LogStream.  
	- Simply removing the integer mask from the log and just serializing the message itself.  
* ADL.Streams.GenPipeStream<T>  
	- A threadsave implementation of a Stream based on a Queue<T>.  
	- Not completely compliant to IO.Stream interface, but usable.  
* ADL.Streams.PipeStream  
	- a class derrived from GenPipeStream<byte>  
	- fully compliant to System.IO.Stream since it is using bytes as T
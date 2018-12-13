# ADL(Advanced Debug Logging)
___
<br><br>
# Table of Content
- [Intro](#intro)
___
- [Documentation ADL.Debug](#documentation-adldebug)
   	+ int ListeningStreams(get)
    + AddOutputStream(LogStream stream)]
    + RemoveOutputStream(LogStream stream)
   	+ RemoveAllOutputStreams(bool CloseStream = true)
   	+ AddPrefixForMask(int mask, string prefix)
   	+ RemovePrefixForMask(int mask)
   	+ RemoveAllPrefixes()
   	+ SetAllPrefixes(params string[] prefixes)
   	+ Log(int mask, string message)
   	+ Log<T>(T mask, string message)
___
- [Documentation ADL.LogStream](#documentation-adllogstream)
    + LogStream(Stream stream)
    + LogStream(TextWriter stream)
    + CloseStream
    + SetMatchingMode(MatchType matchType)
    + SetMask(int newMask)
    + SetTimeStampUsage(bool useTimeStamps)
    + IsContainedInMask(int mask)
    + Log(int mask, string message)
    + CreateLogStreamFromFile(...)
    + CreateLogStreamFromStream(...)
___
- [Documentation ADL.Bitmask](#documentation-adlbitmask)
	+ Bitmask(bool wildcard = false)
	+ Bitmask(int mask)
	+ Bitmask(params int flags)
	+ SetAllFlags(int newFlags)
	+ SetFlag(int flag, bool yes)
	+ HasFlag(int flags, MatchType matchType)
___
- [Documentation ADL.Bitmask Generic T](#documentation-adlbitmask-generic-t)
	+ Bitmask(bool wildcard = false)
	+ Bitmask(T mask)
	+ Bitmask(params T flags)
	+ SetAllFlags(T newFlags)
	+ SetFlag(T flag, bool yes)
	+ HasFlag(T flags, MatchType matchType)
___
- [Documentation ADL.Utils](#documentation-adlutils)
	+ IntPow(int basis, int exp)
	+ string TimeStamp(get)
	+ NumToTimeFormat(int time)
	+ IsContainedInMask(int mask, int flag, bool MatchAll)
	+ GetUniqueFlagsSet(int flag)
	+ IsUniqueFlag(int flag)
	+ CombineMasks(MaskCombineType combineType = MaskCombineTypee.BIT_OR, params int[] masks)
	+ CombineMasks Generic T(MaskCombineType combineType = MaskCombineTypee.BIT_OR, params T[] masks)
	+ RemoveFlags(int mask, int flags)
___
- [Documentation ADL.Unity](#documentation-adlunity)
___
- Best Practices

___
<br><br>
# Intro
___
<br><br>
# Documentation ADL.Debug 

ADL.Debug is the main class in ADL. This is the class you will use the most. It does not need Instanciation because the whole ADL project is designed to be static.
___

## int ListeningStreams(get)

Returns the number of LogStreams in the Debug System.
___

## AddOutputStream(LogStream stream)

Adds a Log Stream to the Debug System. Double Adding is not possible.
___

## RemoveOutputStream(LogStream stream, bool CloseStream = true)

Removes the specified stream from the Debug System.
___

## RemoveAllOutputStreams(bool CloseStream = true)

Removes all streams from the Debug System.
___

## AddPrefixForMask(int mask, string prefix)

Adds a Prefix for the specified Mask. To keep things less confusing its advisable to only use Uniform Flags(Power of 2 numbers) as a Mask
___

## RemovePrefixForMask(int mask)

Removes the Prefix for the specified Mask.
___

## RemoveAllPrefixes()

Removes all prefixes in the Debug system. Meaning that every flag combination has the default Mask Prefix
___

## SetAllPrefixes(params string[] prefixes)

Sets all prefixes in the list from low to high

	prefixes[0] = 1
	prefixes[1] = 2
	prefixes[2] = 4
	prefixes[3] = 8
	...
___

## Log(int mask, string message)

Sends a Debug Log into the Debug System, that sends it to the right LogStreams
___

## Log Generic T (T mask, string message)

Generic Version of Debug.Log. Saves some time writing with enums instead of integers.

This is a bit slower, but especially useful if you have a lot of flags.
___

<br><br>
# Documentation ADL.LogStream
The LogStream class contains the underlying stream and all the extra informations about masks and other flags.

## Standard Settings
		Mask = Wildcard
		Match All Flags
		SetTimeStamp = False
___

## LogStream(Stream stream)
Creates a new LogStream with standard settings.
___

## LogStream(TextWriter stream)
Creates a new LogStream with standard settings
___

## CloseStream()
Closes and Disposes the underlying stream
___

## SetMatchingMode(MatchType matchType)
Sets the Matching Mode.
___

## SetMask(int mask)
Sets the Mask.
___

## SetTimeStampUsage(bool useTimeStamps)
Sets wether there should be a Time Stamp prepended to each log.
___

## IsContainedInmask(int mask)
Returns true if mask is valid according to the Settings of the LogStream
___

## Log(int mask, string message)
Sends a Log to this Stream
This is not meant to be called by other code than the Debug class.
___

## CreateLogStream...(...)
Creates a LogStream with the specified options.
Note: This is useful when you want to make the initializations of LogStreams a one liner.
___

### CreateLogStreamFromFile(...)
Creates a LogFileStream
If no Settings are specified the LogStream will have the Standard Settings
___

### CreateLogStreamFromStream(...)
Creates a LogStream based on any kind of Stream.
If no Settings are specified the LogStream will have the Standard Settings
___

<br><br>
# Documentation ADL.BitMask
BitMask is a optional class that is not nessecary to be used. It tries to simplify the usage of Masks even more.
Default Mask is Nothing

___
## Bitmask(bool wildCard = false)
Creates a new Bitmask Object with wildcard or standard mask
___

## Bitmask(int mask)
Creates a new Bitmask Object with the Specified Mask
___
## Bitmask(params int[] flags)
Creates a new Bitmask with the specified flags.
___

## SetAllFlags(int newFlags)
Discards the current mask and sets it to a new one.
___

## SetFlag(int flag, bool yes)
Sets a Specific flag. Technically its possble to Set multiple Flags at once, but not advisable.
___

## HasFlag(int flags, MatchType matchType)
Returns tre when the mask satisfies the specified flags
___

<br><br>
# Documentation ADL.BitMask Generic T
BitMask is a optional class that is not nessecary to be used. It tries to simplify the usage of Masks even more.
Default Mask is Nothing

___
## Bitmask(bool wildCard = false)
Creates a new Bitmask Object with wildcard or standard mask
___

## Bitmask(int mask)
Creates a new Bitmask Object with the Specified Mask
___

## Bitmask(T mask)
Creates a new Bitmask Object with the Specified Mask

___
## Bitmask(params T[] flags)
Creates a new Bitmask with the specified flags.
___

## SetAllFlags(T newFlags)
Discards the current mask and sets it to a new one.
___

## SetFlag(T flag, bool yes)
Sets a Specific flag. Technically its possble to Set multiple Flags at once, but not advisable.
___

## HasFlag(T flags, MatchType matchType)
Returns tre when the mask satisfies the specified flags
___

<br><br>
# Documentation ADL.Utils
Utils contains all the little helper functions that do not make sense to include it just in one class.

___

## IntPow(int basis, int exp)
Calculates the basis by the power of exp
___

## string TimeStamp(get)
A property that Assembles the TimeStamp Layout the Debug system uses.
___

## NumToTimeFormat(int time)
Returns a string with a 0 appended if time < 10
___

## IsContainedInMask(int mask, int flag, bool MatchAll)
Returns True if the Specified mask satisfies the specified flag.
___

## GetUniqueFlagsSet(int flag)
Returns a list of string with all the unique(power of 2) flags that the mask contains
___

## IsUniqueFlag(int flag)
Returns true if the flag is a power of 2
___

## CombineMasks(MaskCombineType combineType = MaskCombineType.BIT_OR, params int[] masks)
Returns the combined mask of all masks based on the combinetype
___

## CombineMasks Generic T (MaskCombineType combineType = MaskCombineType.BIT_OR, params T[] masks)
Returns the combined mask of all masks based on the combinetype in a Generic object
___

## RemoveFlags(int mask , int flags)
Removes all flags from the mask.
___

<br><br>
# Documentation ADL.MaskCombineType
This enum has 2 values

		BIT_OR = 0 <-- Add everything that both tables have
		BIT_AND = 1 <-- Add flags represented by both tables
___
<br><br>
# Documentation ADL.MatchType
This enum has 2 values

		MATCH_ALL = 0 <-- If there is one of the specified flags set to false, return false
		MATCH_ONE = 1 <-- If there is one of the specified flags set to true, return true
___
<br><br>
# Documentation ADL.Unity.DebugComponent
MonoBehaviour that needs to be in the Hierarchy once and only once.
For more information about the properties of the Component look at the Tooltips in the Unity Editor
___
<br><br>
# Documentation ADL.Unity.LogStreamParams
Serializable Class that contains all the settings for creating a log stream from file.

___
<br><br>
# Documentation ADL.Unity.UnityTextWriter
Inherits from StreamWriter and uses the Unity.Debug.Log.
Its Possible to have warning and error messages if specified in ADL.Unity.DebugComponent.

___
<br><br>
# Documentation ADL.Unity.EnumFlagsAttributeDrawer
Draws a Custom Bitmask Editor in the Unity Editor Inspector
___
<br><br>
# Documentation ADL.Unity.DebugEditorWindow
Uses OnInspectorGUI to Set the specified DebugLevel Names.
___
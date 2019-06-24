# ADL.CustomCMD Class Explanations/Information:  
ADL.CustomCMD is an ADL extension that implements a colored custom command shell.  

## ADL.CMDUtils

### Functions:

* SaveConfig(AdlCustomConsoleConfig, path)
	- Saves the configuration as XML to the specified path

* SaveConfig(backgroundColor, fontColor, fontSize, colorCoding, path)
	- Saves the configuration as a AdlCustomConsoleConfig XML object to the specified path

* CreateCustomConsole(stream, backgroundColor, fontColor, fontSize, frameTime, colorCoding)
	- Creates a custom console based on the settings supplied.

* CreateCustomConsole(stream, AdlCustomConsoleConfig)
	- wrapper that calls CreateCustomConsole with all the "unpacked" settings.

* CreateCustomConsole(stream, configPath)
	- wrapper that will read the config from a file directly

* CreateCustomConsoleNoReturn(stream, AdlCustomConsoleInfo)
	- Creates a custom console but it will not return it.
	- this is useful when your application does not reference Windows.Forms, but you want to have the custom console.

* CreateCustomConsoleNoReturn(stream, configPath)
	- wrapper that will read the config from a file directly

## RichTextBoxExtensions

* AppendText(Textbox, string, color)
	- Appends colored text to a text box without destroying the color information of the previous text.

* ScrollToBottom(Textbox)
	- will use some external C++/Windows intern magic to make the box always scrolled down.

## ADL.Configs.AdlCustomConsoleConfig

### Settings:

* BackgroundColor:Color Specifies the color of the textbox(default black)

* FontColor:Color Specifies the font color of the logs(default white)

* FontSize:float Specifies the size of the font.

* FrameTime:float Specifies how often the Console should check if there are any new logs.

* ColorCoding:Dictionary<int, SerializableColor> Specifies what mask has which color.

## SerializableColor
Implements a Color Struct that can be serialized with XML

## CustomCMDForm
The Main form of the extension

### Settings:

* MaxConsoleLogCount:int specifies how many logs the textbox can display at one until it clears the text

* MinConsoleLogCount:int specifies how many logs should remain after clearing the text to have a continous flow of logs.
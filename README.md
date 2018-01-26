# SubnauticaSplitter
Automatic Subnautica Time Splitter for LiveSplit

## Requirements
* x64 bit Mono Injector
* LiveSplit
* LiveSplit Server


## How to use
1. Make sure you have [LiveSplit](http://livesplit.github.io/) and [LiveSplit Server](https://github.com/LiveSplit/LiveSplit.Server) installed and running
2. Make sure you don't have any splits set-up in LiveSplit
3. Make sure you have a mono injector, suggested injectors are below
4. Open Subnautica and wait until you are in the main menu
5. Inject the compiled **SubnauticaSplitter.dll** into the **Subnautica.exe** process, if the injection was successfully you should see the text "SubnauticaSplitter - Connected" on the bottom left in your Subnautica

**OPTIONAL**
You can also just use the bundled MonoInjector that you can find in Releases, just double-click the "Inject.bat" while you are in Subnautica's main menu and you should be golden

## Suggested Injectors
You need an x64 injector that can inject into mono, some of them are:
* [MInjector](https://github.com/EquiFox/MInjector)
* [MonoInjector](https://github.com/Michidu/MonoInjector)

## Troubleshooting
* If you see the text "SubnauticaSplitter - Not Connected"; you don't have the LiveSplit server running or set up properly, make sure it is running on port **16834**
* When submitting a new issue please make sure you include log file at **Subnautica/Subnautica_Data/output_log.txt**

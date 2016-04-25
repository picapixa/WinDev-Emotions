# Dealing With Emotions

This aims to show the structure of an app following the Model-View-ViewModel pattern and the process of interfacing with Application Programming Interfaces. 
This app runs on Windows 8.1.

## How to study the code

1. [Install MVVMLight templates.](http://www.mvvmlight.net/installing/)
2. Create a new project with the MVVMLight template on Visual C# named "MVVMLight (Win81)". You can also opt to develop for Windows 10 under "MVVMLight (Win10Univ)".
3. For best practices, always do the following when using any template:
    - Go to the *Build* menu, then select *Clean Solution*. Wait for it to succeed.
    - After cleaning, go back to *Build*, then now select *Rebuild Solution*.
4. [Install JSON.NET to your solution file.](http://stackoverflow.com/questions/4444903/how-to-install-json-net-using-nuget)
5. Go to References and check if there are items that begin with the word "GalaSoft". 

    If there is none, follow these steps:

    - Go to *Tools* menu, then *Nuget Package Manager* (or something similar), then look for *Package Manager Console*.
    - Type into the console: `Install-Package MVVMLight`.

6. Because our app will need the capability to access the library, you need to enable it.
    - Open Package.appxmanifest.
    - Go to *Capabilities* Tab.
    - Check the *Pictures Library* capability.
7. Now try to build your solution based on what is in this code.

# Some helpful links:

- **[Windows Developer Center](http://dev.windows.com)** - all relevant design and development documentation are here.
- **[Microsoft Cognitive Services Emotion API documentation](https://dev.projectoxford.ai/docs/services/5639d931ca73072154c1ce89/operations/563b31ea778daf121cc3a5fa)** - all related documentation on using the Emotion APIs.

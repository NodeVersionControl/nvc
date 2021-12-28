# Node Version Control

**.Net 6 Command line tool for managing NodeJS versions without admin privileges**

This application takes inspiration from the popular NVM (Node Version Manager) tool. NVM unfortunately requires constant Admin privileges in order to function properly.

  
With this application, you will be able to install, remove and change between different versions of NodeJS all without having to use admin privileges. NVC will also keep your global NPM packages in working order with each version of NodeJS to allow for a seamless transition between projects.

In order to get NVC to work properly, you will need admin privileges to adjust the system's PATH variable and to optionally store NVC under the \Program Files\ directory for access anywhere in a cmd window.
  
## Setup

1. Clone the repository to your computer.

2. Build the application under Release (Have to have Visual Studio 2022)

3. Copy the Release NVC folder to \Program Files\ and add it to your System's PATH variable. This is so you can access NVC from anywhere in the cmd line/powershell (Requires Admin privledges)

4. Edit your system's PATH variable to change the location of your NodeJS installation.  NVC's Default location will be ``` C:\nodejs ```. See the Configuration section to change the default location.

5. Execute commands from the by using nvc in command line or powershell.

## Commands

### Install a new NodeJS Version

* ``` nvc -i 10.24.1 ```
* ``` nvc --install 10.24.1 ```

### Remove a NodeJS Version

* ``` nvc -r 10.24.1 ```
* ``` nvc -r 10 ```  *(Handles partial strings if unique)*
* ``` nvc --remove 10.24.1 ```

### Change to a NodeJS Version

* ``` nvc -c 10.24.1 ```
* ``` nvc -c 10 ```  *(Handles partial strings if unique)*
* ``` nvc --change 10.24.1 ```

### List all NodeJS Versions

* ``` nvc -l ```
* ``` nvc --list ```


## Configuration

**Configuration changes can be made by adjusting the config.json file.**

### NODE_DIRECTORY

Directory where the node.exe will be executed from. You will need to update your systems environment path to point to this folder as well. **Make sure this directory doesn't require admin privileges to read/write to this location.**

Default Value: ``` C:\nodejs ```

### NODE_VERSIONS_DIRECTORY

The directory that all saved node versions will be stored. **Make sure this directory doesn't require admin privileges to read/write to this location.**

Default Value: ``` C:\nodejsVersions ```

### WINDOWS_ARCITECTURE

Architecture of the NodeJS you want to install. IE x64 or x86

Default Value: ``` x64 ```

### TEMP_FOLDER

Directory to a Temp folder used to download Zip files. **Make sure that it points to an empty folder, as it will regularly remove all files in that folder. You have been warned.**

Default Value: ``` %TEMP%\NodeJSVersionDownloads ```

### NPM_GLOBALS_DIRECTORY

Directory that NPM installs its global packages. The default value is the default folder location for NPM. If you have configured NPM to install global packages elsewhere you will need to adjust this value to match.

Default Value: ``` %USERPROFILE%\AppData\Roaming\npm\node_modules ```
# RESTtest
Tool to test RESTful Web API BETA version

### Description
* C#
* Visual Studio
* System.Net
* WebRequest
* BETA

### Packages
* Newtonsoft.Json
* Newtonsoft.Json.Schema

### How to Use
* Run the executable, Windows Form should appear.
* User can choose to fill fields manually and press Test or 
	they can load XML file with information
* XML way: One file describes the web API to be tested (url,headers,environmental variables,etc.)
* XML way: The other file describes different test cases(method,json string, etc.) 
* XML way: This test XML files must be in a folder called "TestCases" in the same directory as the executable.
* ![alt text](wiki/pics/)
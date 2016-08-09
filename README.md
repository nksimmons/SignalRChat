# README #


### What is this repository for? ###

* This is a simple chat application using SignalR for full-duplex communication over websockets.
* To run the application, hit "Start" to start the web application in your browser in debug mode, or deploy to IIS.

### Application features ###

* The user may enter a name to login to the system without registering.
* There is a button to logout after the user logs in.
* When the user logs in, the last 15​messages will be shown (if available).
* The system allows up to 20​ users at a time.
* The system shows a list of all available users.
* Each user has a text box to type the message. Pressing Enter will send it.

### How do I get set up? ###

* First, create a database in SQL Server called "SignalRChat" (without quotes). Then, run the SQL script contained in TableCreateScriptForSignalRChatDB.sql. This will create the two tables for the application: Messages and Users. Modify the connection string in the web.config to reference your instance of SQL server. Finally, open the solution in Visual Studio. The project was created in VS 2015. 
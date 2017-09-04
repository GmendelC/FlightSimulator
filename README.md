Flight Simulator

The project is simulator of airport: the airporthave legs, receiving plane and move them in legs.

In this project was a plane factory, and contest a trafic of randons planes, search empy next leg, conflict of plane in one leg, piority and dont block the airport.

Requirements: visual studio 2017, monogoDB, browser supporter html5.

File Size: 0.5Kb gzipped no bin and packjet,

Usage:

Run mongoDb in your server.
Run web server (web api project).
Run console airport run (tower runner project).

you can add plane from http request.
you can also use the factory server for add random planes from other computer in http request.
(remeber to remove the factory from tower runner)

if you want you can change the conection string in config file. remenber change in tower runner and wep api).

if you want change the speed of plane factory changing the contanse of INTERVAL_MILISENCOUDS.

if data base all ways drop exeption dromp  the data base by dismarking the drop db in ctor of dal. but mark them after.

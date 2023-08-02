# Overviw
Low level memory manipulation of process during runtime. Makes use of dll native import (leverage functionality not accessible in C# ). Reverse-engineering to scrape data from running process. 
This project is a Jungle Tracker Overlay for the game League of Legends, designed to provide players with valuable in-game information, such as jungle camp respawn times (even when cleared in FoW) and Kindred's mark respawn tracking. 
The overlay is designed to be non-intrusive, appearing only when the game process is open, and it operates externally to the game.

# Technologies used
* SharpDX: SharpDX is a managed wrapper for DirectX and is used for graphics rendering.
* Memory Reading: Interacting the the program's memory to read values and track in-game information.
* Double Buffering: A technique to improve drawing performance and prevent flickering when updating the overlay's content.
* Windows API: The application utilizes Windows API functions to read process memory and manipulate window behavior.
* Windows Forms: The graphical user interface that creates the overlay window.
* Reverse Engineering (Ida & Reclass).
* Threads.
* .NET Framework.


# Event-Driven Architecture with Separation of UI and Logic
The application is developed with an event-driven approach, utilizing threads, timers, and callbacks. Different parts of the application respond to specific events triggered by the main program. 
The separation of user interface and rendering logic is achieved by using Windows Forms for the overlay and SharpDX for rendering.

# Key Features (non-technical) game related
* Enemy Jungle Tracker: The overlay allows players to track jungle camp respawn times even in the fog of war (FoW) after a jungle kill.
* Kindred Mark Respawn Tracking: Players can keep track of Kindred's mark respawn time, making it easier to plan strategic movements.
* Next Kindred Mark Prediction: The overlay predicts the location of the next Kindred mark 45 seconds in advance.
* Non-Intrusive Overlay: The overlay only shows when the game process is open, allowing players to tab out freely without disturbance.
* External and Safe: The application operates externally to the game, ensuring a 0% chance of getting banned for its usage.

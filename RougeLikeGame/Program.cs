using RogueLib.Dungeon;
using RogueLib.Engine;
using RogueLib.Utilities;
using SandBox01.Levels;
using TileSet = System.Collections.Generic.HashSet<RogueLib.Utilities.Vector2>;
using SandBox01.Levels;

namespace RlGameNS;
using Spectre.Console;

class Program {

   static void Main(string[] args) {
      try
        {
            bool isRunning = true;

                do
                {
                    AnsiConsole.Clear();

                    string userSelection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("""
                           [SlateBlue1]
                            ____   ___   ____ _   _ _____   ____  _____ ____   ___  ____  _   _ 
                           |  _ \ / _ \ / ___| | | | ____| |  _ \| ____| __ ) / _ \|  _ \| \ | |
                           | |_) | | | | |  _| | | |  _|   | |_) |  _| |  _ \| | | | |_) |  \| |
                           |  _ <| |_| | |_| | |_| | |___  |  _ <| |___| |_) | |_| |  _ <| |\  |
                           |_| \_\\___/ \____|\___/|_____| |_| \_\_____|____/ \___/|_| \_\_| \_|
                           [/]
                           
                           
                           [blue]Make your selection[/]
                           """)
                    .AddChoices
                        (
                              "(1) Play the game",
                              "Quit"
                        )
                        );

                    switch (userSelection)
                    {
                        case "(1) Play the game":
                                StartGame();
                                break;

                        case "Quit":
                                isRunning = false;
                                AnsiConsole.MarkupLine("[blue]Thanks for playing Rogue Reborn![/]");
                                break;
                        }
                }
                while (isRunning);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message}");
            Console.ReadLine();
        }
    }

    static void StartGame()
        {
            AnsiConsole.Clear();

            AnsiConsole.MarkupLine("[purple]Here's how to play the game:[/] \n");
            AnsiConsole.MarkupLine("[purple]You need to move using the keyboard arrows to explore the dungeon.[/]");
            AnsiConsole.MarkupLine("[purple]Enemies will attempt to stop your heroic attempt at collecting the lost treasure[/]");
            AnsiConsole.MarkupLine("[purple]They might seek you out upon entering a room, be careful, and defeat them![/]");
            AnsiConsole.MarkupLine("[purple]Gold can be found on the floors everywhere, don't forget to collect it![/]");
            AnsiConsole.MarkupLine("[purple]Make it to stairs, and press PgUp to go up a level![/]");
            AnsiConsole.MarkupLine("[purple]Make it to the final treasure to win![/]");

            AnsiConsole.MarkupLine("\n\n[green]Press any button to start your adventure[/]");
            Console.ReadLine();



            Console.Clear();
            Game game = new MyGame();
            game.run();

            if (Level.isWinner)
            {
                AnsiConsole.MarkupLine("""
                                       [green]
                                         _____                        __       __     __  _               __
                                        / ___/__  ___  ___ ________ _/ /___ __/ /__ _/ /_(_)__  ___  ___ / /
                                       / /__/ _ \/ _ \/ _ `/ __/ _ `/ __/ // / / _ `/ __/ / _ \/ _ \(_-</_/ 
                                       \___/\___/_//_/\_, /_/  \_,_/\__/\_,_/_/\_,_/\__/_/\___/_//_/___(_)  
                                                     /___/                                                  
                                       [/]
                                       """);

                AnsiConsole.MarkupLine("[blue]CONGRATULATIONS! YOU ARE A TRUE ADVENTURER![/]");
                AnsiConsole.MarkupLine("[green]\n\nPress any key to go back to the main menu[/]");
                Console.ReadKey();
            }
        else
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("""
                                       [red3]
                                              _______       __       ___      ___   _______ 
                                             /" _   "|     /""\     |"  \    /"  | /"     "|
                                            (: ( \___)    /    \     \   \  //   |(: ______)
                                             \/ \        /' /\  \    /\\  \/.    | \/    |  
                                             //  \ ___  //  __'  \  |: \.        | // ___)_ 
                                            (:   _(  _|/   /  \\  \ |.  \    /:  |(:      "|
                                             \_______)(___/    \___)|___|\__/|___| \_______)
                                                                                       
                                                ______  ___      ___  _______   _______     
                                               /    " \|"  \    /"  |/"     "| /"      \    
                                              // ____  \\   \  //  /(: ______)|:        |   
                                             /  /    ) :)\\  \/. ./  \/    |  |_____/   )   
                                            (: (____/ //  \.    //   // ___)_  //      /    
                                             \        /    \\   /   (:      "||:  __   \    
                                              \"_____/      \__/     \_______)|__|  \___)                                                  
                                       [/]
                                       """);
                AnsiConsole.MarkupLine("[Yellow4]Press any key to return to the main menu[/]");
                Console.ReadKey();
            }
   }
}
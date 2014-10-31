   ======================================
  |  ##   ## ###    ##                   |
  |  ##   ## ####   ##      #            |
  |  ##   ## ## ##  ##  ###   # #   ##   |
 /    ## ##  ##  ## ## #  # # ## # #  #  |
/_    ## ##  ##   ####  ### # #  # ###   |
  |    ###   ##    ###    # # #  # #     |
  |                     ##  # #  #  ##   |
   ======================================

The default files contain an example from which you can learn how to use VNgine and below you will find an explanation of
the commands and how to set up your game.

To start, assign the first file to be used by VNgine in the Setup.xml file located in the Content folder, by replacing the
default "Start" by the filename of your first txt file without the extension (".txt").

The only symbol you need to learn to type is the '¶' symbol, which you can type with Right-Alt (sometimes called "Alt Gr")
in combination with the ';' key. With this key and regular typing you can do everything VNgine has to offer.

You can setup your characters in the Setup.xml file by filling in their full name under "FullName" and the corresponding
abbreviation you would like to use in the .txt files under "Abbreviation", like the Albert and Bruce example that's in the
Setup.xml file by default. This will allow you write dialogue such as:

a¶Where am I?
  This will make the character (in our case, Albert, whom we refer to with "a"), say the line:
  "Where am I?"
  NOTE: The quotation marks are automatically added to dialogue.

To write a command, start a line with ¶ (Right-Alt + ';').
Following is the list of possible commands:

¶choice number
  This creates a choice point for the player. The number following the keyword "choice" is the number of available options
  for this choice. Example:
  ¶choice 3
  This creates a choice point that has 3 options.
  NOTE: This command is ALWAYS used in combination with the following command.

¶name description
  This command defines an option after the created choice. Following the "choice" command, this command has to follow the
  amount of times defined in the "choice" command. The option's name should not contain spaces. Example:
  ¶wenthome Go home.
  This will form an option shown to the player as "Go home.", which can be referred to by the developer as "wenthome".

¶next filename
  This will refer the game to the next text file. Example:
  ¶next chapter2
  That will refer the program to the "chapter2.txt" file.

¶next decision filename
  This will refer the game to the next text file if the designated decision has been made. Example:
  ¶next wenthome chapter3
  That will refer the program to the "chapter3.txt" file if the player chose the option designated as "wenthome".

¶play bgm/sound/video file
  This will play background music (bgm), a sound effect or a video file before displaying the next line. The filename should not
  include the extension. The extension for background music is ".mp3", the extension for a sound effect is ".wav" and for
  a video file the extension is ".avi". Example:
  ¶play video intro
  This will play the video file "intro.avi".

¶set/clear bg/r1/r2/l1/l2 filename
  This will set or clear the image at the designated location before displaying the next line. The location options
  are: "bg" for the background, "r1" for right front, "r2" for right back, "l1" for left front, "l2" for left back.


¶check [choice name]
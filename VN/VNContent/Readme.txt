First, set the first file in the Setup.xml file in the Content folder, by replacing the "filename" text under StartFile by
the filename without the extension (".txt").

To write a command, start a line with ¶ (Right-Alt + ';').
Following is the list of possible commands:
¶setbg filename
 Will set a background image where the filename is the name of the image file without the extension.

¶choice
1
2
3
¶next filename
 This will refer the game to the next text file.
¶next decision filename
 This will refer the game to the next text file if the designated decision has been made.

You can set your characters in the Setup.xml file by filling in their full name under FullName and the abbreviation to use in
the .txt files under Shortcut. This will allow you to use a command like:
¶a "Where am I?"
This will make the character whose name is shortened to "a", say "Where am I?" including the quotation marks.

¶check [choice name]
¶set/clear bg/r1/r2/l1/l2
¶next ([choice name]) file
¶play music/sound/video file ([consecutive file])
¶choice [number]
¶choice name description
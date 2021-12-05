using System;
using System.Collections.Generic;
using System.Globalization;

namespace Flags
{
    using static ConsoleColor;
    using static Console;
    public class Program
    {

        public static void Main()
        {
            // We need this to make sure we can always use periods for decimal points.
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            //UtilityMethods.TrackCursor();
            Draw(Flag.Gambia);
            Draw(Flag.Germany);
            Draw(Flag.Sweden);
            Draw(Flag.Botswana);
        }    
        
        public enum Flag
        {
            Germany,
            Ukraine,
            France,
            Sweden,
            Denmark,
            Benin,
            Botswana,
            Gambia
        }

 
        public static void Draw(Flag country)
        {
            if (country == Flag.Germany)
            {
                Draw3StripedFlag(Black, Red, Yellow);
            }
            if(country == Flag.Ukraine)
            {
                Draw2StripedFlag(Blue, Yellow);
            }
            if(country == Flag.France)
            {
                var colors = new List<ConsoleColor>
                {
                    Blue,
                    White,
                    Red
                };
                DrawVerticalStripes(colors, 7, 6);
            }
            if(country == Flag.Sweden)
            {
                CrossFlag(DarkBlue, Yellow);
            }
            if (country == Flag.Denmark)
            {
                CrossFlag(Red, White);
            }
            if (country == Flag.Benin)
            {
                BeninShape();
            }
            if (country == Flag.Botswana)
            {
                BotswanaShape();
            }
            if (country == Flag.Gambia)
            {
                BotswanaShape(Red, DarkGreen, DarkBlue, White);
            }
            WriteLine();
        }

        public static void CrossFlag(ConsoleColor background, ConsoleColor crossColor)
        {
            for (int i = 0; i < 6; i++)
            {
                DrawRight(background, 15); DrawRight(crossColor, 9); DrawRight(background, 25); WriteLine();
            }
            DrawRectangle(crossColor, 49, 5);
            for (int i = 0; i < 6; i++)
            {
                DrawRight(background, 15); DrawRight(crossColor, 9); DrawRight(background, 25); WriteLine();
            }
        }
        public static void BeninShape(ConsoleColor colorleft = DarkGreen, ConsoleColor colortop = Yellow, ConsoleColor colordown = Red)
        {
            for (int i = 0; i < 8; i++)
            {
                DrawRight(colorleft, 18); DrawRight(colortop, 30); WriteLine();
            }
            for (int i = 0; i < 8; i++)
            {
                DrawRight(colorleft, 18); DrawRight(colordown, 30); WriteLine();
            }

        }

        public static void BotswanaShape(ConsoleColor colorbackgroundTop = Blue, ConsoleColor colorbackgroundBottom = Blue, ConsoleColor colorBoldLine = Black, ConsoleColor colorHighlight = White)
        {
            DrawRectangle(colorbackgroundTop);
            DrawLine(colorHighlight, 30);
            DrawRectangle(colorBoldLine);
            DrawLine(colorHighlight, 30);
            DrawRectangle(colorbackgroundBottom);
        }


        public static void DrawStripedFlag(List<ConsoleColor> consoleColors, int linewidth = 20, int lineheight = 2)
        {
            consoleColors.ForEach(c => DrawRectangle(c, linewidth, lineheight));
        }
        public static void Draw3StripedFlag(ConsoleColor color1, ConsoleColor color2, ConsoleColor color3, int x = 20, int y = 2)
        {
            DrawRectangle(color1, x, y);
            DrawRectangle(color2, x, y);
            DrawRectangle(color3, x, y);
        }

        public static void Draw2StripedFlag(ConsoleColor color1, ConsoleColor color2, int x = 20, int y = 3)
        {
            DrawRectangle(color1, x, y);
            DrawRectangle(color2, x, y);
        }

        public static void DrawRectangle(ConsoleColor color, int x = 30, int y = 3)
        {
            for (int i = 0; i < y; i++)
            {
                DrawLine(color, x);
            }
        }
        public static void DrawLine(ConsoleColor color, int numbOfSpaces)
        {
            BackgroundColor = color;

            for (int i = 0; i < numbOfSpaces; i++)
            {
                Write(" ");
            }
            ResetColor();
            WriteLine();
        }
        public static void DrawRight(List<ConsoleColor> colors, int numbOfSpaces = 5)
        {
            foreach (var c in colors)
            {
                DrawRight(c, numbOfSpaces);
            }
            ResetColor();
        }
        public static void DrawRight(ConsoleColor color, int numbOfSpaces)
        {
            BackgroundColor = color;
            for (int i = 0; i < numbOfSpaces; i++)
            {
                Write(" ");
            }
            ResetColor();
        }
        public static void DrawVerticalStripes(List<ConsoleColor> colors, int numbOfSpaces = 5, int numbOfLines = 5)
        {
            for (int i = 0; i < numbOfLines; i++)
            {
                foreach (var c in colors)
                {
                    DrawRight(c, numbOfSpaces);
                }
                WriteLine();
            }
            ResetColor();
        }
    }
}

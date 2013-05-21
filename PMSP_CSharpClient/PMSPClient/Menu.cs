/*=======================Directives and Pragmas=============================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMSPClient
{
    /// <summary>
    /// This is the Menu class used for outputting instructions to the user and accepting options as input.
    /// </summary>
    class Menu
    {
        //Private fields.
        private string _prompt = "Would you like to browse artists or tracks?  Please enter (a) for artists, (t) for tracks, or ESC to exit the program:";
        private ConsoleKey _selectedOption;
        private string _invalidOptionMessage = "\nThe option you have specified is invalid.";
        private List<ConsoleKey> _validOptions = new List<ConsoleKey>();

        //Public properties.
        public ConsoleKey SelectedOption { get { return _selectedOption; } }

        /// <summary>
        /// This is the main constructor.
        /// </summary>
        /// <param name="ValidOptions">List of valid options.</param>
        public Menu(List<ConsoleKey> validOptions)
        {
            //Set valid options field.
            _validOptions = validOptions;

            //Set selected option.
            SetSelectedOption();
        }

        /// <summary>
        /// This method sets the selected option, and ensures it's valid.
        /// </summary>
        public void SetSelectedOption()
        {
            bool isValid = false;
            ConsoleKey option;

            //Prompt for input.
            Console.WriteLine(_prompt);

            //Get input
            option = Console.ReadKey().Key;

            //Set selected option.
            while (!isValid)
            {
                //If the specified option is valid, set it.
                if (_validOptions.Contains(option))
                {
                    _selectedOption = option;
                    isValid = true;
                }

                //Otherwise, display error and ask user for another option.
                else
                {
                    Console.WriteLine(_invalidOptionMessage);
                    Console.WriteLine(_prompt);
                    option = Console.ReadKey().Key;
                }
            }
        }
    }
}
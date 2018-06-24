using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// The following "using" directive is needed to access the "PrivateFontCollection" class
using System.Drawing.Text;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        

        #region Global Variables
        // matches roman symbols with hindu symbols, ordered from greatest to least
        // "i" is only neccessary to prevent a Runtime error
        Dictionary<string, int> conversionDict = new Dictionary<string, int>()
                                                                                    {
                                                                                        {"m", 1000000},
                                                                                        {"cm", 900000},
                                                                                        {"d", 500000},
                                                                                        {"cd", 400000},
                                                                                        {"c", 100000},
                                                                                        {"xc", 90000},
                                                                                        {"l", 50000},
                                                                                        {"xl", 40000},
                                                                                        {"x", 10000},
                                                                                        {"Mx", 9000},
                                                                                        {"v", 5000},
                                                                                        {"Mv", 4000},

                                                                                        {"M", 1000},
                                                                                        {"CM", 900},
                                                                                        {"D", 500},
                                                                                        {"CD", 400},
                                                                                        {"C", 100},
                                                                                        {"XC", 90},
                                                                                        {"L", 50},
                                                                                        {"XL", 40},
                                                                                        {"X", 10},
                                                                                        {"IX", 9},
                                                                                        {"V", 5},
                                                                                        {"IV", 4},
                                                                                        {"I", 1},

                                                                                        {"i", 1000}
                                                                                    };
        #endregion

        public Form1()
        {
            InitializeComponent();

            // Place the following code within the constructor of the form, just after 
            // the call of the method "InitializeComponent."
            // Note that the font file should be stored in the application’s startup folder.

            PrivateFontCollection customFonts = new PrivateFontCollection();
            customFonts.AddFontFile(Application.StartupPath + "\\Roman Numerals.ttf");

            // Change the font of the text box or other control. The "Families" property is actually
            // an array of fonts. The index '0' is used because only one custom font has been added,
            // which is stored at index '0' of the array.

            numTextbox.Font = new Font(customFonts.Families[0], 16, FontStyle.Regular);
            convertedLabel.Font = new Font(customFonts.Families[0], 16, FontStyle.Regular);
        }

        #region Methods
        private String RomanToHindu(String romanStr, Dictionary <String, int> conversionDict)
        {
            int sum = 0;

            for (int i = 0; i < romanStr.Length - 1; i++)
            {
                int currentValue = conversionDict[romanStr.Substring(i, 1)];
                int nextValue = conversionDict[romanStr.Substring(i + 1, 1)];

                // traverses the dictionary in order
                // if the current roman numeral has a value greater than the numeral after it
                // then add its hindu value to sum (ex. "vi", sum + 1 + 5 = sum + 6)
                if (currentValue >= nextValue)
                {
                    sum += currentValue;
                }
                else //else subtract its value from sum (ex. "iv", sum - 1 + 5 = sum + 4)
                {
                    sum -= currentValue;
                }
            }
            // adds the last roman numeral value from romanStr to sum
            // as the for loop cannot not reach the last index without causing an IndexOutOfBoundsException
            sum += conversionDict[romanStr.Substring(romanStr.Length - 1)];
            return sum.ToString();
        }

        private String HinduToRoman(int hinduNum, Dictionary<String, int> conversionDict)
        {
            String romanStr = "";
            int remainder = hinduNum;

            // traverses through the "KeyValuePair"s in the dictionary in order (greatest to least)
            foreach (KeyValuePair<String, int> pair in conversionDict)
            {
                // for each roman numeral, calculate the number of times it must be printed
                // concatenate the roman numerals to romanStr
                int letterOccurence = remainder / pair.Value;
                for (int i = 0; i < letterOccurence; i++)
                {
                    romanStr += pair.Key;
                }
                remainder = remainder % pair.Value;
            }

            return romanStr;
        }
        #endregion

        #region Event Handlers
        private void numTextbox_TextChanged(object sender, EventArgs e)
        {
            convertedLabel.Text = "";

            // length > 0 prevents an IndexOutOfBounds error
            if (numTextbox.Text.Length > 0)
            {
                // if textbox contains hindu symbols and its value is less than 3999999, convert to roman
                if (char.IsNumber(numTextbox.Text[0]) && Convert.ToInt32(numTextbox.Text) <= 3999999)
                {
                    convertedLabel.Text = HinduToRoman(Convert.ToInt32(numTextbox.Text), conversionDict);
                }
                else if (!char.IsNumber(numTextbox.Text[0])) // else, convert from roman to hindu
                {
                    string hinduStr = RomanToHindu(numTextbox.Text, conversionDict);

                    // gives an output of "" when the input is more than 3 "m"s
                    if (Convert.ToInt32(hinduStr) <= 3999999)
                    {
                        convertedLabel.Text = hinduStr;
                        // corrects user input to adhere to the rules of roman numbers
                        numTextbox.Text = HinduToRoman(Convert.ToInt32(convertedLabel.Text), conversionDict);
                        numTextbox.SelectionStart = numTextbox.Text.Length;
                    }
                }
            }

        }

        private void numTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            
            // array of Keys that are valid under all conditions
            Keys[] validKeys = {Keys.Back, Keys.Control|Keys.A, Keys.Control|Keys.C, Keys.Control|Keys.X,
                                Keys.Left, Keys.Right, Keys.Delete
                                };

            // array of valid Keys to represent roman symbols
            Keys[] validRomanKeys = {Keys.M, Keys.D, Keys.C, Keys.L, Keys.X, Keys.V, Keys.I, Keys.CapsLock,
                                Keys.Shift|Keys.M, Keys.Shift|Keys.D, Keys.Shift|Keys.C, Keys.Shift|Keys.L,
                                Keys.Shift|Keys.X, Keys.Shift|Keys.V, Keys.Shift|Keys.I
                                };

            // array of valid hindu number Keys
            Keys[] validHinduKeys = {Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5,
                                Keys.D6, Keys.D7, Keys.D8, Keys.D9,
                                Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4,
                                Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9,
                                };

            foreach (Keys key in validKeys)
            {
                if (e.KeyData == key)
                {
                    e.SuppressKeyPress = false;
                }
            }

            // any roman or hindu key is valid if the textbox is empty
            // and allows only one type of symbol (roman or hindu) in the textbox at a time
            if (numTextbox.Text.Length == 0 || char.IsNumber(numTextbox.Text[0]))
            {
                foreach (Keys key in validHinduKeys)
                {
                    if (e.KeyData == key)
                    {
                        e.SuppressKeyPress = false;
                    }
                }
            }

            if (numTextbox.Text.Length == 0 || !char.IsNumber(numTextbox.Text[0]))
            {
                foreach (Keys key in validRomanKeys)
                {
                    if (e.KeyData == key)
                    {
                        e.SuppressKeyPress = false;
                    }
                }
            }
        }
        #endregion

    }
}

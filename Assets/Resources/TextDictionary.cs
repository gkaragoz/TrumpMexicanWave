using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TextDictionary
{

	public static Dictionary<Culture,string> CultureLines = 
		new Dictionary<Culture, string>
	{
		{ Culture.MEXICAN, "Trump claims majority of Mexicans are rapists!" },
		{ Culture.LIBERAL, "Trump claims media has Liberal bias!"},
		{ Culture.WOMEN, "Trump grabs women by the pussy!"},
		{ Culture.AFRICANAMERICAN, "Trump to African Americans: 'What do you have to lose?'" },
		{ Culture.MUSLIM, "Trump to create a list of all Muslims in the U.S.!" },
		{ Culture.CAUCASIAN, "Trump angers minority group!" } // Fallback
	};


	public static Dictionary<Culture,string> CultureSubLines = 
		new Dictionary<Culture, string>
	{
		{ Culture.MEXICAN, "But some are good people!" },
		{ Culture.LIBERAL, "Claims stream is lame!"},
		{ Culture.WOMEN, "...They let him do ANYTHING!"},
		{ Culture.AFRICANAMERICAN, "You have nothing; here, have some more!" },
		{ Culture.MUSLIM, "Obama to be included!" },
		{ Culture.CAUCASIAN, "Majority groups enthralled!" } // Fallback
	};

}

using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;  

public class TextFileLoader{

	private string[] profile = {"", "", "", "", "", "", "", "", "", "", ""};
	StringReader theReader = null; 
	
	public string[] Load(string fileName){

		TextAsset textdata = (TextAsset)Resources.Load(fileName, typeof(TextAsset));

		// textdata.text is a string containing the whole file. To read it line-by-line:
		theReader = new StringReader(textdata.text);
		
		// Immediately clean up the reader after this block of code is done.
		// You generally use the "using" statement for potentially memory-intensive objects
		// instead of relying on garbage collection.
		using (theReader){
			processText(theReader);
		}

		// Done reading, close the reader and return true to broadcast success    
		theReader.Close();
		return profile;
		
	}

	void processText(StringReader theReader){

		// first name
		string line = theReader.ReadLine();
		profile[0] = cutOff13(line);

		// surname
		line = theReader.ReadLine();
		profile[1] = cutOff13(line);

		// job
		line = theReader.ReadLine();
		profile[2] = cutOff13(line);

		// accedemic
		line = theReader.ReadLine();
		profile[3] = cutOff13(line);

		// professional
		line = theReader.ReadLine();
		profile[4] = cutOff13(line);

		// Info paragraphs
		string info = "";
		for(int i = 0; i<8; i++){
			line = theReader.ReadLine();
			if(line.Length >= 15){
				info += cutOff13(line) + "\n\n";
			}
		}
		profile[5] = info;

		// office
		line = theReader.ReadLine();
		profile[6] = cutOff13(line);

		// photo
		line = theReader.ReadLine();
		profile[7] = cutOff13(line);

		// regions
		line = theReader.ReadLine();
		profile[8] = cutOff13(line);

		// sectors
		line = theReader.ReadLine();
		profile[9] = cutOff13(line);

		// services
		line = theReader.ReadLine();
		profile[10] = cutOff13(line);

	}

	string cutOff13(string text){
		if(text.Length<15){
			return "TBA";
		}else{
			string cutString = text.Substring(14);
			return cutString;
		}
	}
}


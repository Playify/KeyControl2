using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using KeyControl2.Configuration;
using KeyControl2.Util;
using PlayifyUtility.Jsons;
using PlayifyUtility.Utils.Extensions;

namespace KeyControl2.Features.Strings.HotStrings.Complex;

[InitOnLoad]
public class HotStringComplexFlip:HotStringInternal{
	private static readonly Regex Regex=new("@flip ?(.*?)[\t\n]$",RegexOptions.IgnoreCase);

	private static readonly string[] Conversion={
		"a","ɐ",
		"b","q",
		"c","ɔ",
		"d","p",
		"e","ǝ",
		"f","ɟ",
		"g","ƃ",
		"h","ɥ",
		"i","ᴉ",
		"j","ɾ",
		"k","ʞ",
		"m","ɯ",
		"n","u",
		"r","ɹ",
		"t","ʇ",
		"v","ʌ",
		"w","ʍ",
		"y","ʎ",
		"A","∀",
		"C","Ɔ",
		"E","Ǝ",
		"F","Ⅎ",
		"G","פ",
		//"H","H",
		//"I","I",
		"J","ſ",
		"L","˥",
		"M","W",
		//"N","N",
		"P","Ԁ",
		"T","⊥",
		"U","∩",
		"V","Λ",
		"Y","⅄",
		"1","Ɩ",
		"2","ᄅ",
		"3","Ɛ",
		"4","ㄣ",
		"5","ϛ",
		"6","9",
		"7","ㄥ",
		//"8","8",
		"9","6",
		//"0","0",
		".","˙",
		",","'",
		"\"",",,",
		"?","¿",
		"!","¡",
		"[","]",
		"(",")",
		"{","}",
		"<",">",
		"&","⅋",
		"_","‾",
		"∴","∵",
		"⁅","⁆",
		";","؛",

		"ß","ᙠ",

		"ä","ɐ̤",
		"ö","o̤",
		"ü","n̤",
		"Ä","∀̤",
		"Ö","O̤",
		"Ü","∩̤",

		"^","⌄",
		"â","ɐ̮",
		"ê","ǝ̮",
		"î","ᴉ̮",
		"ô","o̮",
		"û","n̮",
		"Â","∀̮",
		"Ê","Ǝ̮",
		"Î","I̮",
		"Ô","O̮",
		"Û","∩̮",

		"`"," ̖",
		"à","ɐ̖",
		"è","ǝ̖",
		"ì","ì",
		"ò","o̖",
		"ù","n̖",
		"À","∀̖",
		"È","Ǝ̖",
		"Ì","Ì",
		"Ò","O̖",
		"Ù","∩̖",

		"´"," ̗",
		"á","ɐ̗",
		"é","ǝ̗",
		"í","í",
		"ó","o̗",
		"ú","n̗",
		"Á","∀̗",
		"É","Ǝ̗",
		"Í","Í",
		"Ó","O̗",
		"Ú","∩̗",

		"🙃","🙂",
		"👍","👎",
		"👆","👇",
	};

	static HotStringComplexFlip(){
		ConfigServer.Register((ws,json)=>{
			var s=json.AsString();
			ws.Send(new JsonArray("Internal","Flip","A",s).ToString());
			ws.Send(new JsonArray("Internal","Flip","B",Flip(s)).ToString());
		},"Internal","Flip","A");
		ConfigServer.Register((ws,json)=>{
			var s=json.AsString();
			ws.Send(new JsonArray("Internal","Flip","B",s).ToString());
			ws.Send(new JsonArray("Internal","Flip","A",Flip(s)).ToString());
		},"Internal","Flip","B");
	}


	public override (int bs,string s)? Replace(string s){
		if(Regex.Match(s).Push(out var match).Success) return (match.Length,Flip(match.Groups[1].Value));
		return null;
	}

	public static string Flip(string s){
		var enumerator=StringInfo.GetTextElementEnumerator(s);
		var str=new StringBuilder();
		while(enumerator.MoveNext()){
			var element=enumerator.GetTextElement();
			str.Insert(0,FlipSingle(element));
		}
		return str.ToString();
	}

	private static string FlipSingle(string s){
		for(var i=0;i<Conversion.Length;i+=2){
			if(s.Equals(Conversion[i])) return Conversion[i+1];
			if(s.Equals(Conversion[i+1])) return Conversion[i];
		}
		return s;
	}
}
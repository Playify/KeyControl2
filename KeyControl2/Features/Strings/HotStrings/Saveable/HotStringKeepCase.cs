using System.Text;
using PlayifyUtility.Jsons;

namespace KeyControl2.Features.Strings.HotStrings.SaveAble;

public class HotStringKeepCase:HotStringSaveAble{
	private readonly string _from;
	private readonly int _length;
	private readonly string _to;

	public HotStringKeepCase(JsonObject json):base(json){
		_from=json["From"].AsString();
		if(_from.Length==0) throw new ArgumentException("From can't be empty");
		_to=json["To"].AsString();
		if(_from.Length!=_to.Length) throw new Exception("From and To must be of equal Length");
		_length=_from.Length;
	}

	public HotStringKeepCase(string from,string to):base(null){
		_from=from;
		if(_from.Length==0) throw new ArgumentException("From can't be empty");
		_to=to;
		if(_from.Length!=_to.Length) throw new Exception("From and To must be of equal Length");
		_length=from.Length;
	}

	public override JsonObject ToJson()
		=>new(){
			{"From",_from},
			{"To",_to},
		};

	public override (int bs,string s)? Replace(string s){
		try{
			if(s.Length<_length) return null;
			if(!s.EndsWith(_from,StringComparison.OrdinalIgnoreCase)) return null;

			var str=new StringBuilder();
			var sub=s.Substring(s.Length-_length);
			for(var i=0;i<_length;i++) str.Append(char.IsUpper(sub[i])?char.ToUpperInvariant(_to[i]):char.ToLowerInvariant(_to[i]));
			return (_length,str.ToString());
		} catch(IndexOutOfRangeException e){
			Console.WriteLine(e);
			Console.WriteLine(s);
			Console.WriteLine(_from);
			Console.WriteLine(_to);
			return null;
		}
	}
}
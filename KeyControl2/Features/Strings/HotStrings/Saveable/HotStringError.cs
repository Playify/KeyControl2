using PlayifyUtility.Jsons;

namespace KeyControl2.Features.Strings.HotStrings.SaveAble;

public class HotStringError:HotStringSaveAble{
	private readonly JsonObject _json;

	public HotStringError(JsonObject json,Exception exception):base(json){
		_json=(JsonObject)json.DeepCopy();
		_json["Error"]=exception.Message;
		_json["StackTrace"]=exception.ToString();
	}

	public override (int bs,string s)? Replace(string s)=>null;

	public override JsonObject ToJson()=>_json;
}
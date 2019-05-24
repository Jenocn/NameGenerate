using System;
using System.Collections.Generic;
using Newtonsoft;

public enum NameWordLibraryType {
	FirstName,
	Male_CenterWord,
	Male_LastWord,
	Male_CoupleWord,
	Female_CenterWord,
	Female_LastWord,
	Female_CoupleWord,
}

public enum NameWordFeatureType {
	None,
	Feature_Strong,
}

public class NameGenerate {
	private Dictionary<NameWordLibraryType, NameWordLibrary> _nameWordLibrary = new Dictionary<NameWordLibraryType, NameWordLibrary>();
	private Dictionary<NameWordFeatureType, NameWordLibrary> _featureWordLibrary = new Dictionary<NameWordFeatureType, NameWordLibrary>();

	public void Parse(string configJson) {
		var value = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(configJson);
		_nameWordLibrary[NameWordLibraryType.FirstName] = new NameWordLibrary(value["first_name"]);
		_nameWordLibrary[NameWordLibraryType.Male_CenterWord] = new NameWordLibrary(value["male_center"]);
		_nameWordLibrary[NameWordLibraryType.Male_LastWord] = new NameWordLibrary(value["male_last"]);
		_nameWordLibrary[NameWordLibraryType.Male_CoupleWord] = new NameWordLibrary(value["male_couple"]);
		_nameWordLibrary[NameWordLibraryType.Female_CenterWord] = new NameWordLibrary(value["female_center"]);
		_nameWordLibrary[NameWordLibraryType.Female_LastWord] = new NameWordLibrary(value["female_last"]);
		_nameWordLibrary[NameWordLibraryType.Female_CoupleWord] = new NameWordLibrary(value["female_couple"]);
	}

	/// <summary>
	/// firstName: 指定姓, 填空""表示随机
	/// sex: 指定性别, 0随机,1男,2女
	/// nameLength: 名字长度,不包括姓, 0随机
	/// couple: 名字成对, 0随机, 1成对
	/// feature[]: 性格特征
	/// </summary>
	public Tuple<string, int, List<NameWordFeatureType>> Generate(string firstName, int sex, int nameLength, int couple, List<NameWordFeatureType> feature) {

		firstName = string.IsNullOrEmpty(firstName) ? _GenerateFirstName() : firstName;
		sex = sex <= 0 ? _Random(1, 3) : sex;
		nameLength = nameLength <= 0 ? _Random(1, 3) : nameLength;
		couple = couple <= 0 ? _Random(0, 2) : couple;
		var tempFeature = NameWordFeatureType.None;

		var tempName = "";

		List<NameWordFeatureType> _featureTypes = new List<NameWordFeatureType>();
		if (couple == 1) {
			if (feature.Count > 0) {
				tempFeature = feature[_Random(0, feature.Count)];
				_featureTypes.Add(tempFeature);
			}
			var coupleWord = _GenerateCoupleWord(sex, tempFeature);
			for (int i = 0; i < nameLength; ++i) {
				tempName += coupleWord;
			}
			return new Tuple<string, int, List<NameWordFeatureType>>(firstName + tempName, sex, _featureTypes);
		}

		if (nameLength == 1) {
			if (feature.Count > 0) {
				tempFeature = feature[_Random(0, feature.Count)];
			}
			tempName += _GenerateLastWord(sex, tempFeature);
			return new Tuple<string, int, List<NameWordFeatureType>>(firstName + tempName, sex, _featureTypes);
		}

		if (feature.Count > 0) {
			tempFeature = feature[_Random(0, feature.Count)];
		}
		tempName += _GenerateCenterWord(sex, tempFeature);

		for (int i = 0; i < nameLength - 1; ++i) {
			if (feature.Count > 0) {
				tempFeature = feature[_Random(0, feature.Count)];
			}
			tempName += _GenerateLastWord(sex, tempFeature);
		}
		return new Tuple<string, int, List<NameWordFeatureType>>(firstName + tempName, sex, _featureTypes);
	}

	private string _GenerateFirstName() {
		return _nameWordLibrary[NameWordLibraryType.FirstName].Random();
	}

	private string _GenerateCenterWord(int sex, NameWordFeatureType feature) {
		var sexLibrary = sex == 1 ?
			_nameWordLibrary[NameWordLibraryType.Male_CenterWord] :
			_nameWordLibrary[NameWordLibraryType.Female_CenterWord];
		if (feature == NameWordFeatureType.None) {
			return sexLibrary.Random();
		}
		var newLibrary = sexLibrary.CrossWith(_featureWordLibrary[feature]);
		return newLibrary.Random();
	}

	private string _GenerateLastWord(int sex, NameWordFeatureType feature) {
		var sexLibrary = sex == 1 ?
			_nameWordLibrary[NameWordLibraryType.Male_LastWord] :
			_nameWordLibrary[NameWordLibraryType.Female_LastWord];
		if (feature == NameWordFeatureType.None) {
			return sexLibrary.Random();
		}
		var newLibrary = sexLibrary.CrossWith(_featureWordLibrary[feature]);
		return newLibrary.Random();
	}

	private string _GenerateCoupleWord(int sex, NameWordFeatureType feature) {
		var sexLibrary = sex == 1 ?
			_nameWordLibrary[NameWordLibraryType.Male_CoupleWord] :
			_nameWordLibrary[NameWordLibraryType.Female_CoupleWord];
		if (feature == NameWordFeatureType.None) {
			return sexLibrary.Random();
		}
		var newLibrary = sexLibrary.CrossWith(_featureWordLibrary[feature]);
		return newLibrary.Random();
	}

	private int _Random(int min, int max) {
		return new Random(Guid.NewGuid().GetHashCode()).Next(min, max);
	}
}

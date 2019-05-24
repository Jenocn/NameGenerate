using System;
using System.Collections.Generic;

public class NameWordLibrary {
	private HashSet<string> _wordSet = null;
	private List<string> _wordList = null;

	public NameWordLibrary(HashSet<string> library) {
		_wordSet = library;
		_wordList = new List<string>(_wordSet);
	}

	public NameWordLibrary(List<string> library) {
		_wordSet = new HashSet<string>(library);
		_wordList = new List<string>(_wordSet);
	}

	public bool Exists(string word) {
		return _wordSet.Contains(word);
	}

	public string Random() {
		var random = new Random(Guid.NewGuid().GetHashCode());
		var index = random.Next(0, _wordList.Count);
		return _wordList[index];
	}

	public NameWordLibrary CrossWith(NameWordLibrary value) {
		var retSet = new HashSet<string>();
		foreach (var item in value._wordSet) {
			if (Exists(item)) {
				retSet.Add(item);
			}
		}
		return new NameWordLibrary(retSet);
	}
}
